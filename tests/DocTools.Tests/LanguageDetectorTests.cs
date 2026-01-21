using DocTools.Core.Services;
using Xunit;

namespace DocTools.Tests;

/// <summary>
/// Unit tests for the LanguageDetector service.
/// </summary>
public class LanguageDetectorTests
{
    private readonly LanguageDetector _detector;

    public LanguageDetectorTests()
    {
        _detector = new LanguageDetector();
    }

    [Theory]
    [InlineData("test.cs", "C#", "///")]
    [InlineData("test.py", "Python", "#")]
    [InlineData("test.js", "JavaScript", "/**")]
    [InlineData("test.ts", "TypeScript", "/**")]
    public void DetectLanguage_ShouldReturnCorrectInfo(string filePath, string expectedLanguage, string expectedDocComment)
    {
        // Act
        var result = _detector.DetectLanguage(filePath);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedLanguage, result.Name);
        Assert.Equal(expectedDocComment, result.DocCommentSyntax);
    }

    [Fact]
    public void DetectLanguage_ShouldReturnNullForUnknownExtension()
    {
        // Act
        var result = _detector.DetectLanguage("test.unknown");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DetectLanguage_ShouldReturnNullForEmptyPath()
    {
        // Act
        var result = _detector.DetectLanguage("");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetSupportedExtensions_ShouldReturnAllKnownExtensions()
    {
        // Act
        var extensions = _detector.GetSupportedExtensions();

        // Assert
        Assert.NotEmpty(extensions);
        Assert.Contains(".cs", extensions);
        Assert.Contains(".py", extensions);
        Assert.Contains(".js", extensions);
        Assert.Contains(".ts", extensions);
    }
}