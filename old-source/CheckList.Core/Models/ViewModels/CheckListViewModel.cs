//-----------------------------------------------------------------------
// <copyright file="CheckListViewModel.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckList View Model
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckList View Model
    /// </summary>
    public class CheckListViewModel
    {
        /// <summary>
        /// Set Id
        /// </summary>
        [JsonProperty("setId")]
        public int SetId { get; set; }

        /// <summary>
        /// Set Name
        /// </summary>
        [JsonProperty("setName")]
        public string SetName { get; set; }

        /// <summary>
        /// Set Description
        /// </summary>
        [JsonProperty("setDscr")]
        public string SetDscr { get; set; }

        /// <summary>
        /// List Id
        /// </summary>
        [JsonProperty("listId")]
        public int ListId { get; set; }

        /// <summary>
        /// List Name
        /// </summary>
        [JsonProperty("listName")]
        public string ListName { get; set; }

        /// <summary>
        /// List Description
        /// </summary>
        [JsonProperty("listDscr")]
        public string ListDscr { get; set; }

        /// <summary>
        /// Is Active
        /// </summary>
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Sort Order
        /// </summary>
        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Actions Count
        /// </summary>
        [JsonProperty("actionsCount")]
        public int ActionsCount { get; set; }

        /// <summary>
        /// Actions Complete
        /// </summary>
        [JsonProperty("actionsComplete")]
        public int ActionsComplete { get; set; }

        /// <summary>
        /// Percent Complete
        /// </summary>
        [JsonProperty("percentComplete")]
        public float PercentComplete { get; set; }

    }
}
