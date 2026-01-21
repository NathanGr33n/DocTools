using System.CommandLine;
using DocTools.Core.Models;
using DocTools.Core.Services;
using Microsoft.Extensions.Logging;

namespace DocTools.CLI;

/// <summary>
/// Main entry point for the DocTools CLI application.
/// </summary>
class Program
{
    /// <summary>
    /// Main method that configures and runs the CLI.
    /// </summary>
    static async Task<int> Main(string[] args)
    {
        // Create root command
        var rootCommand = new RootCommand("DocTools - AI-powered code documentation generator")
        {
            Name = "dt"
        };
        
        // Add usage examples
        rootCommand.AddExample(new[] { "./src/MyFile.cs" });
        rootCommand.AddExample(new[] { "./src", "-r" });
        rootCommand.AddExample(new[] { "./src", "-r", "-s", "minimal", "-o", "markdown" });

        // Define options
        var pathArgument = new Argument<string>(
            name: "path",
            description: "Path to a file or directory to document");

        var recursiveOption = new Option<bool>(
            aliases: new[] { "--recursive", "-r" },
            description: "Process directories recursively");

        var styleOption = new Option<DocumentationStyle>(
            aliases: new[] { "--style", "-s" },
            getDefaultValue: () => DocumentationStyle.Full,
            description: "Documentation style: minimal, full, or enterprise");

        var outputOption = new Option<OutputMode>(
            aliases: new[] { "--output", "-o" },
            getDefaultValue: () => OutputMode.Inline,
            description: "Output mode: inline (modify source) or markdown (create .md file)");

        var apiKeyOption = new Option<string>(
            aliases: new[] { "--api-key", "-k" },
            description: "OpenAI API key (or set OPENAI_API_KEY environment variable)");

        var modelOption = new Option<string>(
            aliases: new[] { "--model", "-m" },
            getDefaultValue: () => "gpt-4",
            description: "OpenAI model to use (default: gpt-4)");

        // Add options to command
        rootCommand.AddArgument(pathArgument);
        rootCommand.AddOption(recursiveOption);
        rootCommand.AddOption(styleOption);
        rootCommand.AddOption(outputOption);
        rootCommand.AddOption(apiKeyOption);
        rootCommand.AddOption(modelOption);

        // Set handler
        rootCommand.SetHandler(
            async (string path, bool recursive, DocumentationStyle style, OutputMode output, string apiKey, string model) =>
            {
                await ExecuteAsync(path, recursive, style, output, apiKey, model);
            },
            pathArgument, recursiveOption, styleOption, outputOption, apiKeyOption, modelOption);

        return await rootCommand.InvokeAsync(args);
    }

    /// <summary>
    /// Executes the documentation generation process.
    /// </summary>
    private static async Task ExecuteAsync(
        string path,
        bool recursive,
        DocumentationStyle style,
        OutputMode output,
        string apiKey,
        string model)
    {
        // Configure logging
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        try
        {
            // Get API key from option or environment variable
            var key = apiKey ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(key))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: OpenAI API key is required.");
                Console.WriteLine("Provide it via --api-key option or set OPENAI_API_KEY environment variable.");
                Console.ResetColor();
                Environment.Exit(1);
                return;
            }

            // Validate path
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Path argument is required.");
                Console.ResetColor();
                Environment.Exit(1);
                return;
            }

            if (!File.Exists(path) && !Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: Path not found: {path}");
                Console.ResetColor();
                Environment.Exit(1);
                return;
            }

            // Display configuration
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("  DocTools - Documentation Generator");
            Console.WriteLine("═══════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine($"Path:      {path}");
            Console.WriteLine($"Recursive: {recursive}");
            Console.WriteLine($"Style:     {style}");
            Console.WriteLine($"Output:    {output}");
            Console.WriteLine($"Model:     {model}");
            Console.WriteLine();

            // Initialize services
            var httpClient = new HttpClient();
            var fileService = new FileService();
            var languageDetector = new LanguageDetector();
            var aiService = new OpenAiService(
                httpClient,
                loggerFactory.CreateLogger<OpenAiService>(),
                key,
                model);
            var generator = new DocumentationGenerator(
                aiService,
                fileService,
                languageDetector,
                loggerFactory.CreateLogger<DocumentationGenerator>());

            // Create options
            var options = new DocumentationOptions
            {
                Path = path,
                Recursive = recursive,
                Style = style,
                OutputMode = output,
                ApiKey = key,
                Model = model
            };

            // Generate documentation
            Console.WriteLine("Generating documentation...");
            Console.WriteLine();

            var results = await generator.GenerateAsync(options);

            // Display results
            var successCount = results.Count(r => r.Success);
            var failureCount = results.Count(r => !r.Success);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Successfully processed: {successCount} file(s)");
            Console.ResetColor();

            if (failureCount > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"⚠ Failed to process: {failureCount} file(s)");
                Console.ResetColor();

                foreach (var result in results.Where(r => !r.Success))
                {
                    Console.WriteLine($"  - {result.FilePath}: {result.ErrorMessage}");
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Documentation generation complete!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Fatal error: {ex.Message}");
            Console.ResetColor();
            
            var logger = loggerFactory.CreateLogger<Program>();
            logger.LogError(ex, "Fatal error during execution");
            Environment.Exit(1);
        }
    }
}
