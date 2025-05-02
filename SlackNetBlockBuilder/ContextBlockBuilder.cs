namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="ContextBlock"/>
/// </summary>
public class ContextBlockBuilder
{
    private readonly ContextBlock _contextBlock = new();
    
    /// <summary>
    /// The maximum number of elements allowed in a context block
    /// See https://api.slack.com/reference/block-kit/blocks#context for more information
    /// </summary>
    public const int MaxElements = 10;
    
    /// <summary>
    /// The maximum length of a block ID
    /// </summary>
    public const int MaxBlockIdLength = 255;

    /// <summary>
    /// Sets the block ID for the context block
    /// </summary>
    /// <param name="blockId">The block ID to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ContextBlockBuilder BlockId(string blockId)
    {
        _contextBlock.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Adds plain text to the context block
    /// </summary>
    /// <param name="text">The text to add</param>
    /// <param name="emoji">Whether to enable emoji parsing in the text</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ContextBlockBuilder AddText(string text, bool emoji = true)
    {
        _contextBlock.Elements.Add(new PlainText()
            {
                Text = text,
                Emoji = emoji
            });
        return this;
    }

    /// <summary>
    /// Adds markdown text to the context block
    /// </summary>
    /// <param name="text">The markdown text to add</param>
    /// <param name="verbatim">Whether to skip markdown parsing and display the text verbatim</param>
    /// <returns>The same instance so calls can be chained</returns>
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
    /// Add an image from a URL to the context block
    /// </summary>
    /// <param name="imageUrl">URL pointing to the image</param>
    /// <param name="altText">Alt text for the image</param>
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
    /// Add an image from a Slack file to the context block
    /// </summary>
    /// <param name="slackFile">A reference to the Slack file to use</param>
    /// <param name="altText">Alt text for the image</param>
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

    /// <summary>
    /// Builds the context block
    /// </summary>
    /// <returns>The built context block</returns>
    /// <exception cref="InvalidOperationException">Thrown when the block contains too many elements or the block ID is too long</exception>
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