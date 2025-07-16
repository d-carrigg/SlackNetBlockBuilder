# Interactive Message Handling

Example of handling button clicks and other interactions.

```csharp
public async Task HandleInteraction(InteractionPayload payload)
{
    switch (payload.Actions[0].ActionId)
    {
        case "approve":
            await HandleApproval(payload);
            break;
        case "reject":
            await HandleRejection(payload);
            break;
    }
}

private async Task HandleApproval(InteractionPayload payload)
{
    var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
        .RemoveActions() // remove the submit and cancel buttons
        .AddSection("✅ Approved!")
        .Build();

    await slackApi.Chat.Update(new MessageUpdate
    {
        Channel = payload.Channel.Id,
        Ts = payload.Message.Ts,
        Blocks = updatedBlocks
    });
}
```
