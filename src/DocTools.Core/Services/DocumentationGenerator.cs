using DocTools.Core.Models;
using Microsoft.Extensions.Logging;

namespace DocTools.Core.Services;

/// <summary>
/// Orchestrates the documentation generation process across multiple files and coordinates all services.
/// </summary>
public class DocumentationGenerator : IDocumentationGenerator
{
    private readonly IAiService _aiService;
    private readonly IFileService _fileService;
    private readonly ILanguageDetector _languageDetector;
    private readonly ILogger<DocumentationGenerator> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationGenerator"/> class.
    /// </summary>
    /// <param name="aiService">The AI service for documentation generation.</param>
    /// <param name="fileService">The file service for file operations.</param>
    /// <param name="languageDetector">The language detection service.</param>
    /// <param name="logger">The logger instance.</param>
    public DocumentationGenerator(
        IAiService aiService,
        IFileService fileService,
        ILanguageDetector languageDetector,
        ILogger<DocumentationGenerator> logger)
    {
        _aiService = aiService ?? throw new ArgumentNullException(nameof(aiService));
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _languageDetector = languageDetector ?? throw new ArgumentNullException(nameof(languageDetector));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DocumentationResult>> GenerateAsync(
        DocumentationOptions options,
        CancellationToken cancellationToken = default)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.Path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(options));
        }

        if (!_fileService.Exists(options.Path))
        {
            throw new FileNotFoundException($"Path not found: {options.Path}");
        }

        _logger.LogInformation("Starting documentation generation for: {Path}", options.Path);

        var files = GetFilesToProcess(options);
        var results = new List<DocumentationResult>();

        foreach (var file in files)
        {
            var result = await ProcessFileAsync(file, options, cancellationToken);
            results.Add(result);
        }

        _logger.LogInformation("Documentation generation completed. Processed {Count} files", results.Count);
        return results;
    }

    /// <summary>
    /// Gets the list of files to process based on the provided path and options.
    /// </summary>
    private IEnumerable<string> GetFilesToProcess(DocumentationOptions options)
    {
        if (_fileService.IsDirectory(options.Path))
        {
            _logger.LogInformation("Processing directory: {Path} (Recursive: {Recursive})", 
                options.Path, options.Recursive);

            var supportedExtensions = _languageDetector.GetSupportedExtensions().ToArray();
            return _fileService.GetFiles(options.Path, options.Recursive, supportedExtensions);
        }
        else
        {
            _logger.LogInformation("Processing single file: {Path}", options.Path);
            return new[] { options.Path };
        }
    }

    /// <summary>
    /// Processes a single file to generate documentation.
    /// </summary>
    private async Task<DocumentationResult> ProcessFileAsync(
        string filePath,
        DocumentationOptions options,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing file: {FilePath}", filePath);

        try
        {
            // Detect language
            var language = _languageDetector.DetectLanguage(filePath);
            if (language == null)
            {
                _logger.LogWarning("Unsupported file type: {FilePath}", filePath);
                return new DocumentationResult
                {
                    FilePath = filePath,
                    Success = false,
                    ErrorMessage = $"Unsupported file type: {Path.GetExtension(filePath)}"
                };
            }

            // Read source code
            var sourceCode = await _fileService.ReadFileAsync(filePath, cancellationToken);
            
            // Generate documentation using AI
            var documentation = await _aiService.GenerateDocumentationAsync(
                sourceCode,
                language,
                options.Style,
                options.OutputMode,
                cancellationToken);

            // Write output
            string? outputPath = null;
            if (options.OutputMode == OutputMode.Inline)
            {
                await _fileService.WriteFileAsync(filePath, documentation, cancellationToken);
                outputPath = filePath;
                _logger.LogInformation("Updated file with inline documentation: {FilePath}", filePath);
            }
            else // Markdown
            {
                outputPath = Path.ChangeExtension(filePath, ".md");
                await _fileService.WriteFileAsync(outputPath, documentation, cancellationToken);
                _logger.LogInformation("Created markdown documentation: {OutputPath}", outputPath);
            }

            return new DocumentationResult
            {
                FilePath = filePath,
                Content = documentation,
                Success = true,
                OutputPath = outputPath
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process file: {FilePath}", filePath);
            return new DocumentationResult
            {
                FilePath = filePath,
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}
