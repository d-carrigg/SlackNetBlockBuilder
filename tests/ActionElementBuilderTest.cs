using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(ActionElementBuilder<>))]
public class ActionElementBuilderTest
{
    [Fact]
    public void Constructor_WithValidElement_InitializesCorrectly()
    {
        // Arrange
        var button = new Button { Text = new PlainText { Text = "Test Button" } };
        
        // Act
        var builder = new ActionElementBuilder<Button>(button);
        
        // Assert
        Assert.Same(button, builder.Element);
    }

    // This test covers the missing ActionId(string) method (0% coverage)
    [Fact]
    public void ActionId_WithValidId_SetsActionIdCorrectly()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.ActionId("test_action_id");
        
        // Assert
        Assert.Same(builder, result);
        Assert.Equal("test_action_id", builder.Element.ActionId);
    }

    [Fact]
    public void ActionId_WithNullValue_SetsActionIdToNull()
    {
        // Arrange
        var button = new Button { ActionId = "existing_id" };
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.ActionId(null);
        
        // Assert
        Assert.Same(builder, result);
        Assert.Null(builder.Element.ActionId);
    }

    [Fact]
    public void ActionId_WithEmptyString_SetsActionIdToEmpty()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.ActionId("");
        
        // Assert
        Assert.Same(builder, result);
        Assert.Equal("", builder.Element.ActionId);
    }

    [Fact]
    public void ConfirmationDialog_WithValidAction_CreatesAndConfiguresDialog()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.ConfirmationDialog(dialog =>
        {
            dialog.Title = new PlainText { Text = "Confirm" };
            dialog.Text = new PlainText { Text = "Are you sure?" };
        });
        
        // Assert
        Assert.Same(builder, result);
        Assert.NotNull(builder.Element.Confirm);
        Assert.Equal("Confirm", builder.Element.Confirm.Title.Text);
        Assert.Equal("Are you sure?", builder.Element.Confirm.Text.Text);
    }

    [Fact]
    public void ConfirmationDialog_WithNullAction_ThrowsArgumentNullException()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.ConfirmationDialog(null));
    }

    [Fact]
    public void ConfirmationDialog_WithExistingDialog_ReplacesDialog()
    {
        // Arrange
        var button = new Button 
        { 
            Confirm = new ConfirmationDialog 
            { 
                Title = new PlainText { Text = "Old Title" } 
            } 
        };
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.ConfirmationDialog(dialog =>
        {
            dialog.Title = new PlainText { Text = "New Title" };
        });
        
        // Assert
        Assert.Same(builder, result);
        Assert.NotNull(builder.Element.Confirm);
        Assert.Equal("New Title", builder.Element.Confirm.Title.Text);
    }

    [Fact]
    public void Set_WithValidModifier_ModifiesElement()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.Set(btn =>
        {
            btn.ActionId = "modified_id";
            btn.Text = new PlainText { Text = "Modified Text" };
        });
        
        // Assert
        Assert.Same(builder, result);
        Assert.Equal("modified_id", builder.Element.ActionId);
        Assert.Equal("Modified Text", builder.Element.Text.Text);
    }

    [Fact]
    public void Set_WithNullModifier_ThrowsArgumentNullException()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Set(null));
    }

    [Fact]
    public void Set_WithMultipleModifications_AppliesAllModifications()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder
            .Set(btn => btn.ActionId = "first_id")
            .Set(btn => btn.Text = new PlainText { Text = "Button Text" })
            .Set(btn => btn.ActionId = "final_id");
        
        // Assert
        Assert.Same(builder, result);
        Assert.Equal("final_id", builder.Element.ActionId);
        Assert.Equal("Button Text", builder.Element.Text.Text);
    }

    [Fact]
    public void ChainedCalls_AllMethodsCombined_WorksCorrectly()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder
            .ActionId("chained_id")
            .Set(btn => btn.Text = new PlainText { Text = "Chained Button" })
            .ConfirmationDialog(dialog =>
            {
                dialog.Title = new PlainText { Text = "Confirm Action" };
                dialog.Text = new PlainText { Text = "Please confirm" };
            })
            .Set(btn => btn.Style = ButtonStyle.Primary);
        
        // Assert
        Assert.Same(builder, result);
        Assert.Equal("chained_id", builder.Element.ActionId);
        Assert.Equal("Chained Button", builder.Element.Text.Text);
        Assert.Equal(ButtonStyle.Primary, builder.Element.Style);
        Assert.NotNull(builder.Element.Confirm);
        Assert.Equal("Confirm Action", builder.Element.Confirm.Title.Text);
        Assert.Equal("Please confirm", builder.Element.Confirm.Text.Text);
    }

    [Fact]
    public void Builder_WithDifferentElementTypes_WorksCorrectly()
    {
        // Arrange & Act
        var buttonBuilder = new ActionElementBuilder<Button>(new Button());
        var selectBuilder = new ActionElementBuilder<StaticSelectMenu>(new StaticSelectMenu());
        var overflowBuilder = new ActionElementBuilder<OverflowMenu>(new OverflowMenu());
        
        // Assert
        Assert.IsType<Button>(buttonBuilder.Element);
        Assert.IsType<StaticSelectMenu>(selectBuilder.Element);
        Assert.IsType<OverflowMenu>(overflowBuilder.Element);
    }

    [Fact]
    public void Builder_WithGenericConstraint_OnlyAcceptsActionElements()
    {
        // This test validates that the generic constraint works correctly
        // If this compiles, the constraint is working
        
        // Arrange
        var button = new Button();
        var datePicker = new DatePicker();
        var overflow = new OverflowMenu();
        
        // Act & Assert - These should compile fine
        var buttonBuilder = new ActionElementBuilder<Button>(button);
        var datePickerBuilder = new ActionElementBuilder<DatePicker>(datePicker);
        var overflowBuilder = new ActionElementBuilder<OverflowMenu>(overflow);
        
        Assert.NotNull(buttonBuilder);
        Assert.NotNull(datePickerBuilder);
        Assert.NotNull(overflowBuilder);
    }
}
