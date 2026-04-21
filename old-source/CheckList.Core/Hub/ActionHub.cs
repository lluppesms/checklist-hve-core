//-----------------------------------------------------------------------
// <copyright file="ActionHub.cs" company="3M Company, Inc.">
// Copyright 2018, 3M Company, Inc. All rights reserved.
// </copyright>
// <summary>
// Signal R Action Coordinator
// </summary>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using System.Text;

namespace CheckListApp.Hub
{
    /// <summary>
    /// Signal R Action Coordinator
    /// </summary>
    public class ActionHub : Hub<ITypedHubClient>
    {
        /////// <summary>
        /////// Broadcasts the message.
        /////// </summary>
        /////// <param name="userName">Name of the user.</param>
        /////// <param name="roomName">Name of the room.</param>
        /////// <param name="message">The message.</param>
        ////public void BroadcastMessage(string userName, string roomName, string message)
        ////{
        ////    roomName = ValidateRoomName(roomName);
        ////    Clients.Group(roomName).SendAsync("publishBroadcastMessage", userName, roomName, message);
        ////}

        /////// <summary>
        /////// Sends the vote.
        /////// </summary>
        /////// <param name="userName">Name of the user.</param>
        /////// <param name="roomName">Name of the room.</param>
        /////// <param name="message">The number on the face of the card.</param>
        ////public void CastVote(string userName, string roomName, string message)
        ////{
        ////    roomName = ValidateRoomName(roomName);
        ////    Clients.Group(roomName).SendAsync("publishCastVote", userName, roomName, message);
        ////}

        /////// <summary>
        /////// Reveals the votes.
        /////// </summary>
        /////// <param name="userName">Name of the user.</param>
        /////// <param name="roomName">Name of the room.</param>
        /////// <param name="message">The message.</param>
        ////public void RevealVotes(string userName, string roomName, string message)
        ////{
        ////    roomName = ValidateRoomName(roomName);
        ////    Clients.Group(roomName).SendAsync("publishRevealVotes", userName, roomName, message);
        ////}

        /////// <summary>
        /////// Clears the votes.
        /////// </summary>
        /////// <param name="userName">Name of the user.</param>
        /////// <param name="roomName">Name of the room.</param>
        /////// <param name="message">The message.</param>
        ////public void ClearVotes(string userName, string roomName, string message)
        ////{
        ////    roomName = ValidateRoomName(roomName);
        ////    Clients.Group(roomName).SendAsync("publishClearVotes", userName, roomName, message);
        ////}

        /////// <summary>
        /////// User Joined the Room
        /////// </summary>
        /////// <param name="userName">Name of the user.</param>
        /////// <param name="roomName">Name of the room.</param>
        /////// <param name="message">The message.</param>
        ////public void JoinRoom(string userName, string roomName, string message)
        ////{
        ////    roomName = ValidateRoomName(roomName);
        ////    Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        ////    Clients.Group(roomName).SendAsync("publishJoinRoom", userName, roomName, userName + " joined group " + roomName);
        ////}

        /////// <summary>
        /////// User Left the Room
        /////// </summary>
        /////// <param name="userName">Name of the user.</param>
        /////// <param name="roomName">Name of the room.</param>
        /////// <param name="message">The message.</param>
        ////public void LeaveRoom(string userName, string roomName, string message)
        ////{
        ////    roomName = ValidateRoomName(roomName);
        ////    Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        ////    Clients.Group(roomName).SendAsync("publishLeaveRoom", userName, roomName, userName + " left group " + roomName);
        ////}

        /// <summary>
        /// Validates that this string has only letters
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Validated string</returns>
        protected static string ValidateRoomName(string input)
        {
            const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_.";
            var maxLength = 250;
            return IsOnlyTheseCharacters(input, maxLength, ValidChars).ToUpper(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Validates that this string has only letters
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="maxLength">Maximum Length</param>
        /// <param name="validCharacters">The valid characters.</param>
        /// <returns>Validated string</returns>
        protected static string IsOnlyTheseCharacters(string input, int maxLength, string validCharacters)
        {
            var bld = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                if (bld.Length < maxLength)
                {
                    if (validCharacters.Contains(input[i]))
                    {
                        bld.Append(input[i]);
                    }
                }
                else
                {
                    break;
                }
            }
            var newString = bld.ToString();
            return newString;
        }
    }
}
