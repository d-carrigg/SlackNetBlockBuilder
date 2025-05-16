using SlackNet;
using SlackNet.Events;
using SlackNet.Blocks;
using SlackNet.WebApi;

namespace Example1;

public class MessageHandler : IEventHandler<MessageEvent>
{
    private readonly SlackApiClient _slackApiClient;

    public MessageHandler(SlackApiClient slackApiClient)
    {
        _slackApiClient = slackApiClient;
    }

    public async Task Handle(MessageEvent slackEvent)
    {
        // echo the message back, update the blocks to add some additional info using the builer
        var builder = BlockBuilder.From(slackEvent.Blocks);

        builder.AddSection("This is an updated message")
            .AddActions(actions =>
            {
                actions.AddButton("button_1_id", "Button 1",
                    "https://github.com/d-carrigg/SlackNetBlockBuilder");
            });
        var blocks = builder.Build();
        var message = new Message
        {
            Channel = slackEvent.Channel,
            Text = "This is an updated message",
            Blocks = blocks
        };
        
        // Send the message back to the channel
        var response = await _slackApiClient.Chat.PostMessage(message);
    }
}