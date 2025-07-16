# Rich Text Blocks

The `RichTextBuilder` provides basic rich text formatting capabilities.

## Basic Usage

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .BlockId("rich_text_block")
        .AddSection(section => section
            .AddText("Hello ")
            .AddText("world", text => text.Bold())
            .AddText("!")))
    .Build();
```

## Available Methods

- `AddSection()` - Add a text section
- `AddList()` - Add a list (ordered/unordered)
- `AddQuote()` - Add a quote block
- `AddPreformatted()` - Add preformatted text

See SlackNet documentation for full rich text capabilities.
