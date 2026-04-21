using System.Threading.Tasks;

namespace CheckListApp.Hub
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string type, string payload);
    }
}
