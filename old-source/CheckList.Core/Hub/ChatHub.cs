//-----------------------------------------------------------------------
// <copyright file="ActionHub.cs" company="3M Company, Inc.">
// Copyright 2018, 3M Company, Inc. All rights reserved.
// </copyright>
// <summary>
// Signal R Action Coordinator
// </summary>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.SignalR;

namespace CheckListApp
{
    /// <summary>
    /// Signal R Chat Coordinator
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
    public class Chat : Microsoft.AspNetCore.SignalR.Hub
    {
        /// <summary>
        /// Broadcasts the message.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        public void BroadcastMessage(string name, string message)
        {
            Clients.All.SendAsync("broadcastMessage", name, message);
        }

        /// <summary>
        /// Sends the vote.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        public void SendVote(string name, string message)
        {
            Clients.All.SendAsync("sendVote", name, message);
        }

        /// <summary>
        /// Reveals the votes.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        public void RevealVotes(string name, string message)
        {
            Clients.All.SendAsync("revealVotes", name, message);
        }

        /// <summary>
        /// Clears the votes.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        public void ClearVotes(string name, string message)
        {
            Clients.All.SendAsync("clearVotes", name, message);
        }

        /// <summary>
        /// Echoes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="message">The message.</param>
        public void Echo(string name, string message)
        {
            Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
        }
    }
}
