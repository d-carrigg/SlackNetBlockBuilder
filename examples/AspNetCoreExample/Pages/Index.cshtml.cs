using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SlackNet;
using SlackNet.Blocks;

namespace Example1.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
    
    public string[] Examples { get; } = new[]
    {
        "Interactive Form",
        "Message Before Update",
        "Message After Update",
    };

    public async Task<IActionResult> OnGetJson(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = "InteractiveForm"; // Default example
        }

        var blocks = GetBlocks(name);
        
        var settings = SlackNet.Default.JsonSettings(SlackNet.Default.SlackTypeResolver());
        settings.SerializerSettings.Formatting = Formatting.Indented;
        var content = JsonConvert.SerializeObject(blocks,settings.SerializerSettings);
       return Content(content, "application/json");
    }
 
    private static List<Block> GetBlocks(string exampleName)
    {
        var builder = BlockBuilder.Create();
        return exampleName switch
        {
            "Interactive Form" => GetInteractiveFormExample(builder),
            "Message Before Update" => GetMessageBeforeUpdateExample(builder),
            "Message After Update" => GetMessageAfterUpdateExample(builder),
            
            _ => throw new ArgumentException($"Unknown example: {exampleName}", nameof(exampleName))
        };
    }

    private static List<Block> GetInteractiveFormExample(IBlockBuilder builder)
    {
        
        return builder
            .AddHeader("Request Form")
            .AddInput<PlainTextInput>("Title", input => input
                .ActionId("title_input")
                .Placeholder("Enter title")
            )
            .AddInput<PlainTextInput>("Description", input => input
                .ActionId("description_input")
                .Modify(x => x.Multiline = true)
                .Optional())
            .AddInput<StaticSelectMenu>("Priority", input => input
                .ActionId("priority_select")
                .Placeholder("Select priority")
                .AddOption("Low", "low")
                .AddOption("Medium", "medium")
                .AddOption("High", "high"))
            .AddActions(actions => actions
                .AddButton("submit", "Submit")
                .AddButton("cancel", "Cancel"))
            .Build();
    }
    
    private static List<Block> GetMessageBeforeUpdateExample(IBlockBuilder builder)
    {
        return builder
            .AddSection("Please approve this request")
            .AddActions(actions => actions
                .AddButton("approve", "Approve")
                .AddButton("reject", "Reject"))
            .Build();
    }
    
    private static List<Block> GetMessageAfterUpdateExample(IBlockBuilder builder)
    {
        return builder
            .RemoveActions() // Remove buttons
            .AddSection("✅ Request approved!")
            .Build();
    }
}