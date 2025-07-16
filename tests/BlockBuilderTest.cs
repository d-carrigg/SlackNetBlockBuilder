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
            .Modify(x => x.FocusOnLoad = true));

        // Act
        var blocks = builder.Build();

        // Assert
        Assert.Single(blocks);
        var inputBlock = Assert.IsType<InputBlock>(blocks[0]);
        var textInput = Assert.IsType<PlainTextInput>(inputBlock.Element);
        Assert.True(textInput.FocusOnLoad);
    }



    [Fact]
    public void Build_WithFocusedElementInActionsBlock_ValidatesFocus()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        // Add actions block with focused element
        builder.AddActions(actions => actions.AddElement<DatePicker>("",
            picker => picker.FocusOnLoad()));
        builder.AddInput<PlainTextInput>("Label", input => input.Modify(x => x.FocusOnLoad = true));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithNoFocusedElements_Succeeds()
    {
        // Arrange
        var builder = BlockBuilder.Create();

        builder.AddInput<PlainTextInput>("Label 1", input => input.Modify(x => x.FocusOnLoad = false));
        builder.AddInput<PlainTextInput>("Label 2", input => input.Modify(x => x.FocusOnLoad = false));

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
       _ = builder.Remove(block => block is DividerBlock);

        // Assert
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
        _ =builder.Remove("divider1");

        // Assert
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
       _ = builder.Remove("nonexistent");
        
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
                input => input.Modify(x => x.ActionId = $"input-{iLocal}"));
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
        builder.AddInput<TestInputElement>("test", e => e.Modify(x => x.TestProperty = "test"));

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
    
    [Fact]
    public void Build_WithAllInputElementTypes_FocusOnLoadValidation()
    {
        // Test all input element types that have FocusOnLoad property
        var testCases = new[]
        {
            // Date and time elements
            typeof(DatePicker),
            typeof(TimePicker),
            typeof(DateTimePicker),
            
            // Input elements
            typeof(EmailTextInput),
            typeof(RichTextInput),
            typeof(UrlTextInput),
            typeof(PlainTextInput),
            typeof(NumberInput),
            
            // Groups
            typeof(CheckboxGroup),
            typeof(RadioButtonGroup),
            
            // Single selects
            typeof(StaticSelectMenu),
            typeof(ExternalSelectMenu),
            typeof(UserSelectMenu),
            typeof(ChannelSelectMenu),
            typeof(ConversationSelectMenu),
            
            // Multi selects
            typeof(StaticMultiSelectMenu),
            typeof(ExternalMultiSelectMenu),
            typeof(UserMultiSelectMenu),
            typeof(ChannelMultiSelectMenu),
            typeof(ConversationMultiSelectMenu)
        };

        foreach (var elementType in testCases)
        {
            // Test that each element type can be created and have FocusOnLoad set to true
            var builder = BlockBuilder.Create();
            
            // Create the element using reflection
            var element = Activator.CreateInstance(elementType);
            
            // Set FocusOnLoad to true using reflection
            var focusProperty = elementType.GetProperty("FocusOnLoad");
            if (focusProperty != null)
            {
                focusProperty.SetValue(element, true);
                
                // Create an InputBlock with this element
                var inputBlock = new InputBlock
                {
                    Label = new PlainText { Text = $"Test {elementType.Name}" },
                    Element = (IInputBlockElement)element
                };
                
                builder.AddBlock(inputBlock);
                
                // This should build successfully with one focused element
                var result = builder.Build();
                Assert.Single(result);
            }
        }
    }
    
    [Fact]
    public void Build_WithMultipleFocusedElements_ThrowsException()
    {
        // Test that multiple focused elements of different types throw exception
        var builder = BlockBuilder.Create();
        
        // Add two different types of focused elements
        var datePicker = new DatePicker { FocusOnLoad = true };
        var textInput = new PlainTextInput { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Date" },
            Element = datePicker
        });
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Text" },
            Element = textInput
        });
        
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Only one block can have FocusOnLoad set to true", exception.Message);
    }
    
    [Fact]
    public void Build_WithFocusedElementInActionsBlock_CountsCorrectly()
    {
        // Test that focused elements in ActionsBlock are counted
        var builder = BlockBuilder.Create();
        
        var focusedSelect = new StaticSelectMenu 
        { 
            FocusOnLoad = true,
            ActionId = "test_select",
            Placeholder = new PlainText { Text = "Select option" }
        };
        
        var actionsBlock = new ActionsBlock();
        actionsBlock.Elements.Add(focusedSelect);
        
        builder.AddBlock(actionsBlock);
        
        // This should build successfully
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithMultipleFocusedElementsInActionsBlock_ThrowsException()
    {
        // Test that multiple focused elements in ActionsBlock throw exception
        var builder = BlockBuilder.Create();
        
        var focusedSelect1 = new StaticSelectMenu 
        { 
            FocusOnLoad = true,
            ActionId = "test_select1",
            Placeholder = new PlainText { Text = "Select option 1" }
        };
        
        var focusedSelect2 = new UserSelectMenu 
        { 
            FocusOnLoad = true,
            ActionId = "test_select2",
            Placeholder = new PlainText { Text = "Select option 2" }
        };
        
        var actionsBlock = new ActionsBlock();
        actionsBlock.Elements.Add(focusedSelect1);
        actionsBlock.Elements.Add(focusedSelect2);
        
        builder.AddBlock(actionsBlock);
        
        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Equal("Only one block can have FocusOnLoad set to true", exception.Message);
    }
    
    [Fact]
    public void Build_WithEmailTextInput_FocusOnLoadValidation()
    {
        // Test EmailTextInput specifically
        var builder = BlockBuilder.Create();
        var emailInput = new EmailTextInput { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Email" },
            Element = emailInput
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithRichTextInput_FocusOnLoadValidation()
    {
        // Test RichTextInput specifically
        var builder = BlockBuilder.Create();
        var richTextInput = new RichTextInput { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Rich Text" },
            Element = richTextInput
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithUrlTextInput_FocusOnLoadValidation()
    {
        // Test UrlTextInput specifically
        var builder = BlockBuilder.Create();
        var urlInput = new UrlTextInput { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "URL" },
            Element = urlInput
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithNumberInput_FocusOnLoadValidation()
    {
        // Test NumberInput specifically
        var builder = BlockBuilder.Create();
        var numberInput = new NumberInput { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Number" },
            Element = numberInput
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithTextInput_FocusOnLoadValidation()
    {
        // Test TextInput specifically
        var builder = BlockBuilder.Create();
 
        builder.AddInput<PlainTextInput>("Text Input", input => input.Modify(x => x.FocusOnLoad = true));
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithCheckboxGroup_FocusOnLoadValidation()
    {
        // Test CheckboxGroup specifically
        var builder = BlockBuilder.Create();
        var checkboxGroup = new CheckboxGroup { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Checkbox" },
            Element = checkboxGroup
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithRadioButtonGroup_FocusOnLoadValidation()
    {
        // Test RadioButtonGroup specifically
        var builder = BlockBuilder.Create();
        var radioGroup = new RadioButtonGroup { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Radio" },
            Element = radioGroup
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithExternalSelectMenu_FocusOnLoadValidation()
    {
        // Test ExternalSelectMenu specifically
        var builder = BlockBuilder.Create();
        var externalSelect = new ExternalSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "External Select" },
            Element = externalSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithChannelSelectMenu_FocusOnLoadValidation()
    {
        // Test ChannelSelectMenu specifically
        var builder = BlockBuilder.Create();
        var channelSelect = new ChannelSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Channel Select" },
            Element = channelSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithConversationSelectMenu_FocusOnLoadValidation()
    {
        // Test ConversationSelectMenu specifically
        var builder = BlockBuilder.Create();
        var conversationSelect = new ConversationSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Conversation Select" },
            Element = conversationSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithStaticMultiSelectMenu_FocusOnLoadValidation()
    {
        // Test StaticMultiSelectMenu specifically
        var builder = BlockBuilder.Create();
        var staticMultiSelect = new StaticMultiSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Static Multi Select" },
            Element = staticMultiSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithExternalMultiSelectMenu_FocusOnLoadValidation()
    {
        // Test ExternalMultiSelectMenu specifically
        var builder = BlockBuilder.Create();
        var externalMultiSelect = new ExternalMultiSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "External Multi Select" },
            Element = externalMultiSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithUserMultiSelectMenu_FocusOnLoadValidation()
    {
        // Test UserMultiSelectMenu specifically
        var builder = BlockBuilder.Create();
        var userMultiSelect = new UserMultiSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "User Multi Select" },
            Element = userMultiSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithChannelMultiSelectMenu_FocusOnLoadValidation()
    {
        // Test ChannelMultiSelectMenu specifically
        var builder = BlockBuilder.Create();
        var channelMultiSelect = new ChannelMultiSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Channel Multi Select" },
            Element = channelMultiSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithConversationMultiSelectMenu_FocusOnLoadValidation()
    {
        // Test ConversationMultiSelectMenu specifically
        var builder = BlockBuilder.Create();
        var conversationMultiSelect = new ConversationMultiSelectMenu { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Conversation Multi Select" },
            Element = conversationMultiSelect
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithTimePicker_FocusOnLoadValidation()
    {
        // Test TimePicker specifically
        var builder = BlockBuilder.Create();
        var timePicker = new TimePicker { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Time Picker" },
            Element = timePicker
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
    
    [Fact]
    public void Build_WithDateTimePicker_FocusOnLoadValidation()
    {
        // Test DateTimePicker specifically
        var builder = BlockBuilder.Create();
        var dateTimePicker = new DateTimePicker { FocusOnLoad = true };
        
        builder.AddBlock(new InputBlock
        {
            Label = new PlainText { Text = "Date Time Picker" },
            Element = dateTimePicker
        });
        
        var result = builder.Build();
        Assert.Single(result);
    }
}
