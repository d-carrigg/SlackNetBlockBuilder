# BlockBuilder API Reference

The `BlockBuilder` class provides a fluent interface for creating Slack Block Kit UI elements.

## Factory Methods

### Create()
Creates a new empty block builder.

```csharp
var builder = BlockBuilder.Create();
```

### From(IEnumerable<Block> blocks)
Creates a block builder from existing blocks.

```csharp
var builder = BlockBuilder.From(existingBlocks);
```

## Main Methods

### AddHeader(string text)
Adds a header block.

```csharp
.AddHeader("My Header")
```

### AddSection(string text)
Adds a section block with text.

```csharp
.AddSection("Hello world")
```

### AddSection(Action<SectionBuilder> configure)
Adds a configured section block.

```csharp
.AddSection(section => section
    .Text("Hello")
    .Accessory(button => button
        .Text("Click Me")
        .ActionId("click_me")))
```

### AddDivider()
Adds a divider block.

```csharp
.AddDivider()
```

### AddActions(Action<ActionsBlockBuilder> configure)
Adds an actions block with buttons/elements.

```csharp
.AddActions(actions => actions
    .AddButton("approve", "Approve")
    .AddButton("reject", "Reject"))
```

### AddInput<T>(string label, Action<InputBlockBuilder<T>> configure)
Adds an input block for forms.

```csharp
.AddInput<PlainTextInput>("Name", input => input
    .ActionId("name_input")
    .Required())
```

### AddContext(Action<ContextBlockBuilder> configure)
Adds a context block for supplementary info.

```csharp
.AddContext(context => context
    .AddText("Last updated: Today"))
```

### Build()
Returns the list of blocks.

```csharp
List<Block> blocks = builder.Build();
```
