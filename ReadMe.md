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

#### With SlackNetBlockBuilder:

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

#### Without SlackNetBlockBuilder (Traditional Approach):

```csharp
using SlackNet;
using SlackNet.WebApi;
using SlackNet.Blocks;

// Create blocks manually
var blocks = new List<Block>
{
    new SectionBlock
    {
        Text = new PlainText { Text = "Hello, world!" }
    },
    new DividerBlock(),
    new SectionBlock
    {
        Text = new PlainText { Text = "This is a section with some formatted text" },
        Accessory = new Button
        {
            Text = new PlainText { Text = "Click Me" },
            ActionId = "button_click",
            Style = ButtonStyle.Primary
        }
    }
};

// Use with SlackNet to send a message
await slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});
```

### Interactive Actions

#### With SlackNetBlockBuilder:

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

#### Without SlackNetBlockBuilder (Traditional Approach):

```csharp
var blocks = new List<Block>
{
    new SectionBlock
    {
        Text = new PlainText { Text = "Please select an option:" }
    },
    new ActionsBlock
    {
        Elements = new List<IActionElement>
        {
            new Button
            {
                Text = new PlainText { Text = "Approve" },
                ActionId = "approve_button",
                Style = ButtonStyle.Primary
            },
            new Button
            {
                Text = new PlainText { Text = "Reject" },
                ActionId = "reject_button",
                Style = ButtonStyle.Danger
            },
            new StaticSelectMenu
            {
                Placeholder = new PlainText { Text = "Choose an option" },
                ActionId = "select_option",
                Options = new List<Option>
                {
                    new Option
                    {
                        Text = new PlainText { Text = "Option 1" },
                        Value = "value1"
                    },
                    new Option
                    {
                        Text = new PlainText { Text = "Option 2" },
                        Value = "value2"
                    },
                    new Option
                    {
                        Text = new PlainText { Text = "Option 3" },
                        Value = "value3"
                    }
                }
            }
        }
    }
};
```

### Forms with Input Elements

#### With SlackNetBlockBuilder:

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

#### Without SlackNetBlockBuilder (Traditional Approach):

```csharp
var blocks = new List<Block>
{
    new HeaderBlock
    {
        Text = new PlainText { Text = "Submit Request Form" }
    },
    new InputBlock
    {
        Label = new PlainText { Text = "Title" },
        Element = new PlainTextInput
        {
            ActionId = "title_input",
            Placeholder = new PlainText { Text = "Enter a title" }
        },
        Optional = false
    },
    new InputBlock
    {
        Label = new PlainText { Text = "Description" },
        Element = new PlainTextInput
        {
            ActionId = "description_input",
            Multiline = true,
            Placeholder = new PlainText { Text = "Enter a detailed description" }
        },
        Optional = true
    },
    new InputBlock
    {
        Label = new PlainText { Text = "Due Date" },
        Element = new DatePicker
        {
            ActionId = "due_date_input",
            Placeholder = new PlainText { Text = "Select a date" }
        },
        Optional = true
    },
    new ActionsBlock
    {
        Elements = new List<IActionElement>
        {
            new Button
            {
                Text = new PlainText { Text = "Submit" },
                ActionId = "submit_form",
                Style = ButtonStyle.Primary
            },
            new Button
            {
                Text = new PlainText { Text = "Cancel" },
                ActionId = "cancel_form"
            }
        }
    }
};
```

### Updating Messages

One of the powerful features of SlackNetBlockBuilder is the ability to easily update existing messages:

#### With SlackNetBlockBuilder:

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

#### Without SlackNetBlockBuilder (Traditional Approach):

```csharp
// When handling an interaction payload
public async Task HandleInteraction(InteractionPayload payload)
{
    // Get the existing blocks
    var existingBlocks = payload.Message.Blocks;
    
    // Create a new list of blocks
    var updatedBlocks = new List<Block>();
    
    // Copy all blocks except the status block
    foreach (var block in existingBlocks)
    {
        if (block.BlockId != "status_block")
        {
            updatedBlocks.Add(block);
        }
    }
    
    // Add a new status section
    updatedBlocks.Add(new SectionBlock
    {
        BlockId = "status_block",
        Text = new PlainText { Text = "✅ Request approved!" }
    });
    
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

## Performance

SlackNetBlockBuilder is designed to be highly performant. Our performance tests show that builder operations take less than 0.02ms per operation, ensuring that your application remains responsive even when building complex UIs.

## Why Use SlackNetBlockBuilder?

As you can see from the examples above, SlackNetBlockBuilder significantly reduces the amount of code needed to create Slack Block Kit UIs. The traditional approach requires:

- More lines of code
- Deeper nesting of objects
- Manual creation of all objects and properties
- More complex code for updating existing messages

With SlackNetBlockBuilder, you get:

- Clean, fluent API
- Chainable methods for building complex UIs
- Simplified updating of existing messages
- Reduced boilerplate code
- Better readability and maintainability

## License

This project is licensed under the MIT License - see the LICENSE file for details.
