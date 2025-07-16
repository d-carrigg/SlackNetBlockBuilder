# SlackNetBlockBuilder Documentation

SlackNetBlockBuilder is a fluent builder extension for [SlackNet](https://github.com/SlackNet/SlackNet) that simplifies creating Slack Block Kit UI elements.

## Quick Start

```csharp
var blocks = BlockBuilder.Create()
    .AddHeader("Hello World")
    .AddSection("Some text content")
    .AddDivider()
    .AddActions(actions => actions
        .AddButton("approve", "Approve")
        .AddButton("reject", "Reject"))
    .Build();
```

## Files

- `getting-started.md` - Installation and basic usage
- `api/block-builder.md` - API reference
