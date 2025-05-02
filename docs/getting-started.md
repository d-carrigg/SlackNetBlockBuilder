# Getting Started with SlackNetBlockBuilder

SlackNetBlockBuilder is a fluent builder extension for [SlackNet](https://github.com/SlackNet/SlackNet) that simplifies creating Slack Block Kit UI elements with a clean, chainable API.

## Installation

Install the package via NuGet:

```bash
dotnet add package SlackNetBlockBuilder
```

Or using the Package Manager Console:

```powershell
Install-Package SlackNetBlockBuilder
```

## Basic Usage

### 1. Add the necessary using statements

```csharp
using SlackNet;
using SlackNet.Blocks;
// No additional using statement required as all builder classes are in the SlackNet.Blocks namespace
```

### 2. Create a simple message with blocks

```csharp
// Create a new block builder
var blocks = BlockBuilder.Create()
    .AddSection("Hello, world!")
    .AddDivider()
    .AddSection(section => section
        .Text("This is a section with some formatted text")
        .Accessory(button => button
            .Text("Click Me")
            .ActionId("button_click")
            .Primary()))
    .Build();

// Use with SlackNet to send a message
await slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});
```

### 3. Working with interactive elements

```csharp
var blocks = BlockBuilder.Create()
    .AddSection("Please select an option:")
    .AddActions(actions => actions
        .Button(button => button
            .Text("Approve")
            .ActionId("approve_button")
            .Primary())
        .Button(button => button
            .Text("Reject")
            .ActionId("reject_button")
            .Danger())
        .StaticSelect(select => select
            .Placeholder("Choose an option")
            .ActionId("select_option")
            .AddOption("Option 1", "value1")
            .AddOption("Option 2", "value2")
            .AddOption("Option 3", "value3")))
    .Build();
```

### 4. Creating forms with input elements

```csharp
var blocks = BlockBuilder.Create()
    .AddHeader("Submit Request Form")
    .AddInput<PlainTextInput>("Title", input => input
        .ActionId("title_input")
        .Placeholder("Enter a title")
        .Required())
    .AddInput<PlainTextInput>("Description", input => input
        .ActionId("description_input")
        .Multiline()
        .Placeholder("Enter a detailed description")
        .Optional())
    .AddInput<DatePicker>("Due Date", input => input
        .ActionId("due_date_input")
        .Placeholder("Select a date")
        .Optional())
    .AddActions(actions => actions
        .Button(button => button
            .Text("Submit")
            .ActionId("submit_form")
            .Primary())
        .Button(button => button
            .Text("Cancel")
            .ActionId("cancel_form")))
    .Build();
```

## Next Steps

- Explore the [API documentation](api/block-builder.md) for more details on available methods
- Check out the [examples](examples/) for common use cases
- Learn about [advanced features](advanced/) for more complex scenarios

## Common Patterns

### Builder Pattern

SlackNetBlockBuilder uses the builder pattern throughout, allowing for a fluent, chainable API:

```csharp
// Instead of this:
var section = new Section
{
    Text = new MrkdwnText { Text = "Hello, world!" },
    Accessory = new Button
    {
        Text = new PlainText { Text = "Click Me" },
        ActionId = "button_click",
        Style = ButtonStyle.Primary
    }
};

// Create a list of blocks and add the section
var blocks = new List<Block> { section };

// Send the message
await slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});

// You can write this:
var blocks = BlockBuilder.Create()
    .AddSection(section => section
        .Text("Hello, world!")
        .Accessory(button => button
            .Text("Click Me")
            .ActionId("button_click")
            .Primary()))
    .Build();

// Send the message
await slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});
```

### Updating Existing Messages

One of the powerful features is the ability to easily update existing messages:

```csharp
// When handling an interaction payload
public async Task HandleInteraction(InteractionPayload payload)
{
    // Create a new block builder from the existing blocks
    var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
        // Remove a specific block by ID
        .Remove("status_block")
        // Add a new status section
        .AddSection(section => section
            .BlockId("status_block")
            .Text("✅ Request approved!"))
        .Build();
    
    // Update the message
    await slackApi.Chat.Update(new MessageUpdate
    {
        Channel = payload.Channel.Id,
        Ts = payload.Message.Ts,
        Blocks = updatedBlocks
    });
}
```

### Context Blocks

Context blocks are useful for adding supplementary information:

```csharp
var blocks = BlockBuilder.Create()
    .AddSection("Main content goes here")
    .AddContext(context => context
        .AddText("Last updated: Today at 9:30 AM")
        .AddImage("https://example.com/icon.png", "Icon"))
    .Build();
```

### Rich Text Formatting

For more complex text formatting, use the rich text builder:

```csharp
var blocks = BlockBuilder.Create()
    .AddRichText(richText => richText
        .AddSection(section => section
            .AddText("This is ")
            .AddText("bold text", text => text.Bold())
            .AddText(" and ")
            .AddText("italic text", text => text.Italic())))
    .Build();
```

## Handling Interactions

When a user interacts with your message, you'll receive an interaction payload. Here's how to handle it:

```csharp
public async Task HandleInteraction(InteractionPayload payload)
{
    // Check the action ID to determine which element was interacted with
    switch (payload.Actions[0].ActionId)
    {
        case "approve_button":
            // Handle approval
            await HandleApproval(payload);
            break;
        
        case "reject_button":
            // Handle rejection
            await HandleRejection(payload);
            break;
        
        case "select_option":
            // Handle selection
            var selectedValue = payload.Actions[0].SelectedOption.Value;
            await HandleSelection(payload, selectedValue);
            break;
    }
}
```

## Best Practices

1. **Use Block IDs**: Assign block IDs to blocks that you might need to reference later, especially when updating messages.

2. **Use Action IDs**: Always set action IDs for interactive elements to identify them when handling interactions.

3. **Group Related Actions**: Use action blocks to group related actions together.

4. **Provide Feedback**: Update messages to provide feedback after user interactions.

5. **Keep It Simple**: Don't overcomplicate your messages with too many blocks or actions.

6. **Test Your Messages**: Always test your messages in Slack to ensure they appear as expected.