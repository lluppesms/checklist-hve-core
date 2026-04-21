//-----------------------------------------------------------------------
// <copyright file="ListStats.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// List Statistics
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace CheckListApp.Data
{
    /// <summary>
    /// List Statistics
    /// </summary>
    public class ListStats
    {
        /// <summary>
        /// List Id
        /// </summary>
        [JsonProperty("listId")]
        public int ListId { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// Completed
        /// </summary>
        [JsonProperty("completed")]
        public int Completed { get; set; }

        /// <summary>
        /// Percent Complete
        /// </summary>
        [JsonProperty("percentComplete")]
        public int PercentComplete { get; set; }
    }
}
