using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Defines the contract for a block builder.
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
    /// <summary>
    /// Builds and returns the list of added blocks.
    /// </summary>
    /// <returns>The list of blocks that have been added to the builder.</returns>
    [SuppressMessage("Design", "CA1002:Do not expose generic lists")]
    List<Block> Build();
}

/// <summary>
/// Provides a fluent interface for building blocks for Slack messages
/// </summary>
/// <remarks>
/// Create a new instance of the builder using <see cref="Create"/> or <see cref="From"/>. Then chain calls to add
/// blocks. Finally, call <see cref="Build"/> to get the list of blocks in a slack compatible format.
/// </remarks>
/// <example>
/// Create a block builder from scratch to create a new message:
/// <code>
/// var blocks = BlockBuilder.Create()
///              .AddHeader("My Header")
///              .AddSection("Markdown *text*")
///              .AddActions(actions =>
///                 actions.AddButton("button_1_id", "Button 1",
///                  "https://github.com/d-carrigg/SlackNetBlockBuilder"))
///             .Build();
/// </code>
/// You can also use the builder to update an existing message. This is useful when you want to update a message.
/// For example, when a user clicks a button, you can remove the button and add a new message indicating that the
/// action was successful or not.
/// <code>
/// // from some existing Slack message
/// var existingBlocks = sourceMessage.Blocks;
///
/// // create a new block builder from the existing blocks
/// var updateBlocks = BlockBuilder.From(existingBlocks)
///                 .RemoveAction("button_1_id")
///                 .AddSection("Thanks for checking out the site!")
///                 .Build();
/// </code>
/// </example>
public sealed class BlockBuilder : IBlockBuilder
{
    private readonly List<Block> _blocks = new();
    
    /// <summary>
    /// Create a new empty block builder instance.
    /// <summary>
    /// Initializes a new, empty instance of the <see cref="BlockBuilder"/> class for constructing Slack message blocks.
    /// </summary>
    [PublicAPI]
    public BlockBuilder()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlockBuilder"/> class with a predefined collection of blocks.
    /// </summary>
    /// <param name="blocks">The collection of blocks to initialize the builder with.</param>
    private BlockBuilder(IEnumerable<Block> blocks)
    {
        _blocks = blocks.ToList();
    }

    /// <summary>
    /// Creates a new block builder instance initialized with an existing collection of blocks.
    /// Useful for modifying existing block structures.
    /// </summary>
    /// <param name="blocks">The initial collection of blocks.</param>
    /// <summary>
        /// Creates a new <see cref="BlockBuilder"/> initialized with the specified collection of blocks.
        /// </summary>
        /// <param name="blocks">The collection of blocks to initialize the builder with.</param>
        /// <returns>A new <see cref="BlockBuilder"/> instance containing the provided blocks.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="blocks"/> is null.</exception>
    public static BlockBuilder From(IEnumerable<Block> blocks) => 
        blocks is null ? throw new ArgumentNullException(nameof(blocks)) :
        new(blocks);


    /// <summary>
    /// Creates a new instance of the block builder
    /// </summary>
    /// <returns>The builder instance</returns>
    public static BlockBuilder Create() => new();



    /// <summary>
    /// Build the list of blocks
    /// </summary>
    /// <returns>The list of blocks</returns>
    /// <summary>
    /// Finalizes and returns the list of added blocks, ensuring that at most one element across all blocks has <c>FocusOnLoad</c> set to true.
    /// </summary>
    /// <returns>The list of constructed blocks.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if more than one element across all blocks has <c>FocusOnLoad</c> set to true.
    /// </exception>
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
    /// Calculates the total number of elements across all blocks that have FocusOnLoad set to true. (Internal Helper)
    /// <summary>
/// Counts the total number of input elements with FocusOnLoad enabled across all blocks in the builder.
/// </summary>
    private int GetFocusedElementsCount() => _blocks.Sum(GetFocusedElementsCount);
    /// <summary>
    /// Calculates the number of focused elements within a single block. (Internal Helper)
    /// <summary>
    /// Counts the number of input elements with FocusOnLoad enabled within the specified block.
    /// </summary>
    /// <param name="block">The block to inspect for focused input elements.</param>
    /// <returns>The number of input elements in the block that have FocusOnLoad set to true.</returns>
    private int GetFocusedElementsCount(Block block)
    {
        return block switch 
        {
            InputBlock inputBlock => IsElementFocused(inputBlock.Element) ? 1 : 0,
            ActionsBlock ab => ab.Elements.OfType<IInputBlockElement>().Count(IsElementFocused),
            _ => 0
        };
    }
    
    /// <summary>
    /// Checks if a specific input block element has FocusOnLoad set to true. (Internal Helper)
    /// <summary>
    /// Determines whether the specified input block element has the FocusOnLoad property set to true.
    /// </summary>
    /// <param name="block">The input block element to check.</param>
    /// <returns>True if the element's FocusOnLoad property is true; otherwise, false.</returns>
    private bool IsElementFocused(IInputBlockElement block)
    {
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
    /// Removes all blocks that match the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each block for a condition.</param>
    /// <summary>
    /// Removes all blocks that match the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each block for a condition.</param>
    /// <returns>The number of blocks removed.</returns>
    public int Remove(Predicate<Block> predicate)
    {
        return _blocks.RemoveAll(predicate);
    }

    /// <summary>
    /// Removes the first block that matches the specified block ID.
    /// </summary>
    /// <param name="blockId">The BlockId of the block to remove.</param>
    /// <summary>
    /// Removes the first block with the specified block ID.
    /// </summary>
    /// <param name="blockId">The ID of the block to remove.</param>
    /// <returns>True if a block was removed; otherwise, false.</returns>
    public bool Remove(string blockId)
    {
        return Remove(b => b.BlockId == blockId) > 0;
    }
    
    /// <summary>
    /// Finds the first <see cref="ActionsBlock"/> and removes the first element within it that matches the predicate.
    /// </summary>
    /// <param name="predicate">A function to test each action element for a condition.</param>
    /// <summary>
    /// Removes the first action element from the first <see cref="ActionsBlock"/> that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each action element for a condition.</param>
    /// <returns>True if an element was removed; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate"/> is null.</exception>
    public bool RemoveAction(Predicate<IActionElement> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
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
    /// Finds the first <see cref="ActionsBlock"/> and removes the first element within it that matches the specified action ID.
    /// </summary>
    /// <param name="actionId">The ActionId of the element to remove.</param>
    /// <summary>
/// Removes the first action element with the specified action ID from the first <c>ActionsBlock</c>.
/// </summary>
/// <param name="actionId">The action ID of the element to remove.</param>
/// <returns>True if an element was removed; otherwise, false.</returns>
    public bool RemoveAction(string actionId) => RemoveAction(a => a.ActionId == actionId);

    /// <summary>
    /// Adds a block to the builder.
    /// </summary>
    /// <param name="block">The block to add. Must not be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IBlockBuilder AddBlock(Block block)
    {
        ArgumentNullException.ThrowIfNull(block);
        _blocks.Add(block);
        return this;
    }
    
    /// <summary>
    /// Adds a new block of the specified type, configured via an action.
    /// </summary>
    /// <typeparam name="TBlock">The type of block to add. Must have a parameterless constructor.</typeparam>
    /// <param name="modifier">An action that configures the newly created block instance.</param>
    /// <summary>
    /// Creates a new block of the specified type, applies the provided modifier action, adds it to the builder, and returns the builder instance for chaining.
    /// </summary>
    /// <typeparam name="TBlock">The type of block to create and add.</typeparam>
    /// <param name="modifier">An action to configure the new block before it is added.</param>
    /// <returns>The same builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="modifier"/> is null.</exception>
    public IBlockBuilder Add<TBlock>(Action<TBlock> modifier) where TBlock : Block, new()
    {
        ArgumentNullException.ThrowIfNull(modifier);
        var element = new TBlock();
        modifier(element);
        _blocks.Add(element);
        return this;
    }

    /// <summary>
    /// Adds multiple blocks to the builder.
    /// </summary>
    /// <param name="blocks">The collection of blocks to add.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IBlockBuilder AddBlocks(IEnumerable<Block> blocks)
    {
        ArgumentNullException.ThrowIfNull(blocks);
        _blocks.AddRange(blocks);
        return this;
    }
}