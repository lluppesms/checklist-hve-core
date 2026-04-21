//-----------------------------------------------------------------------
// <copyright file="CheckActionViewModel.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckAction View Model
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckAction View Model
    /// </summary>
    public class CheckActionViewModel
    {
        /// <summary>
        /// List Name
        /// </summary>
        [JsonProperty("listName")]
        public string ListName { get; set; }

        /// <summary>
        /// Category Id
        /// </summary>
        [JsonProperty("categoryId")]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Category Id
        /// </summary>
        [JsonProperty("listId")]
        public int? ListId { get; set; }

        /// <summary>
        /// Category Text
        /// </summary>
        [JsonProperty("categoryText")]
        public string CategoryText { get; set; }

        /// <summary>
        /// Action Id
        /// </summary>
        [JsonProperty("actionId")]
        public int ActionId { get; set; }

        /// <summary>
        /// Action Text
        /// </summary>
        [JsonProperty("actionText")]
        public string ActionText { get; set; }

        /// <summary>
        /// Action Dscr
        /// </summary>
        [JsonProperty("actionDscr")]
        public string ActionDscr { get; set; }

        /// <summary>
        /// Is Complete
        /// </summary>
        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }

        /// <summary>
        /// Sort Order
        /// </summary>
        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }

        ///// <summary>
        ///// Create Date Time
        ///// </summary>
        //[JsonProperty("createDateTime")]
        //public DateTime CreateDateTime { get; set; }

        ///// <summary>
        ///// Create User Name
        ///// </summary>
        //[JsonProperty("createUserName")]
        //public string CreateUserName { get; set; }

        ///// <summary>
        ///// Change Date Time
        ///// </summary>
        //[JsonProperty("changeDateTime")]
        //public DateTime ChangeDateTime { get; set; }

        ///// <summary>
        ///// Change User Name
        ///// </summary>
        //[JsonProperty("changeUserName")]
        //public string ChangeUserName { get; set; }

    }
}
