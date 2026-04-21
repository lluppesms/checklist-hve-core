//-----------------------------------------------------------------------
// <copyright file="TemplateSetRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateSet Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// TemplateSet Repository
    /// </summary>
    public class TemplateSetRepository : _BaseRepository, ITemplateSetRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public TemplateSetRepository(AppSettings appSettings, DbContext context)
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
        public TemplateSetGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<TemplateSet> templateSets;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        templateSets = db.TemplateSet
                          .OrderBy(c => c.SetName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateSet.Count();
                    }
                    else
                    {
                        templateSets = db.TemplateSet
                          .Where(c => c.SetName.Contains(searchTxt))
                          .OrderBy(c => c.SetName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateSet.Count(c => c.SetName.Contains(searchTxt));
                    }
                }
                else
                {
                    templateSets = db.TemplateSet.ToList();
                    count = db.TemplateSet.Count();
                }
                var results = new TemplateSetGrid(templateSets, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new TemplateSetGrid
                {
                    Data = new List<TemplateSet>(),
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
        public IQueryable<TemplateSet> FindAll(string requestingUserName)
        {
            return from p in db.TemplateSet select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public TemplateSet FindOne(string requestingUserName, int id)
        {
            return db.TemplateSet.FirstOrDefault(u => u.SetId == id);
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
            if (db.TemplateSet.Any(a => a.SetId == keyValue))
            {
                fieldName = "SetId";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.TemplateSet.Any(a => a.SetName == dscr))
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
        /// <param name="templateSet">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, TemplateSet templateSet)
        {
            try
            {
                templateSet.CreateDateTime = DateTime.Now;
                templateSet.CreateUserName = requestingUserName;
                
                templateSet.ChangeDateTime = DateTime.Now;
                templateSet.ChangeUserName = requestingUserName;
                
                db.TemplateSet.Add(templateSet);
                db.SaveChanges();
                return templateSet.SetId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in TemplateSet.Add: " + GetExceptionMessage(ex);
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
            //// if (db.TemplateSet_Related_Table.Any(a => a.SetId == id))
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
            var templateSet = FindOne(requestingUserName, id);
            db.TemplateSet.Remove(templateSet);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="templateSet">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, TemplateSet templateSet)
        {
            templateSet.ChangeDateTime = DateTime.Now;
            templateSet.ChangeUserName = requestingUserName;
            
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="templateSet">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, TemplateSet templateSet)
        {
            var originalTemplateSet = FindOne(requestingUserName, id);
            if (originalTemplateSet == null) 
            {
                return false;
            }

            originalTemplateSet.SetName = templateSet.SetName;
            originalTemplateSet.SetDscr = templateSet.SetDscr;
            originalTemplateSet.OwnerName = templateSet.OwnerName;
            originalTemplateSet.ActiveInd = templateSet.ActiveInd;
            originalTemplateSet.SortOrder = templateSet.SortOrder;

            originalTemplateSet.ChangeDateTime = DateTime.Now;
            originalTemplateSet.ChangeUserName = requestingUserName;
            
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
