using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

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
            var index = i;
            builder.AddButton($"button_{i}", button => button.Set(b => b.Text = $"Button {index}"));
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

    // === Missing Coverage Tests ===
    
    [Fact]
    public void AddTimePicker_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddTimePicker("time_picker_1", timePicker => timePicker
            .Set(tp => tp.Placeholder = new PlainText { Text = "Select time" })
            .Set(tp => tp.InitialTime = TimeSpan.Parse("14:30")));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var timePickerElement = Assert.IsType<TimePicker>(result.Elements[0]);
        Assert.Equal("time_picker_1", timePickerElement.ActionId);
        Assert.Equal("Select time", timePickerElement.Placeholder.Text);
        Assert.Equal(TimeSpan.Parse("14:30"), timePickerElement.InitialTime);
    }

    [Fact]
    public void AddDateTimePicker_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddDateTimePicker("datetime_picker_1", dateTimePicker => dateTimePicker
            .Set(dtp => dtp.InitialDateTime = DateTime.Parse("2024-01-01")));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var dateTimePickerElement = Assert.IsType<DateTimePicker>(result.Elements[0]);
        Assert.Equal("datetime_picker_1", dateTimePickerElement.ActionId);
        Assert.Equal(DateTime.Parse("2024-01-01"), dateTimePickerElement.InitialDateTime);
    }

    [Fact]
    public void AddRadioButtonGroup_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddRadioButtonGroup("radio_group_1", radioGroup => radioGroup
            .Set(rg => rg.Options = new List<Option>
            {
                new Option { Text = "Option 1", Value = "value1" },
                new Option { Text = "Option 2", Value = "value2" }
            }));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var radioGroupElement = Assert.IsType<RadioButtonGroup>(result.Elements[0]);
        Assert.Equal("radio_group_1", radioGroupElement.ActionId);
        Assert.Equal(2, radioGroupElement.Options.Count);
        Assert.Equal("Option 1", radioGroupElement.Options[0].Text.Text);
        Assert.Equal("value1", radioGroupElement.Options[0].Value);
    }

    [Fact]
    public void AddExternalSelectMenu_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddExternalSelectMenu("external_select_1", externalSelect => externalSelect
            .Set(es => es.Placeholder = new PlainText { Text = "Select external option" })
            .Set(es => es.MinQueryLength = 3));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var externalSelectElement = Assert.IsType<ExternalSelectMenu>(result.Elements[0]);
        Assert.Equal("external_select_1", externalSelectElement.ActionId);
        Assert.Equal("Select external option", externalSelectElement.Placeholder.Text);
        Assert.Equal(3, externalSelectElement.MinQueryLength);
    }

    [Fact]
    public void AddMultiStaticSelectMenu_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddMultiStaticSelectMenu("multi_static_select_1", multiStaticSelect => multiStaticSelect
            .Set(mss => mss.Placeholder = new PlainText { Text = "Select multiple options" })
            .Set(mss => mss.Options = new List<Option>
            {
                new Option { Text = "Option A", Value = "a" },
                new Option { Text = "Option B", Value = "b" }
            })
            .Set(mss => mss.MaxSelectedItems = 5));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var multiStaticSelectElement = Assert.IsType<StaticMultiSelectMenu>(result.Elements[0]);
        Assert.Equal("multi_static_select_1", multiStaticSelectElement.ActionId);
        Assert.Equal("Select multiple options", multiStaticSelectElement.Placeholder.Text);
        Assert.Equal(2, multiStaticSelectElement.Options.Count);
        Assert.Equal(5, multiStaticSelectElement.MaxSelectedItems);
    }

    [Fact]
    public void AddMultiExternalSelectMenu_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddMultiExternalSelectMenu("multi_external_select_1", multiExternalSelect => multiExternalSelect
            .Set(mes => mes.Placeholder = new PlainText { Text = "Select multiple external options" })
            .Set(mes => mes.MinQueryLength = 2)
            .Set(mes => mes.MaxSelectedItems = 10));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var multiExternalSelectElement = Assert.IsType<ExternalMultiSelectMenu>(result.Elements[0]);
        Assert.Equal("multi_external_select_1", multiExternalSelectElement.ActionId);
        Assert.Equal("Select multiple external options", multiExternalSelectElement.Placeholder.Text);
        Assert.Equal(2, multiExternalSelectElement.MinQueryLength);
        Assert.Equal(10, multiExternalSelectElement.MaxSelectedItems);
    }

    [Fact]
    public void AddMultiUserSelectMenu_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddMultiUserSelectMenu("multi_user_select_1", multiUserSelect => multiUserSelect
            .Set(mus => mus.Placeholder = new PlainText { Text = "Select multiple users" })
            .Set(mus => mus.MaxSelectedItems = 3));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var multiUserSelectElement = Assert.IsType<UserMultiSelectMenu>(result.Elements[0]);
        Assert.Equal("multi_user_select_1", multiUserSelectElement.ActionId);
        Assert.Equal("Select multiple users", multiUserSelectElement.Placeholder.Text);
        Assert.Equal(3, multiUserSelectElement.MaxSelectedItems);
    }

    [Fact]
    public void AddMultiConversationSelectMenu_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddMultiConversationSelectMenu("multi_conversation_select_1", multiConversationSelect => multiConversationSelect
            .Set(mcs => mcs.Placeholder = new PlainText { Text = "Select multiple conversations" })
            .Set(mcs => mcs.MaxSelectedItems = 5)
            .Set(mcs => mcs.DefaultToCurrentConversation = true));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var multiConversationSelectElement = Assert.IsType<ConversationMultiSelectMenu>(result.Elements[0]);
        Assert.Equal("multi_conversation_select_1", multiConversationSelectElement.ActionId);
        Assert.Equal("Select multiple conversations", multiConversationSelectElement.Placeholder.Text);
        Assert.Equal(5, multiConversationSelectElement.MaxSelectedItems);
        Assert.True(multiConversationSelectElement.DefaultToCurrentConversation);
    }

    [Fact]
    public void AddMultiChannelSelectMenu_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act
        builder.AddMultiChannelSelectMenu("multi_channel_select_1", multiChannelSelect => multiChannelSelect
            .Set(mcs => mcs.Placeholder = new PlainText { Text = "Select multiple channels" })
            .Set(mcs => mcs.MaxSelectedItems = 7));

        var result = builder.Build();

        // Assert
        Assert.Single(result.Elements);
        var multiChannelSelectElement = Assert.IsType<ChannelMultiSelectMenu>(result.Elements[0]);
        Assert.Equal("multi_channel_select_1", multiChannelSelectElement.ActionId);
        Assert.Equal("Select multiple channels", multiChannelSelectElement.Placeholder.Text);
        Assert.Equal(7, multiChannelSelectElement.MaxSelectedItems);
    }

    // === Integration Tests for Missing Methods ===

    [Fact]
    public void ActionsBlock_WithAllMissingElementTypes_BuildsCorrectly()
    {
        // Arrange
        var builder = ActionsBlockBuilder.Create();

        // Act - Add all the previously uncovered element types
        builder
            .AddTimePicker("time_1", tp => tp.Set(x => x.Placeholder = new PlainText { Text = "Time" }))
            .AddDateTimePicker("datetime_1", dtp => dtp.Set(x => x.InitialDateTime = DateTime.Parse("2024-01-01")))
            .AddRadioButtonGroup("radio_1", rg => rg.Set(x => x.Options = new List<Option> { new Option { Text = "Radio", Value = "r1" } }))
            .AddExternalSelectMenu("external_1", es => es.Set(x => x.Placeholder = new PlainText { Text = "External" }))
            .AddMultiStaticSelectMenu("multi_static_1", mss => mss.Set(x => x.Placeholder = new PlainText { Text = "Multi Static" }))
            .AddMultiExternalSelectMenu("multi_external_1", mes => mes.Set(x => x.Placeholder = new PlainText { Text = "Multi External" }))
            .AddMultiUserSelectMenu("multi_user_1", mus => mus.Set(x => x.Placeholder = new PlainText { Text = "Multi User" }))
            .AddMultiConversationSelectMenu("multi_conv_1", mcs => mcs.Set(x => x.Placeholder = new PlainText { Text = "Multi Conversation" }))
            .AddMultiChannelSelectMenu("multi_channel_1", mcs => mcs.Set(x => x.Placeholder = new PlainText { Text = "Multi Channel" }));

        var result = builder.Build();

        // Assert
        Assert.Equal(9, result.Elements.Count);
        
        // Verify each element type and action ID
        Assert.IsType<TimePicker>(result.Elements[0]);
        Assert.Equal("time_1", result.Elements[0].ActionId);
        
        Assert.IsType<DateTimePicker>(result.Elements[1]);
        Assert.Equal("datetime_1", result.Elements[1].ActionId);
        
        Assert.IsType<RadioButtonGroup>(result.Elements[2]);
        Assert.Equal("radio_1", result.Elements[2].ActionId);
        
        Assert.IsType<ExternalSelectMenu>(result.Elements[3]);
        Assert.Equal("external_1", result.Elements[3].ActionId);
        
        Assert.IsType<StaticMultiSelectMenu>(result.Elements[4]);
        Assert.Equal("multi_static_1", result.Elements[4].ActionId);
        
        Assert.IsType<ExternalMultiSelectMenu>(result.Elements[5]);
        Assert.Equal("multi_external_1", result.Elements[5].ActionId);
        
        Assert.IsType<UserMultiSelectMenu>(result.Elements[6]);
        Assert.Equal("multi_user_1", result.Elements[6].ActionId);
        
        Assert.IsType<ConversationMultiSelectMenu>(result.Elements[7]);
        Assert.Equal("multi_conv_1", result.Elements[7].ActionId);
        
        Assert.IsType<ChannelMultiSelectMenu>(result.Elements[8]);
        Assert.Equal("multi_channel_1", result.Elements[8].ActionId);
    }
}