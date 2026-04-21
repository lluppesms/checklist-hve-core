//-----------------------------------------------------------------------
// <copyright file="TemplateCategoryAPIController.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateCategory API Controller
// </summary>
//-----------------------------------------------------------------------

using CheckListApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace CheckListApp.API
{
    /// <summary>
    /// TemplateCategory API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/TemplateCategorys")]
    public class TemplateCategorysAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// TemplateCategory Repository
        /// </summary>
        public ITemplateCategoryRepository TemplateCategoryRepo { get; private set; }

        /// <summary>
        /// TemplateCategorys API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public TemplateCategorysAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            TemplateCategoryRepo = new TemplateCategoryRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns> 
        [HttpGet]
        public ActionResult<IQueryable<TemplateCategory>> List()
        {
            var userName = GetUserName();
            var data = TemplateCategoryRepo.FindAll(userName);
            return Ok(data);
        }
    }

    /// <summary>
    /// TemplateCategory API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/TemplateCategory")]
    public class TemplateCategoryAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// TemplateCategory Repository
        /// </summary>
        public ITemplateCategoryRepository TemplateCategoryRepo { get; private set; }

        /// <summary>
        /// TemplateCategory API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public TemplateCategoryAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            TemplateCategoryRepo = new TemplateCategoryRepository(AppSettingsValues, database);
        }
        #endregion

        /////// <summary>
        /////// Get List of Records for Grid
        /////// </summary>
        /////// <returns>Records</returns> 
        ////[Route("api/TemplateCategoryGrid")]
        ////[HttpPost]
        ////public TemplateCategoryGrid GetGrid()
        ////{
        ////    var request = HttpContext.Current.Request;
        ////    var skip = CIntNull(request["skip"], 0);
        ////    var take = CIntNull(request["take"], 0);
        ////    var searchTxt = CStrNull(request["SearchTxt"]);
        ////    var data = TemplateCategoryRepo.FindGridRecords(GetUserName(), searchTxt, skip, take);
        ////    return data;
        ////}

        /// <summary>
        /// Get One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns> 
        [Route("{id}")]
        [HttpGet]
        public ActionResult<TemplateCategory> Get(int id)
        {
            var response = new HttpResponseMessage();
            var data = TemplateCategoryRepo.FindOne(GetUserName(), id);
            return Ok(data);
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="templateCategory">Record</param>
        /// <returns>Response</returns> 
        [HttpPost]
        public HttpResponseMessage Post([FromBody]TemplateCategory templateCategory)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (templateCategory == null)
                {
                    response.Headers.Add("X-Status-Reason", "No data supplied!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (TemplateCategoryRepo.DupCheck(templateCategory.CategoryId, templateCategory.CategoryText, ref fieldName, ref errorMessage))
                {
                    response.Headers.Add("X-Status-Reason", errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var categoryId = TemplateCategoryRepo.Add(GetUserName(), templateCategory);
                if (categoryId > 0)
                {
                    response = new HttpResponseMessage { Content = new StringContent(string.Format("{0}", categoryId)) };
                    response.Headers.Location = new Uri(string.Format("/api/TemplateCategory/{0}", categoryId), UriKind.Relative);
                    response.StatusCode = HttpStatusCode.Created;
                }
                else
                {
                    response.Headers.Add("X-Status-Reason", "Insert failed!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                errorMessage = GetExceptionMessage(ex);
                response.Headers.Add("X-Status-Reason", errorMessage);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        /// <summary>
        /// Put (Update) One Record
        /// </summary>
        /// <param name="templateCategory">Record</param>
        /// <returns>Response</returns> 
        [HttpPut]
        public HttpResponseMessage Put([FromBody]TemplateCategory templateCategory)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (templateCategory != null && TemplateCategoryRepo.Save(GetUserName(), templateCategory.CategoryId, templateCategory))
                {
                    response.Content = new StringContent(string.Format("Updated {0}", templateCategory.CategoryId));
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Headers.Add("X-Status-Reason", "Update Failed!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                errorMessage = GetExceptionMessage(ex);
                response.Headers.Add("X-Status-Reason", errorMessage);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }

        /// <summary>
        /// Delete One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Response</returns> 
        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                var templateCategory = TemplateCategoryRepo.FindOne(GetUserName(), id);
                if (templateCategory == null)
                {
                    response.Headers.Add("X-Status-Reason", "Record not found!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (TemplateCategoryRepo.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted {0}", templateCategory.CategoryId));
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Headers.Add("X-Status-Reason", "Delete Failed!");
                        response.StatusCode = HttpStatusCode.BadRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = GetExceptionMessage(ex);
                response.Headers.Add("X-Status-Reason", errorMessage);
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            return response;
        }
    }
}