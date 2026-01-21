# DocTools

A production-ready CLI tool that automatically generates professional documentation for code files using AI. Supports multiple programming languages and documentation styles.

## Features

- **Multi-language Support**: C#, Python, JavaScript, TypeScript, Java, Go, Rust, C++, Ruby, PHP, Swift, Kotlin, and more
- **Flexible Output Modes**: Inline comments (modifies source files) or Markdown documentation files
- **Documentation Styles**: Minimal (high-level), Full (detailed), Enterprise (production-grade)
- **Recursive Processing**: Document entire codebases by processing directories recursively
- **Batch Processing**: Generate documentation for multiple files in a single run
- **Extensible Architecture**: Clean modular design with dependency injection

## Installation

### Prerequisites
- .NET 8.0 or later
- OpenAI API key

### Build from Source
```bash
# Clone the repository
git clone <repository-url>
cd DocTools

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test
```

### Install as Global Tool
```bash
dotnet pack src/DocTools.CLI/DocTools.CLI.csproj
dotnet tool install --global --add-source ./src/DocTools.CLI/nupkg
```

## Usage

### Basic Usage

```bash
# Document a single file (with inline comments)
Doctools.CLI MyFile.cs

# Document an entire directory (non-recursive)
Doctools.CLI ./src

# Document directory recursively
Doctools.CLI ./src --recursive
```

### API Key Configuration

**Option 1**: Command-line flag

```bash
doctools.CLI MyFile.cs --api-key sk-...
```

**Option 2**: Environment variable

```bash
# Windows PowerShell
$env:OPENAI_API_KEY="sk-..."
doctools.CLI MyFile.cs

# Windows CMD
set OPENAI_API_KEY=sk-
doctools.CLI MyFile.cs

# Linux/Mac
export OPENAI_API_KEY="sk-..."
Doctools.CLI MyFile.cs
```

### Command-Line Options

| Option | Alias | Type | Default | Description |
|--------|-------|------|---------|-------------|
| `--style` | `-s` | Minimal, Full, Enterprise | Full | Documentation style level |
| `--output` | `-o` | Inline, Markdown | Inline | Output mode |
| `--recursive` | `-r` | boolean | false | Process directories recursively |
| `--api-key` | `-k` | string | (from env var) | OpenAI API key |
| `--model` | `-m` | string | gpt-4 | AI model to use |

### Examples
#### Generate Markdown Documentation

```bash
# Create .md documentation file
doctools.CLI MyService.cs --output markdown
```

#### Enterprise-style Documentation
```bash
# Use enterprise-style XML/JSDoc comments
doctools.CLI MyController.cs --style enterprise
```

#### Batch Process a Directory
```bash
# Process all supported files in directory recursively
doctools.CLI ./src --recursive --style full --output markdown
```

## Project Structure

```
DocTools/
├── src/
│   ├── DocTools.CLI/          # CLI application
│   │   └── Program.cs
│   └── DocTools.Core/         # Core library
│       ├── Models/            # Data models
│       │   ├── DocumentationOptions.cs
│       │   ├── LanguageInfo.cs
│       │   └── DocumentationResult.cs
│       └── Services/          # Core services
│           ├── IAiService.cs
│           ├── IFileService.cs
│           ├── ILanguageDetector.cs
│           ├── IDocumentationGenerator.cs
│           └── OpenAiService.cs
│           ├── FileService.cs
│           ├── LanguageDetector.cs
│           └── DocumentationGenerator.cs
├── tests/
│   └── DocTools.Tests/        # Unit tests
│       ├── LanguageDetectorTests.cs
│       └── FileServiceTests.cs
└── README.md
```

## Architecture

The system follows a clean, modular architecture with clear separation of concerns:

1. **CLI Layer** (`DocTools.CLI`): Handles command-line interface, argument parsing, and user interaction
2. **Core Layer** (`DocTools.Core`): Contains business logic and services
   - **Models**: Data structures for options, results, and language information
   - **Services**: Injectable interfaces and implementations for AI, files, and orchestration

### Key Components

- **LanguageDetector**: Identifies programming languages from file extensions and provides language-specific syntax information
- **FileService**: Abstracts file system operations (read, write, discovery)
- **OpenAiService**: Handles AI API integration with intelligent prompt generation
- **DocumentationGenerator**: Orchestrates the entire documentation process across files

## Supported Languages

| Language | Extension | Doc Comment Style |
|----------|-----------|-------------------|
| C# | .cs | `///` |
| Python | .py | `#` |
| JavaScript | .js | `/**` |
| TypeScript | .ts | `/**` |
| JavaScript (JSX) | .jsx | `/**` |
| TypeScript (TSX) | .tsx | `/**` |
| Java | .java | `/**` |
| Go | .go | `//` |
| Rust | .rs | `///` |
| C++ | .cpp | `///` |
| C | .c | `/**` |
| Ruby | .rb | # |
| PHP | .php | `/**` |
| Swift | .swift | `///` |
| Kotlin | .kt | `/**` |

## API Key Integration

The tool requires an OpenAI API key for AI-powered documentation generation:

1. Get an API key from [OpenAI](https://platform.openai.com/api-keys)
2. Provide it via `--api-key` flag or `OPENAI_API_KEY` environment variable
3. The key is never logged or stored

## Testing
```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## Development

### Adding a New Language
1. Open `src/DocTools.Core/Services/LanguageDetector.cs`
2. Add a new entry to the `_languageMap` dictionary
3. Include comment syntax patterns for the language

### Customizing Documentation Style
Modify the `BuildPrompt` method in `OpenAiService.cs` to adjust prompt templates and style instructions.

### Error Handling
- All file operations include proper exception handling
- API failures are logged with descriptive messages
- CLI provides clear error messages for validation failures
- Unsupported file types are gracefully skipped

## License

MIT License

A .NET 9.0 command-line tool for document processing.

## Project Structure

```
DocTools/
├── src/
│   └── DocTools.CLI/          # Main CLI application
├── .config/                    # Configuration files
├── DocTool.sln                 # Visual Studio solution
└── README.md
```

## Requirements

- .NET 9.0 SDK or later

## Building

```powershell
dotnet build
```

## Running

```powershell
dotnet run --project src/DocTools.CLI/DocTools.CLI.csproj
```

## Development

This project uses:
- C# with nullable reference types enabled
- Implicit usings enabled
- .NET 9.0 target framework

## License

TBD
