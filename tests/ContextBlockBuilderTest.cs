using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(ContextBlockBuilder))]
public class ContextBlockBuilderTest
{
    [Fact]
    public void Constructor_InitializesEmptyContextBlock()
    {
        // Arrange & Act
        var builder = new ContextBlockBuilder();
        
        // Assert
        Assert.NotNull(builder);
    }
    
    [Fact]
    public void BlockId_SetsBlockId()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.BlockId("context_123");
        
        // Assert
        Assert.Same(builder, result);
        
        // Verify through build
        builder.AddText("Test");
        var block = builder.Build();
        Assert.Equal("context_123", block.BlockId);
    }
    
    [Fact]
    public void BlockId_WithNullBlockId_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.BlockId(null));
    }
    
    [Fact]
    public void AddText_WithEmojiTrue_AddsPlainTextElement()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.AddText("Hello :smile:", true);
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var plainText = Assert.IsType<PlainText>(block.Elements[0]);
        Assert.Equal("Hello :smile:", plainText.Text);
        Assert.True(plainText.Emoji);
    }
    
    [Fact]
    public void AddText_WithEmojiDefault_AddsPlainTextWithEmojiTrue()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.AddText("Hello :smile:");
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var plainText = Assert.IsType<PlainText>(block.Elements[0]);
        Assert.Equal("Hello :smile:", plainText.Text);
        Assert.True(plainText.Emoji);
    }
    
    [Fact]
    public void AddText_WithEmojiFalse_AddsPlainTextWithEmojiFalse()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.AddText("Hello :smile:", false);
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        var plainText = Assert.IsType<PlainText>(block.Elements[0]);
        Assert.Equal("Hello :smile:", plainText.Text);
        Assert.False(plainText.Emoji);
    }
    
    [Fact]
    public void AddText_WithNullText_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddText(null));
    }
    
    [Fact]
    public void AddMarkdown_WithVerbatimFalse_AddsMarkdownElement()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.AddMarkdown("*Bold text*", false);
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var markdown = Assert.IsType<Markdown>(block.Elements[0]);
        Assert.Equal("*Bold text*", markdown.Text);
        Assert.False(markdown.Verbatim);
    }
    
    [Fact]
    public void AddMarkdown_WithVerbatimDefault_AddsMarkdownWithVerbatimFalse()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.AddMarkdown("*Bold text*");
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        var markdown = Assert.IsType<Markdown>(block.Elements[0]);
        Assert.Equal("*Bold text*", markdown.Text);
        Assert.False(markdown.Verbatim);
    }
    
    [Fact]
    public void AddMarkdown_WithVerbatimTrue_AddsMarkdownWithVerbatimTrue()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.AddMarkdown("*Bold text*", true);
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        var markdown = Assert.IsType<Markdown>(block.Elements[0]);
        Assert.Equal("*Bold text*", markdown.Text);
        Assert.True(markdown.Verbatim);
    }
    
    [Fact]
    public void AddMarkdown_WithNullText_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddMarkdown(null));
    }
    
    [Fact]
    public void AddImageFromUrl_WithStringUrl_AddsImageElement()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        var result = builder.AddImageFromUrl("https://example.com/image.png", "Example image");
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var image = Assert.IsType<Image>(block.Elements[0]);
        Assert.Equal("https://example.com/image.png", image.ImageUrl);
        Assert.Equal("Example image", image.AltText);
        Assert.Null(image.SlackFile);
    }
    
    [Fact]
    public void AddImageFromUrl_WithNullUrl_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddImageFromUrl((string)null, "Alt text"));
    }
    
    [Fact]
    public void AddImageFromUrl_WithUriUrl_AddsImageElement()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var uri = new Uri("https://example.com/image.png");
        
        // Act
        var result = builder.AddImageFromUrl(uri, "Example image");
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var image = Assert.IsType<Image>(block.Elements[0]);
        Assert.Equal("https://example.com/image.png", image.ImageUrl);
        Assert.Equal("Example image", image.AltText);
    }
    
    [Fact]
    public void AddImageFromUrl_WithNullUri_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddImageFromUrl((Uri)null, "Alt text"));
    }
    
    [Fact]
    public void AddImageFromSlackFile_WithValidFile_AddsImageElement()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var slackFile = new ImageFileReference { Id = "file123" };
        
        // Act
        var result = builder.AddImageFromSlackFile(slackFile, "Slack file image");
        
        // Assert
        Assert.Same(builder, result);
        
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var image = Assert.IsType<Image>(block.Elements[0]);
        Assert.Same(slackFile, image.SlackFile);
        Assert.Equal("Slack file image", image.AltText);
        Assert.Null(image.ImageUrl);
    }
    
    [Fact]
    public void AddImageFromSlackFile_WithNullFile_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddImageFromSlackFile(null, "Alt text"));
    }
    
    [Fact]
    public void AddImageFromSlackFile_WithNullAltText_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var slackFile = new ImageFileReference { Id = "file123" };
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddImageFromSlackFile(slackFile, null));
    }
    
    [Fact]
    public void Build_WithNoElements_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("At least one element is required in a context block", exception.Message);
    }
    
    [Fact]
    public void Build_WithMaxElements_BuildsSuccessfully()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act - Add exactly 10 elements (MaxElements)
        for (int i = 0; i < ContextBlockBuilder.MaxElements; i++)
        {
            builder.AddText($"Text {i}");
        }
        
        // Assert
        var block = builder.Build();
        Assert.Equal(ContextBlockBuilder.MaxElements, block.Elements.Count);
    }
    
    [Fact]
    public void Build_WithTooManyElements_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act - Add more than MaxElements
        for (int i = 0; i <= ContextBlockBuilder.MaxElements; i++)
        {
            builder.AddText($"Text {i}");
        }
        
        // Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal($"Context blocks can only contain up to {ContextBlockBuilder.MaxElements} elements", exception.Message);
    }
    
    [Fact]
    public void Build_WithBlockIdTooLong_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var longBlockId = new string('A', ContextBlockBuilder.MaxBlockIdLength + 1);
        
        // Act
        builder.BlockId(longBlockId);
        builder.AddText("Test");
        
        // Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal($"The block id can only be up to {ContextBlockBuilder.MaxBlockIdLength} characters long", exception.Message);
    }
    
    [Fact]
    public void Build_WithBlockIdAtMaxLength_BuildsSuccessfully()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var maxLengthBlockId = new string('A', ContextBlockBuilder.MaxBlockIdLength);
        
        // Act
        builder.BlockId(maxLengthBlockId);
        builder.AddText("Test");
        
        // Assert
        var block = builder.Build();
        Assert.Equal(maxLengthBlockId, block.BlockId);
    }
    
    [Fact]
    public void Build_WithMixedElements_BuildsCorrectly()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var imageUri = new Uri("https://example.com/image.png");
        var slackFile = new ImageFileReference { Id = "file123" };
        
        // Act
        builder
            .BlockId("mixed_context")
            .AddText("Plain text with :emoji:", true)
            .AddMarkdown("*Bold* text", false)
            .AddImageFromUrl("https://example.com/image1.png", "Image 1")
            .AddImageFromUrl(imageUri, "Image 2")
            .AddImageFromSlackFile(slackFile, "Slack image");
        
        // Assert
        var block = builder.Build();
        Assert.Equal("mixed_context", block.BlockId);
        Assert.Equal(5, block.Elements.Count);
        
        var plainText = Assert.IsType<PlainText>(block.Elements[0]);
        Assert.Equal("Plain text with :emoji:", plainText.Text);
        Assert.True(plainText.Emoji);
        
        var markdown = Assert.IsType<Markdown>(block.Elements[1]);
        Assert.Equal("*Bold* text", markdown.Text);
        Assert.False(markdown.Verbatim);
        
        var image1 = Assert.IsType<Image>(block.Elements[2]);
        Assert.Equal("https://example.com/image1.png", image1.ImageUrl);
        Assert.Equal("Image 1", image1.AltText);
        
        var image2 = Assert.IsType<Image>(block.Elements[3]);
        Assert.Equal("https://example.com/image.png", image2.ImageUrl);
        Assert.Equal("Image 2", image2.AltText);
        
        var image3 = Assert.IsType<Image>(block.Elements[4]);
        Assert.Same(slackFile, image3.SlackFile);
        Assert.Equal("Slack image", image3.AltText);
    }
    
    [Fact]
    public void Constants_HaveCorrectValues()
    {
        // Assert
        Assert.Equal(10, ContextBlockBuilder.MaxElements);
        Assert.Equal(255, ContextBlockBuilder.MaxBlockIdLength);
    }
    
    [Fact]
    public void ChainedMethodCalls_ReturnSameBuilderInstance()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act & Assert
        var result = builder
            .BlockId("test_block")
            .AddText("Text")
            .AddMarkdown("Markdown")
            .AddImageFromUrl("https://example.com/image.png", "Image");
        
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AddText_WithEmptyString_AddsEmptyTextElement()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        builder.AddText("");
        
        // Assert
        var block = builder.Build();
        var plainText = Assert.IsType<PlainText>(block.Elements[0]);
        Assert.Equal("", plainText.Text);
    }
    
    [Fact]
    public void AddMarkdown_WithEmptyString_AddsEmptyMarkdownElement()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        builder.AddMarkdown("");
        
        // Assert
        var block = builder.Build();
        var markdown = Assert.IsType<Markdown>(block.Elements[0]);
        Assert.Equal("", markdown.Text);
    }
    
    [Fact]
    public void AddImageFromUrl_WithEmptyAltText_AddsImageWithEmptyAltText()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        
        // Act
        builder.AddImageFromUrl("https://example.com/image.png", "");
        
        // Assert
        var block = builder.Build();
        var image = Assert.IsType<Image>(block.Elements[0]);
        Assert.Equal("", image.AltText);
    }
    
    [Fact]
    public void AddImageFromSlackFile_WithEmptyAltText_AddsImageWithEmptyAltText()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var slackFile = new ImageFileReference { Id = "file123" };
        
        // Act
        builder.AddImageFromSlackFile(slackFile, "");
        
        // Assert
        var block = builder.Build();
        var image = Assert.IsType<Image>(block.Elements[0]);
        Assert.Equal("", image.AltText);
    }
    
    [Fact]
    public void Build_WithLongAltText_BuildsSuccessfully()
    {
        // Arrange
        var builder = new ContextBlockBuilder();
        var longAltText = new string('A', 2000); // Maximum length for alt text
        
        // Act
        builder.AddImageFromUrl("https://example.com/image.png", longAltText);
        
        // Assert
        var block = builder.Build();
        var image = Assert.IsType<Image>(block.Elements[0]);
        Assert.Equal(longAltText, image.AltText);
    }
}
