using JetBrains.Annotations;

namespace SlackNet.Blocks;

[PublicAPI]
public static class BlockBuilderExtensions
{
    /// <summary>
    /// Add a group of actions to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="createActions">The action which creations the block builder</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static IBlockBuilder AddActions(this IBlockBuilder builder,
        Action<ActionsBlockBuilder> createActions)
    {
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
    /// <param name="blockId">The Id of the block</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static IBlockBuilder AddCall(this IBlockBuilder builder, string callId, string blockId = null)
        => builder.Add<CallBlock>(call =>
            {
                call.CallId = callId;
                call.BlockId = blockId;
            });



    /// <summary>
    /// Add contextual info, which can include both text and images
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="createContext"></param>
    /// <returns></returns>
    public static IBlockBuilder AddContext(this IBlockBuilder builder, Action<ContextBlockBuilder> createContext)
    {
        var contextBuilder = new ContextBlockBuilder();
        createContext(contextBuilder);
        builder.AddBlock(contextBuilder.Build());
        return builder;
    }

    /// <summary>
    /// Add a divider block to the builder
    /// </summary>
    /// <param name="builder">The build instance</param>
    /// <param name="blockId">Optionally, specify the id of the block, see <see cref="Block.BlockId"/> for more info</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static IBlockBuilder AddDivider(this IBlockBuilder builder, string blockId = null)
    {
        var divider = new DividerBlock()
            {
                BlockId = blockId
            };
        builder.AddBlock(divider);
        return builder;
    }

    /// <summary>
    /// Add a file block to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="externalId">The external id of the file</param>
    /// <param name="source">The source of the file</param>
    /// <param name="blockId">The id of the block</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static IBlockBuilder AddFile(this IBlockBuilder builder, string externalId, string source,
        string blockId = null)
        => builder.Add<FileBlock>(file =>
            {
                file.ExternalId = externalId;
                file.Source = source;
                file.BlockId = blockId;
            });

    /// <summary>
    /// Add a header block to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="text">The plain text content for the header</param>
    /// <param name="emoji">Indicates whether emojis in a text field should be escaped into the colon emoji format</param>
    /// <param name="blockId">Optionally, specify the id of the block, see <see cref="Block.BlockId"/> for more info</param>
    /// <returns></returns>
    public static IBlockBuilder AddHeader(this IBlockBuilder builder, PlainText text, string blockId = null)
    {
        var block = new HeaderBlock
            {
                Text = text,
                BlockId = blockId,
            };
        builder.AddBlock(block);

        return builder;
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
    public static IBlockBuilder AddFileImage(this IBlockBuilder builder,
        string imageUrl,
        string altText,
        PlainText title = null,
        string blockId = null)
        => builder.Add<ImageBlock>(image =>
            {
                image.ImageUrl = imageUrl;
                image.AltText = altText;
                image.BlockId = blockId;
                image.Title = title;
            });

    /// <summary>
    /// Add an image from a slack file to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="slackFile">A reference to the slack file to use</param>
    /// <param name="altText">Alt text for the image</param>
    /// <param name="title">Title of the image</param>
    /// <param name="blockId">The id of the block</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static IBlockBuilder AddSlackImage(this IBlockBuilder builder,
        ImageFileReference slackFile,
        string altText,
        PlainText title = null,
        string blockId = null)
        => builder.Add<ImageBlock>(image =>
            {
                image.SlackFile = slackFile;
                image.AltText = altText;
                image.BlockId = blockId;
                image.Title = title;
            });


    /// <summary>
    /// Add an input block to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="label">The label for the input</param>
    /// <param name="createInput">The action which creates the input using a <see cref="InputBlockBuilder{TElement}"/></param>
    /// <typeparam name="TInput">The type of input block to create</typeparam>
    /// <returns>The same instance so calls can be chained</returns>
    public static IBlockBuilder AddInput<TInput>(this IBlockBuilder builder,
        string label,
        Action<InputBlockBuilder<TInput>> createInput)
        where TInput : IActionElement, IInputBlockElement, new()
    {
        var input = new TInput();
        var inputBuilder = new InputBlockBuilder<TInput>(input, label);
        createInput(inputBuilder);
        builder.AddBlock(inputBuilder.ParentBlock);
        return builder;
    }


    public static IBlockBuilder AddRichText(this IBlockBuilder builder, Action<RichTextBuilder> createRichText)
    {
        var blockBuilder = new RichTextBuilder();
        createRichText(blockBuilder);
        builder.AddBlock(blockBuilder.Build());
        return builder;
    }


    public static IBlockBuilder AddSection(this IBlockBuilder builder, string text) =>
        builder.AddSection(section => section.Markdown(text));
    
    public static IBlockBuilder AddPlainTextSection(this IBlockBuilder builder, string text) =>
        builder.AddSection(section => section.Text(text));


    
    public static IBlockBuilder AddSection(this IBlockBuilder builder, Action<SectionBuilder> createSection)
    {
        var sectionBuilder = new SectionBuilder();
        createSection(sectionBuilder);
        builder.AddBlock(sectionBuilder.Build());
        return builder;
    }


    /// <summary>
    /// Add a video block to the builder
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="videoUrl">The URL of the video</param>
    /// <param name="thumbnailUrl">The URL of the thumbnail</param>
    /// <param name="title">The title of the video</param>
    /// <param name="altText">The alt text for the video</param>
    /// <param name="blockId">The id of the block</param>
    /// <param name="description">The description of the video</param>
    /// <param name="providerIconUrl">The URL of the provider icon</param>
    /// <param name="providerName">The name of the provider</param>
    /// <param name="titleUrl">The URL to be embedded. Must match any existing unfurl domains within the app and point to a HTTPS URL</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static IBlockBuilder AddVideo(this IBlockBuilder builder,
        string videoUrl,
        string thumbnailUrl,
        string title,
        string altText,
        string blockId = null,
        string description = null,
        string providerIconUrl = null,
        string providerName = null,
        string titleUrl = null)
        => builder.Add<VideoBlock>(video =>
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
    
    
    
    
}