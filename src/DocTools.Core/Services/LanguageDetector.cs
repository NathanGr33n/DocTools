using DocTools.Core.Models;

namespace DocTools.Core.Services;

/// <summary>
/// Detects programming languages based on file extensions and provides language-specific metadata.
/// </summary>
public class LanguageDetector : ILanguageDetector
{
    private readonly Dictionary<string, LanguageInfo> _languageMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageDetector"/> class.
    /// </summary>
    public LanguageDetector()
    {
        _languageMap = new Dictionary<string, LanguageInfo>(StringComparer.OrdinalIgnoreCase)
        {
            [".cs"] = new LanguageInfo
            {
                Name = "C#",
                Extension = ".cs",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "///"
            },
            [".py"] = new LanguageInfo
            {
                Name = "Python",
                Extension = ".py",
                SingleLineComment = "#",
                MultiLineCommentStart = "\"\"\"",
                MultiLineCommentEnd = "\"\"\"",
                DocCommentSyntax = "#"
            },
            [".js"] = new LanguageInfo
            {
                Name = "JavaScript",
                Extension = ".js",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".ts"] = new LanguageInfo
            {
                Name = "TypeScript",
                Extension = ".ts",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".jsx"] = new LanguageInfo
            {
                Name = "JavaScript (JSX)",
                Extension = ".jsx",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".tsx"] = new LanguageInfo
            {
                Name = "TypeScript (TSX)",
                Extension = ".tsx",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".java"] = new LanguageInfo
            {
                Name = "Java",
                Extension = ".java",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".go"] = new LanguageInfo
            {
                Name = "Go",
                Extension = ".go",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "//"
            },
            [".rs"] = new LanguageInfo
            {
                Name = "Rust",
                Extension = ".rs",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "///"
            },
            [".cpp"] = new LanguageInfo
            {
                Name = "C++",
                Extension = ".cpp",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "///"
            },
            [".c"] = new LanguageInfo
            {
                Name = "C",
                Extension = ".c",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".h"] = new LanguageInfo
            {
                Name = "C/C++ Header",
                Extension = ".h",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".hpp"] = new LanguageInfo
            {
                Name = "C++ Header",
                Extension = ".hpp",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".rb"] = new LanguageInfo
            {
                Name = "Ruby",
                Extension = ".rb",
                SingleLineComment = "#",
                MultiLineCommentStart = "=begin",
                MultiLineCommentEnd = "=end",
                DocCommentSyntax = "#"
            },
            [".php"] = new LanguageInfo
            {
                Name = "PHP",
                Extension = ".php",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            },
            [".swift"] = new LanguageInfo
            {
                Name = "Swift",
                Extension = ".swift",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "///"
            },
            [".kt"] = new LanguageInfo
            {
                Name = "Kotlin",
                Extension = ".kt",
                SingleLineComment = "//",
                MultiLineCommentStart = "/*",
                MultiLineCommentEnd = "*/",
                DocCommentSyntax = "/**"
            }
        };
    }

    /// <inheritdoc/>
    public LanguageInfo? DetectLanguage(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return null;
        }

        var extension = Path.GetExtension(filePath);
        return _languageMap.TryGetValue(extension, out var languageInfo) ? languageInfo : null;
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetSupportedExtensions()
    {
        return _languageMap.Keys;
    }
}
