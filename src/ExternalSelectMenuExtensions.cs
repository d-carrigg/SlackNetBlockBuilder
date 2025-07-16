using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides extension methods for configuring <see cref="ExternalMultiSelectMenu"/> elements
/// </summary>
[PublicAPI]
public static class ExternalSelectMenuExtensions
{
    /// <summary>
    /// Specifies the maximum number of items that can be selected in a <see cref="StaticMultiSelectMenu"/>.
    /// Minimum number is 1.
    /// </summary>
    /// <typeparam name="TElement">The type of the static multi-select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="maxItems">The maximum number of items.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> MaxSelectedItems<TElement>(
        this InputElementBuilder<TElement> builder,
        int maxItems) where TElement : ExternalMultiSelectMenu =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Set(x => x.MaxSelectedItems = maxItems);
}