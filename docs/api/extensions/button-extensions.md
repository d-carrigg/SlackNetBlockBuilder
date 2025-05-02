# Button Extensions API Reference

The `ButtonExtensions` class provides extension methods for configuring button elements in Slack Block Kit UI.

## Extension Methods

### Text(string text)

Sets the text for the button.

```csharp
public static ButtonBuilder Text(this ButtonBuilder builder, string text)
```

**Parameters:**
- `builder`: The button builder to configure.
- `text`: The text to display on the button.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Click Me")
    .ActionId("button_click");
```

### ActionId(string actionId)

Sets the action ID for the button.

```csharp
public static ButtonBuilder ActionId(this ButtonBuilder builder, string actionId)
```

**Parameters:**
- `builder`: The button builder to configure.
- `actionId`: The action ID for the button.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Click Me")
    .ActionId("button_click");
```

### Url(string url)

Sets the URL to open when the button is clicked.

```csharp
public static ButtonBuilder Url(this ButtonBuilder builder, string url)
```

**Parameters:**
- `builder`: The button builder to configure.
- `url`: The URL to open.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Visit Website")
    .ActionId("visit_website")
    .Url("https://example.com");
```

### Value(string value)

Sets the value for the button.

```csharp
public static ButtonBuilder Value(this ButtonBuilder builder, string value)
```

**Parameters:**
- `builder`: The button builder to configure.
- `value`: The value for the button.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Approve")
    .ActionId("approve_button")
    .Value("item_123");
```

### Primary()

Sets the button style to primary (green).

```csharp
public static ButtonBuilder Primary(this ButtonBuilder builder)
```

**Parameters:**
- `builder`: The button builder to configure.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Approve")
    .ActionId("approve_button")
    .Primary();
```

### Danger()

Sets the button style to danger (red).

```csharp
public static ButtonBuilder Danger(this ButtonBuilder builder)
```

**Parameters:**
- `builder`: The button builder to configure.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Delete")
    .ActionId("delete_button")
    .Danger();
```

### Confirm(string title, string text, string confirm, string deny)

Adds a confirmation dialog to the button.

```csharp
public static ButtonBuilder Confirm(this ButtonBuilder builder, string title, string text, string confirm, string deny)
```

**Parameters:**
- `builder`: The button builder to configure.
- `title`: The title of the confirmation dialog.
- `text`: The text of the confirmation dialog.
- `confirm`: The text for the confirm button.
- `deny`: The text for the deny button.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Delete")
    .ActionId("delete_button")
    .Danger()
    .Confirm(
        "Are you sure?",
        "This action cannot be undone.",
        "Yes, delete it",
        "Cancel"
    );
```

### Confirm(Action<ConfirmationDialogBuilder> configure)

Adds a confirmation dialog to the button with advanced configuration.

```csharp
public static ButtonBuilder Confirm(this ButtonBuilder builder, Action<ConfirmationDialogBuilder> configure)
```

**Parameters:**
- `builder`: The button builder to configure.
- `configure`: An action to configure the confirmation dialog.

**Returns:** The button builder for method chaining.

**Example:**
```csharp
var button = new ButtonBuilder()
    .Text("Delete")
    .ActionId("delete_button")
    .Danger()
    .Confirm(confirm => confirm
        .Title("Are you sure?")
        .Text("This action cannot be undone.")
        .Confirm("Yes, delete it")
        .Deny("Cancel")
        .Style(ConfirmationDialogStyle.Danger)
    );
``` 