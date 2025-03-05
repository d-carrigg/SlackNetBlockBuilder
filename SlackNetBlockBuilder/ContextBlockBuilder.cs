namespace SlackNet.Blocks;

public class ContextBlockBuilder
{
    private readonly ContextBlock _contextBlock = new();
    
    /// <summary>
    /// See https://api.slack.com/reference/block-kit/blocks#context for more information
    /// </summary>
    public const int MaxElements = 10;
    public const int MaxBlockIdLength = 255;

    public ContextBlockBuilder BlockId(string blockId)
    {
        _contextBlock.BlockId = blockId;
        return this;
    }

    public ContextBlockBuilder AddText(string text, bool emoji = true)
    {
        _contextBlock.Elements.Add(new PlainText()
            {
                Text = text,
                Emoji = emoji
            });
        return this;
    }

    public ContextBlockBuilder AddMarkdown(string text, bool verbatim = false)
    {
        _contextBlock.Elements.Add(new Markdown()
            {
                Text = text,
                Verbatim = verbatim
            });
        return this;
    }

    /// <summary>
    /// Add an image from a url to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="imageUrl">URL pointing to the image</param>
    /// <param name="altText">Alt text for the image</param>
    /// <param name="title">Title of the image</param>
    /// <param name="blockId">The id of the block</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ContextBlockBuilder AddFileImage(
        string imageUrl,
        string altText)
    {
        _contextBlock.Elements.Add(new Image()
            {
                ImageUrl = imageUrl,
                AltText = altText,
            });
        return this;
    }


    /// <summary>
    /// Add an image from a slack file to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="slackFile">A reference to the slack file to use</param>
    /// <param name="altText">Alt text for the image</param>
    /// <param name="title">Title of the image</param>
    /// <param name="blockId">The id of the block</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ContextBlockBuilder AddSlackImage(
        ImageFileReference slackFile,
        string altText)
    {
        _contextBlock.Elements.Add(new Image()
            {
                SlackFile = slackFile,
                AltText = altText,
            });
        return this;
    }


    public ContextBlock Build()
    {
        if (_contextBlock.Elements.Count > MaxElements)
        {
            throw new InvalidOperationException($"Context blocks can only contain up to {MaxElements} elements");
        }
        
        if(_contextBlock.BlockId?.Length > MaxBlockIdLength)
        {
            throw new InvalidOperationException($"The block id can only be up to {MaxBlockIdLength} characters long");
        }
        
        return _contextBlock;
    }
}