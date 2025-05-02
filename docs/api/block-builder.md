# BlockBuilder API Reference

The `BlockBuilder` class is the main entry point for creating Slack Block Kit UI elements with SlackNetBlockBuilder. It provides a fluent interface for building complex Slack messages.

## Static Factory Methods

### Create()

Creates a new empty block builder.

```csharp
public static BlockBuilder Create()
```

**Returns:** A new `BlockBuilder` instance with no blocks.

**Example:**
```csharp
var builder = BlockBuilder.Create();
```

### From(IList<Block> blocks)

Creates a new block builder initialized with the provided blocks.

```csharp
public static BlockBuilder From(IList<Block> blocks)
```

**Parameters:**
- `blocks`: A list of existing blocks to initialize the builder with.

**Returns:** A new `BlockBuilder` instance with the provided blocks.

**Example:**
```csharp
// When handling an interaction payload
var existingBlocks = payload.Message.Blocks;
var builder = BlockBuilder.From(existingBlocks);
```

### CreateSection()

Creates a new section block builder.

```csharp
public static SectionBuilder CreateSection()
```

**Returns:** A new `SectionBuilder` instance.

**Example:**
```csharp
var section = BlockBuilder.CreateSection()
    .Text("Hello, world!")
    .Build();
```

## Instance Methods

### AddSection(string text)

Adds a new section block with the specified text.

```csharp
public BlockBuilder AddSection(string text)
```

**Parameters:**
- `text`: The text to display in the section (supports Markdown formatting).

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddSection("Hello, world!")
    .Build();
```

### AddSection(Action<SectionBuilder> configure)

Adds a new section block configured with the provided action.

```csharp
public BlockBuilder AddSection(Action<SectionBuilder> configure)
```

**Parameters:**
- `configure`: An action to configure the section builder.

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddSection(section => section
        .Text("Hello, world!")
        .Accessory(button => button
            .Text("Click Me")
            .ActionId("button_click")
            .Primary()))
    .Build();
```

### AddDivider()

Adds a divider block.

```csharp
public BlockBuilder AddDivider()
```

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddSection("Section 1")
    .AddDivider()
    .AddSection("Section 2")
    .Build();
```

### AddActions(Action<ActionsBlockBuilder> configure)

Adds an actions block configured with the provided action.

```csharp
public BlockBuilder AddActions(Action<ActionsBlockBuilder> configure)
```

**Parameters:**
- `configure`: An action to configure the actions block builder.

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddActions(actions => actions
        .Button(button => button
            .Text("Approve")
            .ActionId("approve_button")
            .Primary())
        .Button(button => button
            .Text("Reject")
            .ActionId("reject_button")
            .Danger()))
    .Build();
```

### AddContext(Action<ContextBlockBuilder> configure)

Adds a context block configured with the provided action.

```csharp
public BlockBuilder AddContext(Action<ContextBlockBuilder> configure)
```

**Parameters:**
- `configure`: An action to configure the context block builder.

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddContext(context => context
        .AddText("Last updated: Today at 9:30 AM")
        .AddImage("https://example.com/icon.png", "Icon"))
    .Build();
```

### AddHeader(string text)

Adds a header block with the specified text.

```csharp
public BlockBuilder AddHeader(string text)
```

**Parameters:**
- `text`: The text to display in the header.

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddHeader("Form Title")
    .AddSection("Form content goes here...")
    .Build();
```

### AddInput<T>(string label, Action<InputBlockBuilder<T>> configure) where T : IInputElement

Adds an input block configured with the provided action.

```csharp
public BlockBuilder AddInput<T>(string label, Action<InputBlockBuilder<T>> configure) where T : IInputElement
```

**Parameters:**
- `label`: The label for the input.
- `configure`: An action to configure the input block builder.

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddInput<PlainTextInput>("Title", input => input
        .ActionId("title_input")
        .Placeholder("Enter a title")
        .Required())
    .Build();
```

### Remove(string blockId)

Removes a block with the specified ID.

```csharp
public BlockBuilder Remove(string blockId)
```

**Parameters:**
- `blockId`: The ID of the block to remove.

**Returns:** The current `BlockBuilder` instance for method chaining.

**Example:**
```csharp
var blocks = BlockBuilder.From(existingBlocks)
    .Remove("status_block")
    .Build();
```

### Build()

Builds and returns the list of blocks.

```csharp
public IList<Block> Build()
```

**Returns:** The list of built blocks.

**Example:**
```csharp
var blocks = BlockBuilder.Create()
    .AddSection("Hello, world!")
    .Build();

await slackApi.Chat.PostMessage(new Message
{
    Channel = channelId,
    Blocks = blocks
});
``` 