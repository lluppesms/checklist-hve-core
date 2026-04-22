namespace CheckList.Api.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class CheckListHub : Hub<ICheckListHubClient>
{
    /// <summary>Join the group for a specific check set to receive real-time updates.</summary>
    public async Task JoinSet(string setId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"set-{setId}");

    /// <summary>Leave the group for a specific check set.</summary>
    public async Task LeaveSet(string setId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"set-{setId}");
}
