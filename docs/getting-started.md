# Getting Started with SlackNetBlockBuilder

SlackNetBlockBuilder is a fluent builder extension for [SlackNet](https://github.com/SlackNet/SlackNet) that simplifies creating Slack Block Kit UI elements.

## Installation

```bash
dotnet add package SlackNetBlockBuilder
```

## Basic Usage

```csharp
using SlackNet;
using SlackNet.Blocks;

// Create blocks
var blocks = BlockBuilder.Create()
    .AddHeader("Hello World")
    .AddSection("Some text content")
    .AddDivider()
    .AddActions(actions => actions
        .AddButton("approve", "Approve")
        .AddButton("reject", "Reject"))
    .Build();

// Use with SlackNet
await slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});
```

## Forms

```csharp
var blocks = BlockBuilder.Create()
    .AddHeader("Request Form")
    .AddInput<PlainTextInput>("Title", input => input
        .ActionId("title_input")
        .Placeholder("Enter title")
        .Required())
    .AddInput<PlainTextInput>("Description", input => input
        .ActionId("description_input")
        .Multiline()
        .Optional())
    .AddActions(actions => actions
        .AddButton("submit", "Submit")
        .AddButton("cancel", "Cancel"))
    .Build();
```

## Updating Messages

```csharp
// Update existing message
var updatedBlocks = BlockBuilder.From(existingMessage.Blocks)
    .AddSection("✅ Updated!")
    .Build();

await slackApi.Chat.Update(new MessageUpdate
{
    Channel = channelId,
    Ts = messageTs,
    Blocks = updatedBlocks
});
```
