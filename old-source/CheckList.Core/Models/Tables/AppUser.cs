//-----------------------------------------------------------------------
// <copyright file="CheckSet.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckSet Table
// </summary>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Identity;

namespace CheckListApp.Data
{
    /// <summary>
    /// Add profile data for application users by adding properties to this class    
    /// </summary>
    public class AppUser : IdentityUser
    {
        //// Extended Properties

        /// <summary>
        /// The first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The Facebook identifier.
        /// </summary>
        public long? FacebookId { get; set; }

        /// <summary>
        /// The picture URL.
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
