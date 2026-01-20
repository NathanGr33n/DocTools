namespace DocTools.Core.Services;

/// <summary>
/// Provides file system operations for reading, writing, and discovering files.
/// </summary>
public class FileService : IFileService
{
    /// <inheritdoc/>
    public async Task<string> ReadFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}", filePath);
        }

        return await File.ReadAllTextAsync(filePath, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task WriteFileAsync(string filePath, string content, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(filePath, content, cancellationToken);
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetFiles(string directoryPath, bool recursive, params string[] extensions)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));
        }

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
        }

        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        
        if (extensions == null || extensions.Length == 0)
        {
            return Directory.GetFiles(directoryPath, "*.*", searchOption);
        }

        var files = new List<string>();
        foreach (var extension in extensions)
        {
            var pattern = $"*{extension}";
            files.AddRange(Directory.GetFiles(directoryPath, pattern, searchOption));
        }

        return files;
    }

    /// <inheritdoc/>
    public bool IsDirectory(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        return Directory.Exists(path);
    }

    /// <inheritdoc/>
    public bool Exists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        return File.Exists(path) || Directory.Exists(path);
    }
}
