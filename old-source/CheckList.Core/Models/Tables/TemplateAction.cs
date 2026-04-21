//-----------------------------------------------------------------------
// <copyright file="TemplateAction.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateAction Table
// </summary>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable once CheckNamespace
namespace CheckListApp.Data
{
    /// <summary>
    /// TemplateAction Table
    /// </summary>
    [Table("TemplateAction")]
    public class TemplateAction
    {
        /// <summary>
        /// Action
        /// </summary>
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Action is required")]
        [Display(Name = "Action", Description = "This is the Action field.", Prompt = "Enter Action")]
        [JsonProperty("actionId")]
        public int ActionId { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        [Display(Name = "Category", Description = "This is the Category field.", Prompt = "Enter Category")]
        [JsonProperty("categoryId")]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Action Text
        /// </summary>
        [Display(Name = "Action Text", Description = "This is the Action Text field.", Prompt = "Enter Action Text")]
        [StringLength(255)]
        [JsonProperty("actionText")]
        public string ActionText { get; set; }

        /// <summary>
        /// Action Description
        /// </summary>
        [Display(Name = "Action Description", Description = "This is the Action Description field.", Prompt = "Enter Action Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        [JsonProperty("actionDscr")]
        public string ActionDscr { get; set; }

        /// <summary>
        /// Complete
        /// </summary>
        [Display(Name = "Complete", Description = "This is the Complete field.", Prompt = "Enter Complete")]
        [StringLength(1)]
        [JsonProperty("completeInd")]
        public string CompleteInd { get; set; }

        /// <summary>
        /// Sort Order
        /// </summary>
        [Required(ErrorMessage = "Sort Order is required")]
        [Display(Name = "Sort Order", Description = "This is the Sort Order field.", Prompt = "Enter Sort Order")]
        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Create Date Time
        /// </summary>
        [Display(Name = "Create Date Time", Description = "This is the Create Date Time field.", Prompt = "Enter Create Date Time")]
        [DataType(DataType.DateTime)]
        [JsonProperty("createDateTime")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Create User Name
        /// </summary>
        [Display(Name = "Create User Name", Description = "This is the Create User Name field.", Prompt = "Enter Create User Name")]
        [StringLength(255)]
        [JsonProperty("createUserName")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// Change Date Time
        /// </summary>
        [Display(Name = "Change Date Time", Description = "This is the Change Date Time field.", Prompt = "Enter Change Date Time")]
        [DataType(DataType.DateTime)]
        [JsonProperty("changeDateTime")]
        public DateTime ChangeDateTime { get; set; }

        /// <summary>
        /// Change User Name
        /// </summary>
        [Display(Name = "Change User Name", Description = "This is the Change User Name field.", Prompt = "Enter Change User Name")]
        [StringLength(255)]
        [JsonProperty("changeUserName")]
        public string ChangeUserName { get; set; }
    }
}
