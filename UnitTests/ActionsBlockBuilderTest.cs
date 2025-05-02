using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests.Extensions.Slack;

[TestSubject(typeof(ActionsBlockBuilder))]
public class ActionsBlockBuilderTest
{
    [Fact]
    public void Build_WithTooManyElements_ThrowsException()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Add more than MaxElements buttons
        for (int i = 0; i <= ActionsBlockBuilder.MaxElements; i++)
        {
            builder.AddButton($"button_{i}", button => button.Set(b => b.Text = $"Button {i}"));
        }

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void Build_WithBlockIdTooLong_ThrowsException()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();
        builder.WithBlockId(new string('a', ActionsBlockBuilder.MaxBlockIdLength + 1));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void AddButton_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddButton("button_1", button => button
            .Set(b => b.Text = "Click me")
            .ConfirmationDialog(dialog =>
            {
                dialog.Title = "Confirm";
                dialog.Text = "Are you sure?";
            }));

        var block = builder.Build();

        // Assert
        Assert.Single(block.Elements);
        var button = Assert.IsType<Button>(block.Elements[0]);
        Assert.Equal("button_1", button.ActionId);
        Assert.Equal("Click me", button.Text.Text);
        Assert.NotNull(button.Confirm);
    }

    [Fact]
    public void AddElement_WithCustomElement_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();
        var customButton = new Button { Text = "Custom" };

        // Act
        builder.AddElement("custom_1", customButton, button =>
            button.Set(b => b.Style = ButtonStyle.Primary));

        var block = builder.Build();

        // Assert
        Assert.Single(block.Elements);
        var button = Assert.IsType<Button>(block.Elements[0]);
        Assert.Equal("custom_1", button.ActionId);
        Assert.Equal("Custom", button.Text.Text);
        Assert.Equal(ButtonStyle.Primary, button.Style);
    }

    [Fact]
    public void AddSelectMenu_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddStaticSelectMenu("select_1", select => select
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .Placeholder("Select an option"));

        var block = builder.Build();

        // Assert
        Assert.Single(block.Elements);
        var select = Assert.IsType<StaticSelectMenu>(block.Elements[0]);
        Assert.Equal("select_1", select.ActionId);
        Assert.Equal(2, select.Options.Count);
        Assert.Equal("Select an option", select.Placeholder.Text);
    }

    [Fact]
    public void AddMultipleElements_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create()
            .WithBlockId("test_block");

        // Act
        builder
            .AddButton("button_1", button => button.Set(b => b.Text = "Button 1"))
            .AddDatePicker("date_1", date => date.Set(d => d.InitialDate = DateTime.Parse("2024-01-01")))
            .AddCheckboxGroup("check_1", check => check.AddOption("opt1", "Option 1"));

        var block = builder.Build();

        // Assert
        Assert.Equal(3, block.Elements.Count);
        Assert.Equal("test_block", block.BlockId);
        Assert.IsType<Button>(block.Elements[0]);
        Assert.IsType<DatePicker>(block.Elements[1]);
        Assert.IsType<CheckboxGroup>(block.Elements[2]);
    }

    [Theory]
    [InlineData(typeof(UserSelectMenu))]
    [InlineData(typeof(ConversationSelectMenu))]
    [InlineData(typeof(ChannelSelectMenu))]
    public void AddVariousSelectMenus_BuildCorrectly(Type menuType)
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();
        var actionId = "select_1";

        // Act
        switch (menuType.Name)
        {
            case nameof(UserSelectMenu):
                builder.AddUserSelectMenu(actionId, menu =>
                    menu.Set(m => m.Placeholder = "Select user"));
                break;
            case nameof(ConversationSelectMenu):
                builder.AddConversationSelectMenu(actionId, menu =>
                    menu.Set(m => m.Placeholder = "Select conversation"));
                break;
            case nameof(ChannelSelectMenu):
                builder.AddChannelSelectMenu(actionId, menu =>
                    menu.Set(m => m.Placeholder = "Select channel"));
                break;
        }

        var block = builder.Build();

        // Assert
        Assert.Single(block.Elements);
        Assert.IsType(menuType, block.Elements[0]);

        Assert.Equal(actionId, block.Elements[0].ActionId);
    }
}