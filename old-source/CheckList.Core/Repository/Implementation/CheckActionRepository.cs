//-----------------------------------------------------------------------
// <copyright file="CheckActionRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckAction Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckAction Repository
    /// </summary>
    public class CheckActionRepository : _BaseRepository, ICheckActionRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public CheckActionRepository(AppSettings appSettings, DbContext context)
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
        public CheckActionGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<CheckAction> checkActions;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        checkActions = db.CheckAction
                          .OrderBy(c => c.ActionText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.CheckAction.Count();
                    }
                    else
                    {
                        checkActions = db.CheckAction
                          .Where(c => c.ActionText.Contains(searchTxt))
                          .OrderBy(c => c.ActionText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.CheckAction.Count(c => c.ActionText.Contains(searchTxt));
                    }
                }
                else
                {
                    checkActions = db.CheckAction.ToList();
                    count = db.CheckAction.Count();
                }
                var results = new CheckActionGrid(checkActions, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new CheckActionGrid
                {
                    Data = new List<CheckAction>(),
                    Total = 0,
                    ReturnMessageTxt = returnMessageTxt
                };
                return results;
            }
        }

        /// <summary>
        /// Find All Actions for a List
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public IQueryable<CheckActionViewModel> FindActionsForList(string requestingUserName, int id)
        {
            return from a in db.CheckAction
                   join c in db.CheckCategory on a.CategoryId equals c.CategoryId
                   join l in db.CheckList on c.ListId equals l.ListId
                   where c.ListId == id
                   select new CheckActionViewModel
                   {
                       ActionId = a.ActionId,
                       CategoryId = c.CategoryId,
                       CategoryText = c.CategoryText,
                       ListName = l.ListName,
                       ListId = l.ListId,
                       ActionText = a.ActionText,
                       ActionDscr = a.ActionDscr,
                       IsComplete = a.CompleteInd == "Y",
                       SortOrder = a.SortOrder
                   };
        }

        /// <summary>
        /// Find All Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <returns>Records</returns>
        public IQueryable<CheckAction> FindAll(string requestingUserName)
        {
            return from p in db.CheckAction select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public CheckAction FindOne(string requestingUserName, int id)
        {
            return db.CheckAction.FirstOrDefault(u => u.ActionId == id);
        }

        /// <summary>
        /// Find One Record Extended
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public CheckActionEx FindOneEx(string requestingUserName, int id)
        {
            var item = (from a in db.CheckAction
                        join c in db.CheckCategory on a.CategoryId equals c.CategoryId
                        where a.ActionId == id
                        select new CheckActionEx
                        {
                            ListId = c.ListId,
                            CategoryText = c.CategoryText,
                            ActionId = a.ActionId,
                            CategoryId = a.CategoryId,
                            ActionText = a.ActionText,
                            ActionDscr = a.ActionDscr,
                            CompleteInd = a.CompleteInd,
                            SortOrder = a.SortOrder,
                            ChangeDateTime = a.ChangeDateTime,
                            ChangeUserName = a.ChangeUserName,
                            CreateDateTime = a.CreateDateTime,
                            CreateUserName = a.CreateUserName
                        }
                        ).FirstOrDefault();
            return item;
        }

        /////// <summary>
        /////// Duplicate Record Check
        /////// </summary>
        /////// <param name="keyValue">Key Value</param>
        /////// <param name="dscr">Description</param>
        /////// <param name="fieldName">Field Name</param>
        /////// <param name="errorMessage">Message</param>
        /////// <returns>Success</returns>
        ////public bool DupCheck(int keyValue, string dscr, ref string fieldName, ref string errorMessage)
        ////{
        ////    if (db.CheckAction.Any(a => a.ActionId == keyValue))
        ////    {
        ////        fieldName = "ActionId";
        ////        errorMessage = "This value already exists!";
        ////    }
        ////    else
        ////    {
        ////        if (!db.CheckAction.Any(a => a.ActionText == dscr))
        ////        {
        ////            return false;
        ////        }
        ////        fieldName = "ActionText";
        ////        errorMessage = "This description already exists!";
        ////    }
        ////    return true;
        ////}

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkAction">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, CheckActionEx checkAction)
        {
            try
            {
                var catetegoryId = CheckForCategory(requestingUserName, checkAction.ListId, checkAction.CategoryText);

                var newCheckAction = new CheckAction
                {
                    CategoryId = catetegoryId,
                    ActionText = checkAction.ActionText,
                    ActionDscr = checkAction.ActionDscr,
                    CompleteInd = checkAction.CompleteInd,
                    SortOrder = checkAction.SortOrder,
                    ChangeDateTime = DateTime.Now,
                    ChangeUserName = requestingUserName,
                    CreateDateTime = DateTime.Now,
                    CreateUserName = requestingUserName
                };

                db.CheckAction.Add(newCheckAction);
                db.SaveChanges();
                return newCheckAction.ActionId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in CheckAction.Add: " + GetExceptionMessage(ex);
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
            //// if (db.CheckAction_Related_Table.Any(a => a.ActionId == id))
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
            var checkAction = FindOne(requestingUserName, id);
            db.CheckAction.Remove(checkAction);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="checkAction">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, CheckActionEx checkAction)
        {

            var originalCheckAction = FindOne(requestingUserName, checkAction.ActionId);
            if (originalCheckAction == null)
            {
                return false;
            }

            var categoryId = CheckForCategory(requestingUserName, checkAction.ListId, checkAction.CategoryText);
            originalCheckAction.CategoryId = categoryId;

            originalCheckAction.ActionText = checkAction.ActionText;
            if (!string.IsNullOrEmpty(checkAction.ActionDscr))
            {
                originalCheckAction.ActionDscr = checkAction.ActionDscr;
            }
            if (!string.IsNullOrEmpty(checkAction.CompleteInd)) {
                originalCheckAction.CompleteInd = checkAction.CompleteInd;
            }
            if (CIntNull(checkAction.SortOrder) > 0)
            {
                originalCheckAction.SortOrder = checkAction.SortOrder;
            }
            originalCheckAction.ChangeDateTime = DateTime.Now;
            originalCheckAction.ChangeUserName = requestingUserName;

            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Set Completed Flag
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="completed">Is Task Completed?</param>
        /// <returns>Success</returns>
        public bool SetCompletedFlag(string requestingUserName, int id, bool completed)
        {
            var originalCheckAction = FindOne(requestingUserName, id);
            if (originalCheckAction == null)
            {
                return false;
            }

            originalCheckAction.CompleteInd = completed ? "Y" : "N";
            originalCheckAction.ChangeDateTime = DateTime.Now;
            originalCheckAction.ChangeUserName = requestingUserName;

            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// See if category exists and create it if it doesn't
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="listId">List Id</param>
        /// <param name="categoryText">Category Text</param>
        /// <returns>Id of Category</returns>
        public int CheckForCategory(string requestingUserName, int listId, string categoryText)
        {
            var category = db.CheckCategory.FirstOrDefault(u => u.ListId == listId && u.CategoryText == categoryText);
            if (category == null)
            {
                var newCategory = new CheckCategory
                {
                    ListId = listId,
                    CategoryText = categoryText,
                    CategoryDscr = string.Empty,
                    SortOrder = 50,
                    ActiveInd = "Y",
                    CreateDateTime = DateTime.Now,
                    CreateUserName = requestingUserName,
                    ChangeDateTime = DateTime.Now,
                    ChangeUserName = requestingUserName
                };
                db.CheckCategory.Add(newCategory);
                db.SaveChanges();
                return newCategory.CategoryId;
            }
            else
            {
                return category.CategoryId;
            }

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
