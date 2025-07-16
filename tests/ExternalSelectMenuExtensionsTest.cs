using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(ExternalSelectMenuExtensions))]
public class ExternalSelectMenuExtensionsTest
{
    [Fact]
    public void MaxSelectedItems_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalMultiSelectMenu>(new ExternalMultiSelectMenu());

        // Act
        builder.MaxSelectedItems(3);

        // Assert
        Assert.Equal(3, builder.Element.MaxSelectedItems);
    }

    [Fact]
    public void MaxSelectedItems_WithNullBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        InputElementBuilder<ExternalMultiSelectMenu> builder = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.MaxSelectedItems(5));
    }

    [Fact]
    public void MaxSelectedItems_ReturnsBuilderForChaining()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalMultiSelectMenu>(new ExternalMultiSelectMenu());

        // Act
        var result = builder.MaxSelectedItems(2);

        // Assert
        Assert.Same(builder, result);
    }
}
