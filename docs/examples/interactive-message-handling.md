# Interactive Message Handling

This guide demonstrates how to handle interactions with Slack messages created using SlackNetBlockBuilder.

## Setting Up an Interaction Handler

First, you need to set up a handler for Slack interactions. This typically involves creating an endpoint in your application that receives POST requests from Slack:

```csharp
using Microsoft.AspNetCore.Mvc;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using System.Threading.Tasks;

[ApiController]
[Route("api/slack")]
public class SlackController : ControllerBase
{
    private readonly ISlackApiClient _slackApi;
    private readonly IInteractionHandler _interactionHandler;
    
    public SlackController(ISlackApiClient slackApi, IInteractionHandler interactionHandler)
    {
        _slackApi = slackApi;
        _interactionHandler = interactionHandler;
    }
    
    [HttpPost("interactions")]
    public async Task<IActionResult> HandleInteraction()
    {
        // Read the request body
        using var reader = new StreamReader(Request.Body);
        var requestBody = await reader.ReadToEndAsync();
        
        // Parse the payload
        var payload = System.Text.Json.JsonSerializer.Deserialize<InteractionPayload>(requestBody);
        
        // Handle the interaction
        await _interactionHandler.Handle(payload);
        
        // Return a 200 OK response
        return Ok();
    }
}
```

## Creating an Interaction Handler

Next, create a class to handle different types of interactions:

```csharp
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using System;
using System.Threading.Tasks;

public class InteractionHandler : IInteractionHandler
{
    private readonly ISlackApiClient _slackApi;
    
    public InteractionHandler(ISlackApiClient slackApi)
    {
        _slackApi = slackApi;
    }
    
    public async Task Handle(InteractionPayload payload)
    {
        // Determine the type of interaction
        switch (payload.Type)
        {
            case "block_actions":
                await HandleBlockActions(payload);
                break;
                
            case "view_submission":
                await HandleViewSubmission(payload);
                break;
                
            case "view_closed":
                await HandleViewClosed(payload);
                break;
                
            default:
                Console.WriteLine($"Unhandled interaction type: {payload.Type}");
                break;
        }
    }
    
    private async Task HandleBlockActions(InteractionPayload payload)
    {
        // Get the first action (there could be multiple)
        var action = payload.Actions[0];
        
        // Handle different action types
        switch (action.ActionId)
        {
            case "approve_button":
                await HandleApproval(payload);
                break;
                
            case "reject_button":
                await HandleRejection(payload);
                break;
                
            case "select_option":
                await HandleSelection(payload);
                break;
                
            default:
                Console.WriteLine($"Unhandled action ID: {action.ActionId}");
                break;
        }
    }
    
    private async Task HandleViewSubmission(InteractionPayload payload)
    {
        // Handle form submissions
        switch (payload.View.CallbackId)
        {
            case "task_creation_modal":
                await HandleTaskCreation(payload);
                break;
                
            default:
                Console.WriteLine($"Unhandled view callback ID: {payload.View.CallbackId}");
                break;
        }
    }
    
    private async Task HandleViewClosed(InteractionPayload payload)
    {
        // Handle modal closed events
        Console.WriteLine($"Modal closed: {payload.View.CallbackId}");
    }
    
    // Implement specific handlers for each interaction
    private async Task HandleApproval(InteractionPayload payload)
    {
        // Update the message to show approval
        var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
            .Remove("status_block")
            .AddSection(section => section
                .BlockId("status_block")
                .Text("Status: *Approved* ✅"))
            .Remove("action_block")
            .AddContext(context => context
                .BlockId("action_info_block")
                .AddText($"Approved by <@{payload.User.Id}> at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"))
            .Build();
        
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = updatedBlocks
        });
    }
    
    private async Task HandleRejection(InteractionPayload payload)
    {
        // Update the message to show rejection
        var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
            .Remove("status_block")
            .AddSection(section => section
                .BlockId("status_block")
                .Text("Status: *Rejected* ❌"))
            .Remove("action_block")
            .AddContext(context => context
                .BlockId("action_info_block")
                .AddText($"Rejected by <@{payload.User.Id}> at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"))
            .Build();
        
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = updatedBlocks
        });
    }
    
    private async Task HandleSelection(InteractionPayload payload)
    {
        // Get the selected option
        var selectedOption = payload.Actions[0].SelectedOption;
        
        // Update the message to show the selection
        var updatedBlocks = BlockBuilder.From(payload.Message.Blocks)
            .Remove("selection_block")
            .AddSection(section => section
                .BlockId("selection_block")
                .Text($"Selected option: *{selectedOption.Text.Text}*"))
            .Build();
        
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = updatedBlocks
        });
    }
    
    private async Task HandleTaskCreation(InteractionPayload payload)
    {
        try
        {
            // Extract values from the submission
            var state = payload.View.State.Values;
            
            var title = state["title_block"]["title_input"].Value;
            var description = state["description_block"]["description_input"].Value;
            var priority = state["priority_block"]["priority_select"].SelectedOption.Value;
            var dueDate = state["due_date_block"]["due_date_input"].SelectedDate;
            var assignee = state["assignee_block"]["assignee_select"].SelectedUser;
            
            // Process the task creation
            Console.WriteLine($"Creating task: {title}");
            
            // Send a message to the channel
            await _slackApi.Chat.PostMessage(new Message
            {
                Channel = payload.User.Id, // Send a DM to the user
                Blocks = BlockBuilder.Create()
                    .AddHeader("Task Created")
                    .AddSection($"*Title:* {title}")
                    .AddSection($"*Description:* {description}")
                    .AddContext(context => context
                        .AddText($"Priority: {priority}")
                        .AddText($"Due Date: {dueDate?.ToString("yyyy-MM-dd") ?? "Not specified"}"))
                    .Build()
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling task creation: {ex.Message}");
        }
    }
}
```

## Registering the Interaction Handler

Register your interaction handler in your application's startup code:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register SlackNet services
    services.AddSlackNet(options =>
    {
        options.UseApiToken("YOUR_BOT_TOKEN");
        options.UseSigningSecret("YOUR_SIGNING_SECRET");
    });
    
    // Register your interaction handler
    services.AddSingleton<IInteractionHandler, InteractionHandler>();
    
    // Other service registrations...
}
```

## Creating Interactive Messages

Now you can create interactive messages that users can interact with:

```csharp
public async Task SendApprovalRequest(string channelId, string requestId, string requestDetails)
{
    var blocks = BlockBuilder.Create()
        .AddHeader("Approval Request")
        .AddSection($"Request ID: *{requestId}*")
        .AddSection($"Details: {requestDetails}")
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
    
    await _slackApi.Chat.PostMessage(new Message
    {
        Channel = channelId,
        Blocks = blocks
    });
}
```

## Handling Multiple Actions

For more complex interactions, you might need to handle multiple actions:

```csharp
private async Task HandleComplexInteraction(InteractionPayload payload)
{
    // Handle each action in the payload
    foreach (var action in payload.Actions)
    {
        switch (action.ActionId)
        {
            case "select_department":
                await HandleDepartmentSelection(payload, action);
                break;
                
            case "select_priority":
                await HandlePrioritySelection(payload, action);
                break;
                
            case "add_comment":
                await HandleCommentAddition(payload, action);
                break;
                
            // Handle other actions...
        }
    }
}

private async Task HandleDepartmentSelection(InteractionPayload payload, BlockAction action)
{
    var selectedDepartment = action.SelectedOption.Value;
    
    // Update the message with department-specific information
    var updatedBlocks = BlockBuilder.From(payload.Message.Blocks);
    
    // Remove any existing department-specific blocks
    updatedBlocks.Remove("department_info_block");
    
    // Add department-specific information
    switch (selectedDepartment)
    {
        case "engineering":
            updatedBlocks.AddSection(section => section
                .BlockId("department_info_block")
                .Text("*Engineering Department*\nApproval requires sign-off from the Engineering Manager."));
            break;
            
        case "marketing":
            updatedBlocks.AddSection(section => section
                .BlockId("department_info_block")
                .Text("*Marketing Department*\nApproval requires sign-off from the Marketing Director."));
            break;
            
        case "finance":
            updatedBlocks.AddSection(section => section
                .BlockId("department_info_block")
                .Text("*Finance Department*\nApproval requires sign-off from the CFO."));
            break;
    }
    
    await _slackApi.Chat.Update(new MessageUpdate
    {
        Channel = payload.Channel.Id,
        Ts = payload.Message.Ts,
        Blocks = updatedBlocks.Build()
    });
}
```

## Handling Errors

Always handle errors gracefully in your interaction handlers:

```csharp
private async Task SafeHandleInteraction(InteractionPayload payload, Func<InteractionPayload, Task> handler)
{
    try
    {
        await handler(payload);
    }
    catch (SlackApiException ex)
    {
        // Handle Slack API errors
        Console.WriteLine($"Slack API error: {ex.Message}");
        
        // Notify the user
        await _slackApi.Chat.PostEphemeral(new Message
        {
            Channel = payload.Channel.Id,
            User = payload.User.Id,
            Text = $"An error occurred: {ex.Message}"
        });
    }
    catch (Exception ex)
    {
        // Handle other errors
        Console.WriteLine($"Error handling interaction: {ex.Message}");
        
        // Notify the user
        await _slackApi.Chat.PostEphemeral(new Message
        {
            Channel = payload.Channel.Id,
            User = payload.User.Id,
            Text = "An unexpected error occurred. Please try again later."
        });
    }
}
```

## Best Practices for Interactive Messages

1. **Use Descriptive Action IDs**: Choose action IDs that clearly describe what the action does.

2. **Handle All Interaction Types**: Be prepared to handle all types of interactions that your messages might generate.

3. **Provide Feedback**: Always update the message or send a response to provide feedback to the user.

4. **Handle Errors Gracefully**: Catch and handle errors to provide a good user experience.

5. **Use Block IDs**: Assign block IDs to blocks that you might need to update or reference later.

6. **Consider Rate Limits**: Be mindful of Slack API rate limits, especially for frequently used interactions.

7. **Test Thoroughly**: Test all possible interaction paths to ensure they work as expected. 