# SlackNetBlockBuilder

A fluent builder extension for [SlackNet](https://github.com/SlackNet/SlackNet) that simplifies creating Slack Block Kit
UI elements with a clean, chainable API.

*This project is not affiliated with Slack or SlackNet.*

## Installation

[![NuGet](https://img.shields.io/nuget/vpre/SlackNetBlockBuilder)](https://www.nuget.org/packages/SlackNetBlockBuilder/)


Install the package via NuGet:

```bash
dotnet add package SlackNetBlockBuilder
```

Visual Studio
```
Install-Package SlackNetBlockBuilder
```


## Getting Started

Here are some examples to help you get started with SlackNetBlockBuilder:

### Simple Message

```csharp
using SlackNet;
using SlackNet.Blocks;
using SlackNet.WebApi;

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

### Updating Messages

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

## Comparison with Traditional Approach

The following examples show how SlackNetBlockBuilder compares to the traditional approach of creating blocks manually.

### Simple Message Creation

#### With SlackNetBlockBuilder:

```csharp
using SlackNet;
using SlackNet.Blocks;
using SlackNet.WebApi;

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

With simple messages, the difference in terms of lines of code is negligible, but you will find that creating a
message is easier and faster to type. Here is the same example as above without the BlockBuilder:

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

## Validation

The library includes validation to ensure that the blocks you create are valid according to Slack's Block Kit
guidelines.
This was added because I got sick of getting 400 errors from Slack, and then having to manually check the blocks to see
what was wrong.
Right now, validation throws InvalidOperationException, but a future release will use a dedicated exception type for
validation errors.

Example:

```csharp
var blocks = new BlockBuilder()
    // throws InvalidOperationException
    // Each field in a section block can have at most 3000 characters
    .AddSection("A string with more than 3000 characters") 
    .Build(); 

```

Constraints on blocks are referenced from [Slack's Block Kit documentation](https://api.slack.com/reference/block-kit).

## Further Examples

I'm working on adding examples to provide a more comprehensive overview of the library's capabilities. Generally
speaking
anything that can be done with the SlackNet API Blocks can be done with this library, so if you have any specific use
cases in mind,
you can follow these guidelines:

1. Remove blocks with the `Remove` method, set block properties with the `Set` method, and add new blocks with the `Add` methods.
   There are many built in extensions for adding blocks (e.g `AddSection`, `AddHeader`, `AddActions`, etc.) but you can
   also use the generic `Add` method
   if you need greater control.
   ```csharp
    builder.Remove("block_id")
        .Modify("block_id", block => block
            .Text("New Text"))
        .AddSection(section => section
            .Text("New Section"));
   ```
2. Complex blocks that have child elements take a lambda function as a parameter to configure the block.
   ```csharp
    builder.AddActions(actions => actions
        .Button(button => button
            .Text("Click Me")
            .ActionId("button_click")
            .Primary())
        .Button(button => button
            .Text("Cancel")
            .ActionId("cancel_button")));
   ```

## License

This project is licensed under the MIT License - see the LICENSE file for details.
