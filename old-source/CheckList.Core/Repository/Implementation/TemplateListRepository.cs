//-----------------------------------------------------------------------
// <copyright file="TemplateListRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateList Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// TemplateList Repository
    /// </summary>
    public class TemplateListRepository : _BaseRepository, ITemplateListRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public TemplateListRepository(AppSettings appSettings, DbContext context)
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
        public TemplateListGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<TemplateList> templateLists;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        templateLists = db.TemplateList
                          .OrderBy(c => c.ListName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateList.Count();
                    }
                    else
                    {
                        templateLists = db.TemplateList
                          .Where(c => c.ListName.Contains(searchTxt))
                          .OrderBy(c => c.ListName)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateList.Count(c => c.ListName.Contains(searchTxt));
                    }
                }
                else
                {
                    templateLists = db.TemplateList.ToList();
                    count = db.TemplateList.Count();
                }
                var results = new TemplateListGrid(templateLists, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new TemplateListGrid
                {
                    Data = new List<TemplateList>(),
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
        public IQueryable<TemplateList> FindAll(string requestingUserName)
        {
            return from p in db.TemplateList select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public TemplateList FindOne(string requestingUserName, int id)
        {
            return db.TemplateList.FirstOrDefault(u => u.ListId == id);
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
            if (db.TemplateList.Any(a => a.ListId == keyValue))
            {
                fieldName = "ListId";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.TemplateList.Any(a => a.ListName == dscr))
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
        /// <param name="templateList">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, TemplateList templateList)
        {
            try
            {
                templateList.CreateDateTime = DateTime.Now;
                templateList.CreateUserName = requestingUserName;
                
                templateList.ChangeDateTime = DateTime.Now;
                templateList.ChangeUserName = requestingUserName;
                
                db.TemplateList.Add(templateList);
                db.SaveChanges();
                return templateList.ListId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in TemplateList.Add: " + GetExceptionMessage(ex);
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
            //// if (db.TemplateList_Related_Table.Any(a => a.ListId == id))
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
            var templateList = FindOne(requestingUserName, id);
            db.TemplateList.Remove(templateList);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="templateList">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, TemplateList templateList)
        {
            templateList.ChangeDateTime = DateTime.Now;
            templateList.ChangeUserName = requestingUserName;
            
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="templateList">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, TemplateList templateList)
        {
            var originalTemplateList = FindOne(requestingUserName, id);
            if (originalTemplateList == null) 
            {
                return false;
            }

            originalTemplateList.SetId = templateList.SetId;
            originalTemplateList.ListName = templateList.ListName;
            originalTemplateList.ListDscr = templateList.ListDscr;
            originalTemplateList.ActiveInd = templateList.ActiveInd;
            originalTemplateList.SortOrder = templateList.SortOrder;

            originalTemplateList.ChangeDateTime = DateTime.Now;
            originalTemplateList.ChangeUserName = requestingUserName;
            
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
