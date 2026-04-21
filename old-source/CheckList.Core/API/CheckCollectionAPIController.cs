//-----------------------------------------------------------------------
// <copyright file="CheckCollectionAPIController.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckSet API Controller
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
    /// CheckSet API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/Collections")]
    public class CheckCollectionsAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckSet Repository
        /// </summary>
        public ICheckSetRepository CheckSetRepo { get; private set; }

        /// <summary>
        /// CheckSets API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckCollectionsAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckSetRepo = new CheckSetRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns> 
        [HttpGet]
        public ActionResult<IQueryable<CheckSet>> List()
        {
            var userName = GetUserName();
            var data = CheckSetRepo.FindAll(userName);
            return Ok(data);
        }
    }

    /// <summary>
    /// CheckSet API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/Collection")]
    public class CheckCollectionAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckSet Repository
        /// </summary>
        public ICheckSetRepository CheckSetRepo { get; private set; }

        /// <summary>
        /// CheckSet API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckCollectionAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckSetRepo = new CheckSetRepository(AppSettingsValues, database);
        }
        #endregion

        /////// <summary>
        /////// Get List of Records for Grid
        /////// </summary>
        /////// <returns>Records</returns> 
        ////[Route("api/CheckSetGrid")]
        ////[HttpPost]
        ////public CheckSetGrid GetGrid()
        ////{
        ////    var request = HttpContext.Current.Request;
        ////    var skip = CIntNull(request["skip"], 0);
        ////    var take = CIntNull(request["take"], 0);
        ////    var searchTxt = CStrNull(request["SearchTxt"]);
        ////    var data = CheckSetRepo.FindGridRecords(GetUserName(), searchTxt, skip, take);
        ////    return data;
        ////}

        /// <summary>
        /// Get One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns> 
        [Route("{id}")]
        [HttpGet]
        public ActionResult<CheckSet> Get(int id)
        {
            var response = new HttpResponseMessage();
            var data = CheckSetRepo.FindOne(GetUserName(), id);
            return Ok(data);
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="checkSet">Record</param>
        /// <returns>Response</returns> 
        [HttpPost]
        public HttpResponseMessage Post([FromBody]CheckSet checkSet)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (checkSet == null)
                {
                    response.Headers.Add("X-Status-Reason", "No data supplied!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (CheckSetRepo.DupCheck(checkSet.SetId, checkSet.SetName, ref fieldName, ref errorMessage))
                {
                    response.Headers.Add("X-Status-Reason", errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var setId = CheckSetRepo.Add(GetUserName(), checkSet);
                if (setId > 0)
                {
                    response = new HttpResponseMessage { Content = new StringContent(string.Format("{0}", setId)) };
                    response.Headers.Location = new Uri(string.Format("/api/CheckSet/{0}", setId), UriKind.Relative);
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
        /// <param name="checkSet">Record</param>
        /// <returns>Response</returns> 
        [HttpPut]
        public HttpResponseMessage Put([FromBody]CheckSet checkSet)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (checkSet != null && CheckSetRepo.Save(GetUserName(), checkSet.SetId, checkSet))
                {
                    response.Content = new StringContent(string.Format("Updated {0}", checkSet.SetId));
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
                var checkSet = CheckSetRepo.FindOne(GetUserName(), id);
                if (checkSet == null)
                {
                    response.Headers.Add("X-Status-Reason", "Record not found!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (CheckSetRepo.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted {0}", checkSet.SetId));
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