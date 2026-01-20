namespace DocTools.Core.Models;

/// <summary>
/// Contains information about a programming language and its documentation conventions.
/// </summary>
public class LanguageInfo
{
    /// <summary>
    /// Gets or sets the name of the programming language.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file extension (e.g., ".cs", ".py", ".js").
    /// </summary>
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the single-line comment syntax (e.g., "//", "#").
    /// </summary>
    public string SingleLineComment { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the multi-line comment start syntax (e.g., "/*", "\"\"\"").
    /// </summary>
    public string MultiLineCommentStart { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the multi-line comment end syntax (e.g., "*/", "\"\"\"").
    /// </summary>
    public string MultiLineCommentEnd { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the documentation comment syntax (e.g., "///", "#", "/**").
    /// </summary>
    public string DocCommentSyntax { get; set; } = string.Empty;
}
