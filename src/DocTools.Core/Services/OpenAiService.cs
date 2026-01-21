using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DocTools.Core.Models;
using Microsoft.Extensions.Logging;

namespace DocTools.Core.Services;

/// <summary>
/// Provides AI-powered documentation generation using OpenAI's GPT models.
/// </summary>
public class OpenAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenAiService> _logger;
    private readonly string _apiKey;
    private readonly string _model;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAiService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API requests.</param>
    /// <param name="logger">The logger instance.</param>
    /// <param name="apiKey">The OpenAI API key.</param>
    /// <param name="model">The model to use (default: gpt-4).</param>
    public OpenAiService(HttpClient httpClient, ILogger<OpenAiService> logger, string apiKey, string model = "gpt-4")
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _apiKey = !string.IsNullOrWhiteSpace(apiKey) 
            ? apiKey 
            : throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
        _model = model ?? "gpt-4";

        _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    /// <inheritdoc/>
    public async Task<string> GenerateDocumentationAsync(
        string sourceCode,
        LanguageInfo language,
        DocumentationStyle style,
        OutputMode outputMode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sourceCode))
        {
            throw new ArgumentException("Source code cannot be null or empty.", nameof(sourceCode));
        }

        if (language == null)
        {
            throw new ArgumentNullException(nameof(language));
        }

        _logger.LogInformation("Generating documentation for {Language} file with style: {Style}", 
            language.Name, style);

        var prompt = BuildPrompt(sourceCode, language, style, outputMode);
        
        try
        {
            var response = await CallOpenAiApiAsync(prompt, cancellationToken);
            _logger.LogInformation("Documentation generated successfully");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate documentation");
            throw;
        }
    }

    /// <summary>
    /// Builds the AI prompt based on the documentation requirements.
    /// </summary>
    private string BuildPrompt(string sourceCode, LanguageInfo language, DocumentationStyle style, OutputMode outputMode)
    {
        var styleDescription = style switch
        {
            DocumentationStyle.Minimal => "short, high-level comments focusing on what the code does",
            DocumentationStyle.Full => "comprehensive line-by-line explanations with detailed comments",
            DocumentationStyle.Enterprise => "professional, production-grade documentation with XML/JSDoc style comments, including parameter descriptions, return values, and examples where appropriate",
            _ => "clear and concise comments"
        };

        if (outputMode == OutputMode.Inline)
        {
            return $@"You are a code documentation expert. Add {styleDescription} to the following {language.Name} code.

IMPORTANT RULES:
1. ONLY add documentation comments - do NOT modify any functional code
2. Use {language.DocCommentSyntax} for documentation comments
3. Preserve all original code structure, indentation, and formatting
4. Return the COMPLETE file with comments added
5. Do not add explanatory text before or after the code
6. Do not use markdown code fences in your response

{language.Name} Code:
{sourceCode}";
        }
        else // Markdown
        {
            return $@"You are a code documentation expert. Create structured markdown documentation for the following {language.Name} code.

Documentation Style: {styleDescription}

Generate documentation in this format:

# File Documentation

## Purpose
[Brief description of what this file does]

## Key Components
[List main classes, functions, or modules]

## Functions/Methods
[For each major function/method, provide:]
- **Name**: function name
- **Purpose**: what it does
- **Parameters**: input parameters and their types
- **Returns**: return value and type
- **Example** (if applicable)

## Dependencies
[List imports/dependencies]

## Notes
[Any additional important information]

{language.Name} Code:
{sourceCode}";
        }
    }

    /// <summary>
    /// Calls the OpenAI API to generate documentation.
    /// </summary>
    private async Task<string> CallOpenAiApiAsync(string prompt, CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            model = _model,
            messages = new[]
            {
                new { role = "system", content = "You are an expert code documentation assistant. You help developers create clear, professional documentation." },
                new { role = "user", content = prompt }
            },
            temperature = 0.3,
            max_tokens = 4000
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("chat/completions", content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"OpenAI API request failed: {response.StatusCode} - {errorContent}");
        }

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        using var document = JsonDocument.Parse(responseJson);
        
        var choices = document.RootElement.GetProperty("choices");
        if (choices.GetArrayLength() == 0)
        {
            throw new InvalidOperationException("No response from OpenAI API");
        }

        var messageContent = choices[0].GetProperty("message").GetProperty("content").GetString();
        return messageContent ?? string.Empty;
    }
}
