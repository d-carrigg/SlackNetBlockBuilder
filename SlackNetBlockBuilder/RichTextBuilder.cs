using System.Collections.ObjectModel;

namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="RichTextBlock"/> instances.
/// Rich text blocks are used to display formatted text with elements like sections, lists, quotes, and preformatted code.
/// See <see href="https://api.slack.com/reference/block-kit/blocks#rich_text">Slack API documentation</see> for more details.
/// </summary>
public class RichTextBuilder
{
    private readonly RichTextBlock _richTextBlock = new();
    
    /// <summary>
    /// The maximum length allowed for a block ID.
    /// </summary>
    public const int MaxIdLength = 255;
    
    /// <summary>
    /// Sets a unique identifier for this rich text block.
    /// </summary>
    /// <param name="blockId">The block ID. Maximum length is <see cref="MaxIdLength"/> characters.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextBuilder BlockId(string blockId)
    {
        _richTextBlock.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Adds a <see cref="RichTextSection"/> element to the block.
    /// Sections are the basic container for text and other elements within rich text.
    /// </summary>
    /// <param name="creationSection">An action that configures the section's elements using a <see cref="RichTextSectionElementBuilder"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextBuilder AddSection(Action<RichTextSectionElementBuilder> creationSection)
    {
        ArgumentNullException.ThrowIfNull(creationSection);
        var builder = new RichTextSectionElementBuilder();
        creationSection(builder);
        _richTextBlock.Elements.Add(new RichTextSection()
            {
                Elements = builder.Build()
            });
        return this;
    }

    /// <summary>
    /// Adds a <see cref="RichTextList"/> element to the block.
    /// Lists can be ordered or unordered.
    /// </summary>
    /// <param name="style">The style of the list (e.g., bulleted or numbered).</param>
    /// <param name="builder">An action that configures the list items and properties using a <see cref="RichTextListBuilder"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextBuilder AddTextList(RichTextListStyle style, Action<RichTextListBuilder> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var listBuilder = new RichTextListBuilder(style);
        builder(listBuilder);

        _richTextBlock.Elements.Add(listBuilder.Build());
        return this;
    }

    /// <summary>
    /// Adds a <see cref="RichTextPreformatted"/> element to the block, typically used for code blocks.
    /// Content is configured using a section element builder.
    /// </summary>
    /// <param name="createContent">An action that configures the content elements using a <see cref="RichTextSectionElementBuilder"/>.</param>
    /// <param name="border">Optional border style attribute (usually 0 or 1).</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextBuilder AddPreformattedText(Action<RichTextSectionElementBuilder> createContent, int border = default)
    {
        ArgumentNullException.ThrowIfNull(createContent);
        var builder = new RichTextSectionElementBuilder();
        createContent(builder);
        _richTextBlock.Elements.Add(new RichTextPreformatted()
            {
                Elements = builder.Build(),
                Border = border
            });
        return this;
    }
    
    /// <summary>
    /// Adds a simple <see cref="RichTextPreformatted"/> element containing only plain text.
    /// </summary>
    /// <param name="text">The plain text content for the preformatted block.</param>
    /// <param name="border">Optional border style attribute (usually 0 or 1).</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextBuilder AddPreformattedText(string text, int border = default)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentNullException(nameof(text), "Text cannot be null or empty.");
 
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
    /// Adds a <see cref="RichTextQuote"/> element to the block.
    /// </summary>
    /// <param name="createContent">An action that configures the content elements of the quote using a <see cref="RichTextSectionElementBuilder"/>.</param>
    /// <param name="border">Optional border style attribute (usually 0 or 1).</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextBuilder AddQuote(Action<RichTextSectionElementBuilder> createContent, int border = default)
    {
        ArgumentNullException.ThrowIfNull(createContent);
        var builder = new RichTextSectionElementBuilder();
        createContent(builder);
        _richTextBlock.Elements.Add(new RichTextQuote()
            {
                Elements = builder.Build(),
                Border = border
            });
        return this;
    }

    /// <summary>
    /// Builds the configured <see cref="RichTextBlock"/>.
    /// </summary>
    /// <returns>The configured <see cref="RichTextBlock"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the configured block ID exceeds <see cref="MaxIdLength"/> characters.</exception>
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
/// A helper builder for creating lists of <see cref="RichTextSectionElement"/> objects.
/// Used internally by other rich text builders (e.g., for sections, lists, quotes).
/// </summary>
public sealed class RichTextSectionElementBuilder
{
    private readonly List<RichTextSectionElement> _elements = new();

    /// <summary>
    /// Returns the list of configured rich text section elements.
    /// </summary>
    public Collection<RichTextSectionElement> Build() => new(_elements);

    /// <summary>
    /// Adds a <see cref="RichTextSectionElement"/> to the list.
    /// </summary>
    /// <typeparam name="TElement">The type of the element to add, must inherit from <see cref="RichTextSectionElement"/>.</typeparam>
    /// <param name="element">The element instance to add.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextSectionElementBuilder Add<TElement>(TElement element) where TElement : RichTextSectionElement
    {
        _elements.Add(element);
        return this;
    }
}

/// <summary>
/// Provides a fluent interface for building <see cref="RichTextList"/> elements.
/// </summary>
public sealed class RichTextListBuilder
{
    private readonly RichTextList _list;

    /// <summary>
    /// Initializes a new instance of the <see cref="RichTextListBuilder"/> class.
    /// </summary>
    /// <param name="style">The visual style of the list (e.g., bulleted or numbered).</param>
    public RichTextListBuilder(RichTextListStyle style)
    {
        _list = new RichTextList()
            {
                Style = style
            };
    }

    /// <summary>
    /// Adds a list item as a <see cref="RichTextSection"/> element to the list.
    /// </summary>
    /// <param name="creationSection">An action that configures the list item's content using a <see cref="RichTextSectionElementBuilder"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextListBuilder AddSection(Action<RichTextSectionElementBuilder> creationSection)
    {
        ArgumentNullException.ThrowIfNull(creationSection);
        var builder = new RichTextSectionElementBuilder();
        creationSection(builder);
        _list.Elements.Add(new RichTextSection()
            {
                Elements = builder.Build()
            });
        return this;
    }
    
    /// <summary>
    /// Sets the indentation level for the list.
    /// </summary>
    /// <param name="indent">The indentation level (integer).</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextListBuilder Indent(int indent)
    {
        _list.Indent = indent;
        return this;
    }
    
    /// <summary>
    /// Sets the starting number for an ordered list.
    /// </summary>
    /// <param name="offset">The starting number (integer).</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextListBuilder Offset(int offset)
    {
        _list.Offset = offset;
        return this;
    }
    
    /// <summary>
    /// Sets the border style for the list.
    /// </summary>
    /// <param name="border">The border style attribute (usually 0 or 1).</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public RichTextListBuilder Border(int border)
    {
        _list.Border = border;
        return this;
    }

    /// <summary>
    /// Returns the configured <see cref="RichTextList"/> instance.
    /// </summary>
    public RichTextList Build() => _list;
}