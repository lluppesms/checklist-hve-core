//-----------------------------------------------------------------------
// <copyright file="CheckCategoryRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckCategory Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckCategory Repository
    /// </summary>
    public class CheckCategoryRepository : _BaseRepository, ICheckCategoryRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public CheckCategoryRepository(AppSettings appSettings, DbContext context)
        {
            AppSettingsValues = appSettings;
            db = (ProjectEntities)context;
            //SetupRetryPolicy();
        }
        #endregion

        /// <summary>
        /// Find Paged Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="searchTxt">Search Text</param>
        /// <param name="skipNbr">Skip</param>
        /// <param name="takeNbr">Take</param>
        /// <returns>Records</returns>
        public CheckCategoryGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<CheckCategory> checkCategories;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        checkCategories = db.CheckCategory
                          .OrderBy(c => c.CategoryText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.CheckCategory.Count();
                    }
                    else
                    {
                        checkCategories = db.CheckCategory
                          .Where(c => c.CategoryText.Contains(searchTxt))
                          .OrderBy(c => c.CategoryText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.CheckCategory.Count(c => c.CategoryText.Contains(searchTxt));
                    }
                }
                else
                {
                    checkCategories = db.CheckCategory.ToList();
                    count = db.CheckCategory.Count();
                }
                var results = new CheckCategoryGrid(checkCategories, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new CheckCategoryGrid
                {
                    Data = new List<CheckCategory>(),
                    Total = 0,
                    ReturnMessageTxt = returnMessageTxt
                };
                return results;
            }
        }

        /// <summary>
        /// Find All Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <returns>Records</returns>
        public IQueryable<CheckCategory> FindAll(string requestingUserName)
        {
            return from p in db.CheckCategory select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public CheckCategory FindOne(string requestingUserName, int id)
        {
            return db.CheckCategory.FirstOrDefault(u => u.CategoryId == id);
        }

        /// <summary>
        /// Duplicate Record Check
        /// </summary>
        /// <param name="keyValue">Key Value</param>
        /// <param name="dscr">Description</param>
        /// <param name="fieldName">Field Name</param>
        /// <param name="errorMessage">Message</param>
        /// <returns>Success</returns>
        public bool DupCheck(int keyValue, string dscr, ref string fieldName, ref string errorMessage)
        {
            if (db.CheckCategory.Any(a => a.CategoryId == keyValue))
            {
                fieldName = "CategoryId";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.CheckCategory.Any(a => a.CategoryText == dscr))
                {
                    return false;
                }
                fieldName = "CategoryText";
                errorMessage = "This description already exists!";
            }
            return true;
        }

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkCategory">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, CheckCategory checkCategory)
        {
            try
            {
                checkCategory.ActiveInd = "Y";
                checkCategory.CreateDateTime = DateTime.Now;
                checkCategory.CreateUserName = requestingUserName;

                checkCategory.ChangeDateTime = DateTime.Now;
                checkCategory.ChangeUserName = requestingUserName;

                db.CheckCategory.Add(checkCategory);
                db.SaveChanges();
                return checkCategory.CategoryId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in CheckCategory.Add: " + GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                return -1;
            }
        }

        /// <summary>
        /// Delete Check
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="errorMessage">Message</param>
        /// <returns>Success</returns>
        public bool DeleteCheck(string requestingUserName, int id, ref string errorMessage)
        {
            //// bool DeleteCheck = false;
            //// if (db.CheckCategory_Related_Table.Any(a => a.CategoryId == id))
            //// {
            ////     DeleteCheck = true;
            ////     errorMessage = "A related record with this key value exists! You cannot delete it!";
            //// }
            //// return DeleteCheck;
            return true;
        }

        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <returns>Success</returns>
        public bool Delete(string requestingUserName, int id)
        {
            var checkCategory = FindOne(requestingUserName, id);
            db.CheckCategory.Remove(checkCategory);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkCategory">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, CheckCategory checkCategory)
        {
            checkCategory.ChangeDateTime = DateTime.Now;
            checkCategory.ChangeUserName = requestingUserName;

            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="checkCategory">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, CheckCategory checkCategory)
        {
            var originalCheckCategory = FindOne(requestingUserName, id);
            if (originalCheckCategory == null)
            {
                return false;
            }

            originalCheckCategory.ListId = checkCategory.ListId;
            originalCheckCategory.CategoryText = checkCategory.CategoryText;
            originalCheckCategory.CategoryDscr = checkCategory.CategoryDscr;
            originalCheckCategory.ActiveInd = checkCategory.ActiveInd;
            originalCheckCategory.SortOrder = checkCategory.SortOrder;

            originalCheckCategory.ChangeDateTime = DateTime.Now;
            originalCheckCategory.ChangeUserName = requestingUserName;

            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Disposal
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
