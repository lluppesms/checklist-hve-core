//-----------------------------------------------------------------------
// <copyright file="ICheckCategoryRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckCategory Interface
// </summary>
//-----------------------------------------------------------------------

using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckCategory Interface
    /// </summary>
    public interface ICheckCategoryRepository
  {
    /// <summary>
    /// Find All Records
    /// </summary>
    /// <param name="requestingUserName">Requesting UserName</param>
    /// <returns>Records</returns>
    IQueryable<CheckCategory> FindAll(string requestingUserName);

    /// <summary>
    /// Find Paged Records
    /// </summary>
    /// <param name="requestingUserName">Requesting UserName</param>
    /// <param name="searchTxt">Search Text</param>
    /// <param name="skipNbr">Skip</param>
    /// <param name="takeNbr">Take</param>
    /// <returns>Records</returns>
    CheckCategoryGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr);

    /// <summary>
    /// Find One Records
    /// </summary>
    /// <param name="requestingUserName">Requesting UserName</param>
    /// <param name="id">id</param>
    /// <returns>Records</returns>
    CheckCategory FindOne(string requestingUserName, int id);

    /// <summary>
    /// Duplicate Record Check
    /// </summary>
    /// <param name="keyValue">Key Value</param>
    /// <param name="dscr">Description</param>
    /// <param name="fieldName">Field Name</param>
    /// <param name="errorMessage">Message</param>
    /// <returns>Success</returns>
    bool DupCheck(int keyValue, string dscr, ref string fieldName, ref string errorMessage);

    /// <summary>
    /// Add Record
    /// </summary>
    /// <param name="requestingUserName">Requesting UserName</param>
    /// <param name="checkCategory">Object</param>
    /// <returns>Success</returns>
    int Add(string requestingUserName, CheckCategory checkCategory);

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
    /// <param name="checkCategory">Object</param>
    /// <returns>Success</returns>
    bool Save(string requestingUserName, CheckCategory checkCategory);

    /// <summary>
    /// Save Record
    /// </summary>
    /// <param name="requestingUserName">Requesting UserName</param>
    /// <param name="id">Id</param>
    /// <param name="checkCategory">Object</param>
    /// <returns>Success</returns>
    bool Save(string requestingUserName, int id, CheckCategory checkCategory);

#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
    /// <summary>
    /// Disposal
    /// </summary>
    void Dispose();
#pragma warning disable S2953 // Methods named "Dispose" should implement "IDisposable.Dispose"
  }
}
