//-----------------------------------------------------------------------
// <copyright file="TemplateCategoryRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateCategory Repository
// </summary>
//-----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckListApp.Data
{
    /// <summary>
    /// TemplateCategory Repository
    /// </summary>
    public class TemplateCategoryRepository : _BaseRepository, ITemplateCategoryRepository
    {
        #region Initialization
        /// <summary>
        /// Default initialization of this repository class.
        /// </summary>
        /// <param name="appSettings">Application Settings</param>
        /// <param name="context">Database Context</param>
        public TemplateCategoryRepository(AppSettings appSettings, DbContext context)
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
        public TemplateCategoryGrid FindGridRecords(string requestingUserName, string searchTxt, int skipNbr, int takeNbr)
        {
            try
            {
                List<TemplateCategory> templateCategories;
                var count = 0;
                if (takeNbr > 0)
                {
                    if (string.IsNullOrEmpty(searchTxt))
                    {
                        templateCategories = db.TemplateCategory
                          .OrderBy(c => c.CategoryText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateCategory.Count();
                    }
                    else
                    {
                        templateCategories = db.TemplateCategory
                          .Where(c => c.CategoryText.Contains(searchTxt))
                          .OrderBy(c => c.CategoryText)
                          .Skip(skipNbr)
                          .Take(takeNbr)
                          .ToList();
                        count = db.TemplateCategory.Count(c => c.CategoryText.Contains(searchTxt));
                    }
                }
                else
                {
                    templateCategories = db.TemplateCategory.ToList();
                    count = db.TemplateCategory.Count();
                }
                var results = new TemplateCategoryGrid(templateCategories, count);
                return results;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = GetExceptionMessage(ex);
                WriteSevereError(returnMessageTxt);
                var results = new TemplateCategoryGrid
                {
                    Data = new List<TemplateCategory>(),
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
        public IQueryable<TemplateCategory> FindAll(string requestingUserName)
        {
            return from p in db.TemplateCategory select p;
        }

        /// <summary>
        /// Find One Records
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">id</param>
        /// <returns>Records</returns>
        public TemplateCategory FindOne(string requestingUserName, int id)
        {
            return db.TemplateCategory.FirstOrDefault(u => u.CategoryId == id);
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
            if (db.TemplateCategory.Any(a => a.CategoryId == keyValue))
            {
                fieldName = "CategoryId";
                errorMessage = "This value already exists!";
            }
            else
            {
                if (!db.TemplateCategory.Any(a => a.CategoryText == dscr))
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
        /// <param name="templateCategory">Object</param>
        /// <returns>Success</returns>
        public int Add(string requestingUserName, TemplateCategory templateCategory)
        {
            try
            {
                templateCategory.CreateDateTime = DateTime.Now;
                templateCategory.CreateUserName = requestingUserName;
                
                templateCategory.ChangeDateTime = DateTime.Now;
                templateCategory.ChangeUserName = requestingUserName;
                
                db.TemplateCategory.Add(templateCategory);
                db.SaveChanges();
                return templateCategory.CategoryId;
            }
            catch (Exception ex)
            {
                var returnMessageTxt = "Error in TemplateCategory.Add: " + GetExceptionMessage(ex);
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
            //// if (db.TemplateCategory_Related_Table.Any(a => a.CategoryId == id))
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
            var templateCategory = FindOne(requestingUserName, id);
            db.TemplateCategory.Remove(templateCategory);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="templateCategory">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, TemplateCategory templateCategory)
        {
            templateCategory.ChangeDateTime = DateTime.Now;
            templateCategory.ChangeUserName = requestingUserName;
            
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Save Record
        /// </summary>
        /// <param name="requestingUserName">Requesting UserName</param>
        /// <param name="id">Record Key</param>
        /// <param name="templateCategory">Object</param>
        /// <returns>Success</returns>
        public bool Save(string requestingUserName, int id, TemplateCategory templateCategory)
        {
            var originalTemplateCategory = FindOne(requestingUserName, id);
            if (originalTemplateCategory == null) 
            {
                return false;
            }

            originalTemplateCategory.ListId = templateCategory.ListId;
            originalTemplateCategory.CategoryText = templateCategory.CategoryText;
            originalTemplateCategory.CategoryDscr = templateCategory.CategoryDscr;
            originalTemplateCategory.ActiveInd = templateCategory.ActiveInd;
            originalTemplateCategory.SortOrder = templateCategory.SortOrder;

            originalTemplateCategory.ChangeDateTime = DateTime.Now;
            originalTemplateCategory.ChangeUserName = requestingUserName;
            
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
