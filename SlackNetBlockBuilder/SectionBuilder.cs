namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="SectionBlock"/>
/// </summary>
public class SectionBuilder
{
    private readonly SectionBlock _sectionBlock;
    
    /// <summary>
    /// Max number of fields in the section
    /// https://api.slack.com/reference/block-kit/blocks#section
    /// </summary>
    public int MaxFields = 10;
    
    /// <summary>
    /// Max length of each field in the section
    /// </summary>
    public int MaxFieldsLength = 2000;
    
    /// <summary>
    /// The maximum length of a block ID
    /// </summary>
    public const int MaxBlockIdLength = 255;

    /// <summary>
    /// Initializes a new instance of the <see cref="SectionBuilder"/> class
    /// </summary>
    public SectionBuilder()
    {
        _sectionBlock = new SectionBlock();
    }
    
    /// <summary>
    /// Sets the text for the section as plain text
    /// </summary>
    /// <param name="text">The text to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public SectionBuilder Text(string text)
    {
        _sectionBlock.Text = text;
        return this;
    }

    /// <summary>
    /// Sets the text for the section as markdown
    /// </summary>
    /// <param name="text">The markdown text to set</param>
    /// <param name="verbatim">Whether to skip markdown parsing and display the text verbatim</param>
    /// <returns>The same instance so calls can be chained</returns>
    public SectionBuilder Markdown(string text, bool verbatim = false)
    {
        _sectionBlock.Text = new Markdown()
            {
                Text = text,
                Verbatim = verbatim
            };
        return this;
    }
    
    /// <summary>
    /// Sets the fields for the section
    /// </summary>
    /// <param name="fields">The fields to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public SectionBuilder Fields(params TextObject[] fields)
    {
        _sectionBlock.Fields = fields;
        return this;
    }

    /// <summary>
    /// Adds a plain text field to the section
    /// </summary>
    /// <param name="text">The text to add</param>
    /// <returns>The same instance so calls can be chained</returns>
    public SectionBuilder AddTextField(string text)
    {
        _sectionBlock.Fields.Add(text);
        return this;
    
    }
    
    /// <summary>
    /// Adds a markdown field to the section
    /// </summary>
    /// <param name="text">The markdown text to add</param>
    /// <param name="verbatim">Whether to skip markdown parsing and display the text verbatim</param>
    /// <returns>The same instance so calls can be chained</returns>
    public SectionBuilder AddMarkdownField(string text, bool verbatim = false)
    {
        _sectionBlock.Fields.Add(new Markdown()
            {
                Text = text,
                Verbatim = verbatim
            });
        return this;
    }
    
    /// <summary>
    /// Sets an accessory for the section
    /// </summary>
    /// <typeparam name="TAccessory">The type of accessory to add</typeparam>
    /// <param name="createAccessory">An action which configures the accessory</param>
    /// <returns>The same instance so calls can be chained</returns>
    public SectionBuilder Accessory<TAccessory>(Action<TAccessory> createAccessory)
    where TAccessory : BlockElement, new()
    {
        var accessory = new TAccessory();
        createAccessory(accessory);
        _sectionBlock.Accessory = accessory;
        return this;
    }
    
    /// <summary>
    /// Sets an existing accessory for the section
    /// </summary>
    /// <param name="accessory">The accessory to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public SectionBuilder Accessory(BlockElement accessory)
    {
        _sectionBlock.Accessory = accessory;
        return this;
    }

    /// <summary>
    /// Builds the section block
    /// </summary>
    /// <returns>The built section block</returns>
    /// <exception cref="InvalidOperationException">Thrown when the block ID is too long</exception>
    /// <exception cref="ArgumentException">Thrown when there are too many fields or fields are too long</exception>
    public SectionBlock Build()
    {
                
        if(_sectionBlock.BlockId?.Length > MaxBlockIdLength)
        {
            throw new InvalidOperationException($"The block id can only be up to {MaxBlockIdLength} characters long");
        }
        if (_sectionBlock.Fields.Count > MaxFields)
        {
            throw new ArgumentException($"Section block can have at most {MaxFields} fields");
        }
        
        if (_sectionBlock.Fields.Any(f => f.Text.Length > MaxFieldsLength))
        {
            throw new ArgumentException($"Each field in a section block can have at most {MaxFieldsLength} characters");
        }
        
        return _sectionBlock;
    }
}