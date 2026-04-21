//-----------------------------------------------------------------------
// <copyright file="CheckSet.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckSet Table
// </summary>
//-----------------------------------------------------------------------

namespace CheckListApp.Data
{
    public class Customer
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public AppUser Identity { get; set; }  // navigation property
        public string Location { get; set; }
        public string Locale { get; set; }
        public string Gender { get; set; }
    }
}
