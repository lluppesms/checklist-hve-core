//-----------------------------------------------------------------------
// <copyright file="CheckCategoryAPIController.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckCategory API Controller
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
    /// CheckCategory API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/CheckCategorys")]
    public class CheckCategorysAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckCategory Repository
        /// </summary>
        public ICheckCategoryRepository CheckCategoryRepo { get; private set; }

        /// <summary>
        /// CheckCategorys API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckCategorysAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckCategoryRepo = new CheckCategoryRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns> 
        [HttpGet]
        public ActionResult<IQueryable<CheckCategory>> List()
        {
            var userName = GetUserName();
            var data = CheckCategoryRepo.FindAll(userName);
            return Ok(data);
        }
    }

    /// <summary>
    /// CheckCategory API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/CheckCategory")]
    public class CheckCategoryAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckCategory Repository
        /// </summary>
        public ICheckCategoryRepository CheckCategoryRepo { get; private set; }

        /// <summary>
        /// CheckCategory API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckCategoryAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckCategoryRepo = new CheckCategoryRepository(AppSettingsValues, database);
        }
        #endregion

        /////// <summary>
        /////// Get List of Records for Grid
        /////// </summary>
        /////// <returns>Records</returns> 
        ////[Route("api/CheckCategoryGrid")]
        ////[HttpPost]
        ////public CheckCategoryGrid GetGrid()
        ////{
        ////    var request = HttpContext.Current.Request;
        ////    var skip = CIntNull(request["skip"], 0);
        ////    var take = CIntNull(request["take"], 0);
        ////    var searchTxt = CStrNull(request["SearchTxt"]);
        ////    var data = CheckCategoryRepo.FindGridRecords(GetUserName(), searchTxt, skip, take);
        ////    return data;
        ////}

        /// <summary>
        /// Get One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns> 
        [Route("{id}")]
        [HttpGet]
        public ActionResult<CheckCategory> Get(int id)
        {
            var response = new HttpResponseMessage();
            var data = CheckCategoryRepo.FindOne(GetUserName(), id);
            return Ok(data);
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="checkCategory">Record</param>
        /// <returns>Response</returns> 
        [HttpPost]
        public HttpResponseMessage Post([FromBody]CheckCategory checkCategory)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (checkCategory == null)
                {
                    response.Headers.Add("X-Status-Reason", "No data supplied!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (CheckCategoryRepo.DupCheck(checkCategory.CategoryId, checkCategory.CategoryText, ref fieldName, ref errorMessage))
                {
                    response.Headers.Add("X-Status-Reason", errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var categoryId = CheckCategoryRepo.Add(GetUserName(), checkCategory);
                if (categoryId > 0)
                {
                    response = new HttpResponseMessage { Content = new StringContent(string.Format("{0}", categoryId)) };
                    response.Headers.Location = new Uri(string.Format("/api/CheckCategory/{0}", categoryId), UriKind.Relative);
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
        /// <param name="checkCategory">Record</param>
        /// <returns>Response</returns> 
        [HttpPut]
        public HttpResponseMessage Put([FromBody]CheckCategory checkCategory)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (checkCategory != null && CheckCategoryRepo.Save(GetUserName(), checkCategory.CategoryId, checkCategory))
                {
                    response.Content = new StringContent(string.Format("Updated {0}", checkCategory.CategoryId));
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
                var checkCategory = CheckCategoryRepo.FindOne(GetUserName(), id);
                if (checkCategory == null)
                {
                    response.Headers.Add("X-Status-Reason", "Record not found!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (CheckCategoryRepo.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted {0}", checkCategory.CategoryId));
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