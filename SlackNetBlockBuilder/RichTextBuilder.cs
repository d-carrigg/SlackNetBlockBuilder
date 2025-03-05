using SlackNet.Blocks;

namespace SlackNetBlockBuilder;

public class RichTextBuilder
{
    private readonly RichTextBlock _richTextBlock = new();
    
    public const int MaxIdLength = 255;
    
    public RichTextBuilder BlockId(string blockId)
    {
        _richTextBlock.BlockId = blockId;
        return this;
    }

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

    public RichTextBuilder AddTextList(RichTextListStyle style, Action<RichTextListBuilder> builder)
    {
        var listBuilder = new RichTextListBuilder(style);
        builder(listBuilder);

        _richTextBlock.Elements.Add(listBuilder.Build());
        return this;
    }

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

    public RichTextBlock Build()
    {
        if (_richTextBlock.BlockId?.Length > MaxIdLength)
        {
            throw new InvalidOperationException($"The block id can only be up to {MaxIdLength} characters long");
        }
        
        return _richTextBlock;
    }
}

public sealed class RichTextSectionElementBuilder
{
    private readonly List<RichTextSectionElement> _elements = new();
    public List<RichTextSectionElement> Build() => _elements;

    public RichTextSectionElementBuilder Add<TElement>(TElement element) where TElement : RichTextSectionElement
    {
        _elements.Add(element);
        return this;
    }
}

public sealed class RichTextListBuilder
{
    private readonly RichTextList _list;

    public RichTextListBuilder(RichTextListStyle style)
    {
        _list = new RichTextList()
            {
                Style = style
            };
    }

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
    
    public RichTextListBuilder Indent(int indent)
    {
        _list.Indent = indent;
        return this;
    }
    
    public RichTextListBuilder Offset(int offset)
    {
        _list.Offset = offset;
        return this;
    }
    
    public RichTextListBuilder Border(int border)
    {
        _list.Border = border;
        return this;
    }

    public RichTextList Build() => _list;
}