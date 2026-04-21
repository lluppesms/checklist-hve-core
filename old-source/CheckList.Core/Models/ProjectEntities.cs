//-----------------------------------------------------------------------
// <copyright file="_ProjectEntities.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc.. All rights reserved.
// </copyright>
// <summary>
// Database Entities
// </summary>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CheckListApp.Data
{
    /// <summary>
    /// Database Entities
    /// </summary>
    public class ProjectEntities : IdentityDbContext<AppUser>
    {
        public ProjectEntities(DbContextOptions<ProjectEntities> options) : base(options)
        {
        }

        /// <summary>
        /// CheckAction Table
        /// </summary>
        public virtual DbSet<CheckAction> CheckAction { get; set; }

        /// <summary>
        /// TemplateSet Table
        /// </summary>
        public DbSet<TemplateSet> TemplateSet { get; set; }

        /// <summary>
        /// TemplateList Table
        /// </summary>
        public DbSet<TemplateList> TemplateList { get; set; }

        /// <summary>
        /// TemplateCategory Table
        /// </summary>
        public DbSet<TemplateCategory> TemplateCategory { get; set; }

        /// <summary>
        /// TemplateAction Table
        /// </summary>
        public DbSet<TemplateAction> TemplateAction { get; set; }

        /// <summary>
        /// CheckSet Table
        /// </summary>
        public DbSet<CheckSet> CheckSet { get; set; }

        /// <summary>
        /// CheckList Table
        /// </summary>
        public DbSet<CheckList> CheckList { get; set; }

        /// <summary>
        /// CheckCategory Table
        /// </summary>
        public DbSet<CheckCategory> CheckCategory { get; set; }

        /// <summary>
        /// Customer Table
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
    }
}
