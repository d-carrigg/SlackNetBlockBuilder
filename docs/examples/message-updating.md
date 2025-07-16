# Message Updating Example

Example of updating an existing message after user interaction.

```csharp
// Original message
var blocks = BlockBuilder.Create()
    .AddSection("Please approve this request")
    .AddActions(actions => actions
        .AddButton("approve", "Approve")
        .AddButton("reject", "Reject"))
    .Build();

// Send message
var response = await slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});

// Update after button click
var updatedBlocks = BlockBuilder.From(response.Message.Blocks)
    .RemoveActions() // Remove buttons
    .AddSection("✅ Request approved!")
    .Build();

await slackApi.Chat.Update(new MessageUpdate
{
    Channel = channelId,
    Ts = response.Ts,
    Blocks = updatedBlocks
});
```
