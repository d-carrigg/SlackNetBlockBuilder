using SlackNet.Blocks;

namespace SlackNetBlockBuilder;

public class SectionBuilder
{
    private readonly SectionBlock _sectionBlock;
    
    /// <summary>
    /// Max number of fields in the section https://api.slack.com/reference/block-kit/blocks#section
    /// </summary>
    public int MaxFields = 10;
    
    /// <summary>
    /// Max length of each field in the section
    /// </summary>
    public int MaxFieldsLength = 2000;
    
    public const int MaxBlockIdLength = 255;


    public SectionBuilder()
    {
        _sectionBlock = new SectionBlock();
    }
    
    public SectionBuilder Text(string text)
    {
        _sectionBlock.Text = text;
        return this;
    }

    public SectionBuilder Markdown(string text, bool verbatim = false)
    {
        _sectionBlock.Text = new Markdown()
            {
                Text = text,
                Verbatim = verbatim
            };
        return this;
    }
    
    public SectionBuilder Fields(params TextObject[] fields)
    {
        _sectionBlock.Fields = fields;
        return this;
    }

    public SectionBuilder AddTextField(string text)
    {
        _sectionBlock.Fields.Add(text);
        return this;
    
    }
    
    public SectionBuilder AddMarkdownField(string text, bool verbatim = false)
    {
        _sectionBlock.Fields.Add(new Markdown()
            {
                Text = text,
                Verbatim = verbatim
            });
        return this;
    }
    
    public SectionBuilder Accessory<TAccessory>(Action<TAccessory> createAccessory)
    where TAccessory : BlockElement, new()
    {
        var accessory = new TAccessory();
        createAccessory(accessory);
        _sectionBlock.Accessory = accessory;
        return this;
    }
    
    public SectionBuilder Accessory(BlockElement accessory)
    {
        _sectionBlock.Accessory = accessory;
        return this;
    }

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