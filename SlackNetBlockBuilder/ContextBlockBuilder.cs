namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="ContextBlock"/> instances.
/// Context blocks display secondary information, like captions or credits, using text and images.
/// </summary>
public class ContextBlockBuilder
{
    private readonly ContextBlock _contextBlock = new();
    
    /// <summary>
    /// The maximum number of elements (text or image) allowed in a context block.
    /// See <see href="https://api.slack.com/reference/block-kit/blocks#context">Slack API documentation</see> for more information.
    /// </summary>
    public const int MaxElements = 10;
    /// <summary>
    /// The maximum length for a block ID.
    /// </summary>
    public const int MaxBlockIdLength = 255;

    /// <summary>
    /// Sets a unique identifier for this context block.
    /// </summary>
    /// <param name="blockId">The block ID. Maximum length is <see cref="MaxBlockIdLength"/> characters.</param>
    /// <summary>
    /// Sets a unique identifier for the context block.
    /// </summary>
    /// <param name="blockId">The unique identifier to assign to the context block.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ContextBlockBuilder BlockId(string blockId)
    {
        _contextBlock.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Adds a plain text element to the context block.
    /// </summary>
    /// <param name="text">The plain text content.</param>
    /// <param name="emoji">Indicates whether emojis in the text should be escaped (e.g., :smile:) or rendered as Unicode characters. Defaults to true.</param>
    /// <summary>
    /// Adds a plain text element to the context block.
    /// </summary>
    /// <param name="text">The text to display in the context block.</param>
    /// <param name="emoji">Whether to render emoji in the text. Defaults to true.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ContextBlockBuilder AddText(string text, bool emoji = true)
    {
        ArgumentNullException.ThrowIfNull(text);
        _contextBlock.Elements.Add(new PlainText()
            {
                Text = text,
                Emoji = emoji
            });
        return this;
    }

    /// <summary>
    /// Adds a Markdown text element to the context block.
    /// </summary>
    /// <param name="text">The Markdown text content. See <see href="https://api.slack.com/reference/surfaces/formatting#basics">Slack's Markdown guide</see>.</param>
    /// <param name="verbatim">If true, URLs will not be automatically linked. Defaults to false.</param>
    /// <summary>
    /// Adds a Markdown text element to the context block.
    /// </summary>
    /// <param name="text">The Markdown-formatted text to display.</param>
    /// <param name="verbatim">
    /// If true, disables automatic URL linking and preserves the original formatting.
    /// </param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ContextBlockBuilder AddMarkdown(string text, bool verbatim = false)
    {
        ArgumentNullException.ThrowIfNull(text);
        _contextBlock.Elements.Add(new Markdown()
            {
                Text = text,
                Verbatim = verbatim
            });
        return this;
    }

    /// <summary>
    /// Adds an image element from a publicly accessible URL to the context block.
    /// </summary>
    /// <param name="imageUrl">The URL of the image to be displayed. Slack CDN images are not supported.</param>
    /// <param name="altText">Plain text summary of the image for accessibility. Maximum length 2000 characters.</param>
    /// <summary>
    /// Adds an image element to the context block using a publicly accessible image URL.
    /// </summary>
    /// <param name="imageUrl">The URL of the image to display. Must not be null.</param>
    /// <param name="altText">Alternative text describing the image for accessibility.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ContextBlockBuilder AddImageFromUrl(
        string imageUrl,
        string altText)
    {
        ArgumentNullException.ThrowIfNull(imageUrl);
        _contextBlock.Elements.Add(new Image()
            {
                ImageUrl = imageUrl,
                AltText = altText,
            });
        return this;
    }
    
    /// <summary>
    /// Adds an image element from a publicly accessible URL to the context block.
    /// </summary>
    /// <param name="imageUrl">The URL of the image to be displayed. Slack CDN images are not supported.</param>
    /// <param name="altText">Plain text summary of the image for accessibility. Maximum length 2000 characters.</param>
    /// <summary>
        /// Adds an image element to the context block using a publicly accessible image URL.
        /// </summary>
        /// <param name="imageUrl">The URI of the image to display. Must not be null.</param>
        /// <param name="altText">Alternative text describing the image.</param>
        /// <returns>The same builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="imageUrl"/> is null.</exception>
    public ContextBlockBuilder AddImageFromUrl(
        Uri imageUrl,
        string altText) => 
        imageUrl is null ? throw new ArgumentNullException(nameof(imageUrl)) :
        AddImageFromUrl(imageUrl.ToString(), altText);


    /// <summary>
    /// Adds an image element using a file hosted on Slack to the context block.
    /// </summary>
    /// <param name="slackFile">A reference (<see cref="ImageFileReference"/>) to the Slack file to use.</param>
    /// <param name="altText">Plain text summary of the image for accessibility. Maximum length 2000 characters.</param>
    /// <summary>
    /// Adds an image element to the context block using a Slack-hosted file reference.
    /// </summary>
    /// <param name="slackFile">Reference to the Slack-hosted image file.</param>
    /// <param name="altText">Alternative text describing the image for accessibility.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ContextBlockBuilder AddImageFromSlackFile(
        ImageFileReference slackFile,
        string altText)
    {
        ArgumentNullException.ThrowIfNull(slackFile);
        ArgumentNullException.ThrowIfNull(altText);
        _contextBlock.Elements.Add(new Image()
            {
                SlackFile = slackFile,
                AltText = altText,
            });
        return this;
    }


    /// <summary>
    /// Builds the configured <see cref="ContextBlock"/>.
    /// </summary>
    /// <returns>The configured <see cref="ContextBlock"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the number of elements exceeds <see cref="MaxElements"/> or if the block ID exceeds <see cref="MaxBlockIdLength"/> characters.
    /// <summary>
    /// Validates and finalizes the construction of the context block, returning the configured <see cref="ContextBlock"/>.
    /// </summary>
    /// <returns>The constructed <see cref="ContextBlock"/> instance.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the context block contains more than <see cref="MaxElements"/> elements,
    /// if the block ID exceeds <see cref="MaxBlockIdLength"/> characters,
    /// or if no elements have been added.
    /// </exception>
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
        
        // at least 1 element is required
        if (_contextBlock.Elements.Count == 0)
        {
            throw new InvalidOperationException("At least one element is required in a context block");
        }
        
        return _contextBlock;
    }
}