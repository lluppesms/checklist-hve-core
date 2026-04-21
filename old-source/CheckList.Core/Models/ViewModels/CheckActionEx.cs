//-----------------------------------------------------------------------
// <copyright file="CheckAction.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckAction Table
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System;

// ReSharper disable once CheckNamespace
namespace CheckListApp.Data
{
    /// <summary>
    /// CheckAction Table
    /// </summary>
    public class CheckActionEx
    {
        /// <summary>
        /// Action
        /// </summary>
        [JsonProperty("actionId")]
        public int ActionId { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Category Text
        /// </summary>
        [JsonProperty("categoryText")]
        public string CategoryText { get; set; }

        /// <summary>
        /// List
        /// </summary>
        [JsonProperty("listId")]
        public int ListId { get; set; }

        /// <summary>
        /// Action Text
        /// </summary>
        [JsonProperty("actionText")]
        public string ActionText { get; set; }

        /// <summary>
        /// Action Description
        /// </summary>
        [JsonProperty("actionDscr")]
        public string ActionDscr { get; set; }

        /// <summary>
        /// Complete
        /// </summary>
        [JsonProperty("completeInd")]
        public string CompleteInd { get; set; }

        /// <summary>
        /// Sort Order
        /// </summary>
        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Create Date Time
        /// </summary>
        [JsonProperty("createDateTime")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Create User Name
        /// </summary>
        [JsonProperty("createUserName")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// Change Date Time
        /// </summary>
        [JsonProperty("changeDateTime")]
        public DateTime ChangeDateTime { get; set; }

        /// <summary>
        /// Change User Name
        /// </summary>
        [JsonProperty("changeUserName")]
        public string ChangeUserName { get; set; }
    }
}
