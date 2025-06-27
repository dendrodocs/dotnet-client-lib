# GitHub Copilot Instructions for DendroDocs.Client

This document provides guidelines for GitHub Copilot to optimize contributions to the DendroDocs.Client library.

## Code Style and Conventions

### Null Checking
- **Use `is null` and `is not null`** instead of `== null` and `!= null`
- Use `ArgumentNullException.ThrowIfNull(parameter)` for parameter validation in public APIs
- Use `string.IsNullOrEmpty()` or `string.IsNullOrWhiteSpace()` for string validation

```csharp
// ✅ Preferred
if (value is null)
    return;

ArgumentNullException.ThrowIfNull(types);

if (string.IsNullOrEmpty(text))
    return text;

// ❌ Avoid
if (value == null)
    return;

if (parameter == null)
    throw new ArgumentNullException(nameof(parameter));
```

### Language Features
- Use modern C# language features and patterns
- Prefer pattern matching over traditional type checking
- Use expression-bodied members for simple properties and methods
- Use collection initializers and object initializers where appropriate
- Follow the existing `.editorconfig` conventions

```csharp
// ✅ Preferred - Pattern matching
if (type.StartsWith("System.Collections.Generic.", StringComparison.Ordinal))
{
    return !type.Contains("Enumerator") && !type.Contains("Compar") && !type.Contains("Exception");
}

// ✅ Preferred - Expression-bodied properties
bool IsPullRequest => GitHubActions?.IsPullRequest ?? false;

// ✅ Preferred - Modern null checking
return type?.IndexOf('>') > -1 && type.TrimEnd().EndsWith('>');
```

### Code Organization
- Use `this.` qualification for instance members (enforced by `.editorconfig`)
- Organize using statements with `System` directives first
- Use PascalCase for constants
- Follow the established namespace structure: `DendroDocs.Extensions` for extension methods

## Build System

### NUKE Build System
- **Use `nuke` to build the project** instead of direct `dotnet` commands when possible
- Available build targets: `Clean`, `Restore`, `Compile`, `UnitTests`, `CodeCoverage`, `Pack`, `Push`
- Default target is `Push` which runs the full CI pipeline

```bash
# ✅ Preferred - Use NUKE for building
./build.sh compile
./build.sh unittests  
./build.sh pack

# ✅ Available for development
dotnet build
dotnet test
```

### GitVersion Requirements
- **Always fetch complete git history** when working with GitVersion for proper versioning
- Use `fetch-depth: 0` in GitHub Actions checkout to ensure GitVersion has access to all tags and commits
- For local development, ensure you have the full repository history with `git fetch --unshallow` if needed

```yaml
# ✅ Required in GitHub Actions workflows
- uses: actions/checkout@v4
  with:
    fetch-depth: 0

# ✅ For local development if repository was shallow cloned
git fetch --unshallow
```

### Testing
- All changes must maintain or improve test coverage
- Use MSTest framework (`[TestClass]`, `[TestMethod]`, `[DataRow]`)
- Use Shouldly for assertions (`result.ShouldBe(expected)`)
- Follow the existing test naming pattern: `MethodName_Scenario_ExpectedBehavior`

```csharp
[TestMethod]
public void ExtensionMethod_NullInput_ShouldThrow()
{
    // Arrange
    string? input = null;
    
    // Act
    Action action = () => input.SomeExtension();
    
    // Assert
    action.ShouldThrow<ArgumentNullException>()
        .ParamName.ShouldBe("input");
}
```

## Git Workflow and Commits

### Commit History
- **Prefer a linear commit history** with only descriptive commits
- **Avoid "initial plan" or "work in progress" commits** in the final PR
- Use conventional commit format when possible: `type(scope): description`
- Each commit should represent a complete, working change

```bash
# ✅ Good commit messages
feat: add support for generic type detection in diagrams
fix: handle null reference in string extension methods
test: add comprehensive tests for inheritance resolution
docs: update API documentation for type description methods

# ❌ Avoid these commit messages
initial plan
wip
temp changes
trying something
```

### Pull Request Guidelines
- **Group changes logically over one or more commits** instead of squashing everything
- **Force push is allowed on feature branches** for history reorganization
- Ensure all tests pass before submitting
- Update documentation for public API changes
- Follow semantic versioning principles for breaking changes

## Project-Specific Patterns

### Extension Methods
- Place extension methods in the `DendroDocs.Extensions` namespace
- Always validate parameters with `ArgumentNullException.ThrowIfNull()`
- Return meaningful defaults for edge cases (e.g., empty collections instead of null)

### Type Analysis
- Use `StringComparison.Ordinal` for performance-critical string operations
- Handle generic types with proper parsing of angle brackets
- Consider inheritance hierarchies when analyzing type relationships

### Performance
- Prefer LINQ methods over manual loops where readability is maintained
- Use `StringBuilder` for string concatenation in loops
- Cache expensive operations when called repeatedly

## Documentation
- Use XML documentation comments for public APIs
- Include parameter validation details in documentation
- Provide usage examples for complex extension methods
- Keep README files concise but comprehensive

## Dependencies
- Minimize external dependencies
- Prefer .NET built-in functionality over third-party libraries
- Use NuGet package management through `Directory.Packages.props`
- Follow semantic versioning for package updates

---

These guidelines ensure consistent, high-quality contributions that align with the project's architecture and coding standards.