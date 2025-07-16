using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(DateTimePickerExtensions))]
public class DateTimePickerExtensionsTest
{
    // === DatePicker Tests ===
    
    [Fact]
    public void DatePicker_InitialDate_SetsInitialDate()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        var testDate = new DateTime(2023, 12, 25);
        
        // Act
        var result = builder.InitialDate(testDate);
        
        // Assert
        Assert.Equal(testDate, datePicker.InitialDate);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DatePicker_InitialDate_WithNull_SetsNullValue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        
        // Act
        var result = builder.InitialDate(null);
        
        // Assert
        Assert.Null(datePicker.InitialDate);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DatePicker_InitialDate_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<DatePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.InitialDate(DateTime.Now));
    }
    
    [Fact]
    public void DatePicker_Placeholder_SetsPlaceholderText()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        
        // Act
        var result = builder.Placeholder("Select a date");
        
        // Assert
        Assert.Equal("Select a date", datePicker.Placeholder.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DatePicker_Placeholder_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<DatePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Placeholder("test"));
    }
    
    [Fact]
    public void DatePicker_Placeholder_WithNullPlaceholder_ThrowsArgumentNullException()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Placeholder(null));
    }
    
    [Fact]
    public void DatePicker_Placeholder_WithEmptyPlaceholder_ThrowsArgumentNullException()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Placeholder(""));
    }
    
    [Fact]
    public void DatePicker_FocusOnLoad_SetsToTrue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        
        // Act
        var result = builder.FocusOnLoad(true);
        
        // Assert
        Assert.True(datePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DatePicker_FocusOnLoad_WithDefaultParameter_SetsToTrue()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        
        // Act
        var result = builder.FocusOnLoad();
        
        // Assert
        Assert.True(datePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DatePicker_FocusOnLoad_SetsToFalse()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        
        // Act
        var result = builder.FocusOnLoad(false);
        
        // Assert
        Assert.False(datePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DatePicker_FocusOnLoad_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<DatePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.FocusOnLoad());
    }
    
    [Fact]
    public void DatePicker_ChainedMethods_BuildCorrectDatePicker()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        var testDate = new DateTime(2023, 12, 25);
        
        // Act
        builder
            .InitialDate(testDate)
            .Placeholder("Select a date")
            .FocusOnLoad(true);
        
        // Assert
        Assert.Equal(testDate, datePicker.InitialDate);
        Assert.Equal("Select a date", datePicker.Placeholder.Text);
        Assert.True(datePicker.FocusOnLoad);
    }
    
    // === TimePicker Tests ===
    
    [Fact]
    public void TimePicker_InitialTime_SetsInitialTime()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        var testTime = new TimeSpan(14, 30, 0);
        
        // Act
        var result = builder.InitialTime(testTime);
        
        // Assert
        Assert.Equal(testTime, timePicker.InitialTime);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_InitialTime_WithNull_SetsNullValue()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        
        // Act
        var result = builder.InitialTime(null);
        
        // Assert
        Assert.Null(timePicker.InitialTime);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_InitialTime_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<TimePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.InitialTime(TimeSpan.Zero));
    }
    
    [Fact]
    public void TimePicker_Placeholder_SetsPlaceholderText()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        
        // Act
        var result = builder.Placeholder("Select a time");
        
        // Assert
        Assert.Equal("Select a time", timePicker.Placeholder.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_Placeholder_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<TimePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Placeholder("test"));
    }
    
    [Fact]
    public void TimePicker_Placeholder_WithNullPlaceholder_SetsNullValue()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        
        // Act
        var result = builder.Placeholder(null);
        
        // Assert
        Assert.Null(timePicker.Placeholder.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_Placeholder_WithEmptyPlaceholder_SetsEmptyValue()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        
        // Act
        var result = builder.Placeholder("");
        
        // Assert
        Assert.Equal("", timePicker.Placeholder.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_FocusOnLoad_SetsToTrue()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        
        // Act
        var result = builder.FocusOnLoad(true);
        
        // Assert
        Assert.True(timePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_FocusOnLoad_WithDefaultParameter_SetsToTrue()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        
        // Act
        var result = builder.FocusOnLoad();
        
        // Assert
        Assert.True(timePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_FocusOnLoad_SetsToFalse()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        
        // Act
        var result = builder.FocusOnLoad(false);
        
        // Assert
        Assert.False(timePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_FocusOnLoad_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<TimePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.FocusOnLoad());
    }
    
    [Fact]
    public void TimePicker_ChainedMethods_BuildCorrectTimePicker()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        var testTime = new TimeSpan(14, 30, 0);
        
        // Act
        builder
            .InitialTime(testTime)
            .Placeholder("Select a time")
            .FocusOnLoad(true);
        
        // Assert
        Assert.Equal(testTime, timePicker.InitialTime);
        Assert.Equal("Select a time", timePicker.Placeholder.Text);
        Assert.True(timePicker.FocusOnLoad);
    }
    
    // === DateTimePicker Tests ===
    
    [Fact]
    public void DateTimePicker_InitialDateTime_SetsInitialDateTime()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputElementBuilder<DateTimePicker>(dateTimePicker);
        var testDateTime = new DateTime(2023, 12, 25, 14, 30, 0);
        
        // Act
        var result = builder.InitialDateTime(testDateTime);
        
        // Assert
        Assert.Equal(testDateTime, dateTimePicker.InitialDateTime);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DateTimePicker_InitialDateTime_WithNull_SetsNullValue()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputElementBuilder<DateTimePicker>(dateTimePicker);
        
        // Act
        var result = builder.InitialDateTime(null);
        
        // Assert
        Assert.Null(dateTimePicker.InitialDateTime);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DateTimePicker_InitialDateTime_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<DateTimePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.InitialDateTime(DateTime.Now));
    }
    
    [Fact]
    public void DateTimePicker_FocusOnLoad_SetsToTrue()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputElementBuilder<DateTimePicker>(dateTimePicker);
        
        // Act
        var result = builder.FocusOnLoad(true);
        
        // Assert
        Assert.True(dateTimePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DateTimePicker_FocusOnLoad_WithDefaultParameter_SetsToTrue()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputElementBuilder<DateTimePicker>(dateTimePicker);
        
        // Act
        var result = builder.FocusOnLoad();
        
        // Assert
        Assert.True(dateTimePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DateTimePicker_FocusOnLoad_SetsToFalse()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputElementBuilder<DateTimePicker>(dateTimePicker);
        
        // Act
        var result = builder.FocusOnLoad(false);
        
        // Assert
        Assert.False(dateTimePicker.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void DateTimePicker_FocusOnLoad_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<DateTimePicker> builder = null;
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.FocusOnLoad());
    }
    
    [Fact]
    public void DateTimePicker_ChainedMethods_BuildCorrectDateTimePicker()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputElementBuilder<DateTimePicker>(dateTimePicker);
        var testDateTime = new DateTime(2023, 12, 25, 14, 30, 0);
        
        // Act
        builder
            .InitialDateTime(testDateTime)
            .FocusOnLoad(true);
        
        // Assert
        Assert.Equal(testDateTime, dateTimePicker.InitialDateTime);
        Assert.True(dateTimePicker.FocusOnLoad);
    }
    
    // === Edge Cases and Additional Tests ===
    
    [Fact]
    public void DatePicker_InitialDate_WithDifferentDateFormats_SetsCorrectly()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        var testDate = new DateTime(2023, 1, 1); // January 1st, 2023
        
        // Act
        builder.InitialDate(testDate);
        
        // Assert
        Assert.Equal(testDate, datePicker.InitialDate);
    }
    
    [Fact]
    public void TimePicker_InitialTime_WithDifferentTimeFormats_SetsCorrectly()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        var testTime = new TimeSpan(23, 59, 59); // 23:59:59
        
        // Act
        builder.InitialTime(testTime);
        
        // Assert
        Assert.Equal(testTime, timePicker.InitialTime);
    }
    
    [Fact]
    public void DateTimePicker_InitialDateTime_WithDifferentDateTimeFormats_SetsCorrectly()
    {
        // Arrange
        var dateTimePicker = new DateTimePicker();
        var builder = new InputElementBuilder<DateTimePicker>(dateTimePicker);
        var testDateTime = new DateTime(2023, 12, 31, 23, 59, 59); // December 31st, 2023 at 23:59:59
        
        // Act
        builder.InitialDateTime(testDateTime);
        
        // Assert
        Assert.Equal(testDateTime, dateTimePicker.InitialDateTime);
    }
    
    [Fact]
    public void DatePicker_Placeholder_WithMaxLength_SetsCorrectly()
    {
        // Arrange
        var datePicker = new DatePicker();
        var builder = new InputElementBuilder<DatePicker>(datePicker);
        var longPlaceholder = new string('A', 150); // Maximum length 150 characters
        
        // Act
        var result = builder.Placeholder(longPlaceholder);
        
        // Assert
        Assert.Equal(longPlaceholder, datePicker.Placeholder.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void TimePicker_Placeholder_WithMaxLength_SetsCorrectly()
    {
        // Arrange
        var timePicker = new TimePicker();
        var builder = new InputElementBuilder<TimePicker>(timePicker);
        var longPlaceholder = new string('B', 150); // Maximum length 150 characters
        
        // Act
        var result = builder.Placeholder(longPlaceholder);
        
        // Assert
        Assert.Equal(longPlaceholder, timePicker.Placeholder.Text);
        Assert.Same(builder, result);
    }
}