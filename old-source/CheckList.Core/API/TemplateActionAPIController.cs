//-----------------------------------------------------------------------
// <copyright file="TemplateActionAPIController.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// TemplateAction API Controller
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
    /// TemplateAction API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/TemplateActions")]
    public class TemplateActionsAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// TemplateAction Repository
        /// </summary>
        public ITemplateActionRepository TemplateActionRepo { get; private set; }

        /// <summary>
        /// TemplateActions API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public TemplateActionsAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            TemplateActionRepo = new TemplateActionRepository(AppSettingsValues, database);
        }
        #endregion

        /// <summary>
        /// Get List of Records
        /// </summary>
        /// <returns>Records</returns> 
        [HttpGet]
        public ActionResult<IQueryable<TemplateAction>> List()
        {
            var userName = GetUserName();
            var data = TemplateActionRepo.FindAll(userName);
            return Ok(data);
        }
    }

    /// <summary>
    /// TemplateAction API Controller
    /// </summary>
    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/TemplateAction")]
    public class TemplateActionAPIController : _BaseController
    {
        #region Initialization
        /// <summary>
        /// TemplateAction Repository
        /// </summary>
        public ITemplateActionRepository TemplateActionRepo { get; private set; }

        /// <summary>
        /// TemplateAction API Controller
        /// </summary>
        /// <param name="settingsAccessor">System Settings</param>
        /// <param name="contextAccessor">System Context</param>
        /// <param name="dbContext">Database Context</param>
        public TemplateActionAPIController(IOptions<AppSettings> settingsAccessor, IHttpContextAccessor contextAccessor, ProjectEntities dbContext)
        {
            database = dbContext;
            context = contextAccessor;
            AppSettingsValues = settingsAccessor.Value;
            AppSettingsValues.UserName = GetUserName();
            TemplateActionRepo = new TemplateActionRepository(AppSettingsValues, database);
        }
        #endregion

        /////// <summary>
        /////// Get List of Records for Grid
        /////// </summary>
        /////// <returns>Records</returns> 
        ////[Route("api/TemplateActionGrid")]
        ////[HttpPost]
        ////public TemplateActionGrid GetGrid()
        ////{
        ////    var request = HttpContext.Current.Request;
        ////    var skip = CIntNull(request["skip"], 0);
        ////    var take = CIntNull(request["take"], 0);
        ////    var searchTxt = CStrNull(request["SearchTxt"]);
        ////    var data = TemplateActionRepo.FindGridRecords(GetUserName(), searchTxt, skip, take);
        ////    return data;
        ////}

        /// <summary>
        /// Get One Record
        /// </summary>
        /// <param name="id">Key</param>
        /// <returns>Record</returns> 
        [Route("{id}")]
        [HttpGet]
        public ActionResult<TemplateAction> Get(int id)
        {
            var response = new HttpResponseMessage();
            var data = TemplateActionRepo.FindOne(GetUserName(), id);
            return Ok(data);
        }

        /// <summary>
        /// Post (Add) One Record
        /// </summary>
        /// <param name="templateAction">Record</param>
        /// <returns>Response</returns> 
        [HttpPost]
        public HttpResponseMessage Post([FromBody]TemplateAction templateAction)
        {
            var response = new HttpResponseMessage();
            var fieldName = string.Empty;
            var errorMessage = string.Empty;
            try
            {
                if (templateAction == null)
                {
                    response.Headers.Add("X-Status-Reason", "No data supplied!");
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                if (TemplateActionRepo.DupCheck(templateAction.ActionId, templateAction.ActionText, ref fieldName, ref errorMessage))
                {
                    response.Headers.Add("X-Status-Reason", errorMessage);
                    response.StatusCode = HttpStatusCode.BadRequest;
                    return response;
                }
                var actionId = TemplateActionRepo.Add(GetUserName(), templateAction);
                if (actionId > 0)
                {
                    response = new HttpResponseMessage { Content = new StringContent(string.Format("{0}", actionId)) };
                    response.Headers.Location = new Uri(string.Format("/api/TemplateAction/{0}", actionId), UriKind.Relative);
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
        /// <param name="templateAction">Record</param>
        /// <returns>Response</returns> 
        [HttpPut]
        public HttpResponseMessage Put([FromBody]TemplateAction templateAction)
        {
            var response = new HttpResponseMessage();
            var errorMessage = string.Empty;
            try
            {
                if (templateAction != null && TemplateActionRepo.Save(GetUserName(), templateAction.ActionId, templateAction))
                {
                    response.Content = new StringContent(string.Format("Updated {0}", templateAction.ActionId));
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
                var templateAction = TemplateActionRepo.FindOne(GetUserName(), id);
                if (templateAction == null)
                {
                    response.Headers.Add("X-Status-Reason", "Record not found!");
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    if (TemplateActionRepo.Delete(GetUserName(), id))
                    {
                        response.Content = new StringContent(string.Format("Deleted {0}", templateAction.ActionId));
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