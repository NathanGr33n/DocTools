using DocTools.Core.Services;
using Xunit;

namespace DocTools.Tests;

/// <summary>
/// Unit tests for the FileService.
/// </summary>
public class FileServiceTests : IDisposable
{
    private readonly FileService _fileService;
    private readonly string _testDirectory;
    private readonly string _testFile;

    public FileServiceTests()
    {
        _fileService = new FileService();
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _testFile = Path.Combine(_testDirectory, "test.txt");
        
        Directory.CreateDirectory(_testDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    [Fact]
    public async Task ReadFileAsync_ShouldReturnFileContent()
    {
        // Arrange
        var expectedContent = "Test content";
        await File.WriteAllTextAsync(_testFile, expectedContent);

        // Act
        var result = await _fileService.ReadFileAsync(_testFile);

        // Assert
        Assert.Equal(expectedContent, result);
    }

    [Fact]
    public async Task WriteFileAsync_ShouldCreateFile()
    {
        // Arrange
        var content = "New content";
        var filePath = Path.Combine(_testDirectory, "newfile.txt");

        // Act
        await _fileService.WriteFileAsync(filePath, content);

        // Assert
        Assert.True(File.Exists(filePath));
        var actualContent = await File.ReadAllTextAsync(filePath);
        Assert.Equal(content, actualContent);
    }

    [Fact]
    public void GetFiles_ShouldReturnFilesFromDirectory()
    {
        // Arrange
        File.WriteAllText(Path.Combine(_testDirectory, "file1.txt"), "content1");
        File.WriteAllText(Path.Combine(_testDirectory, "file2.cs"), "content2");

        // Act
        var files = _fileService.GetFiles(_testDirectory, false).ToList();

        // Assert
        Assert.Equal(2, files.Count);
    }

    [Fact]
    public void GetFiles_WithExtensions_ShouldFilterByExtension()
    {
        // Arrange
        File.WriteAllText(Path.Combine(_testDirectory, "file1.txt"), "content1");
        File.WriteAllText(Path.Combine(_testDirectory, "file2.cs"), "content2");
        File.WriteAllText(Path.Combine(_testDirectory, "file3.cs"), "content3");

        // Act
        var files = _fileService.GetFiles(_testDirectory, false, ".cs").ToList();

        // Assert
        Assert.Equal(2, files.Count);
        Assert.All(files, f => Assert.EndsWith(".cs", f));
    }

    [Fact]
    public void GetFiles_Recursive_ShouldIncludeSubdirectories()
    {
        // Arrange
        var subDir = Path.Combine(_testDirectory, "subdir");
        Directory.CreateDirectory(subDir);
        File.WriteAllText(Path.Combine(subDir, "file.txt"), "content");

        // Act
        var files = _fileService.GetFiles(_testDirectory, true).ToList();

        // Assert
        Assert.Contains(files, f => f.Contains("subdir"));
    }

    [Fact]
    public void IsDirectory_ShouldReturnTrueForDirectory()
    {
        // Act
        var result = _fileService.IsDirectory(_testDirectory);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDirectory_ShouldReturnFalseForFile()
    {
        // Arrange
        File.WriteAllText(_testFile, "content");

        // Act
        var result = _fileService.IsDirectory(_testFile);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Exists_ShouldReturnTrueForExistingPath()
    {
        // Act
        var result = _fileService.Exists(_testDirectory);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Exists_ShouldReturnFalseForNonExistingPath()
    {
        // Act
        var result = _fileService.Exists(Path.Combine(_testDirectory, "nonexistent"));

        // Assert
        Assert.False(result);
    }
}