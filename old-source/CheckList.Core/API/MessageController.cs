using CheckListApp.Hub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;

namespace CheckListApp.API
{
    [Route("api/Message")]
    [Authorize(Policy = "ApiUser")]
    public class MessageController : Controller
    {
        private IHubContext<ActionHub, ITypedHubClient> _hubContext;

        public MessageController(IHubContext<ActionHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public string Post([FromBody]Message msg)
        {
            string retMessage = string.Empty;
            try
            {
                _hubContext.Clients.All.BroadcastMessage(msg.Type, msg.Payload);
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }
            return retMessage;
        }
    }
}
