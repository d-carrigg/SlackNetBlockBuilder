# Modal Dialog Example

Example of building blocks for use in modal dialogs.

```csharp
// Build blocks for modal content
var modalBlocks = BlockBuilder.Create()
    .AddInput<PlainTextInput>("Title", input => input
        .ActionId("title_input")
        .Placeholder("Enter task title"))
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
    .Build();

// Use with SlackNet modal view
var modalView = new ModalView
{
    Type = "modal",
    Title = "Create Task",
    Submit = "Create",
    Close = "Cancel",
    CallbackId = "task_creation_modal",
    Blocks = modalBlocks
};

await slackApi.Views.Open(new ViewsOpenRequest
{
    TriggerId = triggerId,
    View = modalView
});
```
