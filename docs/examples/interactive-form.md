# Building an Interactive Form

This example demonstrates how to build an interactive form using SlackNetBlockBuilder, including various input elements and submission buttons.

## Complete Form Example

```csharp
using SlackNet;
using SlackNet.Blocks;
// All builder classes are in the SlackNet.Blocks namespace

public class FormExample
{
    private readonly ISlackApiClient _slackApi;
    
    public FormExample(ISlackApiClient slackApi)
    {
        _slackApi = slackApi;
    }
    
    public async Task SendRequestForm(string channelId)
    {
        var blocks = BlockBuilder.Create()
            // Add a header
            .AddHeader("Submit Request Form")
            
            // Add a description
            .AddSection("Please fill out the form below to submit a new request.")
            
            // Add a divider
            .AddDivider()
            
            // Add a title input field
            .AddInput<PlainTextInput>("Title", input => input
                .ActionId("title_input")
                .Placeholder("Enter a title")
                .Required())
            
            // Add a description input field (multiline)
            .AddInput<PlainTextInput>("Description", input => input
                .ActionId("description_input")
                .Multiline()
                .Placeholder("Enter a detailed description")
                .Optional())
            
            // Add a priority selection
            .AddInput<StaticSelectMenu>("Priority", input => input
                .ActionId("priority_select")
                .Placeholder("Select priority")
                .AddOption("Low", "low")
                .AddOption("Medium", "medium")
                .AddOption("High", "high")
                .Required())
            
            // Add a due date picker
            .AddInput<DatePicker>("Due Date", input => input
                .ActionId("due_date_input")
                .Placeholder("Select a date")
                .Optional())
            
            // Add assignee selection
            .AddInput<UserSelectMenu>("Assignee", input => input
                .ActionId("assignee_select")
                .Placeholder("Select an assignee")
                .Optional())
            
            // Add a checkbox for notifications
            .AddInput<CheckboxGroup>("Notifications", input => input
                .ActionId("notifications_checkbox")
                .AddOption("Send me email notifications", "email_notifications")
                .AddOption("Notify the channel when completed", "channel_notifications")
                .Optional())
            
            // Add submission buttons
            .AddActions(actions => actions
                .Button(button => button
                    .Text("Submit")
                    .ActionId("submit_form")
                    .Primary())
                .Button(button => button
                    .Text("Cancel")
                    .ActionId("cancel_form")))
            
            .Build();
        
        // Send the message with the form
        await _slackApi.Chat.PostMessage(new Message
        {
            Channel = channelId,
            Blocks = blocks
        });
    }
    
    public async Task HandleFormSubmission(InteractionPayload payload)
    {
        // Extract the form values from the payload
        var values = payload.State.Values;
        
        var title = values["title_input"]["title_input"].Value;
        var description = values["description_input"]["description_input"].Value;
        var priority = values["priority_select"]["priority_select"].SelectedOption.Value;
        var dueDate = values["due_date_input"]["due_date_input"].SelectedDate;
        var assignee = values["assignee_select"]["assignee_select"].SelectedUser;
        
        // Check if notifications are enabled
        var notificationOptions = values["notifications_checkbox"]["notifications_checkbox"].SelectedOptions;
        var emailNotifications = notificationOptions.Any(o => o.Value == "email_notifications");
        var channelNotifications = notificationOptions.Any(o => o.Value == "channel_notifications");
        
        // Process the form submission
        // ...
        
        // Update the message to show confirmation
        var updatedBlocks = BlockBuilder.Create()
            .AddHeader("Request Submitted")
            .AddSection("✅ Your request has been submitted successfully.")
            .AddContext(context => context
                .AddText($"*Title:* {title}")
                .AddText($"*Priority:* {priority}")
                .AddText($"*Due Date:* {dueDate ?? "Not specified"}"))
            .Build();
        
        // Update the original message
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = updatedBlocks
        });
    }
}
```

## Breaking Down the Example

### 1. Form Structure

The form is structured with:
- A header to identify the form
- A description section explaining the purpose
- A divider to separate the header from the form fields
- Multiple input fields of different types
- Action buttons for submission

### 2. Input Types

The example demonstrates various input types:
- `PlainTextInput`: For single-line and multi-line text input
- `StaticSelectMenu`: For selecting from predefined options
- `DatePicker`: For selecting a date
- `UserSelectMenu`: For selecting a user from the workspace
- `CheckboxGroup`: For multiple selection options

### 3. Handling Submissions

The `HandleFormSubmission` method shows how to:
- Extract values from the interaction payload
- Process the form data
- Update the original message to show a confirmation

### 4. Best Practices

- Use clear labels for input fields
- Indicate which fields are required
- Provide placeholder text for guidance
- Group related actions together
- Use appropriate input types for different data
- Provide confirmation after submission

## Additional Considerations

### Error Handling

You might want to add error handling to validate the form data:

```csharp
public async Task HandleFormSubmission(InteractionPayload payload)
{
    try
    {
        // Extract and validate form data
        var values = payload.State.Values;
        
        var title = values["title_input"]["title_input"].Value;
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ValidationException("Title is required");
        }
        
        // Process valid submission
        // ...
        
        // Show success message
        var successBlocks = BlockBuilder.Create()
            .AddHeader("Request Submitted")
            .AddSection("✅ Your request has been submitted successfully.")
            .Build();
            
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = successBlocks
        });
    }
    catch (ValidationException ex)
    {
        // Show error message
        var errorBlocks = BlockBuilder.Create()
            .AddHeader("Error")
            .AddSection($"❌ {ex.Message}")
            .AddActions(actions => actions
                .Button(button => button
                    .Text("Try Again")
                    .ActionId("retry_form")
                    .Primary()))
            .Build();
            
        await _slackApi.Chat.Update(new MessageUpdate
        {
            Channel = payload.Channel.Id,
            Ts = payload.Message.Ts,
            Blocks = errorBlocks
        });
    }
}
``` 