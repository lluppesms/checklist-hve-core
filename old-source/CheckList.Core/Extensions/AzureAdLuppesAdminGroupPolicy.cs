//-----------------------------------------------------------------------
// <copyright file="AzureAdTimeCrunchAdminGroupPolicy.cs" company="Luppes Consulting, Inc.">
// Copyright 2018, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Create security policy for group membership
// </summary>
//-----------------------------------------------------------------------

//// This links up the Group Id returned in the Azure AD claims to group membership policy
//// https://odetocode.com/blogs/scott/archive/2018/02/20/role-based-authorization-in-asp-net-core-with-azure-ad.aspx
//// You must also change Azure AD App Registration to return groups by setting this value:
////   "groupMembershipClaims": "SecurityGroup",
//// To secure a function, use the Authorized-Policy tag
////   [Authorize(Policy = "Admin")]
///
using Microsoft.AspNetCore.Authorization;

namespace CheckListApp.Extensions
{
    public class AzureAdLuppesAdminGroupPolicy
    {
        /// <summary>
        /// The policy name.
        /// </summary>
        public static string Name => "Admin";

        /// <summary>
        /// The group identifier.
        /// </summary>
        public static string GroupId = string.Empty;

        /// <summary>
        /// Builds the specified policy
        /// </summary>
        /// <param name="builder">The builder.</param>
        public static void Build(AuthorizationPolicyBuilder builder) => builder.RequireClaim("groups", GroupId);
    }
}
