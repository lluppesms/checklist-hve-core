//-----------------------------------------------------------------------
// <copyright file="CheckListRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckList Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckList Repository
    /// </summary>
    public class CheckListRepository : _BaseRepository, ICheckListRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public CheckListRepository(AppSettings appSettings, DbContext context)
        {
            AppSettingsValues = appSettings;
            db = (ProjectEntities)context;
            //SetupRetryPolicy();
        }
        #endregion

        /// <summary>
        /// Find All Lists for a Set
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public IQueryable<CheckListViewModel> FindListsForSet(string requestingUserName, int id)
        {
            var stats = from a in db.CheckAction
                        group a by new
                        {
                            a.ListId
                        } into g
                        select new ListStats
                        {
                            ListId = g.Max(x => x.ListId),
                            Completed = g.Sum(x => x.CompleteInd == "Y" ? 1 : 0),
                            Total = g.Count(),
                            PercentComplete = g.Sum(x => x.CompleteInd == "Y" ? 1 : 0) / g.Count()
                        };

            var lists = from l in db.CheckList
                        join s in db.CheckSet on l.SetId equals s.SetId
                        join c in stats on l.ListId equals c.ListId
                        where l.SetId == id
                        select new CheckListViewModel
                        {
                            ListId = l.ListId,
                            ListName = l.ListName,
                            ListDscr = l.ListDscr,
                            SetId = s.SetId,
                            SetName = s.SetName,
                            SetDscr = s.SetDscr,
                            IsActive = l.ActiveInd == "Y",
                            SortOrder = l.SortOrder,
                            ActionsComplete = c.Completed,
                            ActionsCount = c.Total,
                            PercentComplete = c.PercentComplete
                        };
            return lists;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public CheckList FindOne(string requestingUserName, int id)
        {
            return db.CheckList.FirstOrDefault(u => u.ListId == id);
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
            if (db.CheckList.Any(a => a.ListId == keyValue))
            {
                fieldName = "ListId";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.CheckList.Any(a => a.ListName == dscr))
                {
                    return false;
                }
                fieldName = "ListName";
                errorMessage = "This description already exists!";
            }
            return true;
        }

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkList">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, CheckList checkList)
        {
            try
            {
                checkList.ActiveInd = "Y";
                checkList.CreateDateTime = DateTime.Now;
                checkList.CreateUserName = requestingUserName;

                checkList.ChangeDateTime = DateTime.Now;
                checkList.ChangeUserName = requestingUserName;

                db.CheckList.Add(checkList);
                db.SaveChanges();

                var newCategory = new CheckCategory
                {
                    ListId = checkList.ListId,
                    CategoryText = "Main",
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
                return checkList.ListId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in CheckList.Add: " + GetExceptionMessage(ex);
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
            //// if (db.CheckList_Related_Table.Any(a => a.ListId == id))
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
            var checkList = FindOne(requestingUserName, id);
            db.CheckList.Remove(checkList);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkList">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, CheckList checkList)
        {
            checkList.ChangeDateTime = DateTime.Now;
            checkList.ChangeUserName = requestingUserName;

            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="checkList">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, CheckList checkList)
        {
            var originalCheckList = FindOne(requestingUserName, id);
            if (originalCheckList == null)
            {
                return false;
            }

            originalCheckList.SetId = checkList.SetId;
            originalCheckList.ListName = checkList.ListName;
            originalCheckList.ListDscr = checkList.ListDscr;
            originalCheckList.ActiveInd = checkList.ActiveInd;
            originalCheckList.SortOrder = checkList.SortOrder;

            originalCheckList.ChangeDateTime = DateTime.Now;
            originalCheckList.ChangeUserName = requestingUserName;

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
