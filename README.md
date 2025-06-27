# DendroDocs.Client

[![NuGet][NUGET_BADGE]][NUGET_FEED] [![Coverage Status][COVERALLS_BADGE]][COVERALLS_LINK]

**DendroDocs.Client** is a client library for the DendroDocs ecosystem that provides
powerful extension methods for working with code analysis data.
This library enables developers to manipulate and query code structure information,
making it easier to work with TypeDescription, MethodDescription, InvocationDescription,
and other code analysis models used in living documentation generation.

## Features

* **Type Analysis Extensions** - Query and manipulate TypeDescription collections
  with methods for inheritance resolution, member population, and type lookups
* **Method Analysis Extensions** - Filter and analyze MethodDescription collections
  by name and other criteria
* **Invocation Analysis Extensions** - Match method invocations, trace call consequences,
  and analyze method parameters
* **String Utilities** - Helper methods for working with type names, generic types,
  and formatting for diagrams
* **Attribute Extensions** - Filter and query attribute descriptions and their
  relationships
* **Inheritance Support** - Automatically populate inherited base types and members
  from type hierarchies

## Prerequisites

.NET 8.0 SDK or newer.

## Installation

To use **DendroDocs.Client** in your project, install it as a NuGet package:

```shell
dotnet add package DendroDocs.Client
```

## Example Usage

```csharp
using DendroDocs.Extensions;

// Working with type collections
var types = new List<TypeDescription>();
// ... populate types from analysis

// Find a specific type
var myType = types.FirstOrDefault("MyNamespace.MyClass");

// Populate inherited members
types.PopulateInheritedMembers();

// Working with method invocations
var invocation = new InvocationDescription();
var matchingMethods = types.GetInvokedMethod(invocation);
var consequences = types.GetInvocationConsequences(invocation);

// String formatting utilities
var typeName = "System.Collections.Generic.List<System.String>";
var diagramName = typeName.ForDiagram(); // "List<String>"
var isGeneric = typeName.IsGeneric(); // true
var genericTypes = typeName.GenericTypes(); // ["System.String"]

// Method filtering
var methods = new List<MethodDescription>();
var namedMethods = methods.WithName("MyMethod");
```

## The DendroDocs Ecosystem

**DendroDocs.Client** is a key component of the broader DendroDocs ecosystem.
It works together with:

* [DendroDocs.Tool](https://github.com/dendrodocs/dotnet-tool) - Command-line
  analyzer for generating JSON from .NET projects
* [DendroDocs.Shared](https://github.com/dendrodocs/dotnet-shared-lib) - Shared
  library with common utilities and models

Explore [DendroDocs](https://github.com/dendrodocs) to find more tools, libraries,
and documentation resources that help you bridge the gap between your code and its
documentation.

## Contributing

Contributions are welcome! Please feel free to create
[issues](https://github.com/dendrodocs/dotnet-client-lib/issues) or
[pull requests](https://github.com/dendrodocs/dotnet-client-lib/pulls).

## License

This project is licensed under the [MIT License](./LICENSE).

[NUGET_BADGE]: https://img.shields.io/nuget/v/DendroDocs.Client.svg?style=plastic
[NUGET_FEED]: https://www.nuget.org/packages/DendroDocs.Client/
[COVERALLS_BADGE]: https://coveralls.io/repos/github/dendrodocs/dotnet-client-lib/badge.svg?branch=main
[COVERALLS_LINK]: https://coveralls.io/github/dendrodocs/dotnet-client-lib?branch=main
