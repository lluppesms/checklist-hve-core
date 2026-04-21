//-----------------------------------------------------------------------
// <copyright file="TemplateCategory.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateCategory Table
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
    /// TemplateCategory Table
    /// </summary>
    [Table("TemplateCategory")]
    public class TemplateCategory
    {
        /// <summary>
        /// Category
        /// </summary>
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category", Description = "This is the Category field.", Prompt = "Enter Category")]
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// List
        /// </summary>
        [Required(ErrorMessage = "List is required")]
        [Display(Name = "List", Description = "This is the List field.", Prompt = "Enter List")]
        [JsonProperty("listId")]
        public int ListId { get; set; }

        /// <summary>
        /// Category Text
        /// </summary>
        [Required(ErrorMessage = "Category Text is required")]
        [Display(Name = "Category Text", Description = "This is the Category Text field.", Prompt = "Enter Category Text")]
        [StringLength(255)]
        [JsonProperty("categoryText")]
        public string CategoryText { get; set; }

        /// <summary>
        /// Category Description
        /// </summary>
        [Display(Name = "Category Description", Description = "This is the Category Description field.", Prompt = "Enter Category Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        [JsonProperty("categoryDscr")]
        public string CategoryDscr { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        [Required(ErrorMessage = "Active is required")]
        [Display(Name = "Active", Description = "This is the Active field.", Prompt = "Enter Active")]
        [StringLength(1)]
        [JsonProperty("activeInd")]
        public string ActiveInd { get; set; }

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
