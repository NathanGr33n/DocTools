namespace DocTools.Core.Models;

/// <summary>
/// Represents the documentation generation options specified by the user.
/// </summary>
public class DocumentationOptions
{
    /// <summary>
    /// Gets or sets the path to the file or folder to document.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to recursively process subdirectories.
    /// </summary>
    public bool Recursive { get; set; }

    /// <summary>
    /// Gets or sets the documentation style (minimal, full, or enterprise).
    /// </summary>
    public DocumentationStyle Style { get; set; } = DocumentationStyle.Full;

    /// <summary>
    /// Gets or sets the output mode (inline or markdown).
    /// </summary>
    public OutputMode OutputMode { get; set; } = OutputMode.Inline;

    /// <summary>
    /// Gets or sets the OpenAI API key for AI-powered documentation generation.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the AI model to use (default: gpt-4).
    /// </summary>
    public string Model { get; set; } = "gpt-4";
}

/// <summary>
/// Defines the documentation style levels.
/// </summary>
public enum DocumentationStyle
{
    /// <summary>
    /// Short, high-level comments only.
    /// </summary>
    Minimal,

    /// <summary>
    /// Comprehensive line-by-line explanations.
    /// </summary>
    Full,

    /// <summary>
    /// Professional, production-grade documentation style.
    /// </summary>
    Enterprise
}

/// <summary>
/// Defines the output modes for documentation generation.
/// </summary>
public enum OutputMode
{
    /// <summary>
    /// Insert comments directly into the source code file.
    /// </summary>
    Inline,

    /// <summary>
    /// Generate a separate markdown documentation file.
    /// </summary>
    Markdown
}
