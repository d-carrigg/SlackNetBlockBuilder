using JetBrains.Annotations;
using SlackNet.Blocks;
using System.Diagnostics;
using Xunit.Abstractions;

namespace UnitTests;

[TestSubject(typeof(RichTextBuilder))]
public class RichTextBuilderTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public RichTextBuilderTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // === Constructor Tests ===
    
    [Fact]
    public void Constructor_CreatesEmptyRichTextBuilder()
    {
        // Act
        var builder = new RichTextBuilder();
        
        // Assert
        var block = builder.Build();
        Assert.Empty(block.Elements);
        Assert.Null(block.BlockId);
    }
    
    // === BlockId Tests ===
    
    [Fact]
    public void BlockId_SetsBlockId()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.BlockId("test_block_id");
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Equal("test_block_id", block.BlockId);
    }
    
    [Fact]
    public void BlockId_WithNullValue_SetsNull()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.BlockId(null);
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Null(block.BlockId);
    }
    
    [Fact]
    public void BlockId_WithEmptyString_SetsEmptyString()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.BlockId("");
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Equal("", block.BlockId);
    }
    
    // === AddSection Tests ===
    
    [Fact]
    public void AddSection_WithValidConfiguration_AddsRichTextSection()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddSection(section => section.Add(new RichTextText { Text = "Hello world" }));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Single(block.Elements);
        var section = Assert.IsType<RichTextSection>(block.Elements[0]);
        Assert.Single(section.Elements);
        var text = Assert.IsType<RichTextText>(section.Elements[0]);
        Assert.Equal("Hello world", text.Text);
    }
    
    [Fact]
    public void AddSection_WithNullCreationSection_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddSection(null));
    }
    
    [Fact]
    public void AddSection_WithMultipleSections_AddsAllSections()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder
            .AddSection(section => section.Add(new RichTextText { Text = "First section" }))
            .AddSection(section => section.Add(new RichTextText { Text = "Second section" }));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Equal(2, block.Elements.Count);
        
        var firstSection = Assert.IsType<RichTextSection>(block.Elements[0]);
        var firstText = Assert.IsType<RichTextText>(firstSection.Elements[0]);
        Assert.Equal("First section", firstText.Text);
        
        var secondSection = Assert.IsType<RichTextSection>(block.Elements[1]);
        var secondText = Assert.IsType<RichTextText>(secondSection.Elements[0]);
        Assert.Equal("Second section", secondText.Text);
    }
    
    [Fact]
    public void AddSection_WithComplexContent_AddsComplexSection()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddSection(section => section
            .Add(new RichTextText { Text = "Bold text", Style = new RichTextStyle { Bold = true } })
            .Add(new RichTextText { Text = " and " })
            .Add(new RichTextLink { Url = "https://example.com", Text = "link" }));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var richTextSection = Assert.IsType<RichTextSection>(block.Elements[0]);
        Assert.Equal(3, richTextSection.Elements.Count);
        
        var boldText = Assert.IsType<RichTextText>(richTextSection.Elements[0]);
        Assert.Equal("Bold text", boldText.Text);
        Assert.True(boldText.Style.Bold);
        
        var normalText = Assert.IsType<RichTextText>(richTextSection.Elements[1]);
        Assert.Equal(" and ", normalText.Text);
        
        var link = Assert.IsType<RichTextLink>(richTextSection.Elements[2]);
        Assert.Equal("https://example.com", link.Url);
        Assert.Equal("link", link.Text);
    }
    
    // === AddTextList Tests ===
    
    [Fact]
    public void AddTextList_WithBulletStyle_AddsRichTextList()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddTextList(RichTextListStyle.Bullet, list => 
            list.AddSection(section => section.Add(new RichTextText { Text = "List item" })));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var listElement = Assert.IsType<RichTextList>(block.Elements[0]);
        Assert.Equal(RichTextListStyle.Bullet, listElement.Style);
        Assert.Single(listElement.Elements);
        
        var listSection = Assert.IsType<RichTextSection>(listElement.Elements[0]);
        var listText = Assert.IsType<RichTextText>(listSection.Elements[0]);
        Assert.Equal("List item", listText.Text);
    }
    
    [Fact]
    public void AddTextList_WithOrderedStyle_AddsOrderedList()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddTextList(RichTextListStyle.Ordered, list => 
            list.AddSection(section => section.Add(new RichTextText { Text = "First item" }))
                .AddSection(section => section.Add(new RichTextText { Text = "Second item" }))
                .Offset(5));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var listElement = Assert.IsType<RichTextList>(block.Elements[0]);
        Assert.Equal(RichTextListStyle.Ordered, listElement.Style);
        Assert.Equal(2, listElement.Elements.Count);
        Assert.Equal(5, listElement.Offset);
    }
    
    [Fact]
    public void AddTextList_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddTextList(RichTextListStyle.Bullet, null));
    }
    
    // === AddPreformattedText Tests ===
    
    [Fact]
    public void AddPreformattedText_WithStringContent_AddsPreformattedBlock()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddPreformattedText("console.log('Hello World');", 1);
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var preformatted = Assert.IsType<RichTextPreformatted>(block.Elements[0]);
        Assert.Equal(1, preformatted.Border);
        Assert.Single(preformatted.Elements);
        
        var text = Assert.IsType<RichTextText>(preformatted.Elements[0]);
        Assert.Equal("console.log('Hello World');", text.Text);
    }
    
    [Fact]
    public void AddPreformattedText_WithDefaultBorder_UsesDefaultBorder()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddPreformattedText("code");
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        var preformatted = Assert.IsType<RichTextPreformatted>(block.Elements[0]);
        Assert.Equal(0, preformatted.Border);
    }
    
    [Fact]
    public void AddPreformattedText_WithNullString_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddPreformattedText((string)null));
    }
    
    [Fact]
    public void AddPreformattedText_WithEmptyString_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddPreformattedText(""));
    }
    
    [Fact]
    public void AddPreformattedText_WithContentBuilder_AddsPreformattedBlock()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddPreformattedText(content => content
            .Add(new RichTextText { Text = "function ", Style = new RichTextStyle { Bold = true } })
            .Add(new RichTextText { Text = "hello() { ... }" }), 1);
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var preformatted = Assert.IsType<RichTextPreformatted>(block.Elements[0]);
        Assert.Equal(1, preformatted.Border);
        Assert.Equal(2, preformatted.Elements.Count);
        
        var boldText = Assert.IsType<RichTextText>(preformatted.Elements[0]);
        Assert.Equal("function ", boldText.Text);
        Assert.True(boldText.Style.Bold);
        
        var normalText = Assert.IsType<RichTextText>(preformatted.Elements[1]);
        Assert.Equal("hello() { ... }", normalText.Text);
    }
    
    [Fact]
    public void AddPreformattedText_WithNullContentBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddPreformattedText((Action<RichTextSectionElementBuilder>)null));
    }
    
    // === AddQuote Tests ===
    
    [Fact]
    public void AddQuote_WithContent_AddsQuoteBlock()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddQuote(content => content
            .Add(new RichTextText { Text = "To be or not to be, that is the question." }), 1);
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        Assert.Single(block.Elements);
        
        var quote = Assert.IsType<RichTextQuote>(block.Elements[0]);
        Assert.Equal(1, quote.Border);
        Assert.Single(quote.Elements);
        
        var text = Assert.IsType<RichTextText>(quote.Elements[0]);
        Assert.Equal("To be or not to be, that is the question.", text.Text);
    }
    
    [Fact]
    public void AddQuote_WithDefaultBorder_UsesDefaultBorder()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddQuote(content => content
            .Add(new RichTextText { Text = "Quote text" }));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        var quote = Assert.IsType<RichTextQuote>(block.Elements[0]);
        Assert.Equal(0, quote.Border);
    }
    
    [Fact]
    public void AddQuote_WithNullContent_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddQuote(null));
    }
    
    [Fact]
    public void AddQuote_WithComplexContent_AddsComplexQuote()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder.AddQuote(content => content
            .Add(new RichTextText { Text = "The only way to do great work is to ", Style = new RichTextStyle { Italic = true } })
            .Add(new RichTextText { Text = "love", Style = new RichTextStyle { Bold = true } })
            .Add(new RichTextText { Text = " what you do." }));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        var quote = Assert.IsType<RichTextQuote>(block.Elements[0]);
        Assert.Equal(3, quote.Elements.Count);
        
        var italicText = Assert.IsType<RichTextText>(quote.Elements[0]);
        Assert.True(italicText.Style.Italic);
        
        var boldText = Assert.IsType<RichTextText>(quote.Elements[1]);
        Assert.True(boldText.Style.Bold);
        
        var normalText = Assert.IsType<RichTextText>(quote.Elements[2]);
        Assert.Equal(" what you do.", normalText.Text);
    }
    
    // === Build Tests ===
    
    [Fact]
    public void Build_WithBlockIdTooLong_ThrowsInvalidOperationException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        var longBlockId = new string('A', RichTextBuilder.MaxIdLength + 1);
        
        // Act
        builder.BlockId(longBlockId);
        
        // Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal($"The block id can only be up to {RichTextBuilder.MaxIdLength} characters long", exception.Message);
    }
    
    [Fact]
    public void Build_WithBlockIdAtMaxLength_BuildsSuccessfully()
    {
        // Arrange
        var builder = new RichTextBuilder();
        var maxLengthBlockId = new string('A', RichTextBuilder.MaxIdLength);
        
        // Act
        builder.BlockId(maxLengthBlockId);
        
        // Assert
        var block = builder.Build();
        Assert.Equal(maxLengthBlockId, block.BlockId);
    }
    
    [Fact]
    public void Build_WithEmptyBuilder_ReturnsEmptyBlock()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var block = builder.Build();
        
        // Assert
        Assert.Empty(block.Elements);
        Assert.Null(block.BlockId);
    }
    
    [Fact]
    public void Build_CalledMultipleTimes_ReturnsSameInstance()
    {
        // Arrange
        var builder = new RichTextBuilder();
        builder.AddSection(section => section.Add(new RichTextText { Text = "Test" }));
        
        // Act
        var block1 = builder.Build();
        var block2 = builder.Build();
        
        // Assert
        Assert.Same(block1, block2);
    }
    
    // === Constants Tests ===
    
    [Fact]
    public void MaxIdLength_HasCorrectValue()
    {
        // Assert
        Assert.Equal(255, RichTextBuilder.MaxIdLength);
    }
    
    // === Chaining Tests ===
    
    [Fact]
    public void ChainedMethodCalls_BuildComplexRichText()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        var result = builder
            .BlockId("rich_text_block")
            .AddSection(section => section
                .Add(new RichTextText { Text = "Welcome to our ", Style = new RichTextStyle { Bold = true } })
                .Add(new RichTextLink { Url = "https://example.com", Text = "website" }))
            .AddTextList(RichTextListStyle.Bullet, list => list
                .AddSection(section => section.Add(new RichTextText { Text = "First feature" }))
                .AddSection(section => section.Add(new RichTextText { Text = "Second feature" }))
                .Indent(1))
            .AddPreformattedText("npm install example-package")
            .AddQuote(quote => quote
                .Add(new RichTextText { Text = "Great software is built by great teams." }));
        
        // Assert
        Assert.Same(builder, result);
        var block = builder.Build();
        
        Assert.Equal("rich_text_block", block.BlockId);
        Assert.Equal(4, block.Elements.Count);
        
        // Verify section
        var section = Assert.IsType<RichTextSection>(block.Elements[0]);
        Assert.Equal(2, section.Elements.Count);
        
        // Verify list
        var list = Assert.IsType<RichTextList>(block.Elements[1]);
        Assert.Equal(RichTextListStyle.Bullet, list.Style);
        Assert.Equal(2, list.Elements.Count);
        Assert.Equal(1, list.Indent);
        
        // Verify preformatted
        var preformatted = Assert.IsType<RichTextPreformatted>(block.Elements[2]);
        Assert.Single(preformatted.Elements);
        
        // Verify quote
        var quote = Assert.IsType<RichTextQuote>(block.Elements[3]);
        Assert.Single(quote.Elements);
    }
    
    // === Performance Tests ===
    
    [Fact]
    public void Performance_AddingManyTextElements()
    {
        // Arrange
        const int iterations = 1000;
        var stopwatch = new Stopwatch();
        
        // Act
        stopwatch.Start();
        
        for (var i = 0; i < iterations; i++)
        {
            var builder = new RichTextBuilder();
            var iLocal = i; // Capture the loop variable
            
            // Add multiple different elements
            builder.AddSection(section => section.Add(new RichTextText { Text = $"Section {iLocal}" }));
            builder.AddTextList(RichTextListStyle.Bullet, list => 
                list.AddSection(section => section.Add(new RichTextText { Text = $"Item {iLocal}" })));
            builder.AddPreformattedText($"code-{iLocal}");
            builder.AddQuote(quote => quote.Add(new RichTextText { Text = $"Quote {iLocal}" }));
            
            var block = builder.Build();
            Assert.Equal(4, block.Elements.Count);
        }
        
        stopwatch.Stop();
        _testOutputHelper.WriteLine($"Performance test completed in {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");
        
        // Assert - Should complete in reasonable time (less than 5 seconds)
        Assert.True(stopwatch.ElapsedMilliseconds < 5000);
    }
    
    // === Integration Tests ===
    
    [Fact]
    public void ComplexRichTextBlock_AllElementTypes_BuildsCorrectly()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        builder
            .BlockId("comprehensive_block")
            .AddSection(section => section
                .Add(new RichTextText { Text = "Introduction with ", Style = new RichTextStyle { Bold = true } })
                .Add(new RichTextText { Text = "italic", Style = new RichTextStyle { Italic = true } })
                .Add(new RichTextText { Text = " and " })
                .Add(new RichTextLink { Url = "https://example.com", Text = "external link" }))
            .AddTextList(RichTextListStyle.Ordered, list => list
                .AddSection(section => section.Add(new RichTextText { Text = "First ordered item" }))
                .AddSection(section => section.Add(new RichTextText { Text = "Second ordered item" }))
                .Offset(10)
                .Indent(2)
                .Border(1))
            .AddTextList(RichTextListStyle.Bullet, list => list
                .AddSection(section => section.Add(new RichTextText { Text = "Bullet item 1" }))
                .AddSection(section => section.Add(new RichTextText { Text = "Bullet item 2" })))
            .AddPreformattedText(content => content
                .Add(new RichTextText { Text = "const", Style = new RichTextStyle { Bold = true } })
                .Add(new RichTextText { Text = " message = " })
                .Add(new RichTextText { Text = "'Hello World'", Style = new RichTextStyle { Italic = true } })
                .Add(new RichTextText { Text = ";" }), 1)
            .AddQuote(quote => quote
                .Add(new RichTextText { Text = "The best way to predict the future is to ", Style = new RichTextStyle { Italic = true } })
                .Add(new RichTextText { Text = "invent", Style = new RichTextStyle { Bold = true } })
                .Add(new RichTextText { Text = " it." }));
        
        var block = builder.Build();
        
        // Assert
        Assert.Equal("comprehensive_block", block.BlockId);
        Assert.Equal(5, block.Elements.Count);
        
        // Verify all element types are present
        Assert.IsType<RichTextSection>(block.Elements[0]);
        Assert.IsType<RichTextList>(block.Elements[1]);
        Assert.IsType<RichTextList>(block.Elements[2]);
        Assert.IsType<RichTextPreformatted>(block.Elements[3]);
        Assert.IsType<RichTextQuote>(block.Elements[4]);
        
        // Verify ordered list configuration
        var orderedList = Assert.IsType<RichTextList>(block.Elements[1]);
        Assert.Equal(RichTextListStyle.Ordered, orderedList.Style);
        Assert.Equal(10, orderedList.Offset);
        Assert.Equal(2, orderedList.Indent);
        Assert.Equal(1, orderedList.Border);
        
        // Verify bullet list configuration
        var bulletList = Assert.IsType<RichTextList>(block.Elements[2]);
        Assert.Equal(RichTextListStyle.Bullet, bulletList.Style);
        
        // Verify preformatted configuration
        var preformatted = Assert.IsType<RichTextPreformatted>(block.Elements[3]);
        Assert.Equal(1, preformatted.Border);
        Assert.Equal(4, preformatted.Elements.Count);
        
        // Verify quote configuration
        var quote = Assert.IsType<RichTextQuote>(block.Elements[4]);
        Assert.Equal(3, quote.Elements.Count);
    }
}
