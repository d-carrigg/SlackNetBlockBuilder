namespace SlackNet.Blocks;

/// <summary>
/// A helper builder that wraps an existing <see cref="IBlockBuilder"/> instance
/// while also providing access to modify a specific block element of type <typeparamref name="TBlock"/>.
/// This allows modifying the element while continuing the block building chain.
/// </summary>
/// <typeparam name="TBlock">The type of the block element being managed.</typeparam>
public sealed class BlockElementBuilder<TBlock> : IBlockBuilder
{
    private readonly IBlockBuilder _other;


    private BlockElementBuilder(IBlockBuilder other, TBlock element)
    {
        _other = other;
        Element = element;
    }

    /// <summary>
    /// Gets the block element instance being managed by this builder.
    /// </summary>
    public TBlock Element { get; }

    /// <summary>
    /// Applies modifications to the underlying <see cref="Element"/>.
    /// </summary>
    /// <param name="modifier">An action that modifies the <see cref="Element"/>.</param>
    public void Modify(Action<TBlock> modifier)
    {
        modifier(Element);
    }

    /// <summary>
    /// Creates a new <see cref="BlockElementBuilder{TBlock}"/> instance. (Typically for internal use by other builders).
    /// </summary>
    /// <param name="builder">The outer <see cref="IBlockBuilder"/> to wrap.</param>
    /// <param name="element">The specific element instance to manage.</param>
    /// <returns>A new <see cref="BlockElementBuilder{TBlock}"/>.</returns>
    public static BlockElementBuilder<TBlock> Create(IBlockBuilder builder, TBlock element) =>
        new(builder, element);

    /// <inheritdoc />
    public IBlockBuilder AddBlock(Block block)
    {
        return _other.AddBlock(block);
    }

    /// <inheritdoc />
    public IBlockBuilder AddBlocks(IEnumerable<Block> blocks)
    {
        return _other.AddBlocks(blocks);
    }

    /// <inheritdoc />
    public IBlockBuilder Add<TBlock1>(Action<TBlock1> modifier) where TBlock1 : Block, new()
    {
        return _other.Add(modifier);
    }

    /// <inheritdoc />
    public List<Block> Build()
    {
        return _other.Build();
    }
}