using DocTools.Core.Models;

namespace DocTools.Core.Services;

/// <summary>
/// Defines the contract for the main documentation generation orchestrator.
/// </summary>
public interface IDocumentationGenerator
{
    /// <summary>
    /// Generates documentation based on the provided options.
    /// </summary>
    /// <param name="options">The documentation generation options.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A collection of documentation results.</returns>
    Task<IEnumerable<DocumentationResult>> GenerateAsync(
        DocumentationOptions options,
        CancellationToken cancellationToken = default);
}
