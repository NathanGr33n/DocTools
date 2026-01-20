namespace DocTools.Core.Models;

/// <summary>
/// Represents the result of a documentation generation operation.
/// </summary>
public class DocumentationResult
{
    /// <summary>
    /// Gets or sets the original file path that was processed.
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the generated documentation content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the output file path (for markdown mode).
    /// </summary>
    public string? OutputPath { get; set; }
}
