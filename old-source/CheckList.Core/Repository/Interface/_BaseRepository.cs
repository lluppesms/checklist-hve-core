//-----------------------------------------------------------------------
// <copyright file="_BaseRepository.cs" company="Luppes Consulting, Inc.">
// Copyright 2019, Luppes Consulting, Inc. All rights reserved.
// </copyright>
// <summary>
// Base Repository
// </summary>
//-----------------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace CheckListApp.Data
{
    /// <summary>
    /// Base Repository
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class _BaseRepository
    {
        #region Variables
        /// <summary>
        /// Database Entities
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Easier to use in inherited classes")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Easier to use in inherited classes")]
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once InconsistentNaming
        public ProjectEntities db;

        /// <summary>
        /// Application Settings
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed")]
        protected AppSettings AppSettingsValues;
        #endregion

        #region Configuration Functions
        /// <summary>
        /// reads the web.config file 
        /// </summary>
        /// <param name="settingName">Setting Name</param>
        /// <returns>Value</returns>
        public string GetConnectionStringValue(string settingName)
        {
            try
            {
                if (AppSettingsValues != null)
                {
                    switch (settingName.ToLower())
                    {
                        case "projectentities":
                            return AppSettingsValues.ProjectEntities;
                        default:
                            return AppSettingsValues.DefaultConnection;
                    }
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets value from web.config file AppSettings section.
        /// </summary>
        /// <param name="settingName">Setting Name.</param>
        /// <returns>Return value.</returns>
        public string GetConfigKeyValue(string settingName)
        {
            try
            {
                if (AppSettingsValues != null)
                {
                    switch (settingName.ToLower())
                    {
                        case "emailfrom":
                            return AppSettingsValues.EmailFrom;
                        case "senderrorsto":
                            return AppSettingsValues.SendErrorsTo;
                        case "sendgridserver":
                            return AppSettingsValues.SendGridServer;
                        case "sendgriduserid":
                            return AppSettingsValues.SendGridUserId;
                        case "sendgridpassword":
                            return AppSettingsValues.SendGridPassword;
                        case "allowedfiletypesschema":
                            return AppSettingsValues.AllowedFileTypesSchema;
                        default:
                            return string.Empty;
                    }
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region Logging Functions
        /// <summary>
        /// Write value to log.
        /// </summary>
        /// <param name="msg">Message Text.</param>
        public void WriteToLog(string msg)
        {
            System.Diagnostics.Trace.TraceInformation(msg);
            ////var logDirectory = GetConfigKeyValue("LogDirectory");
            ////var logFileName = GetConfigKeyValue("LogFileName");

            ////StreamWriter myStreamWriter = null;
            ////try
            ////{
            ////  if (logDirectory.Length <= 0)
            ////  {
            ////    logDirectory = @"C:\Logs\";
            ////  }

            ////  if (logFileName.Length <= 0)
            ////  {
            ////    logFileName = "TimeCrunchWeb.log";
            ////  }

            ////  logFileName = DateifyFileName(logDirectory + logFileName);
            ////  if (!Directory.Exists(logDirectory))
            ////  {
            ////    Directory.CreateDirectory(logDirectory);
            ////  }

            ////  if (logFileName == string.Empty)
            ////  {
            ////    return;
            ////  }
            ////  myStreamWriter = File.AppendText(logFileName);
            ////  myStreamWriter.WriteLine("{0}\t{1}", DateTime.Now, msg);
            ////  myStreamWriter.Flush();
            ////}
            ////  // ReSharper disable once EmptyGeneralCatchClause
            ////catch
            ////{
            ////  ////  do nothing with the message ?
            ////}
            ////finally
            ////{
            ////  //// Close the object if it has been created.
            ////  if (myStreamWriter != null)
            ////  {
            ////    myStreamWriter.Close();
            ////  }
            ////}
        }

        /// <summary>
        /// Write value to log.
        /// </summary>
        /// <param name="msg">Message Text.</param>
        public void WriteSevereError(string msg)
        {
            WriteSevereError(string.Empty, msg, string.Empty, string.Empty);
        }

        /// <summary>
        /// Write value to log.
        /// </summary>
        /// <param name="msg">Message Text.</param>
        /// <param name="url">URL for the page with an error.</param>
        /// <param name="userName">User Name.</param>
        public void WriteSevereError(string msg, string url, string userName)
        {
            WriteSevereError(string.Empty, msg, url, userName);
        }

        /// <summary>
        /// Write value to log.
        /// </summary>
        /// <param name="subjectText">Subject of email.</param>
        /// <param name="msg">Message Text.</param>
        /// <param name="url">URL for the page with an error.</param>
        /// <param name="userName">User Name.</param>
        public void WriteSevereError(string subjectText, string msg, string url, string userName)
        {
            System.Diagnostics.Trace.TraceError(msg);
            ////////var returnMessageText = string.Empty;
            ////msg = msg.Trim();
            ////if (msg.StartsWith("Thread was being aborted") || msg.EndsWith("Thread was being aborted") ||
            ////    msg.EndsWith("Thread was being aborted."))
            ////{
            ////  return;
            ////}

            ////// always write to database
            ////try
            ////{
            ////  var logEntry = new ErrorLog
            ////  {
            ////    ErrorMessage = msg,
            ////    CreateDateTime = DateTime.Now,
            ////    CreateUserId = userName
            ////  };
            ////  DB.ErrorLog.Add(logEntry);
            ////  DB.SaveChanges();
            ////}
            ////  // ReSharper disable once EmptyGeneralCatchClause
            ////catch
            ////{
            ////  //  do nothing with the message ?
            ////}

            ////if (url == string.Empty)
            ////{
            ////  url = "Unknown";
            ////}
            //////// write to local text file if on local machine
            ////if (url.IndexOf("localhost", StringComparison.CurrentCultureIgnoreCase) > 0)
            ////{
            ////  WriteToLog(msg);
            ////}
            ////else
            ////{
            ////try
            ////{
            //////// send email to admins if it's not local machine
            ////string errorEmail = GetConfigKeyValue("EmailSendTo");
            ////string siteName = GetConfigKeyValue("ApplicationName");
            ////if (siteName == string.Empty)
            ////{
            ////  siteName = "website";
            ////}
            ////if (subjectText == string.Empty)
            ////{
            ////  subjectText = "An error occured in " + siteName + "!";
            ////}
            ////else
            ////{
            ////  subjectText = subjectText.Replace("@SiteName", siteName);
            ////}
            ////var msgPlus = msg + "<br /><br />" + "\n" +
            ////  "Sent from " + url + "<br />" + "\n" +
            ////  "User: " + userName + "<br />" + "\n" +
            ////  "Machine " + Environment.MachineName + "<br />" + "\n";
            ////SendMail(errorEmail, "HTML", subjectText, msgPlus, ref returnMessageText);
            ////}
            //// ReSharper disable once EmptyGeneralCatchClause
            ////catch 
            ////{
            ////  ////  do nothing with the message ?
            ////}
            ////}
        }

        /// <summary>
        /// Convert file name to format that contains date as numbers.
        /// </summary>
        /// <param name="parmFileName">File Name to be changed.</param>
        /// <returns>File Name.</returns>
        public string DateifyFileName(string parmFileName)
        {
            var newDate = DateTime.Today.ToString("yyyyMMdd");
            var period = parmFileName.IndexOf(".", StringComparison.Ordinal);
            if (period > 0)
            {
                parmFileName = parmFileName.Substring(0, period) + newDate + parmFileName.Substring(period, parmFileName.Length - period);
            }
            else
            {
                parmFileName = parmFileName + newDate;
            }

            return parmFileName;
        }

        /// <summary>
        /// Gets inner exception message.
        /// </summary>
        /// <param name="ex">The Exception.</param>
        /// <returns>The Full Message.</returns>
        public string GetExceptionMessage(Exception ex)
        {
            var message = string.Empty;
            if (ex == null)
            {
                return message;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ex.Message != null)
            {
                ////if (ex.GetType().IsAssignableFrom(typeof(DbEntityValidationException)))
                ////{
                ////    var exv = (DbEntityValidationException)ex;
                ////    // ReSharper disable once LoopCanBeConvertedToQuery
                ////    foreach (var eve in exv.EntityValidationErrors)
                ////    {
                ////        // ReSharper disable once LoopCanBeConvertedToQuery
                ////        foreach (var ve in eve.ValidationErrors)
                ////        {
                ////            message += " " + ve.PropertyName + ": " + ve.ErrorMessage;
                ////        }
                ////    }
                ////}
                ////else
                ////{
                message += ex.Message;
                ////}
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

        #region Nullable Object Helpers
        /// <summary>
        /// Returns a string.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <returns>Return value.</returns>
        public string CStrNull(object o)
        {
            return CStrNull(o, string.Empty);
        }

        /// <summary>
        /// Returns a string.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        public string CStrNull(object o, string dflt)
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
        /// Returns a decimal.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <returns>Return value.</returns>
        public decimal CDecNull(object o)
        {
            return CDecNull(o, 0);
        }

        /// <summary>
        /// Returns a decimal.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        public decimal CDecNull(object o, decimal dflt)
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
        /// Returns an integer.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <returns>Return value.</returns>
        public int CIntNull(object o)
        {
            return CIntNull(o, 0);
        }

        /// <summary>
        /// Returns an integer.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        public int CIntNull(object o, int dflt)
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
        /// Returns a DateTime object.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <returns>Return value.</returns>
        public DateTime CDateNull(object o)
        {
            return CDateNull(o, DateTime.MinValue);
        }

        /// <summary>
        /// Returns a DateTime object.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <param name="dflt">Default value.</param>
        /// <returns>Return value.</returns>
        public DateTime CDateNull(object o, DateTime dflt)
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

        /// <summary>
        /// Returns an Guid.
        /// </summary>
        /// <param name="o">Input parameter.</param>
        /// <returns>Return value.</returns>
        public Guid CGuidNull(object o)
        {
            Guid returnValue;
            try
            {
                if (o != null && !Convert.IsDBNull(o) && Convert.ToString(o) != string.Empty)
                {
                    returnValue = (Guid)o;
                }
                else
                {
                    returnValue = Guid.Empty;
                }
            }
            catch
            {
                return Guid.Empty;
            }

            return returnValue;
        }
        #endregion

        #region Database Stored Proc Functions

        /// <summary>
        /// Execute SQL command and return value
        /// </summary>
        /// <param name="mySQL">SQL</param>
        /// <param name="sqlParams">Parameters</param>
        /// <param name="cmdType">Type of command</param>
        /// <param name="errorMsg">Error Message</param>
        /// <returns>Value</returns>
        public int ExecuteScalar(string mySQL, SqlParameter[] sqlParams, CommandType cmdType, ref string errorMsg)
        {
            SqlConnection myConnection = null;
            int rowsAffected;
            try
            {
                var connectString = GetConnectionStringValue("DefaultConnection");

                errorMsg = string.Empty;
                myConnection = new SqlConnection(connectString);
                var myCommand = new SqlCommand(mySQL, myConnection)
                {
                    CommandType = cmdType
                };
                if (sqlParams != null)
                {
                    AttachParameters(myCommand, sqlParams);
                }
                myConnection.Open();
                var obj = myCommand.ExecuteScalar();
                rowsAffected = CIntNull(obj);
            }
            catch (Exception ex)
            {
                rowsAffected = -1;
                errorMsg = ex.Message;
            }
            finally
            {
                if (myConnection != null)
                {
                    myConnection.Close();
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Execute SQL command and return value
        /// </summary>
        /// <param name="mySQL">SQL</param>
        /// <param name="sqlParams">Parameters</param>
        /// <param name="cmdType">Type of command</param>
        /// <returns>Value</returns>
        public async Task<ValueMessage> ExecuteScalarAsync(string mySQL, SqlParameter[] sqlParams, CommandType cmdType)
        {
            SqlConnection myConnection = null;
            ////var rowsAffected = 0;
            ////var errorMsg = string.Empty;
            var result = new ValueMessage();
            try
            {
                var connectString = GetConnectionStringValue("DefaultConnection");

                ////errorMsg = string.Empty;
                myConnection = new SqlConnection(connectString);
                var myCommand = new SqlCommand(mySQL, myConnection)
                {
                    CommandType = cmdType
                };
                if (sqlParams != null)
                {
                    AttachParameters(myCommand, sqlParams);
                }
                myConnection.Open();
                var obj = await myCommand.ExecuteScalarAsync();
                result.Value = CIntNull(obj);
            }
            catch (Exception ex)
            {
                result.Value = -1;
                result.Message = ex.Message;
            }
            finally
            {
                if (myConnection != null)
                {
                    myConnection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Execute SQL command and return a dataset
        /// </summary>
        /// <param name="mySQL">SQL</param>
        /// <param name="sqlParams">Parameters</param>
        /// <param name="cmdType">Type of command</param>
        /// <param name="errorMsg">Error Message</param>
        /// <returns>Dataset</returns>
        public DataSet ExecuteDataset(string mySQL, SqlParameter[] sqlParams, CommandType cmdType, ref string errorMsg)
        {
            SqlConnection myConnection = null;
            // ReSharper disable once RedundantAssignment
            var myDataSet = new DataSet();
            SqlDataAdapter myAdapter = null;

            try
            {
                var connectString = GetConnectionStringValue("DefaultConnection");

                errorMsg = string.Empty;
                myConnection = new SqlConnection(connectString);
                var myCommand = new SqlCommand(mySQL, myConnection)
                {
                    CommandType = cmdType
                };

                if (sqlParams != null)
                {
                    AttachParameters(myCommand, sqlParams);
                }
                myDataSet = new DataSet();
                myAdapter = new SqlDataAdapter(myCommand);
                myAdapter.Fill(myDataSet);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                // ReSharper disable once RedundantAssignment
                myDataSet = null;
                throw;
            }
            finally
            {
                if (myAdapter != null)
                {
                    myAdapter.Dispose();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }
            }
            return myDataSet;
        }

        /// <summary>
        /// Attach parameters to command
        /// </summary>
        /// <param name="command">Command object</param>
        /// <param name="commandParameters">Parameters</param>
        public void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            command.Parameters.Clear();
            foreach (var p in commandParameters)
            {
                ////check for derived output value with no value assigned
                switch (p.DbType)
                {
                    case DbType.Date:
                    case DbType.DateTime:
                    case DbType.DateTime2:
                        if (p.Value != null && (DateTime)p.Value == DateTime.MinValue)
                        {
                            p.Value = null;
                        }
                        break;
                    case DbType.Decimal:
                        if (CDecNull(p.Value, decimal.MinValue) == decimal.MinValue)
                        {
                            p.Value = null;
                        }
                        break;
                    case DbType.Int16:
                        if (CIntNull(p.Value, short.MinValue) == short.MinValue)
                        {
                            p.Value = null;
                        }
                        break;
                    case DbType.Int32:
                        if (CIntNull(p.Value, int.MinValue) == int.MinValue)
                        {
                            p.Value = null;
                        }
                        break;
                    case DbType.Int64:
                        if (CIntNull(p.Value, int.MinValue) == int.MinValue)
                        {
                            p.Value = null;
                        }
                        break;
                    default:
                        if (p.Direction == ParameterDirection.InputOutput & p.Value == null)
                        {
                            p.Value = null;
                        }
                        if (p.Direction == ParameterDirection.Output & p.Value == null &
                            (p.DbType == DbType.String || p.DbType == DbType.AnsiString))
                        {
                            p.Value = string.Empty;
                        }
                        break;
                }
                command.Parameters.Add(p);
            }
        }

        /// <summary>
        /// Create string from parameter values for logging
        /// </summary>
        /// <param name="sqlParams">SQL Parameters</param>
        /// <returns>String</returns>
        public string ParamValuesString(SqlParameter[] sqlParams)
        {
            var tmpString = " ";
            var sb = new StringBuilder();
            try
            {
                if (sqlParams != null)
                {
                    int i;
                    for (i = 0; i <= sqlParams.GetLength(0) - 1; i++)
                    {
                        if (sqlParams[i].Direction == ParameterDirection.Output)
                        {
                            sb.Append(sqlParams[i] + "=@" + sqlParams[i] + " OUTPUT, ");
                        }
                        else
                        {
                            if (sqlParams[i].Value == null)
                            {
                                sb.Append(sqlParams[i] + "=NULL, ");
                            }
                            else
                            {
                                switch (sqlParams[i].SqlDbType)
                                {
                                    case SqlDbType.Date:
                                    case SqlDbType.DateTime:
                                    case SqlDbType.DateTime2:
                                        if (sqlParams[i].Value == null)
                                        {
                                            sb.Append(sqlParams[i] + "=NULL, ");
                                        }
                                        else if ((DateTime)sqlParams[i].Value == DateTime.MinValue)
                                        {
                                            sb.Append(sqlParams[i] + "=NULL, ");
                                        }
                                        else
                                        {
                                            sb.Append(sqlParams[i] + "='" +
                                                      ((DateTime)sqlParams[i].Value).ToString("MM-dd-yyyy HH:mm:ss:fff") + "'" + ", ");
                                        }
                                        break;
                                    case SqlDbType.Char:
                                        break;
                                    case SqlDbType.VarChar:
                                        sb.Append(sqlParams[i] + "=" + "'" + sqlParams[i].Value + "'" + ", ");
                                        break;
                                    default:
                                        sb.Append(sqlParams[i] + "=" + sqlParams[i].Value + ", ");
                                        break;
                                }
                            }
                        }
                    }
                    tmpString = sb.ToString();
                    tmpString = tmpString.Substring(0, tmpString.Length - 2);
                }
            }
            catch (Exception objError)
            {
                var message = "ParamValuesString: An error occurred: " + objError.Message;
                System.Diagnostics.Trace.TraceError(message);
                tmpString = string.Empty;
            }
            return tmpString;
        }
        #endregion
    }
}