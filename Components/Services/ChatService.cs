using Microsoft.AspNetCore.SignalR;

namespace BlazorAzureChat.Components.Services;

public class ChatService
{
    private readonly IHubContext<ChatHub> _hubContext;

    // String queue for storing messages, limited to 10 entries
    private readonly Queue<string> _messageQueue = new($"Startad {DateTime.Now}");

    public ChatService(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    // Adds a message to the queue, removing the oldest if limit is exceeded
    public async Task EnqueueMessage(string message)
    {
        while (_messageQueue.Count >= 10)
        {
            _messageQueue.Dequeue();
        }
        _messageQueue.Enqueue(message);

        await _hubContext.Clients.All.SendAsync("ReceiveChat", GetMessages());
    }

    // Returns all messages in the queue as a list
    public IReadOnlyList<string>? GetMessages()
    {
        return _messageQueue.ToArray();
    }
}