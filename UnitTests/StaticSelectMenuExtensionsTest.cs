using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(SelectMenuBaseExtensions))]
public class StaticSelectMenuExtensionsTest
{
    [Fact]
    public void AddOption_AddsOptionToStaticSelectMenu()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        
        // Act
        var result = builder.AddOption("value1", "Option 1");
        
        // Assert
        Assert.Single(selectMenu.Options);
        Assert.Equal("value1", selectMenu.Options[0].Value);
        Assert.Equal("Option 1", selectMenu.Options[0].Text.Text);
        Assert.Null(selectMenu.Options[0].Description);
        Assert.Same(builder, result); // Ensures method returns the same builder for chaining
    }
    
    [Fact]
    public void AddOption_WithDescription_AddsOptionWithDescription()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        var description = new PlainText { Text = "Description text" };
        
        // Act
        var result = builder.AddOption("value1", "Option 1", description);
        
        // Assert
        Assert.Single(selectMenu.Options);
        Assert.Equal("value1", selectMenu.Options[0].Value);
        Assert.Equal("Option 1", selectMenu.Options[0].Text.Text);
        Assert.Same(description, selectMenu.Options[0].Description);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AddOptionGroup_WithOptionsList_AddsOptionGroupToStaticSelectMenu()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        var options = new List<Option>
        {
            new() { Text = "Option 1", Value = "value1" },
            new() { Text = "Option 2", Value = "value2" }
        };
        
        // Act
        var result = builder.AddOptionGroup("Group 1", options);
        
        // Assert
        Assert.Single(selectMenu.OptionGroups);
        Assert.Equal("Group 1", selectMenu.OptionGroups[0].Label.Text);
        Assert.Equal(2, selectMenu.OptionGroups[0].Options.Count);
        Assert.Equal("value1", selectMenu.OptionGroups[0].Options[0].Value);
        Assert.Equal("Option 1", selectMenu.OptionGroups[0].Options[0].Text.Text);
        Assert.Equal("value2", selectMenu.OptionGroups[0].Options[1].Value);
        Assert.Equal("Option 2", selectMenu.OptionGroups[0].Options[1].Text.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AddOptionGroup_WithBuilder_AddsOptionGroupToStaticSelectMenu()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        
        // Act
        var result = builder.AddOptionGroup("Group 1", group =>
        {
            group.AddOption("value1", "Option 1");
            group.AddOption("value2", "Option 2");
        });
        
        // Assert
        Assert.Single(selectMenu.OptionGroups);
        Assert.Equal("Group 1", selectMenu.OptionGroups[0].Label.Text);
        Assert.Equal(2, selectMenu.OptionGroups[0].Options.Count);
        Assert.Equal("value1", selectMenu.OptionGroups[0].Options[0].Value);
        Assert.Equal("Option 1", selectMenu.OptionGroups[0].Options[0].Text.Text);
        Assert.Equal("value2", selectMenu.OptionGroups[0].Options[1].Value);
        Assert.Equal("Option 2", selectMenu.OptionGroups[0].Options[1].Text.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InitialOption_SetsInitialOptionByValue()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        
        // Add some options
        builder.AddOption("value1", "Option 1");
        builder.AddOption("value2", "Option 2");
        builder.AddOption("value3", "Option 3");
        
        // Act
        var result = builder.InitialOption("value2");
        
        // Assert
        Assert.NotNull(selectMenu.InitialOption);
        Assert.Equal("value2", selectMenu.InitialOption.Value);
        Assert.Equal("Option 2", selectMenu.InitialOption.Text.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InitialOption_WithNonExistentValue_SetsInitialOptionToNull()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        
        // Add some options
        builder.AddOption("value1", "Option 1");
        builder.AddOption("value2", "Option 2");
        
        // Set an initial option first
        builder.InitialOption("value1");
        Assert.NotNull(selectMenu.InitialOption);
        
        // Act
        var result = builder.InitialOption("value3");
        
        // Assert
        Assert.Null(selectMenu.InitialOption);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Placeholder_SetsPlaceholderText()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        
        // Act
        var result = builder.Placeholder("Select an option");
        
        // Assert
        Assert.Equal("Select an option", selectMenu.Placeholder.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void FocusOnLoad_SetsFocusOnLoadProperty()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        
        // Act
        var result = builder.FocusOnLoad();
        
        // Assert
        Assert.True(selectMenu.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void ChainedMethods_BuildCorrectStaticSelectMenu()
    {
        // Arrange
        var selectMenu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(selectMenu);
        
        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .Placeholder("Select an option")
            .FocusOnLoad()
            .InitialOption("value2");
        
        // Assert
        Assert.Equal(2, selectMenu.Options.Count);
        Assert.Equal("Select an option", selectMenu.Placeholder.Text);
        Assert.True(selectMenu.FocusOnLoad);
        Assert.NotNull(selectMenu.InitialOption);
        Assert.Equal("value2", selectMenu.InitialOption.Value);
    }
    
    [Fact]
    public void AddStaticSelectMenu_InActionsBlock_AddsSelectMenuToBlock()
    {
        // Arrange
        var actionsBuilder = ActionsBlockBuilder.Create();
        
        // Act
        actionsBuilder.AddStaticSelectMenu("select_1", select => 
            select.AddOption("value1", "Option 1")
                .AddOption("value2", "Option 2")
                .Placeholder("Select an option")
                .InitialOption("value1"));
        
        var block = actionsBuilder.Build();
        
        // Assert
        Assert.Single(block.Elements);
        var selectMenu = Assert.IsType<StaticSelectMenu>(block.Elements[0]);
        Assert.Equal("select_1", selectMenu.ActionId);
        Assert.Equal(2, selectMenu.Options.Count);
        Assert.Equal("Select an option", selectMenu.Placeholder.Text);
        Assert.Equal("value1", selectMenu.InitialOption.Value);
    }
} 