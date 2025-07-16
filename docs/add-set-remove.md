# Add/Set/Remove Functionality

The BlockBuilder provides three core methods for custom modifications when the prebuilt extension methods don't meet your needs:

## Add<T> Method

The `Add<T>` method allows you to add any block type directly to the builder.

### Syntax
```csharp
IBlockBuilder Add<TBlock>(Action<TBlock> modifier) where TBlock : Block, new()
```

### Usage Examples

```csharp
// Add a divider block with custom block ID
var builder = BlockBuilder.Create()
    .Add<DividerBlock>(divider => divider.BlockId = "custom_divider");

// Add a call block with custom configuration
builder.Add<CallBlock>(call =>
{
    call.CallId = "call_123";
    call.BlockId = "my_call_block";
});

// Add an image block with full customization
builder.Add<ImageBlock>(image =>
{
    image.ImageUrl = "https://example.com/image.jpg";
    image.AltText = "Example image";
    image.Title = new PlainText { Text = "My Image" };
    image.BlockId = "image_block_1";
});

// Add a video block with all optional properties
builder.Add<VideoBlock>(video =>
{
    video.VideoUrl = "https://youtube.com/watch?v=abc123";
    video.ThumbnailUrl = "https://example.com/thumb.jpg";
    video.Title = "My Video";
    video.AltText = "Video description";
    video.BlockId = "video_1";
    video.Description = "Additional video info";
    video.ProviderName = "YouTube";
    video.ProviderIconUrl = "https://youtube.com/icon.png";
});
```

## Set Method

The `Set` method is available on various builders and allows direct modification of the underlying element when extension methods aren't available.

```csharp
// Modify button properties directly
var actionsBuilder = ActionsBlockBuilder.Create();
actionsBuilder.AddButton("my_button", button => 
    button.Set(btn => 
    {
        btn.Text = new PlainText { Text = "Custom Button" };
        btn.Style = ButtonStyle.Primary;
        btn.Value = "custom_value";
    }));

// Chain multiple Set calls
actionsBuilder.AddButton("chained_button", button => 
    button.Set(btn => btn.ActionId = "first_id")
          .Set(btn => btn.Text = new PlainText { Text = "Button Text" })
          .Set(btn => btn.ActionId = "final_id"));

// Configure input elements with custom properties
var builder = BlockBuilder.Create();
builder.AddInput<PlainTextInput>("Enter text", input => 
    input.Set(element => 
    {
        element.ActionId = "text_input_1";
        element.Placeholder = "Type here...";
        element.Multiline = true;
        element.MinLength = 10;
        element.MaxLength = 500;
    }));

// Combine Set with other methods
builder.AddInput<PlainTextInput>("User Input", input => 
    input.Set(element => element.Placeholder = "Enter value")
         .BlockId("custom_input_block")
         .Optional(true)
         .Hint("This field is optional"));
```

## Remove Methods

The BlockBuilder provides several remove methods for modifying existing block structures.

### Remove by Predicate

```csharp
// Remove all divider blocks
builder.Remove(block => block is DividerBlock);

// Remove blocks by type and condition
builder.Remove(block => block is HeaderBlock header && 
                       header.Text.Text.Contains("Old"));

// Remove blocks with specific block IDs
builder.Remove(block => block.BlockId == "temp_block" || 
                       block.BlockId == "another_temp");
```

### Remove by Block ID

```csharp
// Remove a specific block by its ID
builder.Remove("block_to_remove");

// Remove multiple blocks by chaining
builder.Remove("block1")
       .Remove("block2")
       .Remove("block3");
```

### Remove Actions

```csharp
// Remove action by ID (finds first ActionsBlock and removes matching element)
builder.RemoveAction("button_to_remove");

// Remove action by predicate
builder.RemoveAction(action => action.ActionId.StartsWith("temp_"));

// Remove all actions of a specific type
builder.RemoveActions(); // Removes all ActionsBlocks entirely
```

### Complete Example: Message Update Flow

```csharp
// Start with existing blocks from a message
var existingBlocks = sourceMessage.Blocks;

// Create builder from existing blocks and modify
var updatedBlocks = BlockBuilder.From(existingBlocks)
    // Remove the old action buttons
    .RemoveAction("approve_button")
    .RemoveAction("reject_button")
    
    // Add custom status block
    .Add<SectionBlock>(section => 
    {
        section.Text = new Markdown { Text = "*Status:* Approved ✅" };
        section.BlockId = "status_section";
    })
    
    // Add new actions with custom configuration
    .AddActions(actions => 
        actions.AddButton("view_details", button => 
            button.Set(btn => 
            {
                btn.Text = new PlainText { Text = "View Details" };
                btn.Style = ButtonStyle.Primary;
                btn.Url = "https://example.com/details";
            })))
    
    // Remove any temporary blocks
    .Remove(block => block.BlockId?.StartsWith("temp_") == true)
    
    .Build();
```

## When to Use Each Method

- **Add<T>**: When you need to create blocks with properties that don't have dedicated extension methods
- **Set**: When you need to modify element properties that aren't covered by existing extension methods
- **Remove**: When updating existing messages or cleaning up dynamically generated content
