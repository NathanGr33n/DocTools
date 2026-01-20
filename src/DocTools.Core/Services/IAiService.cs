using DocTools.Core.Models;

namespace DocTools.Core.Services;

/// <summary>
/// Defines the contract for AI-powered documentation generation services.
/// </summary>
public interface IAiService
{
    /// <summary>
    /// Generates documentation for the provided source code asynchronously.
    /// </summary>
    /// <param name="sourceCode">The source code to document.</param>
    /// <param name="language">Information about the programming language.</param>
    /// <param name="style">The documentation style to apply.</param>
    /// <param name="outputMode">The output mode (inline or markdown).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The generated documentation content.</returns>
    Task<string> GenerateDocumentationAsync(
        string sourceCode,
        LanguageInfo language,
        DocumentationStyle style,
        OutputMode outputMode,
        CancellationToken cancellationToken = default);
}
