# SlackNetBlockBuilder

A fluent builder extension for [SlackNet](https://github.com/SlackNet/SlackNet) that simplifies creating Slack Block Kit UI elements with a clean, chainable API.

## Installation

Install the package via NuGet:

```bash
dotnet add package SlackNetBlockBuilder
```

## Getting Started

SlackNetBlockBuilder provides a fluent interface for building Slack Block Kit UIs. It extends the SlackNet library to make creating complex Slack messages more intuitive and maintainable.

### Simple Message Creation

Create a basic message with text sections and a divider:

```csharp
using SlackNet;
using SlackNet.Blocks;

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

### Interactive Actions

Add interactive elements like buttons and select menus:

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

### Forms with Input Elements

Create forms with various input elements:

```csharp
var blocks = BlockBuilder.Create()
    .AddHeader(new PlainText("Submit Request Form"))
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

### Updating Messages

One of the powerful features of SlackNetBlockBuilder is the ability to easily update existing messages:

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

## Advanced Features

- **Block IDs and Action IDs**: Easily set IDs for blocks and actions for interaction handling
- **Conditional Elements**: Add or remove blocks based on conditions
- **Rich Text Formatting**: Create complex text layouts with rich text blocks
- **Context Blocks**: Add contextual information with mixed text and images
- **Input Validation**: Set validation rules for input elements

## License

This project is licensed under the MIT License - see the LICENSE file for details.
