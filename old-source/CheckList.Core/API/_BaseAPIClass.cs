//-----------------------------------------------------------------------
// <copyright file="_BaseAPIClass.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Base API Controller
// </summary>
//-----------------------------------------------------------------------

using CheckListApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CheckListApp.API
{
    /// <summary>
    /// Base API Controller
    /// </summary>
    public class _BaseController : Controller
    {
        #region Initialization
        /// <summary>
        /// Access Application Settings 
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Easier to use in inherited classes")]
        protected AppSettings AppSettingsValues;

        /// <summary>
        /// Access the HTTPContext for this request
        /// </summary>
        protected IHttpContextAccessor context;

        /// <summary>
        /// Access the HTTPContext for this request
        /// </summary>
        protected ProjectEntities database;

        /// <summary>
        /// Base Controller Instantiation
        /// </summary>
        public _BaseController()
        {
        }
        #endregion

        #region Auth Helpers
        /// <summary>
        /// Returns User Name if logged on, UNKNOWN if not
        /// </summary>
        /// <returns>User Name</returns>
        protected string GetUserName()
        {
            if (context != null)
            {
                var currentUser = context.HttpContext.User;
                if (currentUser != null && currentUser.Identity != null && currentUser.Identity.Name != null)
                {
                    var userName = currentUser.Identity.Name.ToLower();
                    var delimiter = userName.IndexOf("#");
                    if (delimiter > 0)
                    {
                        userName = userName.Substring(delimiter + 1, userName.Length - delimiter - 1);
                    }
                    //// these are all pseudonyms for when I log in at work... :)
                    if (userName == "lyle.luppes@gmail.com" || userName == "a5bfhzz@mmm.com" || userName == "lluppes@mmm.com" || userName == "lluppes@3m.com")
                    {
                        userName = "lyle@luppes.com";
                    }
                    return userName;
                }
                //// This is for JWT Tokens
                if (currentUser.Claims.Count() > 0)
                {
                    var userName = currentUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
                    return userName;
                }
                return "UNKNOWN";
            }
            return "lyle@luppes.com";
        }

        /// <summary>
        /// Returns UserId if logged on, empty if not
        /// </summary>
        /// <returns>UserId</returns>
        protected string GetUserId()
        {
            var currentUser = context.HttpContext.User;
            if (currentUser != null)
            {
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                return userId;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns true if user is in Admin Role, false if not or not logged in
        /// </summary>
        /// <returns>Is Admin</returns>
        protected bool IsAdmin()
        {
            var currentUser = context.HttpContext.User;
            if (currentUser != null)
            {
                var isAdmin = currentUser.IsInRole("Admin");
                if (!isAdmin)
                {
                    isAdmin = currentUser.HasClaim("groups", AppSettingsValues.AdminGroupId);
                }
                if (!isAdmin)
                {
                    isAdmin = context.HttpContext.User.Identity.Name.ToLower().Contains("lyle@luppes.com");
                }
                return isAdmin;
            }
            return false;
        }
        #endregion

        #region Web Page Helpers
        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        protected bool IsAjaxRequest()
        {
            if (context?.HttpContext?.Request?.Headers != null)
            {
                return context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
            return false;
        }
        #endregion

        #region Messages
        /// <summary>
        /// Write Info Message
        /// </summary>
        /// <param name="msg">Message</param>
        protected void WriteToLog(string msg)
        {
            Trace.WriteLine(msg);
        }

        /// <summary>
        /// Write Error Message
        /// </summary>
        /// <param name="msg">Message</param>
        protected void WriteSevereError(string msg)
        {
            Trace.WriteLine(msg);
        }

        /// <summary>
        /// Build one string from multiple nested error messages for logging
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>All the messages together</returns>
        protected string GetExceptionMessage(Exception ex)
        {
            var message = string.Empty;
            if (ex == null)
            {
                return message;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ex.Message != null)
            {
                message += ex.Message;
            }

            if (ex.InnerException == null)
            {
                return message;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ex.InnerException.Message != null)
            {
                message += " " + ex.InnerException.Message;
            }

            if (ex.InnerException.InnerException == null)
            {
                return message;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ex.InnerException.InnerException.Message != null)
            {
                message += " " + ex.InnerException.InnerException.Message;
            }

            if (ex.InnerException.InnerException.InnerException == null)
            {
                return message;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ex.InnerException.InnerException.InnerException.Message != null)
            {
                message += " " + ex.InnerException.InnerException.InnerException.Message;
            }

            return message;
        }
        #endregion

        #region Null Object Helpers
        /// <summary>
        /// Convert object to a valid string even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Value</returns>
        protected string CStrNull(object o)
        {
            return CStrNull(o, string.Empty);
        }

        /// <summary>
        /// Convert object to a valid string even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="dflt">Default Value</param>
        /// <returns>Value</returns>
        protected string CStrNull(object o, string dflt)
        {
            string returnValue;
            try
            {
                if (o != null && !Convert.IsDBNull(o))
                {
                    returnValue = o.ToString();
                }
                else
                {
                    returnValue = dflt;
                }
            }
            catch
            {
                returnValue = null;
            }
            return returnValue;
        }

        /// <summary>
        /// Convert object to a valid decimal even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Value</returns>
        protected decimal CDecNull(object o)
        {
            return CDecNull(o, 0);
        }

        /// <summary>
        /// Convert object to a valid decimal even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="dflt">Default Value</param>
        /// <returns>Value</returns>
        protected decimal CDecNull(object o, decimal dflt)
        {
            decimal returnValue;
            try
            {
                if (o != null && !Convert.IsDBNull(o))
                {
                    returnValue = Convert.ToDecimal(o);
                }
                else
                {
                    returnValue = dflt;
                }
            }
            catch
            {
                return decimal.MinValue;
            }
            return returnValue;
        }

        /// <summary>
        /// Convert object to a valid integer even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Value</returns>
        protected int CIntNull(object o)
        {
            return CIntNull(o, 0);
        }

        /// <summary>
        /// Convert object to a valid integer even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="dflt">Default Value</param>
        /// <returns>Value</returns>
        protected int CIntNull(object o, int dflt)
        {
            int returnValue;
            try
            {
                if (o != null && !Convert.IsDBNull(o) && Convert.ToString(o) != string.Empty)
                {
                    returnValue = Convert.ToInt32(o);
                }
                else
                {
                    returnValue = dflt;
                }
            }
            catch
            {
                return int.MinValue;
            }
            return returnValue;
        }

        /// <summary>
        /// Convert object to a valid date time even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <returns>Value</returns>
        protected DateTime CDateNull(object o)
        {
            return CDateNull(o, DateTime.MinValue);
        }

        /// <summary>
        /// Convert object to a valid date time even if null
        /// </summary>
        /// <param name="o">Object</param>
        /// <param name="dflt">Default Value</param>
        /// <returns>Value</returns>
        protected DateTime CDateNull(object o, DateTime dflt)
        {
            DateTime returnValue;
            try
            {
                if (o != null && !Convert.IsDBNull(o))
                {
                    returnValue = Convert.ToDateTime(o);
                }
                else
                {
                    returnValue = dflt;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
            return returnValue;
        }
        #endregion

        #region MVC Helpers
        /// <summary>
        /// Gets a list of errors in the model state and combines them into one simple string.
        /// Very useful for debugging if you get an error in your model, but you are not displaying the field on the screen so you don't know what it is.
        /// </summary>
        /// <param name="ms">Model State.</param>
        /// <returns>List of errors in one string.</returns>
        protected string ShowAllModelStateErrors(ModelStateDictionary ms)
        {
            //// if you have errors you can't find, set a breakpoint after the ModelState.IsValid and run this command in the Immediate Window:
            ////    ? ShowAllModelStateErrors(ModelState)
            //// OR -- if you want to use this more often, you can put it right into your controller & view
            ////   put code like this in your Controller
            ////      if (ModelState.IsValid)
            ////        {... do stuff here...}
            ////      else
            ////        ViewBag.allErrors = ShowAllModelStateErrors(ModelState);
            ////   put this somewhere in your view 
            ////      @{ if (ViewBag.allErrors != null) { <!-- Errors: @ViewBag.allErrors  --> } }
            ////   now when you hit the screen again, those errors will be visible
            var errors = ms
              .Where(x => x.Value.Errors.Count > 0)
              .Select(x => new
              {
                  x.Key,
                  x.Value.Errors
              })
              .ToArray();
            var allErrors = string.Empty;
            foreach (var err in errors)
            {
                allErrors += err.Key + ": ";
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var e2 in err.Errors)
                {
                    allErrors += e2.ErrorMessage + "; ";
                }
            }

            return allErrors;
        }

        /// <summary>
        /// Removes auto-populated audit fields from validation
        /// </summary>
        protected void RemoveAuditFieldsFromValidation()
        {
            RemoveAuditFieldsFromValidation(new[] { "CreateUserName", "CreateDateTime", "ChangeUserName", "ChangeDateTime" });
        }

        /// <summary>
        /// Removes auto-populated audit fields from validation
        /// </summary>
        /// <param name="fieldsToRemove">List of fields to remove from ModelState</param>
        protected void RemoveAuditFieldsFromValidation(string[] fieldsToRemove)
        {
            if (fieldsToRemove == null)
            {
                return;
            }
            foreach (var s in fieldsToRemove)
            {
                ModelState.Remove(s);
            }
        }
        #endregion

        #region Standard Lookup Tables
        /// <summary>
        /// Get Ranged Number List
        /// </summary>
        /// <param name="minValue">Min value</param>
        /// <param name="maxValue">Max value</param>
        /// <returns>List</returns>
        protected List<StaticNumberTable> GetRangedNumberList(int minValue, int maxValue)
        {
            return
              (from r in Enumerable.Range(minValue, maxValue)
               select new StaticNumberTable(r, r)).ToList();
        }

        /// <summary>
        /// Get Number 1-99 List
        /// </summary>
        /// <returns>List</returns>
        protected List<StaticNumberTable> GetSortOrderList()
        {
            return (from r in Enumerable.Range(1, 99)
                    select new StaticNumberTable(r, r)
                   ).ToList();
        }

        /// <summary>
        /// Get Yes/No List
        /// </summary>
        /// <returns>List</returns>
        protected List<StaticCodeTable> GetYesNoList()
        {
            return new List<StaticCodeTable> { new StaticCodeTable("Y", "Yes"), new StaticCodeTable("N", "No") };
        }
        #endregion

        #region Serialization Helpers
        /// <summary>
        /// Serializes an object to an XML stream
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>String</returns>
        protected string SerializeObjectToXMLString(object obj)
        {
            var xs = new XmlSerializer(obj.GetType());
            var memoryStream = new MemoryStream();
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = System.Xml.Formatting.Indented }; ////XmlTextWriter - fast, non-cached, forward-only way of generating streams or files containing XML data
            xs.Serialize(xmlTextWriter, obj); ////Serialize emp in the xmlTextWriter
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream; ////Get the BaseStream of the xmlTextWriter in the Memory Stream6
            return UTF8ByteArrayToString(memoryStream.ToArray()); ////Convert to array
        }

        /// <summary>
        /// Serializes an object to an XML stream
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>String</returns>
        protected byte[] SerializeObjectToXMLByteArray(object obj)
        {
            return StringToUTF8ByteArray(SerializeObjectToXMLString(obj)); // Convert to Byte Array
        }

        /// <summary>
        /// Serialize object to JSON string
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>String</returns>
        protected string SerializeObjectToJsonString(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            return json;
        }

        /// <summary>
        /// Serialize object to JSON Byte Array
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Byte Array</returns>
        protected byte[] SerializeObjectToJsonByteArray(object obj)
        {
            return Encoding.UTF8.GetBytes(SerializeObjectToJsonString(obj));
        }

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        protected static string UTF8ByteArrayToString(byte[] characters)
        {
            var encoding = new UTF8Encoding();
            var constructedString = encoding.GetString(characters);
            return constructedString;
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="stringToConvert">String to Convert</param>
        /// <returns>Byte Array</returns>
        protected byte[] StringToUTF8ByteArray(string stringToConvert)
        {
            var encoding = new UTF8Encoding();
            var byteArray = encoding.GetBytes(stringToConvert);
            return byteArray;
        }
        #endregion
    }
}
