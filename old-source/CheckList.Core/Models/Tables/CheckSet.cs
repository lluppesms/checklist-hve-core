//-----------------------------------------------------------------------
// <copyright file="CheckSet.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckSet Table
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
    /// CheckSet Table
    /// </summary>
    [Table("CheckSet")]
    public class CheckSet
    {
        /// <summary>
        /// Set
        /// </summary>
        [Key, Column(Order = 0)]
        [Required(ErrorMessage = "Set is required")]
        [Display(Name = "Set", Description = "This is the Set field.", Prompt = "Enter Set")]
        [JsonProperty("setId")]
        public int SetId { get; set; }

        /// <summary>
        /// Template Set
        /// </summary>
        [Display(Name = "Template Set", Description = "This is the Template Set field.", Prompt = "Enter Template Set")]
        [JsonProperty("templateSetId")]
        public int? TemplateSetId { get; set; }

        /// <summary>
        /// Set Name
        /// </summary>
        [Required(ErrorMessage = "Set Name is required")]
        [Display(Name = "Set Name", Description = "This is the Set Name field.", Prompt = "Enter Set Name")]
        [StringLength(255)]
        [JsonProperty("setName")]
        public string SetName { get; set; }

        /// <summary>
        /// Set Description
        /// </summary>
        [Display(Name = "Set Description", Description = "This is the Set Description field.", Prompt = "Enter Set Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        [JsonProperty("setDscr")]
        public string SetDscr { get; set; }

        /// <summary>
        /// Owner Name
        /// </summary>
        [Required(ErrorMessage = "Owner Name is required")]
        [Display(Name = "Owner Name", Description = "This is the Owner Name field.", Prompt = "Enter Owner Name")]
        [StringLength(256)]
        [JsonProperty("ownerName")]
        public string OwnerName { get; set; }

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
