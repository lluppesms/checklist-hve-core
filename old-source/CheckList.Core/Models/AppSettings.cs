//-----------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Luppes Consulting, Inc.">
// Copyright 2018, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Application Settings
// </summary>
//-----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace CheckListApp.Data
{
    /// <summary>
    /// Application Settings
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Default Connection 
        /// </summary>
        public string DefaultConnection { get; set; }

        /// <summary>
        /// Project Entities
        /// </summary>
        public string ProjectEntities { get; set; }

        /// <summary>
        /// Email From
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// Send Errors To
        /// </summary>
        public string SendErrorsTo { get; set; }

        /// <summary>
        /// Send Grid Server
        /// </summary>
        public string SendGridServer { get; set; }

        /// <summary>
        /// Send Grid User
        /// </summary>
        public string SendGridUserId { get; set; }

        /// <summary>
        /// Send Grid Password
        /// </summary>
        public string SendGridPassword { get; set; }

        /// <summary>
        /// Allowed File Types
        /// </summary>
        public string AllowedFileTypesSchema { get; set; }

        /// <summary>
        /// Admin Group Id
        /// </summary>
        public string AdminGroupId { get; set; }

        /// <summary>
        /// Signal R Hub
        /// </summary>
        public string SignalRHub { get; set; }

        /// <summary>
        /// API Keys
        /// </summary>
        public string APIKeys { get; set; }

        /// <summary>
        /// Application Settings
        /// </summary>
        public AppSettings()
        {
            UserName = string.Empty;
        }
    }
}
