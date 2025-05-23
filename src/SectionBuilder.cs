namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="SectionBlock"/> instances.
/// Section blocks are versatile blocks that can contain text, markdown, fields, and an optional accessory element.
/// See <see href="https://api.slack.com/reference/block-kit/blocks#section">Slack API documentation</see> for more details.
/// </summary>
public class SectionBuilder
{
    private readonly SectionBlock _sectionBlock;
    
    /// <summary>
    /// The maximum number of fields allowed in a section block.
    /// See <see href="https://api.slack.com/reference/block-kit/blocks#section_fields">Slack API documentation</see>.
    /// </summary>
    public const int MaxFields = 10;

    /// <summary>
    /// The maximum length allowed for the text in each field.
    /// See <see href="https://api.slack.com/reference/block-kit/blocks#section_fields">Slack API documentation</see>.
    /// </summary>
    public const int MaxFieldsLength = 2000; // Corrected based on Slack docs

    /// <summary>
    /// The maximum length allowed for a block ID.
    /// </summary>
    public const int MaxBlockIdLength = 255;

    /// <summary>
    /// Initializes a new instance of the <see cref="SectionBuilder"/> class.
    /// </summary>
    public SectionBuilder()
    {
        _sectionBlock = new SectionBlock();
    }
    
    /// <summary>
    /// Sets the main text content of the section using a plain text string. 
    /// This will create a <see cref="PlainText"/> object.
    /// Use <see cref="Markdown"/> for formatted text.
    /// Required unless <c>fields</c> is provided.
    /// Maximum length 3000 characters.
    /// </summary>
    /// <param name="text">The plain text content.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder Text(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        _sectionBlock.Text = text;
        return this;
    }

    /// <summary>
    /// Sets the main text content of the section using Markdown formatting.
    /// This will create a <see cref="Markdown"/> object.
    /// Required unless <c>fields</c> is provided.
    /// Maximum length 3000 characters.
    /// </summary>
    /// <param name="text">The Markdown formatted text content.</param>
    /// <param name="verbatim">Whether to disable formatting detection for URLs, channels, users, etc. Defaults to false.</param>
    /// <param name="expand">Whether to expand the section. Defaults to false.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder Markdown(string text, bool verbatim = false, bool expand = false)
    {
        _sectionBlock.Text = new Markdown()
            {
                Text = text,
                Verbatim = verbatim,
            };
        _sectionBlock.Expand = expand;
        return this;
    }
    
    /// <summary>
    /// Sets the fields to be displayed in the section. 
    /// Required unless <c>text</c> is provided.
    /// A maximum of <see cref="MaxFields"/> fields are allowed.
    /// Each field's text has a maximum length of <see cref="MaxFieldsLength"/> characters.
    /// Use <see cref="AddTextField"/> or <see cref="AddMarkdownField"/> for adding fields individually.
    /// </summary>
    /// <param name="fields">An array of <see cref="TextObject"/> instances representing the fields.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder Fields(params TextObject[] fields)
    {
        _sectionBlock.Fields = fields;
        return this;
    }

    /// <summary>
    /// Adds a plain text field to the section's fields list.
    /// </summary>
    /// <param name="text">The plain text content for the field.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder AddTextField(string text)
    {
        _sectionBlock.Fields.Add(text);
        return this;
    
    }
    
    /// <summary>
    /// Adds a Markdown formatted field to the section's fields list.
    /// </summary>
    /// <param name="text">The Markdown formatted text content for the field.</param>
    /// <param name="verbatim">Whether to disable formatting detection. Defaults to false.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder AddMarkdownField(string text, bool verbatim = false)
    {
        ArgumentNullException.ThrowIfNull(text);
        _sectionBlock.Fields.Add(new Markdown()
            {
                Text = text,
                Verbatim = verbatim
            });
        return this;
    }
    
    /// <summary>
    /// Adds an accessory element to the section using a configuration action.
    /// Only one accessory element is allowed per section.
    /// </summary>
    /// <typeparam name="TAccessory">The type of the accessory element (e.g., <see cref="Button"/>).</typeparam>
    /// <param name="createAccessory">An action that configures the accessory element.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder Accessory<TAccessory>(Action<TAccessory> createAccessory)
    where TAccessory : BlockElement, new()
    {
        ArgumentNullException.ThrowIfNull(createAccessory);
        var accessory = new TAccessory();
        createAccessory(accessory);
        _sectionBlock.Accessory = accessory;
        return this;
    }
    
    /// <summary>
    /// Adds a pre-configured accessory element to the section.
    /// Only one accessory element is allowed per section.
    /// </summary>
    /// <param name="accessory">The accessory element instance.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder Accessory(BlockElement accessory)
    {
        _sectionBlock.Accessory = accessory;
        return this;
    }
    
    /// <summary>
    /// Whether the section should be expanded or not. Useful for AI assistant messages.
    /// </summary>
    /// <param name="expand">True to expand the section, false to collapse it.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public SectionBuilder Expand(bool expand = true)
    {
        _sectionBlock.Expand = expand;
        return this;
    }

    /// <summary>
    /// Builds the configured <see cref="SectionBlock"/>.
    /// </summary>
    /// <returns>The configured <see cref="SectionBlock"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the configured block ID exceeds <see cref="MaxBlockIdLength"/> characters.</exception>
    /// <exception cref="ArgumentException">Thrown if the number of fields exceeds <see cref="MaxFields"/> or if any field's text exceeds <see cref="MaxFieldsLength"/> characters.</exception>
    public SectionBlock Build()
    {
                
        if(_sectionBlock.BlockId?.Length > MaxBlockIdLength)
        {
            throw new InvalidOperationException($"The block id can only be up to {MaxBlockIdLength} characters long");
        }
        var fields = _sectionBlock.Fields ?? [];
        if (fields.Count > MaxFields)
        {
            throw new ArgumentException($"Section block can have at most {MaxFields} fields");
        }
        
        // Corrected validation based on Slack docs: field length applies to text
        if (fields.Any(f => (f is PlainText pt ? pt.Text.Length : (f is Markdown md ? md.Text.Length : 0)) > MaxFieldsLength))
        {
            throw new ArgumentException($"Each field's text in a section block can have at most {MaxFieldsLength} characters");
        }
        
        return _sectionBlock;
    }
}