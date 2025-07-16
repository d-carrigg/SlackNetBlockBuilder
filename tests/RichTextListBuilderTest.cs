using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(RichTextListBuilder))]
public class RichTextListBuilderTest
{
    [Fact]
    public void Constructor_WithBulletedStyle_InitializesCorrectly()
    {
        // Act
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Assert
        var list = builder.Build();
        Assert.Equal(RichTextListStyle.Bullet, list.Style);
        Assert.Empty(list.Elements);
    }
    
    [Fact]
    public void Constructor_WithOrderedStyle_InitializesCorrectly()
    {
        // Act
        var builder = new RichTextListBuilder(RichTextListStyle.Ordered);
        
        // Assert
        var list = builder.Build();
        Assert.Equal(RichTextListStyle.Ordered, list.Style);
        Assert.Empty(list.Elements);
    }
    
    [Fact]
    public void AddSection_WithValidConfiguration_AddsRichTextSection()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.AddSection(section => section.Add(new RichTextText { Text = "List item text" }));
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Single(list.Elements);
        
        var richTextSection = Assert.IsType<RichTextSection>(list.Elements[0]);
        Assert.Single(richTextSection.Elements);
        
        var textElement = Assert.IsType<RichTextText>(richTextSection.Elements[0]);
        Assert.Equal("List item text", textElement.Text);
    }
    
    [Fact]
    public void AddSection_WithNullCreationSection_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddSection(null!));
    }
    
    [Fact]
    public void AddSection_WithMultipleSections_AddsAllSections()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder
            .AddSection(section => section.Add(new RichTextText { Text = "First item" }))
            .AddSection(section => section.Add(new RichTextText { Text = "Second item" }))
            .AddSection(section => section.Add(new RichTextText { Text = "Third item" }));
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(3, list.Elements.Count);
        
        var firstSection = Assert.IsType<RichTextSection>(list.Elements[0]);
        var firstText = Assert.IsType<RichTextText>(firstSection.Elements[0]);
        Assert.Equal("First item", firstText.Text);
        
        var secondSection = Assert.IsType<RichTextSection>(list.Elements[1]);
        var secondText = Assert.IsType<RichTextText>(secondSection.Elements[0]);
        Assert.Equal("Second item", secondText.Text);
        
        var thirdSection = Assert.IsType<RichTextSection>(list.Elements[2]);
        var thirdText = Assert.IsType<RichTextText>(thirdSection.Elements[0]);
        Assert.Equal("Third item", thirdText.Text);
    }
    
    [Fact]
    public void AddSection_WithComplexContent_AddsComplexSection()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.AddSection(section => section
            .Add(new RichTextText { Text = "Bold text", Style = new RichTextStyle { Bold = true } })
            .Add(new RichTextText { Text = " and " })
            .Add(new RichTextText { Text = "italic text", Style = new RichTextStyle { Italic = true } })
            .Add(new RichTextLink { Url = "https://example.com", Text = "link" }));
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Single(list.Elements);
        
        var richTextSection = Assert.IsType<RichTextSection>(list.Elements[0]);
        Assert.Equal(4, richTextSection.Elements.Count);
        
        var boldText = Assert.IsType<RichTextText>(richTextSection.Elements[0]);
        Assert.Equal("Bold text", boldText.Text);
        Assert.True(boldText.Style.Bold);
        
        var normalText = Assert.IsType<RichTextText>(richTextSection.Elements[1]);
        Assert.Equal(" and ", normalText.Text);
        
        var italicText = Assert.IsType<RichTextText>(richTextSection.Elements[2]);
        Assert.Equal("italic text", italicText.Text);
        Assert.True(italicText.Style.Italic);
        
        var link = Assert.IsType<RichTextLink>(richTextSection.Elements[3]);
        Assert.Equal("https://example.com", link.Url);
        Assert.Equal("link", link.Text);
    }
    
    [Fact]
    public void Indent_SetsIndentLevel()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.Indent(2);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(2, list.Indent);
    }
    
    [Fact]
    public void Indent_WithZero_SetsIndentToZero()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.Indent(0);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(0, list.Indent);
    }
    
    [Fact]
    public void Indent_WithNegativeValue_SetsNegativeIndent()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.Indent(-1);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(-1, list.Indent);
    }
    
    [Fact]
    public void Offset_SetsOffsetValue()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Ordered);
        
        // Act
        var result = builder.Offset(5);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(5, list.Offset);
    }
    
    [Fact]
    public void Offset_WithZero_SetsOffsetToZero()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Ordered);
        
        // Act
        var result = builder.Offset(0);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(0, list.Offset);
    }
    
    [Fact]
    public void Offset_WithNegativeValue_SetsNegativeOffset()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Ordered);
        
        // Act
        var result = builder.Offset(-2);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(-2, list.Offset);
    }
    
    [Fact]
    public void Border_SetsBorderValue()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.Border(1);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(1, list.Border);
    }
    
    [Fact]
    public void Border_WithZero_SetsBorderToZero()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.Border(0);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(0, list.Border);
    }
    
    [Fact]
    public void Border_WithNegativeValue_SetsNegativeBorder()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder.Border(-1);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(-1, list.Border);
    }
    
    [Fact]
    public void Build_ReturnsConfiguredRichTextList()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Ordered);
        
        // Act
        builder
            .AddSection(section => section.Add(new RichTextText { Text = "First item" }))
            .AddSection(section => section.Add(new RichTextText { Text = "Second item" }))
            .Indent(1)
            .Offset(3)
            .Border(1);
        
        var list = builder.Build();
        
        // Assert
        Assert.Equal(RichTextListStyle.Ordered, list.Style);
        Assert.Equal(2, list.Elements.Count);
        Assert.Equal(1, list.Indent);
        Assert.Equal(3, list.Offset);
        Assert.Equal(1, list.Border);
    }
    
    [Fact]
    public void Build_CalledMultipleTimes_ReturnsSameInstance()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        builder.AddSection(section => section.Add(new RichTextText { Text = "Item" }));
        
        // Act
        var list1 = builder.Build();
        var list2 = builder.Build();
        
        // Assert
        Assert.Same(list1, list2);
    }
    
    [Fact]
    public void ChainedMethodCalls_BuildCompleteList()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var result = builder
            .AddSection(section => section
                .Add(new RichTextText { Text = "First item with " })
                .Add(new RichTextText { Text = "bold", Style = new RichTextStyle { Bold = true } }))
            .AddSection(section => section
                .Add(new RichTextText { Text = "Second item with " })
                .Add(new RichTextLink { Url = "https://example.com", Text = "link" }))
            .AddSection(section => section.Add(new RichTextText { Text = "Third item" }))
            .Indent(2)
            .Offset(1)
            .Border(1);
        
        // Assert
        Assert.Same(builder, result);
        
        var list = builder.Build();
        Assert.Equal(RichTextListStyle.Bullet, list.Style);
        Assert.Equal(3, list.Elements.Count);
        Assert.Equal(2, list.Indent);
        Assert.Equal(1, list.Offset);
        Assert.Equal(1, list.Border);
        
        // Verify first section
        var firstSection = Assert.IsType<RichTextSection>(list.Elements[0]);
        Assert.Equal(2, firstSection.Elements.Count);
        var firstText = Assert.IsType<RichTextText>(firstSection.Elements[0]);
        Assert.Equal("First item with ", firstText.Text);
        var boldText = Assert.IsType<RichTextText>(firstSection.Elements[1]);
        Assert.Equal("bold", boldText.Text);
        Assert.True(boldText.Style.Bold);
        
        // Verify second section
        var secondSection = Assert.IsType<RichTextSection>(list.Elements[1]);
        Assert.Equal(2, secondSection.Elements.Count);
        var secondText = Assert.IsType<RichTextText>(secondSection.Elements[0]);
        Assert.Equal("Second item with ", secondText.Text);
        var link = Assert.IsType<RichTextLink>(secondSection.Elements[1]);
        Assert.Equal("https://example.com", link.Url);
        Assert.Equal("link", link.Text);
        
        // Verify third section
        var thirdSection = Assert.IsType<RichTextSection>(list.Elements[2]);
        Assert.Single(thirdSection.Elements);
        var thirdText = Assert.IsType<RichTextText>(thirdSection.Elements[0]);
        Assert.Equal("Third item", thirdText.Text);
    }
    
    [Fact]
    public void Build_WithEmptyList_ReturnsEmptyList()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        var list = builder.Build();
        
        // Assert
        Assert.Equal(RichTextListStyle.Bullet, list.Style);
        Assert.Empty(list.Elements);
        Assert.Equal(0, list.Indent);
        Assert.Equal(0, list.Offset);
        Assert.Equal(0, list.Border);
    }
    
    [Fact]
    public void AllRichTextListStyles_WorkCorrectly()
    {
        // Test Bullet style
        var bulletBuilder = new RichTextListBuilder(RichTextListStyle.Bullet);
        var bulletList = bulletBuilder.Build();
        Assert.Equal(RichTextListStyle.Bullet, bulletList.Style);
        
        // Test Ordered style
        var orderedBuilder = new RichTextListBuilder(RichTextListStyle.Ordered);
        var orderedList = orderedBuilder.Build();
        Assert.Equal(RichTextListStyle.Ordered, orderedList.Style);
    }
    
    [Fact]
    public void PropertyOrder_DoesNotMatter()
    {
        // Arrange
        var builder1 = new RichTextListBuilder(RichTextListStyle.Ordered);
        var builder2 = new RichTextListBuilder(RichTextListStyle.Ordered);
        
        // Act - Different order of setting properties
        builder1
            .Indent(2)
            .Offset(3)
            .Border(1)
            .AddSection(section => section.Add(new RichTextText { Text = "Item" }));
        
        builder2
            .AddSection(section => section.Add(new RichTextText { Text = "Item" }))
            .Border(1)
            .Offset(3)
            .Indent(2);
        
        var list1 = builder1.Build();
        var list2 = builder2.Build();
        
        // Assert - Both should have same configuration
        Assert.Equal(list1.Style, list2.Style);
        Assert.Equal(list1.Indent, list2.Indent);
        Assert.Equal(list1.Offset, list2.Offset);
        Assert.Equal(list1.Border, list2.Border);
        Assert.Equal(list1.Elements.Count, list2.Elements.Count);
    }
    
    [Fact]
    public void ModifyingAfterBuild_DoesNotAffectPreviousBuilds()
    {
        // Arrange
        var builder = new RichTextListBuilder(RichTextListStyle.Bullet);
        
        // Act
        builder.AddSection(section => section.Add(new RichTextText { Text = "First item" }));
        var list1 = builder.Build();
        
        builder.AddSection(section => section.Add(new RichTextText { Text = "Second item" }));
        var list2 = builder.Build();
        
        // Assert
        Assert.Equal(2, list1.Elements.Count); // Both builds return same instance
        Assert.Equal(2, list2.Elements.Count);
        Assert.Same(list1, list2); // Same instance returned
    }
}
