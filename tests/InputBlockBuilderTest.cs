using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(InputBlockBuilder<>))]
public class InputBlockBuilderTest
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        // Arrange
        var datePicker = new DatePicker();
        var label = "Select a date";
        
        // Act
        var builder = new InputBlockBuilder<DatePicker>(datePicker, label);
        
        // Assert
        Assert.NotNull(builder.ParentBlock);
        Assert.Equal(label, builder.ParentBlock.Label.Text);
        Assert.Same(datePicker, builder.ParentBlock.Element);
        Assert.Same(datePicker, builder.Element);
    }
    
    [Fact]
    public void Constructor_WithNullElement_ThrowsArgumentNullException()
    {
        // Arrange
        DatePicker datePicker = null;
        var label = "Select a date";
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new InputBlockBuilder<DatePicker>(datePicker, label));
    }
    
    [Fact]
    public void BlockId_SetsBlockId()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.BlockId("block_123");
        
        // Assert
        Assert.Equal("block_123", builder.ParentBlock.BlockId);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void BlockId_WithNullBlockId_SetsNullValue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.BlockId(null);
        
        // Assert
        Assert.Null(builder.ParentBlock.BlockId);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DispatchAction_WithTrue_SetsToTrue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.DispatchAction(true);
        
        // Assert
        Assert.True(builder.ParentBlock.DispatchAction);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DispatchAction_WithDefaultParameter_SetsToTrue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.DispatchAction();
        
        // Assert
        Assert.True(builder.ParentBlock.DispatchAction);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DispatchAction_WithFalse_SetsToFalse()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.DispatchAction(false);
        
        // Assert
        Assert.False(builder.ParentBlock.DispatchAction);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Hint_SetsHintText()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.Hint("This is a hint");
        
        // Assert
        Assert.Equal("This is a hint", builder.ParentBlock.Hint.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Hint_WithNullHint_SetsNullValue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.Hint(null);
        
        // Assert
        Assert.Null(builder.ParentBlock.Hint.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Hint_WithEmptyHint_SetsEmptyValue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.Hint("");
        
        // Assert
        Assert.Equal("", builder.ParentBlock.Hint.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Optional_WithTrue_SetsToTrue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.Optional(true);
        
        // Assert
        Assert.True(builder.ParentBlock.Optional);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Optional_WithDefaultParameter_SetsToTrue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.Optional();
        
        // Assert
        Assert.True(builder.ParentBlock.Optional);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Optional_WithFalse_SetsToFalse()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.Optional(false);
        
        // Assert
        Assert.False(builder.ParentBlock.Optional);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void ChainedMethods_BuildCorrectInputBlock()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Select a date");
        var testDate = new DateTime(2023, 12, 25);
        
        // Act
        builder
            .BlockId("date_block")
            .DispatchAction(true)
            .Hint("Choose your preferred date")
            .Optional(true)
            .InitialDate(testDate)
            .Placeholder("Click to select")
            .FocusOnLoad(true);
        
        // Assert
        // InputBlock properties
        Assert.Equal("date_block", builder.ParentBlock.BlockId);
        Assert.True(builder.ParentBlock.DispatchAction);
        Assert.Equal("Choose your preferred date", builder.ParentBlock.Hint.Text);
        Assert.True(builder.ParentBlock.Optional);
        Assert.Equal("Select a date", builder.ParentBlock.Label.Text);
        Assert.Same(datePicker, builder.ParentBlock.Element);
        
        // Element properties (inherited functionality)
        Assert.Equal(testDate, datePicker.InitialDate);
        Assert.Equal("Click to select", datePicker.Placeholder.Text);
        Assert.True(datePicker.FocusOnLoad);
    }
    
    [Fact]
    public void InheritsFromInputElementBuilder_CanUseElementMethods()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.ActionId("date_action");
        
        // Assert
        Assert.Equal("date_action", datePicker.ActionId);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InheritsFromInputElementBuilder_CanUseSetMethod()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        
        // Act
        var result = builder.Modify(x => x.ActionId = "custom_action");
        
        // Assert
        Assert.Equal("custom_action", datePicker.ActionId);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void WithTimePicker_AllMethodsWork()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputBlockBuilder<TimePicker>(timePicker, "Select time");
        var testTime = new TimeSpan(14, 30, 0);
        
        // Act
        builder
            .BlockId("time_block")
            .Hint("Select your preferred time")
            .InitialTime(testTime)
            .Placeholder("Click to select time");
        
        // Assert
        Assert.Equal("time_block", builder.ParentBlock.BlockId);
        Assert.Equal("Select your preferred time", builder.ParentBlock.Hint.Text);
        Assert.Equal(testTime, timePicker.InitialTime);
        Assert.Equal("Click to select time", timePicker.Placeholder.Text);
    }
    
    [Fact]
    public void WithDateTimePicker_AllMethodsWork()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputBlockBuilder<DateTimePicker>(dateTimePicker, "Select date and time");
        var testDateTime = new DateTime(2023, 12, 25, 14, 30, 0);
        
        // Act
        builder
            .BlockId("datetime_block")
            .Optional(false)
            .InitialDateTime(testDateTime)
            .FocusOnLoad(true);
        
        // Assert
        Assert.Equal("datetime_block", builder.ParentBlock.BlockId);
        Assert.False(builder.ParentBlock.Optional);
        Assert.Equal(testDateTime, dateTimePicker.InitialDateTime);
        Assert.True(dateTimePicker.FocusOnLoad);
    }
    
    [Fact]
    public void Hint_WithMaxLength_SetsCorrectly()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        var longHint = new string('H', 2000); // Maximum length 2000 characters
        
        // Act
        var result = builder.Hint(longHint);
        
        // Assert
        Assert.Equal(longHint, builder.ParentBlock.Hint.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Label_WithMaxLength_SetsCorrectly()
    {
        // Arrange
        var datePicker = new DatePicker();
        var longLabel = new string('L', 2000); // Maximum length 2000 characters
        
        // Act
        var builder = new InputBlockBuilder<DatePicker>(datePicker, longLabel);
        
        // Assert
        Assert.Equal(longLabel, builder.ParentBlock.Label.Text);
    }
    
    [Fact]
    public void BlockId_WithMaxLength_SetsCorrectly()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputBlockBuilder<DatePicker>(datePicker, "Label");
        var longBlockId = new string('B', 255); // Maximum length 255 characters
        
        // Act
        var result = builder.BlockId(longBlockId);
        
        // Assert
        Assert.Equal(longBlockId, builder.ParentBlock.BlockId);
        Assert.Same(builder, result);
    }
}
