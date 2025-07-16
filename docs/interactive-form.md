# Interactive Form Example

Simple example of creating a form with input validation.

```csharp
var blocks = BlockBuilder.Create()
    .AddHeader("Request Form")
    .AddInput<PlainTextInput>("Title", input => input
        .ActionId("title_input")
        .Placeholder("Enter title"))
    .AddInput<PlainTextInput>("Description", input => input
        .ActionId("description_input")
        .Multiline()
        .Optional())
    .AddInput<StaticSelectMenu>("Priority", input => input
        .ActionId("priority_select")
        .Placeholder("Select priority")
        .AddOption("Low", "low")
        .AddOption("Medium", "medium")
        .AddOption("High", "high"))
    .AddActions(actions => actions
        .AddButton("submit", "Submit")
        .AddButton("cancel", "Cancel"))
    .Build();
```

## Handling Form Submission

```csharp
public async Task HandleFormSubmission(InteractionPayload payload)
{
    var title = payload.State.Values["title_input"]["title_input"].Value;
    var description = payload.State.Values["description_input"]["description_input"].Value;
    var priority = payload.State.Values["priority_select"]["priority_select"].SelectedOption.Value;
    
    // Process form data...
}
```
