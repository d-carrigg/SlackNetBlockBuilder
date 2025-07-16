using System.Diagnostics;
using JetBrains.Annotations;
using SlackNet.Blocks;
using Xunit.Abstractions;

namespace UnitTests;

[TestSubject(typeof(SectionBuilder))]
public class SectionBuilderTest
{
    private readonly ITestOutputHelper _output;

    public SectionBuilderTest(ITestOutputHelper output)
    {
        _output = output;
    }

    // === Constructor Tests ===
    
    [Fact]
    public void Constructor_CreatesEmptySectionBuilder()
    {
        // Act
        var builder = new SectionBuilder();
        
        // Assert
        Assert.NotNull(builder);
    }
    
    // === Text Tests ===
    
    [Fact]
    public void Text_SetsPlainText()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Text("Hello world");
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Equal("Hello world", section.Text.Text);
        Assert.IsType<PlainText>(section.Text);
    }
    
    [Fact]
    public void Text_WithNullText_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Text(null));
    }
    
    [Fact]
    public void Text_WithEmptyText_SetsEmptyText()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Text("");
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Equal("", section.Text.Text);
    }
    
    [Fact]
    public void Text_WithLongText_SetsLongText()
    {
        // Arrange
        var builder = new SectionBuilder();
        var longText = new string('A', 3000); // Maximum length for section text
        
        // Act
        var result = builder.Text(longText);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Equal(longText, section.Text.Text);
    }
    
    // === Markdown Tests ===
    
    [Fact]
    public void Markdown_SetsMarkdownText()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Markdown("*Hello* world");
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Equal("*Hello* world", section.Text.Text);
        Assert.IsType<Markdown>(section.Text);
        var markdown = Assert.IsType<Markdown>(section.Text);
        Assert.False(markdown.Verbatim);
        Assert.False(section.Expand);
    }
    
    [Fact]
    public void Markdown_WithVerbatimTrue_SetsVerbatimTrue()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Markdown("*Hello* world", verbatim: true);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        var markdown = Assert.IsType<Markdown>(section.Text);
        Assert.True(markdown.Verbatim);
        Assert.False(section.Expand);
    }
    
    [Fact]
    public void Markdown_WithExpandTrue_SetsExpandTrue()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Markdown("*Hello* world", expand: true);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        var markdown = Assert.IsType<Markdown>(section.Text);
        Assert.False(markdown.Verbatim);
        Assert.True(section.Expand);
    }
    
    [Fact]
    public void Markdown_WithVerbatimAndExpandTrue_SetsBothTrue()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Markdown("*Hello* world", verbatim: true, expand: true);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        var markdown = Assert.IsType<Markdown>(section.Text);
        Assert.True(markdown.Verbatim);
        Assert.True(section.Expand);
    }
    
    [Fact]
    public void Markdown_WithNullText_AddsNullMarkdown()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Markdown(null);
        
        // Assert
        Assert.Null(builder.Build().Text.Text);
    }
    
    // === Fields Tests ===
    
    [Fact]
    public void Fields_WithTextObjects_SetsFields()
    {
        // Arrange
        var builder = new SectionBuilder();
        var field1 = new PlainText { Text = "Field 1" };
        var field2 = new Markdown { Text = "*Field 2*" };
        
        // Act
        var result = builder.Fields(field1, field2);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Equal(2, section.Fields.Count);
        Assert.Same(field1, section.Fields[0]);
        Assert.Same(field2, section.Fields[1]);
    }
    
    [Fact]
    public void Fields_WithEmptyArray_SetsEmptyFields()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Fields();
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Empty(section.Fields);
    }
    
    [Fact]
    public void Fields_WithNullArray_SetsNullFields()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Fields(null);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Null(section.Fields);
    }
    
    // === AddTextField Tests ===
    
    [Fact]
    public void AddTextField_AddsPlainTextField()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.AddTextField("Field 1");
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Single(section.Fields);
        
        var field = Assert.IsType<PlainText>(section.Fields[0]);
        Assert.Equal("Field 1", field.Text);
    }
    
    [Fact]
    public void AddTextField_WithMultipleFields_AddsAllFields()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder
            .AddTextField("Field 1")
            .AddTextField("Field 2")
            .AddTextField("Field 3");
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Equal(3, section.Fields.Count);
        
        for (int i = 0; i < 3; i++)
        {
            var field = Assert.IsType<PlainText>(section.Fields[i]);
            Assert.Equal($"Field {i + 1}", field.Text);
        }
    }
    
    [Fact]
    public void AddTextField_WithNullText_AddsNullField()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.AddTextField(null);
        // Assert
        Assert.Single(result.Build().Fields);
    }
    
    // === AddMarkdownField Tests ===
    
    [Fact]
    public void AddMarkdownField_AddsMarkdownField()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.AddMarkdownField("*Bold field*");
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Single(section.Fields);
        
        var field = Assert.IsType<Markdown>(section.Fields[0]);
        Assert.Equal("*Bold field*", field.Text);
        Assert.False(field.Verbatim);
    }
    
    [Fact]
    public void AddMarkdownField_WithVerbatimTrue_SetsVerbatimTrue()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.AddMarkdownField("*Bold field*", verbatim: true);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        var field = Assert.IsType<Markdown>(section.Fields[0]);
        Assert.True(field.Verbatim);
    }
    
    [Fact]
    public void AddMarkdownField_WithMultipleFields_AddsAllFields()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder
            .AddMarkdownField("*Field 1*")
            .AddMarkdownField("_Field 2_", verbatim: true)
            .AddMarkdownField("~Field 3~");
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Equal(3, section.Fields.Count);
        
        var field1 = Assert.IsType<Markdown>(section.Fields[0]);
        Assert.Equal("*Field 1*", field1.Text);
        Assert.False(field1.Verbatim);
        
        var field2 = Assert.IsType<Markdown>(section.Fields[1]);
        Assert.Equal("_Field 2_", field2.Text);
        Assert.True(field2.Verbatim);
        
        var field3 = Assert.IsType<Markdown>(section.Fields[2]);
        Assert.Equal("~Field 3~", field3.Text);
        Assert.False(field3.Verbatim);
    }
    
    [Fact]
    public void AddMarkdownField_WithNullText_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.AddMarkdownField(null));
    }
    
    // === Accessory Tests ===
    
    [Fact]
    public void Accessory_WithCreateAction_SetsAccessory()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Accessory<Button>(button => 
        {
            button.ActionId = "test_button";
            button.Text = new PlainText { Text = "Click me" };
        });
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.NotNull(section.Accessory);
        
        var button = Assert.IsType<Button>(section.Accessory);
        Assert.Equal("test_button", button.ActionId);
        Assert.Equal("Click me", button.Text.Text);
    }
    
    [Fact]
    public void Accessory_WithNullCreateAction_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Accessory<Button>(null));
    }
    
    [Fact]
    public void Accessory_WithPreConfiguredElement_SetsAccessory()
    {
        // Arrange
        var builder = new SectionBuilder();
        var button = new Button
        {
            ActionId = "preconfigured_button",
            Text = new PlainText { Text = "Pre-configured" }
        };
        
        // Act
        var result = builder.Accessory(button);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Same(button, section.Accessory);
    }
    
    [Fact]
    public void Accessory_WithNullElement_SetsNullAccessory()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Accessory((BlockElement)null);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.Null(section.Accessory);
    }
    
    // === Expand Tests ===
    
    [Fact]
    public void Expand_WithTrue_SetsExpandTrue()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Expand(true);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.True(section.Expand);
    }
    
    [Fact]
    public void Expand_WithDefaultParameter_SetsExpandTrue()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Expand();
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.True(section.Expand);
    }
    
    [Fact]
    public void Expand_WithFalse_SetsExpandFalse()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        var result = builder.Expand(false);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        Assert.False(section.Expand);
    }
    
    // === Build Tests ===
    
    [Fact]
    public void Build_WithValidConfiguration_ReturnsConfiguredSection()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        builder.Text("Section text");
        var section = builder.Build();
        
        // Assert
        Assert.IsType<SectionBlock>(section);
        Assert.Equal("Section text", section.Text.Text);
    }
    
    [Fact]
    public void Build_WithTooManyFields_ThrowsArgumentException()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act - Add more than MaxFields
        for (int i = 0; i < SectionBuilder.MaxFields + 1; i++)
        {
            builder.AddTextField($"Field {i}");
        }
        
        // Assert
        var exception = Assert.Throws<ArgumentException>(() => builder.Build());
        Assert.Equal($"Section block can have at most {SectionBuilder.MaxFields} fields", exception.Message);
    }
    
    [Fact]
    public void Build_WithMaxFields_BuildsSuccessfully()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act - Add exactly MaxFields
        for (int i = 0; i < SectionBuilder.MaxFields; i++)
        {
            builder.AddTextField($"Field {i}");
        }
        
        var section = builder.Build();
        
        // Assert
        Assert.Equal(SectionBuilder.MaxFields, section.Fields.Count);
    }
    
    [Fact]
    public void Build_WithFieldTextTooLong_ThrowsArgumentException()
    {
        // Arrange
        var builder = new SectionBuilder();
        var longText = new string('A', SectionBuilder.MaxFieldsLength + 1);
        
        // Act
        builder.AddTextField(longText);
        
        // Assert
        var exception = Assert.Throws<ArgumentException>(() => builder.Build());
        Assert.Equal($"Each field's text in a section block can have at most {SectionBuilder.MaxFieldsLength} characters", exception.Message);
    }
    
    [Fact]
    public void Build_WithFieldTextAtMaxLength_BuildsSuccessfully()
    {
        // Arrange
        var builder = new SectionBuilder();
        var maxLengthText = new string('A', SectionBuilder.MaxFieldsLength);
        
        // Act
        builder.AddTextField(maxLengthText);
        
        // Assert
        var section = builder.Build();
        Assert.Single(section.Fields);
        Assert.Equal(maxLengthText, section.Fields[0].Text);
    }
    
    [Fact]
    public void Build_WithMarkdownFieldTextTooLong_ThrowsArgumentException()
    {
        // Arrange
        var builder = new SectionBuilder();
        var longText = new string('A', SectionBuilder.MaxFieldsLength + 1);
        
        // Act
        builder.AddMarkdownField(longText);
        
        // Assert
        var exception = Assert.Throws<ArgumentException>(() => builder.Build());
        Assert.Equal($"Each field's text in a section block can have at most {SectionBuilder.MaxFieldsLength} characters", exception.Message);
    }
    
    [Fact]
    public void Build_CalledMultipleTimes_ReturnsSameInstance()
    {
        // Arrange
        var builder = new SectionBuilder();
        builder.Text("Test");
        
        // Act
        var section1 = builder.Build();
        var section2 = builder.Build();
        
        // Assert
        Assert.Same(section1, section2);
    }
    
    // === Constants Tests ===
    
    [Fact]
    public void Constants_HaveCorrectValues()
    {
        // Assert
        Assert.Equal(10, SectionBuilder.MaxFields);
        Assert.Equal(2000, SectionBuilder.MaxFieldsLength);
        Assert.Equal(255, SectionBuilder.MaxBlockIdLength);
    }
    
    // === Chaining Tests ===
    
    [Fact]
    public void ChainedMethodCalls_BuildComplexSection()
    {
        // Arrange
        var builder = new SectionBuilder();
        var button = new Button
        {
            ActionId = "section_button",
            Text = new PlainText { Text = "Action" }
        };
        
        // Act
        var result = builder
            .Markdown("*Welcome* to our service", verbatim: false, expand: true)
            .AddTextField("Field 1")
            .AddTextField("Field 2")
            .AddMarkdownField("*Bold field*", verbatim: true)
            .Accessory(button)
            .Expand(true);
        
        // Assert
        Assert.Same(builder, result);
        var section = builder.Build();
        
        // Verify text
        var markdown = Assert.IsType<Markdown>(section.Text);
        Assert.Equal("*Welcome* to our service", markdown.Text);
        Assert.False(markdown.Verbatim);
        Assert.True(section.Expand);
        
        // Verify fields
        Assert.Equal(3, section.Fields.Count);
        
        var field1 = Assert.IsType<PlainText>(section.Fields[0]);
        Assert.Equal("Field 1", field1.Text);
        
        var field2 = Assert.IsType<PlainText>(section.Fields[1]);
        Assert.Equal("Field 2", field2.Text);
        
        var field3 = Assert.IsType<Markdown>(section.Fields[2]);
        Assert.Equal("*Bold field*", field3.Text);
        Assert.True(field3.Verbatim);
        
        // Verify accessory
        Assert.Same(button, section.Accessory);
    }
    
    // === Mixed Field Types Tests ===
    
    [Fact]
    public void MixedFieldTypes_AddTextAndMarkdownFields_WorksCorrectly()
    {
        // Arrange
        var builder = new SectionBuilder();
        
        // Act
        builder
            .AddTextField("Plain text field")
            .AddMarkdownField("*Markdown field*")
            .AddTextField("Another plain field")
            .AddMarkdownField("_Another markdown field_", verbatim: true);
        
        var section = builder.Build();
        
        // Assert
        Assert.Equal(4, section.Fields.Count);
        
        var plainField1 = Assert.IsType<PlainText>(section.Fields[0]);
        Assert.Equal("Plain text field", plainField1.Text);
        
        var markdownField1 = Assert.IsType<Markdown>(section.Fields[1]);
        Assert.Equal("*Markdown field*", markdownField1.Text);
        Assert.False(markdownField1.Verbatim);
        
        var plainField2 = Assert.IsType<PlainText>(section.Fields[2]);
        Assert.Equal("Another plain field", plainField2.Text);
        
        var markdownField2 = Assert.IsType<Markdown>(section.Fields[3]);
        Assert.Equal("_Another markdown field_", markdownField2.Text);
        Assert.True(markdownField2.Verbatim);
    }
    
    // === Performance Tests ===
    
    [Fact]
    public void Performance_BuildingManySections()
    {
        // Arrange
        const int iterations = 1000;
        var stopwatch = new Stopwatch();
        
        // Act
        stopwatch.Start();
        
        for (var i = 0; i < iterations; i++)
        {
            var builder = new SectionBuilder();
            var iLocal = i; // Capture the loop variable
            
            // Build a section with various elements
            builder
                .Markdown($"*Section {iLocal}*")
                .AddTextField($"Field 1 - {iLocal}")
                .AddTextField($"Field 2 - {iLocal}")
                .AddMarkdownField($"*Bold field {iLocal}*")
                .Expand(i % 2 == 0)
                .Accessory<Button>(button => 
                {
                    button.ActionId = $"button_{iLocal}";
                    button.Text = new PlainText { Text = $"Action {iLocal}" };
                });
            
            var section = builder.Build();
            Assert.Equal(3, section.Fields.Count);
            Assert.NotNull(section.Accessory);
        }
        
        stopwatch.Stop();
        _output.WriteLine($"Performance test completed in {stopwatch.ElapsedMilliseconds}ms for {iterations} iterations");
        
        // Assert - Should complete in reasonable time (less than 5 seconds)
        Assert.True(stopwatch.ElapsedMilliseconds < 5000);
    }
    
    // === Integration Tests ===
    
    [Fact]
    public void ComplexSectionBlock_AllFeatures_BuildsCorrectly()
    {
        // Arrange
        var builder = new SectionBuilder();
        var selectMenu = new StaticSelectMenu
        {
            ActionId = "section_select",
            Placeholder = new PlainText { Text = "Choose option" }
        };
        
        // Act
        builder
            .Markdown("*Project Status Dashboard*", verbatim: false, expand: true)
            .AddTextField("Total Tasks: 42")
            .AddTextField("Completed: 38")
            .AddMarkdownField("*Remaining: 4*", verbatim: false)
            .AddMarkdownField("_Due: Today_", verbatim: true)
            .AddTextField("Progress: 90%")
            .Accessory(selectMenu)
            .Expand(true);
        
        var section = builder.Build();
        
        // Assert
        Assert.IsType<SectionBlock>(section);
        
        // Verify main text
        var markdown = Assert.IsType<Markdown>(section.Text);
        Assert.Equal("*Project Status Dashboard*", markdown.Text);
        Assert.False(markdown.Verbatim);
        Assert.True(section.Expand);
        
        // Verify fields
        Assert.Equal(5, section.Fields.Count);
        
        var totalField = Assert.IsType<PlainText>(section.Fields[0]);
        Assert.Equal("Total Tasks: 42", totalField.Text);
        
        var completedField = Assert.IsType<PlainText>(section.Fields[1]);
        Assert.Equal("Completed: 38", completedField.Text);
        
        var remainingField = Assert.IsType<Markdown>(section.Fields[2]);
        Assert.Equal("*Remaining: 4*", remainingField.Text);
        Assert.False(remainingField.Verbatim);
        
        var dueField = Assert.IsType<Markdown>(section.Fields[3]);
        Assert.Equal("_Due: Today_", dueField.Text);
        Assert.True(dueField.Verbatim);
        
        var progressField = Assert.IsType<PlainText>(section.Fields[4]);
        Assert.Equal("Progress: 90%", progressField.Text);
        
        // Verify accessory
        Assert.Same(selectMenu, section.Accessory);
    }
}

