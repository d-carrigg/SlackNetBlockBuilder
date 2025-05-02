using JetBrains.Annotations;
using SlackNet.Blocks;
using System.Diagnostics;
using Xunit.Abstractions;

namespace UnitTests;

[TestSubject(typeof(BlockBuilder))]
public class BlockBuilderTest
{
    private readonly ITestOutputHelper _output;

    public BlockBuilderTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Build_WithMultipleFocusedElements_ThrowsException()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        
        // Add two input blocks with focused elements
        builder.AddInput<PlainTextInput>("Label 1", input => input
            .Set(x => x.FocusOnLoad = true));
            
        builder.AddInput<PlainTextInput>("Label 2", input => input
            .Set(x => x.FocusOnLoad = true));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithSingleFocusedElement_Succeeds()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        
        // Add input block with focused element
        builder.AddInput<PlainTextInput>("Label", input => input
            .Set(x => x.FocusOnLoad = true));

        // Act
        var blocks = builder.Build();

        // Assert
        Assert.Single(blocks);
        var inputBlock = Assert.IsType<InputBlock>(blocks[0]);
        var textInput = Assert.IsType<PlainTextInput>(inputBlock.Element);
        Assert.True(textInput.FocusOnLoad);
    }

 
    
    
    [Fact]
    public void AddBlock_ChainedCalls_BuildsCorrectly()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act
        builder
            .AddDivider()
            .AddHeader(new PlainText("Header"))
            .AddSection(section => section.Text("Section text"));

        var blocks = builder.Build();

        // Assert
        Assert.Equal(3, blocks.Count);
        Assert.IsType<DividerBlock>(blocks[0]);
        Assert.IsType<HeaderBlock>(blocks[1]); 
        Assert.IsType<SectionBlock>(blocks[2]);
    }

    [Fact]
    public void Performance_AddingManyBlocks()
    {
        // Arrange
        const int iterations = 1000;
        var stopwatch = new Stopwatch();
        
        // Act
        stopwatch.Start();
        
        for (var i = 0; i < iterations; i++)
        {
            var builder = BlockBuilder.Create();
            var iLocal = i; // Capture the loop variable
            
            // Add 10 different blocks
            builder.AddDivider();
            builder.AddHeader(new PlainText($"Header {iLocal}"));
            builder.AddSection(section => section.Text($"Section text {iLocal}"));
            builder.AddContext(context => context.AddText($"Context text {iLocal}"));
            builder.AddActions(actions => actions.AddButton($"Button {iLocal}", $"button-{iLocal}"));
            builder.AddInput<PlainTextInput>($"Label {iLocal}", input => input.Set(x => x.ActionId = $"input-{iLocal}"));
            builder.AddDivider();
            builder.AddHeader(new PlainText($"Another Header {iLocal}"));
            builder.AddSection(section => section.Text($"Another Section text {iLocal}"));
            builder.AddContext(context => context.AddText($"Another Context text {iLocal}"));
            
            // Build the blocks
            var blocks = builder.Build();
            Assert.NotEmpty(blocks);
        }
        
        stopwatch.Stop();
        
        // Output performance metrics
        var msPerIteration = (double)stopwatch.ElapsedMilliseconds / iterations;
        _output.WriteLine($"BlockBuilder operations took {msPerIteration:F6} ms per iteration");
        
        // No specific assertion, this is a baseline measurement
    }
    
    [Fact]
    public void RemoveBlock_ValidBlockId_RemovesBlock()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddHeader("Header", "id_1")
            .AddHeader("Header 2", "id_2");
        

        // Act
        var isRemoved = builder.Remove("id_1");
        var blocks = builder.Build();

        // Assert
        Assert.True(isRemoved);
        Assert.Single(blocks);
        Assert.Equal("id_2", ((HeaderBlock)blocks[0]).BlockId);
    }
    
    // removing a block that doesn't exist should return false
    [Fact]
    public void RemoveBlock_InvalidBlockId_ReturnsFalse()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddHeader("Header", "id_1")
            .AddHeader("Header 2", "id_2");

        
        // Act
        var isRemoved = builder.Remove("id_3");
        
        // Assert
        Assert.False(isRemoved);
    }
    
    
    // removing an action that exists should return true
    [Fact]
    public void RemoveAction_ValidActionId_RemovesAction()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddActions(actions => actions
            .AddButton("button_1", "Button 1")
            .AddButton("button_2", "Button 2"));

        // Act
        var isRemoved = builder.RemoveAction("button_1");
        var blocks = builder.Build();

        // Assert
        Assert.True(isRemoved);
        Assert.Single(blocks);
        var actionsBlock = Assert.IsType<ActionsBlock>(blocks[0]);
        Assert.Single(actionsBlock.Elements);
    }
    
    // removing an action that doesn't exist should return false
    [Fact]
    public void RemoveAction_InvalidActionId_ReturnsFalse()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddActions(actions => actions
            .AddButton("button_1", "Button 1")
            .AddButton("button_2", "Button 2"));

        // Act
        var isRemoved = builder.RemoveAction("button_3");
        
        // Assert
        Assert.False(isRemoved);
    }
    
 
}