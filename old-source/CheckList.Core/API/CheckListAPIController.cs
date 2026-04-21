//-----------------------------------------------------------------------
// <copyright file="CheckListController.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckList API Controller
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
    /// CheckList API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/Lists")]
    public class CheckListsAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckList Repository
        /// </summary>
        public ICheckListRepository CheckListRepo { get; private set; }

        /// <summary>
        /// CheckLists API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckListsAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckListRepo = new CheckListRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns>
        [Route("{id}")]
        [HttpGet]
        public ActionResult<IQueryable<CheckListViewModel>> List(int id)
        {
            var userName = GetUserName();
            var data = CheckListRepo.FindListsForSet(userName, id);
            return Ok(data);
        }
    }

    /// <summary>
    /// CheckList API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/List")]
    public class CheckListAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckList Repository
        /// </summary>
        public ICheckListRepository CheckListRepo { get; private set; }

        /// <summary>
        /// CheckList API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckListAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckListRepo = new CheckListRepository(AppSettingsValues, database);
        }
        #endregion

        /////// <summary>
        /////// Get List of Records for Grid
        /////// </summary>
        /////// <returns>Records</returns> 
        ////[Route("api/CheckListGrid")]
        ////[HttpPost]
        ////public CheckListGrid GetGrid()
        ////{
        ////    var request = HttpContext.Current.Request;
        ////    var skip = CIntNull(request["skip"], 0);
        ////    var take = CIntNull(request["take"], 0);
        ////    var searchTxt = CStrNull(request["SearchTxt"]);
        ////    var data = CheckListRepo.FindGridRecords(GetUserName(), searchTxt, skip, take);
        ////    return data;
        ////}

        /// <summary>
        /// Get One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns> 
        [Route("{id}")]
        [HttpGet]
        public ActionResult<Data.CheckList> Get(int id)
        {
            var response = new HttpResponseMessage();
            var data = CheckListRepo.FindOne(GetUserName(), id);
            return Ok(data);
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="checkList">Record</param>
        /// <returns>Response</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Data.CheckList checkList)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (checkList == null)
                {
                    response.Headers.Add("X-Status-Reason", "No data supplied!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (CheckListRepo.DupCheck(checkList.ListId, checkList.ListName, ref fieldName, ref errorMessage))
                {
                    response.Headers.Add("X-Status-Reason", errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var listId = CheckListRepo.Add(GetUserName(), checkList);
                if (listId > 0)
                {
                    response = new HttpResponseMessage { Content = new StringContent(string.Format("{0}", listId)) };
                    response.Headers.Location = new Uri(string.Format("/api/CheckList/{0}", listId), UriKind.Relative);
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
        /// <param name="checkList">Record</param>
        /// <returns>Response</returns>
        [HttpPut]
        public HttpResponseMessage Put([FromBody]Data.CheckList checkList)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (checkList != null && CheckListRepo.Save(GetUserName(), checkList.ListId, checkList))
                {
                    response.Content = new StringContent(string.Format("Updated {0}", checkList.ListId));
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
                var checkList = CheckListRepo.FindOne(GetUserName(), id);
                if (checkList == null)
                {
                    response.Headers.Add("X-Status-Reason", "Record not found!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (CheckListRepo.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted {0}", checkList.ListId));
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