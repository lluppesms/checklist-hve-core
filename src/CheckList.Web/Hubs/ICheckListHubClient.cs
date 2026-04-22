namespace CheckList.Web.Hubs;

using CheckList.Shared.DTOs;

public interface ICheckListHubClient
{
    /// <summary>Sent to all clients watching the same set when an action is toggled.</summary>
    Task ReceiveActionUpdate(ActionUpdateDto update);
}
