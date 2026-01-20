using DocTools.Core.Models;

namespace DocTools.Core.Services;

/// <summary>
/// Defines the contract for detecting programming languages from file extensions.
/// </summary>
public interface ILanguageDetector
{
    /// <summary>
    /// Detects the programming language based on the file extension.
    /// </summary>
    /// <param name="filePath">The file path to analyze.</param>
    /// <returns>Language information, or null if not supported.</returns>
    LanguageInfo? DetectLanguage(string filePath);

    /// <summary>
    /// Gets all supported file extensions.
    /// </summary>
    /// <returns>A collection of supported extensions.</returns>
    IEnumerable<string> GetSupportedExtensions();
}
