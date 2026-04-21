//-----------------------------------------------------------------------
// <copyright file="TemplateActionRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateAction Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// TemplateAction Repository
    /// </summary>
    public class TemplateActionRepository : _BaseRepository, ITemplateActionRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public TemplateActionRepository(AppSettings appSettings, DbContext context)
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
        public TemplateActionGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<TemplateAction> templateActions;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        templateActions = db.TemplateAction
                          .OrderBy(c => c.ActionText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateAction.Count();
                    }
                    else
                    {
                        templateActions = db.TemplateAction
                          .Where(c => c.ActionText.Contains(searchTxt))
                          .OrderBy(c => c.ActionText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateAction.Count(c => c.ActionText.Contains(searchTxt));
                    }
                }
                else
                {
                    templateActions = db.TemplateAction.ToList();
                    count = db.TemplateAction.Count();
                }
                var results = new TemplateActionGrid(templateActions, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new TemplateActionGrid
                {
                    Data = new List<TemplateAction>(),
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
        public IQueryable<TemplateAction> FindAll(string requestingUserName)
        {
            return from p in db.TemplateAction select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public TemplateAction FindOne(string requestingUserName, int id)
        {
            return db.TemplateAction.FirstOrDefault(u => u.ActionId == id);
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
            if (db.TemplateAction.Any(a => a.ActionId == keyValue))
            {
                fieldName = "ActionId";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.TemplateAction.Any(a => a.ActionText == dscr))
                {
                    return false;
                }
                fieldName = "ActionText";
                errorMessage = "This description already exists!";
            }
            return true;
        }

        /// <summary>
        /// Add Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="templateAction">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, TemplateAction templateAction)
        {
            try
            {
                templateAction.CreateDateTime = DateTime.Now;
                templateAction.CreateUserName = requestingUserName;
                
                templateAction.ChangeDateTime = DateTime.Now;
                templateAction.ChangeUserName = requestingUserName;
                
                db.TemplateAction.Add(templateAction);
                db.SaveChanges();
                return templateAction.ActionId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in TemplateAction.Add: " + GetExceptionMessage(ex);
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
            //// if (db.TemplateAction_Related_Table.Any(a => a.ActionId == id))
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
            var templateAction = FindOne(requestingUserName, id);
            db.TemplateAction.Remove(templateAction);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="templateAction">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, TemplateAction templateAction)
        {
            templateAction.ChangeDateTime = DateTime.Now;
            templateAction.ChangeUserName = requestingUserName;
            
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="templateAction">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, TemplateAction templateAction)
        {
            var originalTemplateAction = FindOne(requestingUserName, id);
            if (originalTemplateAction == null) 
            {
                return false;
            }

            originalTemplateAction.CategoryId = templateAction.CategoryId;
            originalTemplateAction.ActionText = templateAction.ActionText;
            originalTemplateAction.ActionDscr = templateAction.ActionDscr;
            originalTemplateAction.CompleteInd = templateAction.CompleteInd;
            originalTemplateAction.SortOrder = templateAction.SortOrder;

            originalTemplateAction.ChangeDateTime = DateTime.Now;
            originalTemplateAction.ChangeUserName = requestingUserName;
            
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
