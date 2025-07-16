using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(BlockBuilderExtensions))]
public class BlockBuilderExtensionsTest
{
    private readonly BlockBuilder _blockBuilder;

    public BlockBuilderExtensionsTest()
    {
        _blockBuilder = new BlockBuilder();
    }

    // === AddActions Tests ===

    [Fact]
    public void AddActions_WithValidConfiguration_AddsActionsBlock()
    {
        // Act
        var result = _blockBuilder.AddActions(actions => actions.AddButton("btn1", "Click me"));

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        Assert.IsType<ActionsBlock>(_blockBuilder.Build()[0]);
    }

    [Fact]
    public void AddActions_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddActions(actions => { }));
    }

    [Fact]
    public void AddActions_WithNullCreateActions_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddActions(null));
    }

    // === AddCall Tests ===

    [Fact]
    public void AddCall_WithValidCallId_AddsCallBlock()
    {
        // Act
        var result = _blockBuilder.AddCall("call123");

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var callBlock = Assert.IsType<CallBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("call123", callBlock.CallId);
        Assert.Null(callBlock.BlockId);
    }

    [Fact]
    public void AddCall_WithBlockId_AddsCallBlockWithBlockId()
    {
        // Act
        var result = _blockBuilder.AddCall("call123", "block456");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var callBlock = Assert.IsType<CallBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("call123", callBlock.CallId);
        Assert.Equal("block456", callBlock.BlockId);
    }

    [Fact]
    public void AddCall_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddCall("call123"));
    }

    [Fact]
    public void AddCall_WithNullCallId_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddCall(null));
    }

    [Fact]
    public void AddCall_WithCallIdTooLong_ThrowsArgumentException()
    {
        // Arrange
        var longCallId = new string('A', 256);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddCall(longCallId));
        Assert.Equal("Call ID must be 255 characters or less. (Parameter 'callId')", exception.Message);
    }

    [Fact]
    public void AddCall_WithMaxLengthCallId_WorksCorrectly()
    {
        // Arrange
        var maxCallId = new string('A', 255);

        // Act
        _blockBuilder.AddCall(maxCallId);

        // Assert
        var callBlock = Assert.IsType<CallBlock>(_blockBuilder.Build()[0]);
        Assert.Equal(maxCallId, callBlock.CallId);
    }

    // === AddContext Tests ===

    [Fact]
    public void AddContext_WithValidConfiguration_AddsContextBlock()
    {
        // Act
        var result = _blockBuilder.AddContext(context => context.AddText("Context text"));

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        Assert.IsType<ContextBlock>(_blockBuilder.Build()[0]);
    }

    [Fact]
    public void AddContext_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddContext(context => { }));
    }

    [Fact]
    public void AddContext_WithNullCreateContext_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddContext(null));
    }

    // === AddDivider Tests ===

    [Fact]
    public void AddDivider_WithoutBlockId_AddsDividerBlock()
    {
        // Act
        var result = _blockBuilder.AddDivider();

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var dividerBlock = Assert.IsType<DividerBlock>(_blockBuilder.Build()[0]);
        Assert.Null(dividerBlock.BlockId);
    }

    [Fact]
    public void AddDivider_WithBlockId_AddsDividerBlockWithBlockId()
    {
        // Act
        var result = _blockBuilder.AddDivider("divider_block");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var dividerBlock = Assert.IsType<DividerBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("divider_block", dividerBlock.BlockId);
    }

    [Fact]
    public void AddDivider_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddDivider());
    }

    // === AddFile Tests ===

    [Fact]
    public void AddFile_WithValidParameters_AddsFileBlock()
    {
        // Act
        var result = _blockBuilder.AddFile("external123");

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var fileBlock = Assert.IsType<FileBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("external123", fileBlock.ExternalId);
        Assert.Equal("remote", fileBlock.Source);
        Assert.Null(fileBlock.BlockId);
    }

    [Fact]
    public void AddFile_WithCustomSource_AddsFileBlockWithCustomSource()
    {
        // Act
        var result = _blockBuilder.AddFile("external123", "custom_source", "file_block");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var fileBlock = Assert.IsType<FileBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("external123", fileBlock.ExternalId);
        Assert.Equal("custom_source", fileBlock.Source);
        Assert.Equal("file_block", fileBlock.BlockId);
    }

    [Fact]
    public void AddFile_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddFile("external123"));
    }

    // === AddHeader Tests ===

    [Fact]
    public void AddHeader_WithValidText_AddsHeaderBlock()
    {
        // Arrange
        var headerText = new PlainText { Text = "Header Title" };

        // Act
        var result = _blockBuilder.AddHeader(headerText);

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var headerBlock = Assert.IsType<HeaderBlock>(_blockBuilder.Build()[0]);
        Assert.Same(headerText, headerBlock.Text);
        Assert.Null(headerBlock.BlockId);
    }

    [Fact]
    public void AddHeader_WithBlockId_AddsHeaderBlockWithBlockId()
    {
        // Arrange
        var headerText = new PlainText { Text = "Header Title" };

        // Act
        var result = _blockBuilder.AddHeader(headerText, "header_block");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var headerBlock = Assert.IsType<HeaderBlock>(_blockBuilder.Build()[0]);
        Assert.Same(headerText, headerBlock.Text);
        Assert.Equal("header_block", headerBlock.BlockId);
    }

    [Fact]
    public void AddHeader_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;
        var headerText = new PlainText { Text = "Header Title" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddHeader(headerText));
    }

    [Fact]
    public void AddHeader_WithNullText_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddHeader(null));
    }

    // === AddImageFromUrl Tests ===

    [Fact]
    public void AddImageFromUrl_WithStringUrl_AddsImageBlock()
    {
        // Act
        var result = _blockBuilder.AddImageFromUrl("https://example.com/image.png", "Alt text");

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var imageBlock = Assert.IsType<ImageBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("https://example.com/image.png", imageBlock.ImageUrl);
        Assert.Equal("Alt text", imageBlock.AltText);
        Assert.Null(imageBlock.Title);
        Assert.Null(imageBlock.BlockId);
    }

    [Fact]
    public void AddImageFromUrl_WithStringUrlAndTitle_AddsImageBlockWithTitle()
    {
        // Arrange
        var title = new PlainText { Text = "Image Title" };

        // Act
        var result = _blockBuilder.AddImageFromUrl("https://example.com/image.png", "Alt text", title, "image_block");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var imageBlock = Assert.IsType<ImageBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("https://example.com/image.png", imageBlock.ImageUrl);
        Assert.Equal("Alt text", imageBlock.AltText);
        Assert.Same(title, imageBlock.Title);
        Assert.Equal("image_block", imageBlock.BlockId);
    }

    [Fact]
    public void AddImageFromUrl_WithUriUrl_AddsImageBlock()
    {
        // Arrange
        var uri = new Uri("https://example.com/image.png");

        // Act
        var result = _blockBuilder.AddImageFromUrl(uri, "Alt text");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var imageBlock = Assert.IsType<ImageBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("https://example.com/image.png", imageBlock.ImageUrl);
        Assert.Equal("Alt text", imageBlock.AltText);
    }

    [Fact]
    public void AddImageFromUrl_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddImageFromUrl("https://example.com/image.png", "Alt text"));
    }

    [Fact]
    public void AddImageFromUrl_WithNullStringUrl_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddImageFromUrl((string)null, "Alt text"));
    }

    [Fact]
    public void AddImageFromUrl_WithEmptyStringUrl_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddImageFromUrl("", "Alt text"));
    }

    [Fact]
    public void AddImageFromUrl_WithNullUri_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddImageFromUrl((Uri)null, "Alt text"));
    }

    [Fact]
    public void AddImageFromUrl_WithNullAltText_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddImageFromUrl("https://example.com/image.png", null));
        Assert.Equal("Alt text cannot be null or whitespace. (Parameter 'altText')", exception.Message);
    }

    [Fact]
    public void AddImageFromUrl_WithWhitespaceAltText_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddImageFromUrl("https://example.com/image.png", "   "));
        Assert.Equal("Alt text cannot be null or whitespace. (Parameter 'altText')", exception.Message);
    }

    // === AddImageFromSlackFile Tests ===

    [Fact]
    public void AddImageFromSlackFile_WithValidFile_AddsImageBlock()
    {
        // Arrange
        var slackFile = new ImageFileReference { Id = "file123" };

        // Act
        var result = _blockBuilder.AddImageFromSlackFile(slackFile, "Alt text");

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var imageBlock = Assert.IsType<ImageBlock>(_blockBuilder.Build()[0]);
        Assert.Same(slackFile, imageBlock.SlackFile);
        Assert.Equal("Alt text", imageBlock.AltText);
        Assert.Null(imageBlock.Title);
        Assert.Null(imageBlock.BlockId);
    }

    [Fact]
    public void AddImageFromSlackFile_WithTitleAndBlockId_AddsImageBlockWithTitleAndBlockId()
    {
        // Arrange
        var slackFile = new ImageFileReference { Id = "file123" };
        var title = new PlainText { Text = "Image Title" };

        // Act
        var result = _blockBuilder.AddImageFromSlackFile(slackFile, "Alt text", title, "image_block");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var imageBlock = Assert.IsType<ImageBlock>(_blockBuilder.Build()[0]);
        Assert.Same(slackFile, imageBlock.SlackFile);
        Assert.Equal("Alt text", imageBlock.AltText);
        Assert.Same(title, imageBlock.Title);
        Assert.Equal("image_block", imageBlock.BlockId);
    }

    [Fact]
    public void AddImageFromSlackFile_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;
        var slackFile = new ImageFileReference { Id = "file123" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddImageFromSlackFile(slackFile, "Alt text"));
    }

    // === AddInput Tests ===

    [Fact]
    public void AddInput_WithValidParameters_AddsInputBlock()
    {
        // Act
        var result = _blockBuilder.AddInput<DatePicker>("Select Date", input => input.ActionId("date_picker"));

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var inputBlock = Assert.IsType<InputBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("Select Date", inputBlock.Label.Text);
        Assert.IsType<DatePicker>(inputBlock.Element);
        Assert.Equal("date_picker", inputBlock.Element.ActionId);
    }

    [Fact]
    public void AddInput_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddInput<DatePicker>("Label", input => { }));
    }

    [Fact]
    public void AddInput_WithNullLabel_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddInput<DatePicker>(null, input => { }));
        Assert.Equal("Label cannot be null or whitespace. (Parameter 'label')", exception.Message);
    }

    [Fact]
    public void AddInput_WithWhitespaceLabel_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddInput<DatePicker>("   ", input => { }));
        Assert.Equal("Label cannot be null or whitespace. (Parameter 'label')", exception.Message);
    }

    [Fact]
    public void AddInput_WithNullCreateInput_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddInput<DatePicker>("Label", null));
    }

    // === AddRichText Tests ===

    [Fact]
    public void AddRichText_WithValidConfiguration_AddsRichTextBlock()
    {
        // Act
        var result = _blockBuilder.AddRichText(richText => richText.AddSection(section => section.Add(new RichTextText { Text = "Rich text" })));
        
        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        Assert.IsType<RichTextBlock>(_blockBuilder.Build()[0]);
    }

    [Fact]
    public void AddRichText_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddRichText(richText => { }));
    }

    [Fact]
    public void AddRichText_WithNullCreateRichText_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddRichText(null));
    }

    // === AddSection Tests ===

    [Fact]
    public void AddSection_WithMarkdownText_AddsSectionBlock()
    {
        // Act
        var result = _blockBuilder.AddSection("*Bold text*");

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var sectionBlock = Assert.IsType<SectionBlock>(_blockBuilder.Build()[0]);
        Assert.IsType<Markdown>(sectionBlock.Text);
        Assert.Equal("*Bold text*", sectionBlock.Text.Text);
    }

    [Fact]
    public void AddPlainTextSection_WithPlainText_AddsSectionBlock()
    {
        // Act
        var result = _blockBuilder.AddPlainTextSection("Plain text");

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var sectionBlock = Assert.IsType<SectionBlock>(_blockBuilder.Build()[0]);
        Assert.IsType<PlainText>(sectionBlock.Text);
        Assert.Equal("Plain text", sectionBlock.Text.Text);
    }

    [Fact]
    public void AddSection_WithConfiguration_AddsSectionBlock()
    {
        // Act
        var result = _blockBuilder.AddSection(section => section.Markdown("*Bold text*"));
        
        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var sectionBlock = Assert.IsType<SectionBlock>(_blockBuilder.Build()[0]);
        Assert.IsType<Markdown>(sectionBlock.Text);
        Assert.Equal("*Bold text*", sectionBlock.Text.Text);
    }

    [Fact]
    public void AddSection_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddSection("Text"));
    }

    [Fact]
    public void AddSection_WithNullCreateSection_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddSection((Action<SectionBuilder>)null));
    }

    // === AddVideo Tests ===

    [Fact]
    public void AddVideo_WithStringUrls_AddsVideoBlock()
    {
        // Act
        var result = _blockBuilder.AddVideo(
            "https://example.com/video.mp4",
            "https://example.com/thumbnail.jpg",
            "Video Title",
            "Video description");

        // Assert
        Assert.Same(_blockBuilder, result);
        Assert.Single(_blockBuilder.Build());
        
        var videoBlock = Assert.IsType<VideoBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("https://example.com/video.mp4", videoBlock.VideoUrl);
        Assert.Equal("https://example.com/thumbnail.jpg", videoBlock.ThumbnailUrl);
        Assert.Equal("Video Title", videoBlock.Title);
        Assert.Equal("Video description", videoBlock.AltText);
        Assert.Null(videoBlock.BlockId);
    }

    [Fact]
    public void AddVideo_WithAllParameters_AddsVideoBlockWithAllProperties()
    {
        // Act
        var result = _blockBuilder.AddVideo(
            "https://example.com/video.mp4",
            "https://example.com/thumbnail.jpg",
            "Video Title",
            "Video description",
            "video_block",
            "Video description text",
            "https://example.com/provider-icon.png",
            "ExampleProvider",
            "https://example.com/video-link");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var videoBlock = Assert.IsType<VideoBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("https://example.com/video.mp4", videoBlock.VideoUrl);
        Assert.Equal("https://example.com/thumbnail.jpg", videoBlock.ThumbnailUrl);
        Assert.Equal("Video Title", videoBlock.Title);
        Assert.Equal("Video description", videoBlock.AltText);
        Assert.Equal("video_block", videoBlock.BlockId);
        Assert.Equal("Video description text", videoBlock.Description.Text);
        Assert.Equal("https://example.com/provider-icon.png", videoBlock.ProviderIconUrl);
        Assert.Equal("ExampleProvider", videoBlock.ProviderName);
        Assert.Equal("https://example.com/video-link", videoBlock.TitleUrl);
    }

    [Fact]
    public void AddVideo_WithUriUrls_AddsVideoBlock()
    {
        // Arrange
        var videoUri = new Uri("https://example.com/video.mp4");
        var thumbnailUri = new Uri("https://example.com/thumbnail.jpg");

        // Act
        var result = _blockBuilder.AddVideo(videoUri, thumbnailUri, "Video Title", "Video description");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var videoBlock = Assert.IsType<VideoBlock>(_blockBuilder.Build()[0]);
        Assert.Equal("https://example.com/video.mp4", videoBlock.VideoUrl);
        Assert.Equal("https://example.com/thumbnail.jpg", videoBlock.ThumbnailUrl);
        Assert.Equal("Video Title", videoBlock.Title);
        Assert.Equal("Video description", videoBlock.AltText);
    }

    [Fact]
    public void AddVideo_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        IBlockBuilder builder = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddVideo("video", "thumb", "title", "alt"));
    }

    [Fact]
    public void AddVideo_WithNullVideoUrl_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddVideo(null, "thumb", "title", "alt"));
        Assert.Equal("Video URL cannot be null or whitespace. (Parameter 'videoUrl')", exception.Message);
    }

    [Fact]
    public void AddVideo_WithNullThumbnailUrl_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddVideo("video", null, "title", "alt"));
        Assert.Equal("Thumbnail URL cannot be null or whitespace. (Parameter 'thumbnailUrl')", exception.Message);
    }

    [Fact]
    public void AddVideo_WithNullTitle_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddVideo("video", "thumb", null, "alt"));
        Assert.Equal("Title cannot be null or whitespace. (Parameter 'title')", exception.Message);
    }

    [Fact]
    public void AddVideo_WithNullAltText_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _blockBuilder.AddVideo("video", "thumb", "title", null));
        Assert.Equal("Alt text cannot be null or whitespace. (Parameter 'altText')", exception.Message);
    }

    [Fact]
    public void AddVideo_WithNullVideoUri_ThrowsArgumentNullException()
    {
        // Arrange
        var thumbnailUri = new Uri("https://example.com/thumbnail.jpg");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddVideo(null, thumbnailUri, "title", "alt"));
    }

    [Fact]
    public void AddVideo_WithNullThumbnailUri_ThrowsArgumentNullException()
    {
        // Arrange
        var videoUri = new Uri("https://example.com/video.mp4");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _blockBuilder.AddVideo(videoUri, null, "title", "alt"));
    }

    // === Complex Integration Tests ===

    [Fact]
    public void ChainedMethodCalls_BuildMultipleBlocks()
    {
        // Arrange
        var headerText = new PlainText { Text = "Header" };
        var slackFile = new ImageFileReference { Id = "file123" };

        // Act
        var result = _blockBuilder
            .AddHeader(headerText, "header_block")
            .AddDivider("divider_block")
            .AddSection("*Section text*")
            .AddActions(actions => actions.AddButton("btn1", "Button"))
            .AddContext(context => context.AddText("Context text"))
            .AddImageFromSlackFile(slackFile, "Image alt text")
            .AddCall("call123", "call_block")
            .AddFile("external123", "remote", "file_block")
            .AddInput<DatePicker>("Select Date", input => input.ActionId("date_picker"))
            .AddRichText(richText => richText.AddSection(section => section.Add(new RichTextText { Text = "Rich text" })))
            .AddVideo("https://example.com/video.mp4", "https://example.com/thumb.jpg", "Video", "Alt");

        // Assert
        Assert.Same(_blockBuilder, result);
        
        var blocks = _blockBuilder.Build();
        Assert.Equal(11, blocks.Count);
        Assert.IsType<HeaderBlock>(blocks[0]);
        Assert.IsType<DividerBlock>(blocks[1]);
        Assert.IsType<SectionBlock>(blocks[2]);
        Assert.IsType<ActionsBlock>(blocks[3]);
        Assert.IsType<ContextBlock>(blocks[4]);
        Assert.IsType<ImageBlock>(blocks[5]);
        Assert.IsType<CallBlock>(blocks[6]);
        Assert.IsType<FileBlock>(blocks[7]);
        Assert.IsType<InputBlock>(blocks[8]);
        Assert.IsType<RichTextBlock>(blocks[9]);
        Assert.IsType<VideoBlock>(blocks[10]);
    }
}
