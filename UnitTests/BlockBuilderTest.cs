using JetBrains.Annotations;

using SlackNet.Blocks;

namespace UnitTests.Extensions.Slack;

[TestSubject(typeof(BlockBuilder))]
public class BlockBuilderTest
{
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
}