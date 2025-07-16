# SlackNetBlockBuilder

A fluent builder extension for [SlackNet](https://github.com/SlackNet/SlackNet) that simplifies creating Slack Block Kit UI elements with a clean, chainable API.

> [!NOTE]
> This package is currently in beta. The code passes all tests, but breaking changes may occur until the v1 release.
> 
> 
## Installation

[![NuGet](https://img.shields.io/nuget/vpre/SlackNetBlockBuilder)](https://www.nuget.org/packages/SlackNetBlockBuilder/)

```bash
dotnet add package SlackNetBlockBuilder
```

## Quick Example

```csharp
using SlackNet.Blocks;

var blocks = BlockBuilder.Create()
    .AddSection("Hello, world!")
    .AddDivider()
    .AddSection(section => section
        .Text("This is a section with formatted text")
        .Accessory(button => button
            .Text("Click Me")
            .ActionId("button_click")
            .Primary()))
    .Build();
```

## Key Features

- **Fluent API**: Chain methods for easy block creation
- **Validation**: Runtime validation against Slack's Block Kit guidelines
- **Update Support**: Easily modify existing blocks with `BlockBuilder.From()`
- **Complete Coverage**: Support for all Slack Block Kit elements

## Common Use Cases

### Interactive Forms
Create input forms with validation and actions:

```csharp
var blocks = BlockBuilder.Create()
    .AddHeader("Submit Request")
    .AddInput<PlainTextInput>("Title", input => input
        .ActionId("title_input")
        .Placeholder("Enter a title")
        .MaxLength(100))
    .AddActions(actions => actions
        .Button(button => button
            .Text("Submit")
            .ActionId("submit_form")
            .Primary()))
    .Build();
```

### Message Updates
Modify existing messages easily:

```csharp
var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
    .Remove("status_block")
    .AddSection(section => section
        .BlockId("status_block")
        .Text("✅ Request approved!"))
    .Build();
```

## Getting Started

1. [Installation & Setup](docs/getting-started.md)
2. [API Reference](/api-reference)
3. [Examples](docs/examples)

