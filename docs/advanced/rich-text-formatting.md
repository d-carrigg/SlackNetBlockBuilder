# Advanced Rich Text Formatting

SlackNetBlockBuilder provides powerful capabilities for creating rich text content in your Slack messages. This guide covers advanced techniques for formatting text beyond basic Markdown.

## Rich Text Blocks

The `RichTextBuilder` class allows you to create complex text layouts with multiple formatting options.

### Basic Rich Text Example

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddSection(section => section
            .AddText("This is ")
            .AddText("bold text", text => text.Bold())
            .AddText(" and ")
            .AddText("italic text", text => text.Italic())
            .AddText(" and ")
            .AddText("strikethrough", text => text.Strike())
            .AddText(" and ")
            .AddText("code", text => text.Code())))
    .Build();
```

### Combining Text Styles

You can combine multiple text styles for more complex formatting:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddSection(section => section
            .AddText("Important: ", text => text.Bold())
            .AddText("This deadline ", text => text.Italic())
            .AddText("cannot be extended", text => text.Bold().Italic().Color("#FF0000"))))
    .Build();
```

### Links and Mentions

You can add links and mentions to users, channels, and user groups:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddSection(section => section
            .AddText("Please check ")
            .AddLink("this document", "https://example.com/document")
            .AddText(" and notify ")
            .AddUserMention("U01234567")
            .AddText(" in ")
            .AddChannelMention("C01234567")))
    .Build();
```

### Lists

You can create ordered and unordered lists:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddSection(section => section
            .AddText("Project Tasks:"))
        .AddUnorderedList(list => list
            .AddItem(item => item.AddText("Research requirements"))
            .AddItem(item => item.AddText("Create design mockups"))
            .AddItem(item => item.AddText("Implement solution"))
            .AddItem(item => item.AddText("Test and deploy")))
        .AddSection(section => section
            .AddText("Project Timeline:"))
        .AddOrderedList(list => list
            .AddItem(item => item.AddText("Week 1: Planning"))
            .AddItem(item => item.AddText("Week 2: Development"))
            .AddItem(item => item.AddText("Week 3: Testing"))
            .AddItem(item => item.AddText("Week 4: Deployment"))))
    .Build();
```

### Quotes and Code Blocks

You can add quotes and code blocks for special formatting:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddQuote(quote => quote
            .AddText("The best way to predict the future is to invent it."))
        .AddSection(section => section
            .AddText("Example code:"))
        .AddCodeBlock(code => code
            .Language("csharp")
            .AddText("var blocks = BlockBuilder.Create()\n    .AddSection(\"Hello, world!\")\n    .Build();")))
    .Build();
```

## Advanced Layouts

### Multi-Column Layout

You can create multi-column layouts using rich text sections:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddColumns(columns => columns
            .AddColumn(column => column
                .AddSection(section => section
                    .AddText("Left Column Header", text => text.Bold()))
                .AddUnorderedList(list => list
                    .AddItem(item => item.AddText("Item 1"))
                    .AddItem(item => item.AddText("Item 2"))
                    .AddItem(item => item.AddText("Item 3"))))
            .AddColumn(column => column
                .AddSection(section => section
                    .AddText("Right Column Header", text => text.Bold()))
                .AddUnorderedList(list => list
                    .AddItem(item => item.AddText("Item A"))
                    .AddItem(item => item.AddText("Item B"))
                    .AddItem(item => item.AddText("Item C"))))))
    .Build();
```

### Tables

You can create table-like structures using rich text:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddSection(section => section
            .AddText("Project Status Report", text => text.Bold().Size(RichTextSize.Large)))
        .AddColumns(columns => columns
            .AddColumn(column => column
                .AddSection(section => section
                    .AddText("Task", text => text.Bold()))
                .AddSection(section => section
                    .AddText("Research"))
                .AddSection(section => section
                    .AddText("Design"))
                .AddSection(section => section
                    .AddText("Development"))
                .AddSection(section => section
                    .AddText("Testing")))
            .AddColumn(column => column
                .AddSection(section => section
                    .AddText("Status", text => text.Bold()))
                .AddSection(section => section
                    .AddText("Complete", text => text.Color("#00AA00")))
                .AddSection(section => section
                    .AddText("Complete", text => text.Color("#00AA00")))
                .AddSection(section => section
                    .AddText("In Progress", text => text.Color("#AAAA00")))
                .AddSection(section => section
                    .AddText("Not Started", text => text.Color("#AA0000"))))
            .AddColumn(column => column
                .AddSection(section => section
                    .AddText("Assigned To", text => text.Bold()))
                .AddSection(section => section
                    .AddUserMention("U01234567"))
                .AddSection(section => section
                    .AddUserMention("U02345678"))
                .AddSection(section => section
                    .AddUserMention("U03456789"))
                .AddSection(section => section
                    .AddUserMention("U04567890")))))
    .Build();
```

## Best Practices

### 1. Keep It Readable

While rich formatting can enhance your messages, too much formatting can make them harder to read. Use formatting judiciously to highlight important information.

### 2. Consistent Styling

Maintain consistent styling throughout your application for a professional look and feel. Consider creating helper methods for common formatting patterns.

### 3. Accessibility

Remember that some formatting may not be accessible to all users, especially those using screen readers. Provide alternative text where appropriate.

### 4. Testing

Always test your rich text formatting in Slack to ensure it appears as expected. Different clients (desktop, mobile, web) may render formatting slightly differently.

### 5. Performance

Complex rich text structures can be more resource-intensive to render. For very large messages, consider breaking them into multiple, simpler messages.

## Helper Methods

You can create helper methods to standardize your rich text formatting:

```csharp
public static class RichTextHelpers
{
    public static RichTextSectionBuilder AddSuccessMessage(this RichTextSectionBuilder builder, string message)
    {
        return builder
            .AddText("✅ ", text => text.Bold())
            .AddText(message, text => text.Bold().Color("#00AA00"));
    }
    
    public static RichTextSectionBuilder AddErrorMessage(this RichTextSectionBuilder builder, string message)
    {
        return builder
            .AddText("❌ ", text => text.Bold())
            .AddText(message, text => text.Bold().Color("#AA0000"));
    }
    
    public static RichTextSectionBuilder AddWarningMessage(this RichTextSectionBuilder builder, string message)
    {
        return builder
            .AddText("⚠️ ", text => text.Bold())
            .AddText(message, text => text.Bold().Color("#AAAA00"));
    }
}
```

Usage:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddSection(section => section
            .AddSuccessMessage("Operation completed successfully"))
        .AddSection(section => section
            .AddWarningMessage("Some items could not be processed"))
        .AddSection(section => section
            .AddErrorMessage("Failed to connect to the server")))
    .Build();
``` 