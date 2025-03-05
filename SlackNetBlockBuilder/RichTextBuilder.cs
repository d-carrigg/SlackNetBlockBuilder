namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="RichTextBlock"/>
/// </summary>
public class RichTextBuilder
{
    private readonly RichTextBlock _richTextBlock = new();
    
    /// <summary>
    /// The maximum length of a block ID
    /// </summary>
    public const int MaxIdLength = 255;
    
    /// <summary>
    /// Sets the block ID for the rich text block
    /// </summary>
    /// <param name="blockId">The block ID to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextBuilder BlockId(string blockId)
    {
        _richTextBlock.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Adds a rich text section to the block
    /// </summary>
    /// <param name="creationSection">An action which configures the section</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextBuilder AddSection(Action<RichTextSectionElementBuilder> creationSection)
    {
        var builder = new RichTextSectionElementBuilder();
        creationSection(builder);
        _richTextBlock.Elements.Add(new RichTextSection()
            {
                Elements = builder.Build()
            });
        return this;
    }

    /// <summary>
    /// Adds a text list to the block
    /// </summary>
    /// <param name="style">The style of the list (ordered or bullet)</param>
    /// <param name="builder">An action which configures the list</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextBuilder AddTextList(RichTextListStyle style, Action<RichTextListBuilder> builder)
    {
        var listBuilder = new RichTextListBuilder(style);
        builder(listBuilder);

        _richTextBlock.Elements.Add(listBuilder.Build());
        return this;
    }

    /// <summary>
    /// Adds preformatted text to the block
    /// </summary>
    /// <param name="createQuote">An action which configures the preformatted text</param>
    /// <param name="border">The border size for the preformatted text</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextBuilder AddPreformattedText(Action<RichTextSectionElementBuilder> createQuote, int border = default)
    {
        var builder = new RichTextSectionElementBuilder();
        createQuote(builder);
        _richTextBlock.Elements.Add(new RichTextPreformatted()
            {
                Elements = builder.Build(),
                Border = border
            });
        return this;
    }
    
    /// <summary>
    /// Adds preformatted text to the block
    /// </summary>
    /// <param name="text">The text to add</param>
    /// <param name="border">The border size for the preformatted text</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextBuilder AddPreformattedText(string text, int border = default)
    {
 
        _richTextBlock.Elements.Add(new RichTextPreformatted()
        {
            Elements = [new RichTextText()
            {
                Text = text,
            }],
            Border = border
        });
        return this;
    }

    /// <summary>
    /// Adds a quote to the block
    /// </summary>
    /// <param name="createQuote">An action which configures the quote</param>
    /// <param name="border">The border size for the quote</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextBuilder AddQuote(Action<RichTextSectionElementBuilder> createQuote, int border = default)
    {
        var builder = new RichTextSectionElementBuilder();
        createQuote(builder);
        _richTextBlock.Elements.Add(new RichTextQuote()
            {
                Elements = builder.Build(),
                Border = border
            });
        return this;
    }

    /// <summary>
    /// Builds the rich text block
    /// </summary>
    /// <returns>The built rich text block</returns>
    /// <exception cref="InvalidOperationException">Thrown when the block ID is too long</exception>
    public RichTextBlock Build()
    {
        if (_richTextBlock.BlockId?.Length > MaxIdLength)
        {
            throw new InvalidOperationException($"The block id can only be up to {MaxIdLength} characters long");
        }
        
        return _richTextBlock;
    }
}

/// <summary>
/// Builder for rich text section elements
/// </summary>
public sealed class RichTextSectionElementBuilder
{
    private readonly List<RichTextSectionElement> _elements = new();
    
    /// <summary>
    /// Builds the list of rich text section elements
    /// </summary>
    /// <returns>The list of elements</returns>
    public List<RichTextSectionElement> Build() => _elements;

    /// <summary>
    /// Adds an element to the section
    /// </summary>
    /// <typeparam name="TElement">The type of element to add</typeparam>
    /// <param name="element">The element to add</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextSectionElementBuilder Add<TElement>(TElement element) where TElement : RichTextSectionElement
    {
        _elements.Add(element);
        return this;
    }
}

/// <summary>
/// Builder for rich text lists
/// </summary>
public sealed class RichTextListBuilder
{
    private readonly RichTextList _list;

    /// <summary>
    /// Initializes a new instance of the <see cref="RichTextListBuilder"/> class
    /// </summary>
    /// <param name="style">The style of the list (ordered or bullet)</param>
    public RichTextListBuilder(RichTextListStyle style)
    {
        _list = new RichTextList()
            {
                Style = style
            };
    }

    /// <summary>
    /// Adds a section to the list
    /// </summary>
    /// <param name="creationSection">An action which configures the section</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextListBuilder AddSection(Action<RichTextSectionElementBuilder> creationSection)
    {
        var builder = new RichTextSectionElementBuilder();
        creationSection(builder);
        _list.Elements.Add(new RichTextSection()
            {
                Elements = builder.Build()
            });
        return this;
    }
    
    /// <summary>
    /// Sets the indentation level for the list
    /// </summary>
    /// <param name="indent">The indentation level</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextListBuilder Indent(int indent)
    {
        _list.Indent = indent;
        return this;
    }
    
    /// <summary>
    /// Sets the offset for the list
    /// </summary>
    /// <param name="offset">The offset value</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextListBuilder Offset(int offset)
    {
        _list.Offset = offset;
        return this;
    }
    
    /// <summary>
    /// Sets the border size for the list
    /// </summary>
    /// <param name="border">The border size</param>
    /// <returns>The same instance so calls can be chained</returns>
    public RichTextListBuilder Border(int border)
    {
        _list.Border = border;
        return this;
    }

    /// <summary>
    /// Builds the rich text list
    /// </summary>
    /// <returns>The built rich text list</returns>
    public RichTextList Build() => _list;
}