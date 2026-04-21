//-----------------------------------------------------------------------
// <copyright file="ICheckActionRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckAction Interface
// </summary>
//-----------------------------------------------------------------------

using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckAction Interface
    /// </summary>
    public interface ICheckActionRepository
    {
        /// <summary>
        /// Find All Actions for a List
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        IQueryable<CheckActionViewModel> FindActionsForList(string requestingUserName, int id);

        /// <summary>
        /// Find All Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <returns>Records</returns>
        IQueryable<CheckAction> FindAll(string requestingUserName);

        /// <summary>
        /// Find Paged Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="searchTxt">Search Text</param>
        /// <param name="skipNbr">Skip</param>
        /// <param name="takeNbr">Take</param>
        /// <returns>Records</returns>
        CheckActionGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr);

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        CheckAction FindOne(string requestingUserName, int id);

        /// <summary>
        /// Find One Record Extended
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        CheckActionEx FindOneEx(string requestingUserName, int id);

        /////// <summary>
        /////// Duplicate Record Check
        /////// </summary>
        /////// <param name="keyValue">Key Value</param>
        /////// <param name="dscr">Description</param>
        /////// <param name="fieldName">Field Name</param>
        /////// <param name="errorMessage">Message</param>
        /////// <returns>Success</returns>
        ////bool DupCheck(int keyValue, string dscr, ref string fieldName, ref string errorMessage);

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkAction">Object</param>
        /// <returns>Success</returns>
        int Add(string requestingUserName, CheckActionEx checkAction);

        /// <summary>
        /// Delete Check
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="errorMessage">Message</param>
        /// <returns>Success</returns>
        bool DeleteCheck(string requestingUserName, int id, ref string errorMessage);

        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <returns>Success</returns>
        bool Delete(string requestingUserName, int id);

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkAction">Object</param>
        /// <returns>Success</returns>
        bool Save(string requestingUserName, CheckActionEx checkAction);

        /// <summary>
        /// Set Completed Flag
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="completed">Is Task Completed?</param>
        /// <returns>Success</returns>
        bool SetCompletedFlag(string requestingUserName, int id, bool completed);

#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
        /// <summary>
        /// Disposal
        /// </summary>
        void Dispose();
#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
    }
}
