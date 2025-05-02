# Message Updating Examples

This guide demonstrates various techniques for updating Slack messages using SlackNetBlockBuilder.

## Basic Message Update

The simplest way to update a message is to create a new set of blocks and use the `Chat.Update` method:

```csharp
using SlackNet;
using SlackNet.Blocks;
using SlackNet.WebApi;

public class MessageUpdater
{
    private readonly ISlackApiClient _slackApi;
    
    public MessageUpdater(ISlackApiClient slackApi)
    {
        _slackApi = slackApi;
    }
    
    public async Task UpdateMessage(string channelId, string messageTs, string newStatus)
    {
        // Create new blocks for the updated message
        var updatedBlocks = BlockBuilder.Create()
            .AddHeader("Status Update")
            .AddSection($"Current status: *{newStatus}*")
            .AddContext(context => context
                .AddText($"Last updated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"))
            .Build();
        
        // Update the message
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = channelId,
            Ts = messageTs,
            Blocks = updatedBlocks
        });
    }
}
```

## Updating from Existing Blocks

A more common scenario is updating an existing message while preserving some of its content. SlackNetBlockBuilder makes this easy with the `From` method:

```csharp
public async Task HandleStatusUpdate(InteractionPayload payload)
{
    // Get the selected status from the payload
    var newStatus = payload.Actions[0].SelectedOption.Value;
    
    // Create a new block builder from the existing blocks
    var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
        // Remove the old status block
        .Remove("status_block")
        // Add a new status block
        .AddSection(section => section
            .BlockId("status_block")
            .Text($"Current status: *{newStatus}*"))
        // Update the timestamp in the context block
        .Remove("timestamp_block")
        .AddContext(context => context
            .BlockId("timestamp_block")
            .AddText($"Last updated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"))
        .Build();
    
    // Update the message
    await _slackApi.Chat.Update(new MessageUpdate
    {
        Channel = payload.Channel.Id,
        Ts = payload.Message.Ts,
        Blocks = updatedBlocks
    });
}
```

## Progressive Updates

You can implement a multi-step workflow by progressively updating a message:

```csharp
public async Task StartApprovalWorkflow(string channelId)
{
    // Step 1: Create the initial message
    var initialBlocks = BlockBuilder.Create()
        .AddHeader("Approval Request")
        .AddSection("A new request requires your approval.")
        .AddSection(section => section
            .BlockId("status_block")
            .Text("Status: *Pending*"))
        .AddActions(actions => actions
            .BlockId("action_block")
            .Button(button => button
                .Text("Approve")
                .ActionId("approve_button")
                .Primary())
            .Button(button => button
                .Text("Reject")
                .ActionId("reject_button")
                .Danger()))
        .Build();
    
    // Send the initial message
    var response = await _slackApi.Chat.PostMessage(new Message
    {
        Channel = channelId,
        Blocks = initialBlocks
    });
    
    // Store the message timestamp for later updates
    string messageTs = response.Ts;
}

public async Task HandleApprovalAction(InteractionPayload payload)
{
    string action = payload.Actions[0].ActionId;
    string newStatus;
    string emoji;
    
    if (action == "approve_button")
    {
        newStatus = "Approved";
        emoji = "✅";
    }
    else if (action == "reject_button")
    {
        newStatus = "Rejected";
        emoji = "❌";
    }
    else
    {
        return; // Unknown action
    }
    
    // Step 2: Update the message to show the action taken
    var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
        // Update the status block
        .Remove("status_block")
        .AddSection(section => section
            .BlockId("status_block")
            .Text($"Status: *{newStatus}* {emoji}"))
        // Remove the action buttons
        .Remove("action_block")
        // Add information about who took the action
        .AddContext(context => context
            .BlockId("action_info_block")
            .AddText($"Action taken by <@{payload.User.Id}> at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"))
        .Build();
    
    // Update the message
    await _slackApi.Chat.Update(new MessageUpdate
    {
        Channel = payload.Channel.Id,
        Ts = payload.Message.Ts,
        Blocks = updatedBlocks
    });
    
    // Step 3: If approved, update with next steps
    if (action == "approve_button")
    {
        // Wait a moment to show the approval state
        await Task.Delay(2000);
        
        var finalBlocks = BlockBuilder.From(updatedBlocks)
            .AddSection(section => section
                .BlockId("next_steps_block")
                .Text("📋 *Next Steps:*\n• Request has been sent for processing\n• You will be notified when complete"))
            .Build();
        
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = finalBlocks
        });
    }
}
```

## Ephemeral Updates

Sometimes you want to update a message only for a specific user. You can use ephemeral messages for this:

```csharp
public async Task ShowUserSpecificUpdate(InteractionPayload payload)
{
    // Update the original message for everyone
    await _slackApi.Chat.Update(new MessageUpdate
    {
        Channel = payload.Channel.Id,
        Ts = payload.Message.Ts,
        Blocks = BlockBuilder.From(payload.Message.Blocks)
            .Remove("status_block")
            .AddSection(section => section
                .BlockId("status_block")
                .Text("Status: *In Progress*"))
            .Build()
    });
    
    // Send an ephemeral message only to the user who clicked the button
    await _slackApi.Chat.PostEphemeral(new Message
    {
        Channel = payload.Channel.Id,
        User = payload.User.Id,
        Blocks = BlockBuilder.Create()
            .AddSection("You've started processing this request. Here are some additional details only visible to you:")
            .AddSection("• The request will be automatically closed after 24 hours if not completed\n• You can reassign this request if needed")
            .AddActions(actions => actions
                .Button(button => button
                    .Text("Complete")
                    .ActionId("complete_button")
                    .Primary())
                .Button(button => button
                    .Text("Reassign")
                    .ActionId("reassign_button")))
            .Build()
    });
}
```

## Handling Race Conditions

When multiple users might update the same message, you need to handle potential race conditions:

```csharp
public async Task SafeMessageUpdate(InteractionPayload payload)
{
    try
    {
        // Get the latest version of the message
        var messageInfo = await _slackApi.Chat.GetPermalink(new GetPermalinkRequest
        {
            Channel = payload.Channel.Id,
            MessageTs = payload.Message.Ts
        });
        
        // Use the permalink to get the latest message content
        // (This is a simplified example - in practice, you might need to use conversations.history)
        
        // Update based on the latest version
        var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
            // Make your updates here
            .Build();
        
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = updatedBlocks
        });
    }
    catch (SlackApiException ex) when (ex.Message.Contains("message_not_found"))
    {
        // Handle the case where the message was deleted or is no longer available
        await _slackApi.Chat.PostEphemeral(new Message
        {
            Channel = payload.Channel.Id,
            User = payload.User.Id,
            Text = "The message you're trying to update is no longer available."
        });
    }
    catch (SlackApiException ex)
    {
        // Handle other API errors
        Console.WriteLine($"Error updating message: {ex.Message}");
    }
}
```

## Best Practices for Message Updates

1. **Use Block IDs**: Always assign block IDs to blocks that you might need to update or remove later.

2. **Preserve Context**: When updating a message, try to preserve the context so users understand what changed.

3. **Show Progress**: For multi-step workflows, update the message to show progress through the steps.

4. **Handle Errors**: Always handle potential errors when updating messages.

5. **Consider Timing**: Be mindful of the timing of updates, especially for multi-step workflows.

6. **Provide Feedback**: Always provide clear feedback about what changed and why.

7. **Test Concurrency**: Test scenarios where multiple users might try to update the same message simultaneously. 