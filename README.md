# DocTools

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
