# Building Modal Dialogs

This example demonstrates how to create and open modal dialogs using SlackNetBlockBuilder, as well as how to handle submissions.

## Creating a Modal Dialog

```csharp
using SlackNet;
using SlackNet.Blocks;
using SlackNet.WebApi;

public class ModalExample
{
    private readonly ISlackApiClient _slackApi;
    
    public ModalExample(ISlackApiClient slackApi)
    {
        _slackApi = slackApi;
    }
    
    public async Task OpenTaskCreationModal(string triggerId)
    {
        // Build the modal view
        var modalView = new ModalViewBuilder()
            .Title("Create New Task")
            .CallbackId("task_creation_modal")
            .Submit("Create")
            .Close("Cancel")
            .Blocks(blocks => blocks
                // Add a title input
                .AddInput<PlainTextInput>("Title", input => input
                    .BlockId("title_block")
                    .ActionId("title_input")
                    .Placeholder("Enter task title")
                    .Required())
                
                // Add a description input
                .AddInput<PlainTextInput>("Description", input => input
                    .BlockId("description_block")
                    .ActionId("description_input")
                    .Multiline()
                    .Placeholder("Enter task description")
                    .Optional())
                
                // Add a priority selection
                .AddInput<StaticSelectMenu>("Priority", input => input
                    .BlockId("priority_block")
                    .ActionId("priority_select")
                    .Placeholder("Select priority")
                    .AddOption("Low", "low")
                    .AddOption("Medium", "medium")
                    .AddOption("High", "high")
                    .Required())
                
                // Add a due date picker
                .AddInput<DatePicker>("Due Date", input => input
                    .BlockId("due_date_block")
                    .ActionId("due_date_input")
                    .Placeholder("Select due date")
                    .Optional())
                
                // Add assignee selection
                .AddInput<UserSelectMenu>("Assignee", input => input
                    .BlockId("assignee_block")
                    .ActionId("assignee_select")
                    .Placeholder("Select assignee")
                    .Optional()))
            .Build();
        
        // Open the modal
        await _slackApi.Views.Open(new OpenViewRequest
        {
            TriggerId = triggerId,
            View = modalView
        });
    }
}
```

## Handling Modal Submissions

When a user submits a modal, you'll receive a view submission payload. Here's how to handle it:

```csharp
public async Task<ViewSubmissionResponse> HandleTaskCreationSubmission(ViewSubmissionPayload payload)
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
        
        // Validate the submission
        var errors = new Dictionary<string, string>();
        
        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add("title_block", "Title is required");
        }
        
        if (errors.Any())
        {
            // Return validation errors
            return new ViewSubmissionResponse
            {
                ResponseAction = ViewResponseAction.Errors,
                Errors = errors
            };
        }
        
        // Process the submission (e.g., create a task in your system)
        await CreateTask(title, description, priority, dueDate, assignee);
        
        // Return a success response
        return new ViewSubmissionResponse
        {
            ResponseAction = ViewResponseAction.Clear
        };
    }
    catch (Exception ex)
    {
        // Log the error
        Console.WriteLine($"Error handling task creation submission: {ex.Message}");
        
        // Return an error response
        return new ViewSubmissionResponse
        {
            ResponseAction = ViewResponseAction.Errors,
            Errors = new Dictionary<string, string>
            {
                { "title_block", "An error occurred. Please try again." }
            }
        };
    }
}

private async Task CreateTask(string title, string description, string priority, DateTime? dueDate, string assignee)
{
    // Implement your task creation logic here
    // This might involve calling your task management system's API
    
    // For demonstration purposes, we'll just log the task details
    Console.WriteLine($"Created task: {title}");
    Console.WriteLine($"Description: {description}");
    Console.WriteLine($"Priority: {priority}");
    Console.WriteLine($"Due Date: {dueDate}");
    Console.WriteLine($"Assignee: {assignee}");
    
    // You might also want to send a message to a channel to notify about the new task
    await _slackApi.Chat.PostMessage(new Message
    {
        Channel = "task-notifications",
        Blocks = BlockBuilder.Create()
            .AddHeader("New Task Created")
            .AddSection($"*Title:* {title}")
            .AddContext(context => context
                .AddText($"Priority: {priority}")
                .AddText($"Due Date: {dueDate?.ToString("yyyy-MM-dd") ?? "Not specified"}"))
            .Build()
    });
}
```

## Opening a Modal from a Button Click

To open a modal when a user clicks a button, you need to handle the button interaction and then open the modal:

```csharp
public async Task HandleButtonClick(InteractionPayload payload)
{
    if (payload.Actions[0].ActionId == "create_task_button")
    {
        // Open the task creation modal
        await OpenTaskCreationModal(payload.TriggerId);
    }
}
```

## Creating a Button to Open the Modal

```csharp
var blocks = BlockBuilder.Create()
    .AddSection("Task Management")
    .AddActions(actions => actions
        .Button(button => button
            .Text("Create Task")
            .ActionId("create_task_button")
            .Primary()))
    .Build();

await _slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});
```

## Updating a Modal

You can also update an existing modal in response to user interactions:

```csharp
public async Task HandleBlockActions(InteractionPayload payload)
{
    if (payload.Actions[0].ActionId == "priority_select")
    {
        var selectedPriority = payload.Actions[0].SelectedOption.Value;
        
        if (selectedPriority == "high")
        {
            // Update the modal to show additional fields for high priority tasks
            var updatedView = new ModalViewBuilder()
                .Title("Create New Task")
                .CallbackId("task_creation_modal")
                .Submit("Create")
                .Close("Cancel")
                .Blocks(blocks => blocks
                    // Include all the original blocks
                    // ...
                    
                    // Add a new block for high priority tasks
                    .AddInput<PlainTextInput>("Justification", input => input
                        .BlockId("justification_block")
                        .ActionId("justification_input")
                        .Multiline()
                        .Placeholder("Please provide justification for high priority")
                        .Required()))
                .Build();
            
            await _slackApi.Views.Update(new UpdateViewRequest
            {
                ViewId = payload.View.Id,
                View = updatedView
            });
        }
    }
}
```

## Best Practices for Modal Dialogs

1. **Keep It Focused**: Modals should focus on a single task or workflow.

2. **Provide Clear Instructions**: Use labels and placeholder text to guide users.

3. **Validate Input**: Validate user input and provide clear error messages.

4. **Use Appropriate Input Types**: Choose the right input type for each field (text, select, date picker, etc.).

5. **Handle Errors Gracefully**: Provide helpful error messages when something goes wrong.

6. **Provide Feedback**: Let users know when their submission has been processed successfully.

7. **Consider Progressive Disclosure**: Show additional fields only when needed, based on user selections.

8. **Test Thoroughly**: Test your modals on different devices and screen sizes to ensure they work well everywhere. 