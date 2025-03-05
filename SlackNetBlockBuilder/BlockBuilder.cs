namespace SlackNet.Blocks;

public interface IBlockBuilder
{
    /// <summary>
    /// Add a block to the builder
    /// </summary>
    /// <param name="block">The block to add</param>
    /// <returns>The same instance so calls can be chained</returns>
    IBlockBuilder AddBlock(Block block);


    /// <summary>
    /// Add multiple blocks to the builder
    /// </summary>
    /// <param name="blocks">The blocks to add</param>
    /// <returns>The same instance so calls can be chained</returns>
    IBlockBuilder AddBlocks(IEnumerable<Block> blocks);

    /// <summary>
    /// Add a block to the builder
    /// </summary>
    /// <param name="modifier">An action which is run over the created element</param>
    /// <typeparam name="TBlock">The type of block to add</typeparam>
    /// <returns>The same instance so calls can be chained</returns>
    IBlockBuilder Add<TBlock>(Action<TBlock> modifier) where TBlock : Block, new();

    /// <summary>
    /// Combine all blocks into a list of blocks
    /// </summary>
    /// <returns>The built list of blocks</returns>
    List<Block> Build();
}

/// <summary>
/// Helper class for creating blocks for slack messages
/// </summary>
public sealed class BlockBuilder : IBlockBuilder
{
    private readonly List<Block> _blocks = new();
    public BlockBuilder()
    {
    }

    private BlockBuilder(IEnumerable<Block> blocks)
    {
        _blocks = blocks.ToList();
    }

    public static BlockBuilder From(IEnumerable<Block> blocks) => new(blocks);


    /// <summary>
    /// Creates a new instance of the block builder
    /// </summary>
    /// <returns>The builder instance</returns>
    public static BlockBuilder Create() => new();



    /// <summary>
    /// Build the list of blocks
    /// </summary>
    /// <returns>The list of blocks</returns>
    public List<Block> Build()  
    {
        // if more than 1 block has FocusOnLoad, throw an exception
        if(GetFocusedElementsCount() > 1)
        {
            throw new InvalidOperationException("Only one block can have FocusOnLoad set to true");
        }
        
        return _blocks;
    }

    private int GetFocusedElementsCount() => _blocks.Sum(GetFocusedElementsCount);
    private int GetFocusedElementsCount(Block block)
    {
        return block switch 
        {
            InputBlock inputBlock => IsElementFocused(inputBlock.Element) ? 1 : 0,
            ActionsBlock ab => ab.Elements.OfType<IInputBlockElement>().Count(IsElementFocused),
            ContextBlock cb => cb.Elements.OfType<IInputBlockElement>().Count(IsElementFocused),
            _ => 0
        };
    }
    
    private bool IsElementFocused(IInputBlockElement block)
    {
        // TODO: Don't do this, add a marker interface to the elements that have FocusOnLoad
        var isFocused = block switch 
            {
                // date and time
                DatePicker b => b.FocusOnLoad,
                TimePicker b => b.FocusOnLoad,
                DateTimePicker b => b.FocusOnLoad,
       
                // inputs
                EmailTextInput b => b.FocusOnLoad,
                RichTextInput b => b.FocusOnLoad,
                UrlTextInput b => b.FocusOnLoad,
                PlainTextInput b => b.FocusOnLoad,
                NumberInput b => b.FocusOnLoad,
                TextInput b => b.FocusOnLoad,
                
                // groups
                CheckboxGroup b => b.FocusOnLoad,
                RadioButtonGroup b => b.FocusOnLoad,
                
                // single selects
                StaticSelectMenu b => b.FocusOnLoad,
                ExternalSelectMenu b => b.FocusOnLoad,
                UserSelectMenu b => b.FocusOnLoad,
                ChannelSelectMenu b => b.FocusOnLoad,
                ConversationSelectMenu b => b.FocusOnLoad,
                
                // multi selects
                StaticMultiSelectMenu b => b.FocusOnLoad,
                ExternalMultiSelectMenu b => b.FocusOnLoad,
                UserMultiSelectMenu b => b.FocusOnLoad,
                ChannelMultiSelectMenu b => b.FocusOnLoad,
                ConversationMultiSelectMenu b => b.FocusOnLoad,
                _ => false
            };
        return isFocused;
    }

    
    public int Remove(Predicate<Block> predicate)
    {
        return _blocks.RemoveAll(predicate);
    }

    public bool Remove(string blockId)
    {
        return Remove(b => b.BlockId == blockId) > 0;
    }
    
    public bool RemoveAction(Predicate<IActionElement> predicate)
    {
        var actionsBlocks = _blocks.OfType<ActionsBlock>();
        
 
        foreach (var actionsBlock in actionsBlocks)
        {
            foreach (var subBlock in actionsBlock.Elements)
            {
                if(predicate(subBlock))
                {
                    return actionsBlock.Elements.Remove(subBlock);
                }
            }
        }

        return false;
    }
    public bool RemoveAction(string actionId) => RemoveAction(a => a.ActionId == actionId);

    /// <inheritdoc />
    public IBlockBuilder AddBlock(Block block)
    {
        _blocks.Add(block);
        return this;
    }
    
    public IBlockBuilder Add<TBlock>(Action<TBlock> modifier) where TBlock : Block, new()
    {
        var element = new TBlock();
        modifier(element);
        _blocks.Add(element);
        return this;
    }

    /// <inheritdoc />
    public IBlockBuilder AddBlocks(IEnumerable<Block> blocks)
    {
        _blocks.AddRange(blocks);
        return this;
    }
}