using JetBrains.Annotations;
using SlackNet.Blocks;
using System.Diagnostics;

namespace UnitTests;

[TestSubject(typeof(RichTextBuilder))]
public class RichTextBuilderTest
{
    [Fact]
    public void Build_WithBlockId_SetsBlockId()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        builder.BlockId("test_block_id");
        var block = builder.Build();
        
        // Assert
        Assert.Equal("test_block_id", block.BlockId);
    }
    
    [Fact]
    public void AddSection_AddsRichTextSection()
    {
        // Arrange
        var builder = new RichTextBuilder();
        
        // Act
        builder.AddSection(section => section.Add(new RichTextText { Text = "Hello world" }));
        var block = builder.Build();
        
        // Assert
        Assert.Single(block.Elements);
        var section = Assert.IsType<RichTextSection>(block.Elements[0]);
        Assert.Single(section.Elements);
        var text = Assert.IsType<RichTextText>(section.Elements[0]);
        Assert.Equal("Hello world", text.Text);
    }
    
    [Fact]
    public void Performance_AddingManyTextElements()
    {
        // Arrange
        const int iterations = 1000;
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < iterations; i++)
        {
            var builder = new RichTextBuilder();
            
            // Add multiple text sections with different formatting
            builder.AddSection(section => 
            {
                for (int j = 0; j < 10; j++)
                {
                    section.Add(new RichTextText { Text = $"Text section {j} in iteration {i}" });
                }
            });
            
            // Add a list with multiple items
            builder.AddTextList(RichTextListStyle.Bullet, list =>
            {
                for (int j = 0; j < 5; j++)
                {
                    list.AddSection(section => 
                        section.Add(new RichTextText { Text = $"List item {j} in iteration {i}" }));
                }
            });
            
            // Build the block
            builder.Build();
        }
        
        stopwatch.Stop();
        
        // Output performance metrics
        var msPerOperation = stopwatch.ElapsedMilliseconds / (double)iterations;
        Console.WriteLine($"RichTextBuilder operations took {msPerOperation:F6} ms per iteration");
        
        // No specific assertion, this is a baseline measurement
    }
} 