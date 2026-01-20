namespace DocTools.Core.Services;

/// <summary>
/// Defines the contract for file operations.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Reads the content of a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to read.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>The file content as a string.</returns>
    Task<string> ReadFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes content to a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the file to write.</param>
    /// <param name="content">The content to write.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    Task WriteFileAsync(string filePath, string content, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all files in a directory with optional recursion.
    /// </summary>
    /// <param name="directoryPath">The directory path to search.</param>
    /// <param name="recursive">Whether to search subdirectories.</param>
    /// <param name="extensions">Optional file extensions to filter (e.g., ".cs", ".py").</param>
    /// <returns>A collection of file paths.</returns>
    IEnumerable<string> GetFiles(string directoryPath, bool recursive, params string[] extensions);

    /// <summary>
    /// Checks if a path is a directory.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if the path is a directory; otherwise, false.</returns>
    bool IsDirectory(string path);

    /// <summary>
    /// Checks if a file or directory exists.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns>True if the path exists; otherwise, false.</returns>
    bool Exists(string path);
}
