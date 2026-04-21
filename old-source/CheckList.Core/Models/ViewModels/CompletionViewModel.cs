//-----------------------------------------------------------------------
// <copyright file="CompletionViewModel.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Action Completion View Model
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace CheckListApp.Data
{
    /// <summary>
    /// Action Completion View Model
    /// </summary>
    public class CompletionViewModel
    {
        /// <summary>
        /// Action Id
        /// </summary>
        [JsonProperty("actionId")]
        public int ActionId { get; set; }

        /// <summary>
        /// Is Complete
        /// </summary>
        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }
    }
}
