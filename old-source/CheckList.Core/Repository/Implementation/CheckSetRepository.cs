//-----------------------------------------------------------------------
// <copyright file="CheckSetRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckSet Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// CheckSet Repository
    /// </summary>
    public class CheckSetRepository : _BaseRepository, ICheckSetRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public CheckSetRepository(AppSettings appSettings, DbContext context)
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
        public CheckSetGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<CheckSet> checkSets;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        checkSets = db.CheckSet
                          .OrderBy(c => c.SetName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.CheckSet.Count();
                    }
                    else
                    {
                        checkSets = db.CheckSet
                          .Where(c => c.SetName.Contains(searchTxt))
                          .OrderBy(c => c.SetName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.CheckSet.Count(c => c.SetName.Contains(searchTxt));
                    }
                }
                else
                {
                    checkSets = db.CheckSet.ToList();
                    count = db.CheckSet.Count();
                }
                var results = new CheckSetGrid(checkSets, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new CheckSetGrid
                {
                    Data = new List<CheckSet>(),
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
        public IQueryable<CheckSet> FindAll(string requestingUserName)
        {
            return from p in db.CheckSet select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public CheckSet FindOne(string requestingUserName, int id)
        {
            return db.CheckSet.FirstOrDefault(u => u.SetId == id);
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
            if (db.CheckSet.Any(a => a.SetId == keyValue))
            {
                fieldName = "SetId";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.CheckSet.Any(a => a.SetName == dscr))
                {
                    return false;
                }
                fieldName = "SetName";
                errorMessage = "This description already exists!";
            }
            return true;
        }

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkSet">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, CheckSet checkSet)
        {
            try
            {
                checkSet.ActiveInd = "Y";
                checkSet.CreateDateTime = DateTime.Now;
                checkSet.CreateUserName = requestingUserName;

                checkSet.ChangeDateTime = DateTime.Now;
                checkSet.ChangeUserName = requestingUserName;

                db.CheckSet.Add(checkSet);
                db.SaveChanges();
                return checkSet.SetId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in CheckSet.Add: " + GetExceptionMessage(ex);
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
            //// if (db.CheckSet_Related_Table.Any(a => a.SetId == id))
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
            var checkSet = FindOne(requestingUserName, id);
            db.CheckSet.Remove(checkSet);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="checkSet">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, CheckSet checkSet)
        {
            checkSet.ChangeDateTime = DateTime.Now;
            checkSet.ChangeUserName = requestingUserName;

            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="checkSet">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, CheckSet checkSet)
        {
            var originalCheckSet = FindOne(requestingUserName, id);
            if (originalCheckSet == null)
            {
                return false;
            }

            originalCheckSet.TemplateSetId = checkSet.TemplateSetId;
            originalCheckSet.SetName = checkSet.SetName;
            originalCheckSet.SetDscr = checkSet.SetDscr;
            originalCheckSet.OwnerName = checkSet.OwnerName;
            originalCheckSet.ActiveInd = checkSet.ActiveInd;
            originalCheckSet.SortOrder = checkSet.SortOrder;

            originalCheckSet.ChangeDateTime = DateTime.Now;
            originalCheckSet.ChangeUserName = requestingUserName;

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
