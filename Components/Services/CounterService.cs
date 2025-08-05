using Microsoft.AspNetCore.SignalR;

public class CounterService
{
    private readonly IHubContext<CounterHub> _hubContext;
    public int CurrentCount { get; private set; } = 0;

    public CounterService(IHubContext<CounterHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task IncrementCountAsync()
    {
        CurrentCount++;
        await _hubContext.Clients.All.SendAsync("ReceiveCount", CurrentCount);
    }
}