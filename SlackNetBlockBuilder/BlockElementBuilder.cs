namespace SlackNet.Blocks;

/// <summary>
/// A builder for block elements that wraps another block builder
/// </summary>
/// <typeparam name="TBlock">The type of block element being built</typeparam>
public sealed class BlockElementBuilder<TBlock> : IBlockBuilder
{
    private readonly IBlockBuilder _other;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockElementBuilder{TBlock}"/> class
    /// </summary>
    /// <param name="other">The underlying block builder</param>
    /// <param name="element">The element being built</param>
    private BlockElementBuilder(IBlockBuilder other, TBlock element)
    {
        _other = other;
        Element = element;
    }

    /// <summary>
    /// Gets the element being built
    /// </summary>
    public TBlock Element { get; }

    /// <summary>
    /// Modifies the element using the provided action
    /// </summary>
    /// <param name="modifier">The action to modify the element</param>
    public void ModifyElement(Action<TBlock> modifier)
    {
        modifier(Element);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="BlockElementBuilder{TBlock}"/> class
    /// </summary>
    /// <param name="builder">The underlying block builder</param>
    /// <param name="element">The element to build</param>
    /// <returns>A new BlockElementBuilder instance</returns>
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