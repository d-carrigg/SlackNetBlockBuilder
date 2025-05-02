using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides extension methods for configuring and adding blocks to a <see cref="BlockBuilder"/>.
/// </summary>
[PublicAPI]
public static class BlockBuilderExtensions
{
    
    /// <summary>
    /// Add a group of actions to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="createActions">An action that configures the <see cref="ActionsBlock"/> using an <see cref="ActionsBlockBuilder"/>.</param>
    /// <summary>
    /// Adds an Actions block to the builder, allowing configuration of interactive elements such as buttons or menus.
    /// </summary>
    /// <param name="createActions">A delegate to configure the actions within the block.</param>
    /// <returns>The same <see cref="IBlockBuilder"/> instance for method chaining.</returns>
    public static IBlockBuilder AddActions(this IBlockBuilder builder,
        Action<ActionsBlockBuilder> createActions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(createActions);
        var block = ActionsBlockBuilder.Create();
        
        createActions(block);
        builder.AddBlock(block.Build());
        return builder;
    }

    /// <summary>
    /// Add a call block to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="callId">The Id of the call</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <summary>
            /// Adds a Slack Call block referencing a call by its unique ID.
            /// </summary>
            /// <param name="callId">The unique identifier for the call (maximum 255 characters).</param>
            /// <param name="blockId">An optional unique identifier for this block.</param>
            /// <returns>The same <see cref="IBlockBuilder"/> instance for chaining.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="callId"/> is null.</exception>
            /// <exception cref="ArgumentException">Thrown if <paramref name="callId"/> exceeds 255 characters.</exception>
    public static IBlockBuilder AddCall(this IBlockBuilder builder, string callId, string? blockId = null)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            callId is null ? throw new ArgumentNullException(nameof(callId)) :
            callId.Length > 255 ? throw new ArgumentException("Call ID must be 255 characters or less.",nameof(callId)) :
            builder.Add<CallBlock>(call =>
            {
                call.CallId = callId;
                call.BlockId = blockId;
            });



    /// <summary>
    /// Adds a context block, used to display secondary information such as captions or credits.
    /// Context blocks can contain text (mrkdwn or plain_text) and image elements.
    /// </summary>
    /// <param name="builder">The block builder instance.</param>
    /// <param name="createContext">An action that configures the <see cref="ContextBlock"/> using a <see cref="ContextBlockBuilder"/>.</param>
    /// <summary>
    /// Adds a context block to the builder, allowing display of secondary information such as images or text.
    /// </summary>
    /// <param name="createContext">An action to configure the context block's content.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public static IBlockBuilder AddContext(this IBlockBuilder builder, Action<ContextBlockBuilder> createContext)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(createContext);
        
        var contextBuilder = new ContextBlockBuilder();
        createContext(contextBuilder);
        builder.AddBlock(contextBuilder.Build());
        return builder;
    }

    /// <summary>
    /// Add a divider block to the builder
    /// </summary>
    /// <param name="builder">The build instance</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <summary>
    /// Adds a divider block to visually separate content within a Slack Block Kit layout.
    /// </summary>
    /// <param name="blockId">An optional unique identifier for the divider block.</param>
    /// <returns>The same <see cref="IBlockBuilder"/> instance for method chaining.</returns>
    public static IBlockBuilder AddDivider(this IBlockBuilder builder, string? blockId = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var divider = new DividerBlock()
            {
                BlockId = blockId
            };
        builder.AddBlock(divider);
        return builder;
    }

    /// <summary>
    /// Adds a file block to display a remote file.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="externalId">The external ID of the remote file. This ID is assigned by the uploading app.</param>
    /// <param name="source">Always "remote" for remote files.</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <summary>
            /// Adds a file block to display a remote file in the Slack message.
            /// </summary>
            /// <param name="externalId">The external unique identifier for the file to display.</param>
            /// <param name="source">The source of the file, typically "remote".</param>
            /// <param name="blockId">An optional unique identifier for this block.</param>
            /// <returns>The same builder instance for method chaining.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is null.</exception>
    public static IBlockBuilder AddFile(this IBlockBuilder builder, string externalId, string source = "remote",
        string? blockId = null)
        =>
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Add<FileBlock>(file =>
            {
                file.ExternalId = externalId;
                file.Source = source; // Typically "remote"
                file.BlockId = blockId;
            });

    /// <summary>
    /// Adds a header block, which displays a larger text title.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="text">A plain_text text object for the header. Maximum length 150 characters.</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <summary>
    /// Adds a header block with the specified plain text to the block builder.
    /// </summary>
    /// <param name="text">The plain text to display as the header.</param>
    /// <param name="blockId">An optional unique identifier for the block.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public static IBlockBuilder AddHeader(this IBlockBuilder builder, PlainText text, string? blockId = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(text);
        var block = new HeaderBlock
            {
                Text = text,
                BlockId = blockId,
            };
        builder.AddBlock(block);

        return builder;
    }

    /// <summary>
    /// Adds an image block from a publicly accessible URL.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="imageUrl">The URL of the image to be displayed. Slack CDN images are not supported.</param>
    /// <param name="altText">Plain text summary of the image for accessibility. Maximum length 2000 characters.</param>
    /// <param name="title">An optional plain_text title for the image. Maximum length 2000 characters.</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <summary>
            /// Adds an image block to the builder using a public image URL.
            /// </summary>
            /// <param name="imageUrl">The URL of the image to display.</param>
            /// <param name="altText">A plain-text summary of the image for accessibility.</param>
            /// <param name="title">Optional title displayed above the image.</param>
            /// <param name="blockId">Optional unique identifier for the block.</param>
            /// <returns>The same <see cref="IBlockBuilder"/> instance for chaining.</returns>
    public static IBlockBuilder AddImageFromUrl(this IBlockBuilder builder,
        string imageUrl,
        string altText,
        PlainText? title = null,
        string? blockId = null)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Add<ImageBlock>(image =>
            {
                image.ImageUrl = imageUrl;
                image.AltText = altText;
                image.BlockId = blockId;
                image.Title = title;
            });
    
    /// <summary>
    /// Adds an image block from a publicly accessible URL.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="imageUrl">The URL of the image to be displayed. Slack CDN images are not supported.</param>
    /// <param name="altText">Plain text summary of the image for accessibility. Maximum length 2000 characters.</param>
    /// <param name="title">An optional plain_text title for the image. Maximum length 2000 characters.</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <summary>
        /// Adds an image block to the builder using a public image URL.
        /// </summary>
        /// <param name="imageUrl">The URI of the image to display. Must not be null.</param>
        /// <param name="altText">A plain-text description of the image for accessibility.</param>
        /// <param name="title">Optional title displayed above the image.</param>
        /// <param name="blockId">An optional unique identifier for this block.</param>
        /// <returns>The same <see cref="IBlockBuilder"/> instance for chaining.</returns>
    public static IBlockBuilder AddImageFromUrl(this IBlockBuilder builder,
        Uri imageUrl,
        string altText,
        PlainText? title = null,
        string? blockId = null) => 
    imageUrl is null ? throw new ArgumentNullException(nameof(imageUrl)) :
        builder.AddImageFromUrl(imageUrl.ToString(), altText, title, blockId);

    /// <summary>
    /// Adds an image block using a file hosted on Slack.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="slackFile">A reference (<see cref="ImageFileReference"/>) to the Slack file to use.</param>
    /// <param name="altText">Plain text summary of the image for accessibility. Maximum length 2000 characters.</param>
    /// <param name="title">An optional plain_text title for the image. Maximum length 2000 characters.</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <summary>
            /// Adds an image block referencing a Slack-hosted file to the block builder.
            /// </summary>
            /// <param name="slackFile">The Slack file reference to display as an image.</param>
            /// <param name="altText">Alternative text for the image, used for accessibility.</param>
            /// <param name="title">Optional title displayed above the image.</param>
            /// <param name="blockId">Optional unique identifier for the block.</param>
            /// <returns>The same <see cref="IBlockBuilder"/> instance for chaining.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is null.</exception>
    public static IBlockBuilder AddImageFromSlackFile(this IBlockBuilder builder,
        ImageFileReference slackFile,
        string altText,
        PlainText? title = null,
        string? blockId = null)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Add<ImageBlock>(image =>
            {
                image.SlackFile = slackFile;
                image.AltText = altText;
                image.BlockId = blockId;
                image.Title = title;
            });


    /// <summary>
    /// Adds an input block that collects information from users.
    /// </summary>
    /// <param name="builder">The block builder instance</param>
    /// <param name="label">A plain_text label for the input element. Maximum length 2000 characters.</param>
    /// <param name="createInput">An action that configures the input element (e.g., <see cref="PlainTextInput"/>) using an <see cref="InputBlockBuilder{TElement}"/>.</param>
    /// <typeparam name="TInput">The type of input element to add (e.g., <see cref="PlainTextInput"/>, <see cref="DatePicker"/>).</typeparam>
    /// <summary>
    /// Adds an input block with a specified label and configurable input element to the block builder.
    /// </summary>
    /// <param name="label">The label for the input block. Must not be null or whitespace.</param>
    /// <param name="createInput">An action to configure the input element within the block.</param>
    /// <typeparam name="TInput">The type of input element to include in the block.</typeparam>
    /// <returns>The same <see cref="IBlockBuilder"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="label"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="createInput"/> is null.</exception>
    public static IBlockBuilder AddInput<TInput>(this IBlockBuilder builder,
        string label,
        Action<InputBlockBuilder<TInput>> createInput)
        where TInput : class, IActionElement, IInputBlockElement, new()
    {
        if(string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label cannot be null or whitespace.", nameof(label));
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(createInput);
        var input = new TInput();
        var inputBuilder = new InputBlockBuilder<TInput>(input, label);
        createInput(inputBuilder);
        builder.AddBlock(inputBuilder.ParentBlock);
        return builder;
    }


    /// <summary>
    /// Adds a rich text block, a complex block that allows for advanced formatting.
    /// </summary>
    /// <param name="builder">The block builder instance.</param>
    /// <param name="createRichText">An action that configures the rich text content using a <see cref="RichTextBuilder"/>.</param>
    /// <summary>
    /// Adds a rich text block to the builder, allowing advanced formatting and layout using a <see cref="RichTextBuilder"/>.
    /// </summary>
    /// <param name="createRichText">An action to configure the rich text content.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public static IBlockBuilder AddRichText(this IBlockBuilder builder, Action<RichTextBuilder> createRichText)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(createRichText);
        var blockBuilder = new RichTextBuilder();
        createRichText(blockBuilder);
        builder.AddBlock(blockBuilder.Build());
        return builder;
    }


    /// <summary>
    /// Adds a section block with markdown text content.
    /// </summary>
    /// <param name="builder">The block builder instance.</param>
    /// <param name="text">The markdown text for the section. Maximum length 3000 characters.</param>
    /// <summary>
        /// Adds a section block with the specified markdown text to the builder.
        /// </summary>
        /// <param name="text">The markdown text to display in the section block.</param>
        /// <returns>The same builder instance for method chaining.</returns>
    public static IBlockBuilder AddSection(this IBlockBuilder builder, string text) =>
        builder.AddSection(section => section.Markdown(text));
    
    /// <summary>
    /// Adds a section block with plain text content.
    /// </summary>
    /// <param name="builder">The block builder instance.</param>
    /// <param name="text">The plain text for the section. Maximum length 3000 characters.</param>
    /// <summary>
        /// Adds a section block with plain text to the builder.
        /// </summary>
        /// <param name="text">The plain text content for the section block.</param>
        /// <returns>The same builder instance for method chaining.</returns>
    public static IBlockBuilder AddPlainTextSection(this IBlockBuilder builder, string text) =>
        builder.AddSection(section => section.Text(text));


    
    /// <summary>
    /// Adds a section block, allowing flexible configuration of text, fields, and accessory elements.
    /// </summary>
    /// <param name="builder">The block builder instance.</param>
    /// <param name="createSection">An action that configures the <see cref="SectionBlock"/> using a <see cref="SectionBuilder"/>.</param>
    /// <summary>
    /// Adds a section block to the builder, allowing flexible configuration of text, fields, and accessory elements.
    /// </summary>
    /// <param name="createSection">An action to configure the section block's content and layout.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public static IBlockBuilder AddSection(this IBlockBuilder builder, Action<SectionBuilder> createSection)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(createSection);
        var sectionBuilder = new SectionBuilder();
        createSection(sectionBuilder);
        builder.AddBlock(sectionBuilder.Build());
        return builder;
    }


    /// <summary>
    /// Adds a video block, which embeds a video player from a supported provider.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="videoUrl">The URL of the video to embed. Must be one of the <see href="https://api.slack.com/reference/block-kit/blocks#video_providers">supported video providers</see>.</param>
    /// <param name="thumbnailUrl">The URL of a thumbnail image for the video.</param>
    /// <param name="title">A plain_text title for the video. Maximum length 200 characters.</param>
    /// <param name="altText">A plain_text description of the video for accessibility. Maximum length 2000 characters.</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <param name="description">An optional plain_text description shown below the title. Maximum length 200 characters.</param>
    /// <param name="providerIconUrl">An optional URL for an icon representing the video provider.</param>
    /// <param name="providerName">An optional plain_text name of the video provider. Maximum length 30 characters.</param>
    /// <param name="titleUrl">An optional URL that makes the video title clickable. Must be an HTTPS URL. Requires <c>videoUrl</c> to belong to an <see href="https://api.slack.com/apps/managing#configuring-unfurls">unfurl domain</see> configured for the app.</param>
    /// <summary>
            /// Adds a video block to the builder, embedding a video player from a supported provider with the specified URLs, title, and alt text.
            /// </summary>
            /// <param name="videoUrl">The URL of the video to embed. Must not be null or whitespace.</param>
            /// <param name="thumbnailUrl">The URL of the video's thumbnail image. Must not be null or whitespace.</param>
            /// <param name="title">The title of the video. Must not be null or whitespace.</param>
            /// <param name="altText">Alternative text for the video, used for accessibility. Must not be null or whitespace.</param>
            /// <param name="blockId">A unique identifier for this block. Optional.</param>
            /// <param name="description">A description of the video. Optional.</param>
            /// <param name="providerIconUrl">URL of the provider's icon. Optional.</param>
            /// <param name="providerName">Name of the video provider. Optional.</param>
            /// <param name="titleUrl">URL to link the title. Optional.</param>
            /// <returns>The same <see cref="IBlockBuilder"/> instance for chaining.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is null.</exception>
            /// <exception cref="ArgumentException">Thrown if <paramref name="videoUrl"/>, <paramref name="thumbnailUrl"/>, <paramref name="title"/>, or <paramref name="altText"/> is null or whitespace.</exception>
    [SuppressMessage("Design", "CA1054:URI-like parameters should not be strings")]
    public static IBlockBuilder AddVideo(this IBlockBuilder builder,
        string videoUrl,
        string thumbnailUrl,
        string title,
        string altText,
        string? blockId = null,
        string? description = null,
        string? providerIconUrl = null,
        string? providerName = null,
        string? titleUrl = null)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            // validate videoUrl and thumbnailUrl, title, altText
            string.IsNullOrWhiteSpace(videoUrl) ? throw new ArgumentException("Video URL cannot be null or whitespace.", nameof(videoUrl)) :
            string.IsNullOrWhiteSpace(thumbnailUrl) ? throw new ArgumentException("Thumbnail URL cannot be null or whitespace.", nameof(thumbnailUrl)) :
            string.IsNullOrWhiteSpace(title) ? throw new ArgumentException("Title cannot be null or whitespace.", nameof(title)) :
            string.IsNullOrWhiteSpace(altText) ? throw new ArgumentException("Alt text cannot be null or whitespace.", nameof(altText)) :
            builder.Add<VideoBlock>(video =>
            {
                video.VideoUrl = videoUrl;
                video.ThumbnailUrl = thumbnailUrl;
                video.Title = title;
                video.AltText = altText;
                video.BlockId = blockId;

                video.Description = description;
                video.ProviderIconUrl = providerIconUrl;
                video.ProviderName = providerName;
                video.TitleUrl = titleUrl;
            });
    
    /// <summary>
    /// Adds a video block, which embeds a video player from a supported provider.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="videoUrl">The URL of the video to embed. Must be one of the <see href="https://api.slack.com/reference/block-kit/blocks#video_providers">supported video providers</see>.</param>
    /// <param name="thumbnailUrl">The URL of a thumbnail image for the video.</param>
    /// <param name="title">A plain_text title for the video. Maximum length 200 characters.</param>
    /// <param name="altText">A plain_text description of the video for accessibility. Maximum length 2000 characters.</param>
    /// <param name="blockId">A unique identifier for this block. Maximum length is 255 characters. If not specified, one will be generated.</param>
    /// <param name="description">An optional plain_text description shown below the title. Maximum length 200 characters.</param>
    /// <param name="providerIconUrl">An optional URL for an icon representing the video provider.</param>
    /// <param name="providerName">An optional plain_text name of the video provider. Maximum length 30 characters.</param>
    /// <param name="titleUrl">An optional URL that makes the video title clickable. Must be an HTTPS URL. Requires <c>videoUrl</c> to belong to an <see href="https://api.slack.com/apps/managing#configuring-unfurls">unfurl domain</see> configured for the app.</param>
    /// <summary>
    /// Adds a video block to the builder, embedding a video player from a supported provider using <see cref="Uri"/> parameters.
    /// </summary>
    /// <param name="videoUrl">The URL of the video to embed. Must be a valid, supported video provider URL.</param>
    /// <param name="thumbnailUrl">The URL of the video's thumbnail image.</param>
    /// <param name="title">The title of the video.</param>
    /// <param name="altText">Alternative text for the video, used for accessibility.</param>
    /// <param name="blockId">A unique identifier for this block (optional).</param>
    /// <param name="description">A description of the video (optional).</param>
    /// <param name="providerIconUrl">The URL of the provider's icon image (optional).</param>
    /// <param name="providerName">The name of the video provider (optional).</param>
    /// <param name="titleUrl">A URL to link the video title to (optional).</param>
    /// <returns>The same <see cref="IBlockBuilder"/> instance for chaining.</returns>
    public static IBlockBuilder AddVideo(this IBlockBuilder builder,
        Uri videoUrl,
        Uri thumbnailUrl,
        string title,
        string altText,
        string? blockId = null,
        string? description = null,
        Uri? providerIconUrl = null,
        string? providerName = null,
        Uri? titleUrl = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(videoUrl);
        ArgumentNullException.ThrowIfNull(thumbnailUrl);
        return builder.AddVideo(videoUrl.ToString(), thumbnailUrl.ToString(), title, altText, blockId, description,
            providerIconUrl?.ToString(), providerName, titleUrl?.ToString());
    }

 
}