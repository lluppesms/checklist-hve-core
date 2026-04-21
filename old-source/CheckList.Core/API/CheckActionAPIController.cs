//-----------------------------------------------------------------------
// <copyright file="CheckActionAPIController.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// CheckAction API Controller
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
    /// CheckAction API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/Actions")]
    public class CheckActionsAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckAction Repository
        /// </summary>
        public ICheckActionRepository CheckActionRepo { get; private set; }

        /// <summary>
        /// CheckActions API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckActionsAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckActionRepo = new CheckActionRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Get List of Actions
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Records</returns>
        [Route("{id}")]
        [HttpGet]
        public ActionResult<IQueryable<CheckActionViewModel>> List(int id)
        {
            var userName = GetUserName();
            var data = CheckActionRepo.FindActionsForList(userName, id);
            return Ok(data);
        }
    }

    /// <summary>
    /// CheckAction API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/Action")]
    public class CheckActionAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckAction Repository
        /// </summary>
        public ICheckActionRepository CheckActionRepo { get; private set; }

        /// <summary>
        /// CheckAction API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckActionAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckActionRepo = new CheckActionRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Get One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns>
        [Route("{id}")]
        [HttpGet]
        public ActionResult<CheckActionEx> Get(int id)
        {
            var response = new HttpResponseMessage();
            var data = CheckActionRepo.FindOneEx(GetUserName(), id);
            return Ok(data);
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="checkAction">Record</param>
        /// <returns>Response</returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]CheckActionEx checkAction)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (checkAction == null)
                {
                    response.Headers.Add("X-Status-Reason", "No data supplied!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                checkAction.CompleteInd = "N";
                checkAction.SortOrder = 50;
                var actionId = CheckActionRepo.Add(GetUserName(), checkAction);
                if (actionId > 0)
                {
                    response = new HttpResponseMessage { Content = new StringContent(string.Format("{0}", actionId)) };
                    response.Headers.Location = new Uri(string.Format("/api/CheckAction/{0}", actionId), UriKind.Relative);
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
        /// <param name="checkAction">Record</param>
        /// <returns>Response</returns>
        [HttpPut]
        public HttpResponseMessage Put([FromBody]CheckActionEx checkAction)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (checkAction != null && CheckActionRepo.Save(GetUserName(), checkAction))
                {
                    response.Content = new StringContent(string.Format("Updated {0}", checkAction.ActionId));
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
                var checkAction = CheckActionRepo.FindOne(GetUserName(), id);
                if (checkAction == null)
                {
                    response.Headers.Add("X-Status-Reason", "Record not found!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (CheckActionRepo.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted {0}", checkAction.ActionId));
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

    /// <summary>
    /// CheckAction API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/ActionComplete")]
    public class CheckActionCompleteAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// CheckAction Repository
        /// </summary>
        public ICheckActionRepository CheckActionRepo { get; private set; }

        /// <summary>
        /// CheckAction Complete API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public CheckActionCompleteAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            CheckActionRepo = new CheckActionRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Update One Record
        /// </summary>
        /// <param name="model">Completion model</param>
        /// <returns>success</returns>
        [Route("{id}")]
        [HttpPut]
        public ActionResult<CheckAction> SetComplete([FromBody]CompletionViewModel model)
        {
            var response = new HttpResponseMessage();
            var data = CheckActionRepo.SetCompletedFlag(GetUserName(), model.ActionId, model.IsComplete);
            return Ok(data);
        }
    }
}