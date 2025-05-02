using System.Diagnostics;
using JetBrains.Annotations;
using SlackNet.Blocks;
using Xunit.Abstractions;

namespace UnitTests;

[TestSubject(typeof(SectionBuilder))]
public class SectionBuilderTest
{
    private readonly ITestOutputHelper _output;

    public SectionBuilderTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Text_SetsPlainText()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        builder.Text("Hello world");
        var result = builder.Build();
        
        // Assert
        Assert.Equal("Hello world", result.Text.Text);
        Assert.IsType<PlainText>(result.Text);
    }

    [Fact]
    public void Markdown_SetsMarkdownText()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        builder.Markdown("*Hello* world");
        var result = builder.Build();
        
        // Assert
        Assert.Equal("*Hello* world", result.Text.Text);
        Assert.IsType<Markdown>(result.Text);
    }

    [Fact]
    public void AddTextField_AddsPlainTextField()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        builder.AddTextField("Field 1");
        var result = builder.Build();
        
        // Assert
        Assert.Single(result.Fields);
        Assert.Equal("Field 1", result.Fields[0].Text);
        Assert.IsType<PlainText>(result.Fields[0]);
    }

    [Fact]
    public void AddMarkdownField_AddsMarkdownField()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        builder.AddMarkdownField("*Field 1*");
        var result = builder.Build();
        
        // Assert
        Assert.Single(result.Fields);
        Assert.Equal("*Field 1*", result.Fields[0].Text);
        Assert.IsType<Markdown>(result.Fields[0]);
    }

    [Fact]
    public void Performance_AddingManyTextFields()
    {
        // Arrange
        var stopwatch = new Stopwatch();
        const int iterations = 1000;
        
        // Act
        stopwatch.Start();
        for (int i = 0; i < iterations; i++)
        {
            // We can only add 10 fields max to a section, so we need to create a new builder each time
            var sectionBuilder = new SectionBuilder();
            
            // Add 10 fields (max allowed)
            for (int j = 0; j < 10; j++)
            {
                sectionBuilder.AddTextField($"Field {i}-{j}");
            }
            
            // Build the section
            sectionBuilder.Build();
        }
        stopwatch.Stop();
        
        // Output performance metrics
        var timePerIteration = stopwatch.Elapsed.TotalMilliseconds / iterations;
        _output.WriteLine($"SectionBuilder operations took {timePerIteration:F6} ms per iteration");
    }

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