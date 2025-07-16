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

    // === Constructor Tests ===

    [Fact]
    public void Constructor_CreatesEmptyBuilder()
    {
        // Act
        var builder = new BlockBuilder();

        // Assert
        var blocks = builder.Build();
        Assert.Empty(blocks);
    }

    [Fact]
    public void Create_CreatesNewInstance()
    {
        // Act
        var builder = BlockBuilder.Create();

        // Assert
        Assert.NotNull(builder);
        var blocks = builder.Build();
        Assert.Empty(blocks);
    }

    [Fact]
    public void From_WithValidBlocks_CreatesBuilderWithBlocks()
    {
        // Arrange
        var existingBlocks = new List<Block>
        {
            new DividerBlock(),
            new HeaderBlock { Text = new PlainText { Text = "Header" } }
        };

        // Act
        var builder = BlockBuilder.From(existingBlocks);

        // Assert
        var blocks = builder.Build();
        Assert.Equal(2, blocks.Count);
        Assert.IsType<DividerBlock>(blocks[0]);
        Assert.IsType<HeaderBlock>(blocks[1]);
    }

    [Fact]
    public void From_WithNullBlocks_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => BlockBuilder.From(null));
    }

    // === AddBlock Tests ===

    [Fact]
    public void AddBlock_WithValidBlock_AddsBlock()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        var block = new DividerBlock();

        // Act
        var result = builder.AddBlock(block);

        // Assert
        Assert.Same(builder, result);
        var blocks = builder.Build();
        Assert.Single(blocks);
        Assert.Same(block, blocks[0]);
    }

    [Fact]
    public void AddBlock_WithNullBlock_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddBlock(null));
    }

    [Fact]
    public void AddBlocks_WithValidBlocks_AddsAllBlocks()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        var blocksToAdd = new List<Block>
        {
            new DividerBlock(),
            new HeaderBlock { Text = new PlainText { Text = "Header" } }
        };

        // Act
        var result = builder.AddBlocks(blocksToAdd);

        // Assert
        Assert.Same(builder, result);
        var blocks = builder.Build();
        Assert.Equal(2, blocks.Count);
        Assert.IsType<DividerBlock>(blocks[0]);
        Assert.IsType<HeaderBlock>(blocks[1]);
    }

    [Fact]
    public void AddBlocks_WithNullBlocks_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddBlocks(null));
    }

    // === Add<T> Tests ===

    [Fact]
    public void Add_WithValidModifier_AddsConfiguredBlock()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act
        var result = builder.Add<DividerBlock>(divider => divider.BlockId = "test_divider");

        // Assert
        Assert.Same(builder, result);
        var blocks = builder.Build();
        Assert.Single(blocks);
        var divider = Assert.IsType<DividerBlock>(blocks[0]);
        Assert.Equal("test_divider", divider.BlockId);
    }

    [Fact]
    public void Add_WithNullModifier_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Add<DividerBlock>(null));
    }

    // === Focus Validation Tests ===

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
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Only one block can have FocusOnLoad set to true", exception.Message);
    }

    [Fact]
    public void Build_WithFocusedElementInActionsBlock_ValidatesFocus()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Add actions block with focused element
        builder.AddActions(actions => actions.AddElement<DatePicker>("",
            picker => picker.FocusOnLoad()));
        builder.AddInput<PlainTextInput>("Label", input => input.Set(x => x.FocusOnLoad = true));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithNoFocusedElements_Succeeds()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        builder.AddInput<PlainTextInput>("Label 1", input => input.Set(x => x.FocusOnLoad = false));
        builder.AddInput<PlainTextInput>("Label 2", input => input.Set(x => x.FocusOnLoad = false));

        // Act
        var blocks = builder.Build();

        // Assert
        Assert.Equal(2, blocks.Count);
    }

    // === Remove Tests ===

    [Fact]
    public void Remove_WithValidPredicate_RemovesMatchingBlocks()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddDivider("divider1");
        builder.AddDivider("divider2");
        builder.AddHeader(new PlainText { Text = "Header" });

        // Act
        var removedCount = builder.Remove(block => block is DividerBlock);

        // Assert
        Assert.Equal(2, removedCount);
        var blocks = builder.Build();
        Assert.Single(blocks);
        Assert.IsType<HeaderBlock>(blocks[0]);
    }

    [Fact]
    public void Remove_WithBlockId_RemovesCorrectBlock()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddDivider("divider1");
        builder.AddDivider("divider2");

        // Act
        var result = builder.Remove("divider1");

        // Assert
        Assert.True(result);
        var blocks = builder.Build();
        Assert.Single(blocks);
        Assert.Equal("divider2", blocks[0].BlockId);
    }

    [Fact]
    public void Remove_WithNonExistentBlockId_ReturnsFalse()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddDivider("divider1");

        // Act
        var result = builder.Remove("nonexistent");

        // Assert
        Assert.False(result);
        var blocks = builder.Build();
        Assert.Single(blocks);
    }

    [Fact]
    public void Remove_WithNullOrEmptyBlockId_ThrowsArgumentException()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Remove((string)null));
        Assert.Throws<ArgumentException>(() => builder.Remove(""));
    }

    // === RemoveAction Tests ===

    [Fact]
    public void RemoveAction_WithValidPredicate_RemovesMatchingAction()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddActions(actions =>
        {
            actions.AddButton("btn1", "Button 1");
            actions.AddButton("btn2", "Button 2");
        });

        // Act
        var result = builder.RemoveAction(action => action.ActionId == "btn1");

        // Assert
        var blocks = builder.Build();
        var actionsBlock = Assert.IsType<ActionsBlock>(blocks[0]);
        Assert.Single(actionsBlock.Elements);
        Assert.Equal("btn2", actionsBlock.Elements[0].ActionId);
    }

    [Fact]
    public void RemoveAction_WithActionId_RemovesCorrectAction()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddActions(actions =>
        {
            actions.AddButton("btn1", "Button 1");
            actions.AddButton("btn2", "Button 2");
        });

        // Act
        var result = builder.RemoveAction("btn1");

        // Assert
        var blocks = builder.Build();
        var actionsBlock = Assert.IsType<ActionsBlock>(blocks[0]);
        Assert.Single(actionsBlock.Elements);
        Assert.Equal("btn2", actionsBlock.Elements[0].ActionId);
    }

    [Fact]
    public void RemoveAction_WithNonExistentAction_DoesNotRemoveAnything()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddActions(actions => actions.AddButton("btn1", "Button 1"));

        // Act
        builder.RemoveAction("nonexistent");

        // Assert
        Assert.Single(builder.Build());

    }

    [Fact]
    public void RemoveAction_WithNullPredicate_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.RemoveAction((Predicate<IActionElement>)null));
    }

    [Fact]
    public void RemoveAction_WithNoActionsBlock_DoesNothing()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddDivider();

        // Act
        var result = builder.RemoveAction("btn1");

        // Assert
        Assert.Single(builder.Build());
    }

    // === Chaining Tests ===

    [Fact]
    public void AddBlock_ChainedCalls_BuildsCorrectly()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act
        builder
            .AddDivider()
            .AddHeader(new PlainText { Text = "Header" })
            .AddSection(section => section.Text("Section text"));

        var blocks = builder.Build();

        // Assert
        Assert.Equal(3, blocks.Count);
        Assert.IsType<DividerBlock>(blocks[0]);
        Assert.IsType<HeaderBlock>(blocks[1]);
        Assert.IsType<SectionBlock>(blocks[2]);
    }

    // === Complex Integration Tests ===

    [Fact]
    public void ComplexWorkflow_BuildUpdateAndRemove_WorksCorrectly()
    {
        // Arrange - Create initial message
        var builder = BlockBuilder.Create()
            .AddHeader(new PlainText { Text = "Task Management" })
            .AddSection(section => section.Text("Please select an action:"))
            .AddActions(actions =>
            {
                actions.AddButton("approve", "Approve");
                actions.AddButton("reject", "Reject");
            });

        var initialBlocks = builder.Build();
        Assert.Equal(3, initialBlocks.Count);
        Assert.Equal(2, initialBlocks.OfType<ActionsBlock>().Sum(x => x.Elements.Count));

        // Act - Update message after user interaction
        var updatedBuilder = BlockBuilder.From(initialBlocks)
            .RemoveAction("approve")
            .RemoveAction("reject")
            .AddSection(section => section.Text("✅ Task has been approved!"))
            .AddContext(context => context.AddText("Approved by user123"));

        var updatedBlocks = updatedBuilder.Build();

        // Assert
        Assert.Equal(0, updatedBlocks.OfType<ActionsBlock>().Sum(x => x.Elements.Count));
        Assert.IsType<HeaderBlock>(updatedBlocks[0]);
        Assert.IsType<SectionBlock>(updatedBlocks[1]);
        Assert.IsType<ActionsBlock>(updatedBlocks[2]);
        Assert.IsType<SectionBlock>(updatedBlocks[3]);
        Assert.IsType<ContextBlock>(updatedBlocks[4]);

        // Verify actions block is now empty
        var actionsBlock = Assert.IsType<ActionsBlock>(updatedBlocks[2]);
        Assert.Empty(actionsBlock.Elements);
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

            // Add 10 different blocks with 7 at the top level
            builder.AddDivider();
            builder.AddHeader(new PlainText { Text = $"Header {iLocal}" });
            builder.AddSection(section => section.Text($"Section text {iLocal}"));
            builder.AddContext(context => context.AddText($"Context text {iLocal}"));
            builder.AddActions(actions => actions.AddButton($"Button {iLocal}", $"button-{iLocal}"));
            builder.AddInput<PlainTextInput>($"Label {iLocal}",
                input => input.Set(x => x.ActionId = $"input-{iLocal}"));
            builder.AddDivider();

            var blocks = builder.Build();
            Assert.Equal(7, blocks.Count);
        }

        stopwatch.Stop();
        _output.WriteLine(
            $"Performance test completed in {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");

        // Assert - Should complete in reasonable time (less than 5 seconds)
        Assert.True(stopwatch.ElapsedMilliseconds < 5000);
    }

    [Fact]
    public void Build_CalledMultipleTimes_ReturnsSameList()
    {
        // Arrange
        var builder = BlockBuilder.Create();
        builder.AddDivider();
        builder.AddHeader(new PlainText { Text = "Header" });

        // Act
        var blocks1 = builder.Build();
        var blocks2 = builder.Build();

        // Assert
        Assert.Equal(blocks1.Count, blocks2.Count);
        Assert.Equal(blocks1[0].GetType(), blocks2[0].GetType());
        Assert.Equal(blocks1[1].GetType(), blocks2[1].GetType());
    }

    [Fact]
    public void EmptyBuilder_Build_ReturnsEmptyList()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Act
        var blocks = builder.Build();

        // Assert
        Assert.Empty(blocks);
    }

    // === Additional Focus Validation Tests for IsElementFocused Coverage ===
    //
    // [Fact]
    // public void Build_WithAllInputElementTypes_ValidatesFocusCorrectly()
    // {
    //     // This test covers the missing branches in IsElementFocused method (27% coverage)
    //     // Testing all input element types that were not covered
    //
    //     var elementTypes = new[]
    //     {
    //         // Date and Time elements (some were missing coverage)
    //         typeof(TimePicker),
    //         typeof(DateTimePicker),
    //
    //         // Input elements (some were missing coverage)
    //         typeof(EmailTextInput),
    //         typeof(RichTextInput),
    //         typeof(UrlTextInput),
    //         typeof(NumberInput),
    //         typeof(TextInput),
    //
    //         // Group elements (some were missing coverage)
    //         typeof(CheckboxGroup),
    //         typeof(RadioButtonGroup),
    //
    //         // Single select menus (some were missing coverage)
    //         typeof(StaticSelectMenu),
    //         typeof(ExternalSelectMenu),
    //         typeof(UserSelectMenu),
    //         typeof(ChannelSelectMenu),
    //         typeof(ConversationSelectMenu),
    //
    //         // Multi-select menus (some were missing coverage)
    //         typeof(StaticMultiSelectMenu),
    //         typeof(ExternalMultiSelectMenu),
    //         typeof(UserMultiSelectMenu),
    //         typeof(ChannelMultiSelectMenu),
    //         typeof(ConversationMultiSelectMenu)
    //     };
    //
    //     foreach (var elementType in elementTypes)
    //     {
    //         // Test with focus enabled
    //         var builderWithFocus = BlockBuilder.Create();
    //         if (elementType == typeof(DatePicker) || elementType == typeof(TimePicker) ||
    //             elementType == typeof(DateTimePicker))
    //         {
    //             // These go in actions blocks
    //             builderWithFocus.AddActions(actions =>
    //             {
    //                 var element = (IActionElement)Activator.CreateInstance(elementType);
    //                 actions.AddElement("",
    //                     element,
    //                     builder =>
    //                         builder.Set(e =>
    //                         {
    //                             if (e is IInputBlockElement inputElement)
    //                             {
    //                                 var focusProperty = elementType.GetProperty("FocusOnLoad");
    //                                 focusProperty?.SetValue(inputElement, true);
    //                             }
    //                         }));
    //             });
    //         }
    //         else
    //         {
    //             // These go in input blocks
    //             builderWithFocus.AddInput(elementType, "", builder => builder.Set(e =>
    //             {
    //                 var focusProperty = elementType.GetProperty("FocusOnLoad");
    //                 focusProperty?.SetValue(e, true);
    //             }));
    //         }
    //
    //         // Should not throw for single focused element
    //         var blocks = builderWithFocus.Build();
    //         Assert.Single(blocks);
    //
    //         // Test with focus disabled
    //         var builderWithoutFocus = BlockBuilder.Create();
    //         if (elementType == typeof(DatePicker) || elementType == typeof(TimePicker) ||
    //             elementType == typeof(DateTimePicker))
    //         {
    //             builderWithoutFocus.AddActions(actions =>
    //             {
    //                 var element = (IActionElement)Activator.CreateInstance(elementType);
    //                 actions.AddElement("", element, builder => builder.Set(e =>
    //                 {
    //                     if (e is IInputBlockElement inputElement)
    //                     {
    //                         var focusProperty = elementType.GetProperty("FocusOnLoad");
    //                         focusProperty?.SetValue(inputElement, false);
    //                     }
    //                 }));
    //             });
    //         }
    //         else
    //         {
    //             builderWithoutFocus.AddInput(elementType, "", builder => builder.Set(e =>
    //             {
    //                 var focusProperty = elementType.GetProperty("FocusOnLoad");
    //                 focusProperty?.SetValue(e, false);
    //             }));
    //         }
    //
    //         // Should not throw for non-focused element
    //         var blocksWithoutFocus = builderWithoutFocus.Build();
    //         Assert.Single(blocksWithoutFocus);
    //     }
    // }

    [Fact]
    public void Build_WithUnsupportedInputElementType_ReturnsFalseForFocus()
    {
        // Test the default case in IsElementFocused (the "_ => false" branch)
        var builder = BlockBuilder.Create();

        // Add a mock input element that doesn't have FocusOnLoad property
        var mockElement = new TestInputElement(); // This would be an element without FocusOnLoad
        builder.AddInput<TestInputElement>("test", e => e.Set(x => x.TestProperty = "test"));

        // Should not throw and should not count as focused
        var blocks = builder.Build();
        Assert.Single(blocks);
    }

    // Test helper class to simulate an input element without FocusOnLoad
    public class TestInputElement : IInputBlockElement
    {
        public string TestProperty { get; set; }
        // Does not have FocusOnLoad property - will hit the default case
        public string Type { get; }
        public string ActionId { get; set; }
    }
}

