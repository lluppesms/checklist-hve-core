//-----------------------------------------------------------------------
// <copyright file="RegistrationViewModel.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Registration View Model
// </summary>
//-----------------------------------------------------------------------

namespace CheckListApp.Data
{
    public class RegistrationViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
    }
}
