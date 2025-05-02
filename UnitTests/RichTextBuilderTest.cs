
using SlackNet.Blocks;

namespace UnitTests;

public class RichTextBuilderTest
{
    [Fact]
    public void Build_WithBlockIdTooLong_ThrowsException()
    {
        // Arrange
        var builder = new RichTextBuilder();
        builder.BlockId(new string('a', RichTextBuilder.MaxIdLength + 1));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void AddSection_WithText_BuildsCorrectly()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        builder.AddSection(section => section.Add(new RichTextText { Text = "Test text" }));
        var block = builder.Build();

        // Assert
        Assert.Single(block.Elements);
        var section = Assert.IsType<RichTextSection>(block.Elements[0]);
        var text = Assert.IsType<RichTextText>(section.Elements[0]);
        Assert.Equal("Test text", text.Text);
    }

    [Fact]
    public void AddTextList_BuildsCorrectly()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        builder.AddTextList(RichTextListStyle.Bullet, list => 
            list.AddSection(section => 
                section.Add(new RichTextText { Text = "List item" }))
        );
        var block = builder.Build();

        // Assert
        Assert.Single(block.Elements);
        var list = Assert.IsType<RichTextList>(block.Elements[0]);
        Assert.Equal(RichTextListStyle.Bullet, list.Style);
        Assert.Single(list.Elements);
    }
} 