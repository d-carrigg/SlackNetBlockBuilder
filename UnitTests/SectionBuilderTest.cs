
using SlackNet.Blocks;

namespace UnitTests;

public class SectionBuilderTest
{
    [Fact]
    public void Build_WithTooManyFields_ThrowsException()
    {
        // Arrange
        var builder = new SectionBuilder();
        for (int i = 0; i < 11; i++) // Max is 10
        {
            builder.AddTextField($"Field {i}");
        }

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithFieldTooLong_ThrowsException()
    {
        // Arrange
        var builder = new SectionBuilder();
        builder.AddTextField(new string('a', 2001)); // Max is 2000

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithValidConfiguration_Succeeds()
    {
        // Arrange
        var builder = new SectionBuilder();
        builder
            .Text("Main text")
            .AddTextField("Field 1")
            .AddMarkdownField("*Field 2*");

        // Act
        var block = builder.Build();

        // Assert
        Assert.Equal("Main text", block.Text.Text);
        Assert.Equal(2, block.Fields.Count);
        Assert.Equal("Field 1", block.Fields[0].Text);
        Assert.Equal("*Field 2*", block.Fields[1].Text);
    }

    [Fact]
    public void Build_WithAccessory_Succeeds()
    {
        // Arrange
        var builder = new SectionBuilder();
        builder
            .Text("Text")
            .Accessory(new Button 
            { 
                Text = new PlainText("Click me"),
                ActionId = "button1"
            });

        // Act
        var block = builder.Build();

        // Assert
        Assert.NotNull(block.Accessory);
        var button = Assert.IsType<Button>(block.Accessory);
        Assert.Equal("Click me", button.Text.Text);
        Assert.Equal("button1", button.ActionId);
    }
} 