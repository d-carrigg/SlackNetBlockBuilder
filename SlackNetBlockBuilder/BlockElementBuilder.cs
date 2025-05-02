namespace SlackNet.Blocks;

public sealed class BlockElementBuilder<TBlock> : IBlockBuilder
{
    private readonly IBlockBuilder _other;


    private BlockElementBuilder(IBlockBuilder other, TBlock element)
    {
        _other = other;
        Element = element;
    }

    public TBlock Element { get; }

    public void Modify(Action<TBlock> modifier)
    {
        modifier(Element);
    }

    public static BlockElementBuilder<TBlock> Create(IBlockBuilder builder, TBlock element) =>
        new(builder, element);

    public IBlockBuilder AddBlock(Block block)
    {
        return _other.AddBlock(block);
    }

    public IBlockBuilder AddBlocks(IEnumerable<Block> blocks)
    {
        return _other.AddBlocks(blocks);
    }

    public IBlockBuilder Add<TBlock1>(Action<TBlock1> modifier) where TBlock1 : Block, new()
    {
        return _other.Add(modifier);
    }

    public List<Block> Build()
    {
        return _other.Build();
    }
}