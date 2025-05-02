namespace SlackNet.Blocks;

/// <summary>
/// Interface for building blocks for Slack messages
/// </summary>
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
    
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockBuilder"/> class
    /// </summary>
    public BlockBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockBuilder"/> class with existing blocks
    /// </summary>
    /// <param name="blocks">The initial blocks to include</param>
    private BlockBuilder(IEnumerable<Block> blocks)
    {
        _blocks = blocks.ToList();
    }

    /// <summary>
    /// Creates a new BlockBuilder from an existing collection of blocks
    /// </summary>
    /// <param name="blocks">The blocks to include</param>
    /// <returns>A new BlockBuilder instance</returns>
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
    /// <exception cref="InvalidOperationException">Thrown when more than one block has FocusOnLoad set to true</exception>
    public List<Block> Build()  
    {
        // if more than 1 block has FocusOnLoad, throw an exception
        if(GetFocusedElementsCount() > 1)
        {
            throw new InvalidOperationException("Only one block can have FocusOnLoad set to true");
        }
        
        return _blocks;
    }

    /// <summary>
    /// Gets the total count of elements with focus across all blocks
    /// </summary>
    /// <returns>The count of focused elements</returns>
    private int GetFocusedElementsCount() => _blocks.Sum(GetFocusedElementsCount);
    
    /// <summary>
    /// Gets the count of focused elements within a single block
    /// </summary>
    /// <param name="block">The block to check</param>
    /// <returns>The count of focused elements in the block</returns>
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
    
    /// <summary>
    /// Determines if an input block element has focus
    /// </summary>
    /// <param name="block">The element to check</param>
    /// <returns>True if the element has focus, false otherwise</returns>
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

    /// <summary>
    /// Removes blocks that match the specified predicate
    /// </summary>
    /// <param name="predicate">The predicate to match blocks against</param>
    /// <returns>The number of blocks removed</returns>
    public int Remove(Predicate<Block> predicate)
    {
        return _blocks.RemoveAll(predicate);
    }

    /// <summary>
    /// Removes a block with the specified block ID
    /// </summary>
    /// <param name="blockId">The block ID to remove</param>
    /// <returns>True if a block was removed, false otherwise</returns>
    public bool Remove(string blockId)
    {
        return Remove(b => b.BlockId == blockId) > 0;
    }
    
    /// <summary>
    /// Removes an action element that matches the specified predicate
    /// </summary>
    /// <param name="predicate">The predicate to match action elements against</param>
    /// <returns>True if an action element was removed, false otherwise</returns>
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
    
    /// <summary>
    /// Removes an action element with the specified action ID
    /// </summary>
    /// <param name="actionId">The action ID to remove</param>
    /// <returns>True if an action element was removed, false otherwise</returns>
    public bool RemoveAction(string actionId) => RemoveAction(a => a.ActionId == actionId);

    /// <inheritdoc />
    public IBlockBuilder AddBlock(Block block)
    {
        _blocks.Add(block);
        return this;
    }
    
    /// <summary>
    /// Adds a new block of the specified type and applies the modifier to it
    /// </summary>
    /// <typeparam name="TBlock">The type of block to add</typeparam>
    /// <param name="modifier">The action to modify the block</param>
    /// <returns>The same instance so calls can be chained</returns>
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