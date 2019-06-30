using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using TestAPI.Models;
using TestAPI.Models.Utility;

using SqlKata;
using SqlKata.Execution;
using MySql.Data;
using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using Newtonsoft.Json;
using static TestAPI.Models.Utility.Constants;

namespace TestAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // Mark: This method returns the success response block
        protected object ApiSuccess(object result, string responseMethod, string message, HttpStatusCode responseCode)
        {
            ResponseModel response = new ResponseModel(
                message,
                responseCode,
                responseMethod,
                result);
            Console.WriteLine(response);
            return response;
        }

        // Mark: This method returns the failure response block
        protected object ApiFailure(object result, string message, HttpStatusCode responseCode, string methodType)
        {
            ResponseModel response = new ResponseModel(
                message,
                responseCode,
                methodType,
                (string)result);
            return response;
        }

        [Route("api")]
        public async Task<object> Post()
        {

            
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            //string ServiceURL = ConfigurationManager.AppSettings[MPAPIConstants.DataServiceKey];
            string tokenHeader = HttpContext.Current.Request.Headers[Constants.TokenKey];
            string requestMethod = HttpContext.Current.Request.Form[Constants.RequestMethodKey];
            string requestData = HttpContext.Current.Request.Form[Constants.RequestDataKey];
            string requestDateTime = HttpContext.Current.Request.Form[Constants.RequestDateTimeKey];

            if (IsMaliciousRequest(requestData))
            {
                //Logger._log.Error("MaliciousRequest=" + requestData);
                return ApiFailure("Bad Request", Constants.Failure, HttpStatusCode.BadRequest, requestMethod);

            }

            string fileLocationPath = HttpContext.Current.Server.MapPath("~/App_Data");
            //CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileLocationPath);
            List<string> fileList = new List<string>();

            List<FileModel> files = new List<FileModel>();
            /*try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    string fileName = file.Headers.ContentDisposition.FileName;
                    string serverPath = file.LocalFileName;
                    fileList.Add(Path.GetFileName(file.LocalFileName));

                    FileModel fileModel = ModelHelper.ReadFile(serverPath);
                    files.Add(fileModel);
                }

                RequestModel request = new RequestModel(requestMethod, requestData, requestDateTime, tokenHeader, files);
                return null;//RouteToManager(request);
            }
            catch (Exception e)
            {
                return ApiFailure(e.Message, Constants.Failure, HttpStatusCode.BadRequest, requestMethod);
            }*/
            RequestModel request = new RequestModel(requestMethod, requestData, requestDateTime, tokenHeader, files);
            return RouteToManager(request);
        }
        
        private object RouteToManager(RequestModel request)
        {
            object requestValidate = ValidateRequest(request);
            if (requestValidate.GetType() == typeof(ErrorResponse))
            {
                ErrorResponse error = (ErrorResponse)requestValidate;
                return ApiFailure("{}", error.ErrorMessage, error.ErrorCode, "null");
            }

            string projectName = "";
            string managerName = request.RequestMethod.Split('.').First();
            string methodName = request.RequestMethod.Split('.').Last();
            string projectNamespace = "";

            Type typeProject = Type.GetType("TestAPI.Models."+ managerName + ",TestAPI.Models");//MPAPIConstants.ManagerNamespace + ".ProjectManager.ProjectManager," + MPAPIConstants.ManagerNamespace);

            if (typeProject == null)
            {
                return ApiFailure("{}", string.Format("({0}) {1}", request.RequestMethod, Constants.MethodNotFound), HttpStatusCode.BadRequest, request.RequestMethod);
            }

            //request.RequestMethod = managerName + "." + managerName;
            object[] argumentsProjects = new[] { request };
            object responseDataProjects = typeProject.GetMethod(methodName).Invoke(null, argumentsProjects);

            if (responseDataProjects.GetType() == typeof(ErrorResponse))
            {
                ErrorResponse error = (ErrorResponse)responseDataProjects;
                return ApiFailure("{}", error.ErrorMessage, error.ErrorCode, request.RequestMethod);
            }
            else if (responseDataProjects.GetType() == typeof(SuccessResponse))
            {
                SuccessResponse response = (SuccessResponse)responseDataProjects;
                if (request.IsPostRequest)
                {
                    return ApiSuccess(response.Data, methodName, response.ResponseMessage, response.ResponseCode);
                }
                else
                {
                    return responseDataProjects;
                }
            }
            //else if (responseData != null)
            //{
            //    return ApiSuccess(responseData, request.RequestMethod);
            //}
            else
            {
                return ApiFailure("{}", Constants.Failure, HttpStatusCode.BadRequest, methodName);
            }
            /*
            projectNamespace = projectName.Replace("." + projectName.Split('.').Last(), "");
            Type type = Type.GetType(projectName + ", " + projectNamespace);
            if (type == null)
            {
                return ApiFailure("{}", string.Format("({0}) {1}", methodName, Constants.MethodNotFound), HttpStatusCode.BadRequest, request.RequestMethod);
            }

            request.RequestMethod = projectName + "." + methodName;
            object[] arguments = new[] { request };
            object responseData = type.GetMethod("").Invoke(null, arguments);

            if (responseData.GetType() == typeof(ErrorResponse))
            {
                ErrorResponse error = (ErrorResponse)responseData;
                return ApiFailure("{}", error.ErrorMessage, error.ErrorCode, methodName);
            }
            else if (responseData.GetType() == typeof(SuccessResponse))
            {
                SuccessResponse response = (SuccessResponse)responseData;
                if (request.IsPostRequest)
                {
                    return ApiSuccess(response.Data, methodName, response.ResponseMessage, response.ResponseCode);
                }
                else
                {
                    return responseData;
                }
            }
            //else if (responseData != null)
            //{
            //    return ApiSuccess(responseData, request.RequestMethod);
            //}
            else
            {
                return ApiFailure("{}", Constants.Failure, HttpStatusCode.BadRequest, methodName);
            }*/
        }

        private object ValidateRequest(RequestModel request)
        {
            if (request == null || request.RequestMethod == null)
            {
                return new ErrorResponse(Constants.Failure, HttpStatusCode.BadRequest);
            }
            else if (request.RequestMethod == ""/*Constants.GetOneTimeCode*/)// || request.RequestMethod == Constants.GetApplicationParameter)
            {
                //   request.TokenHeader = StringConstants.Key.JWTSecretToken;

                if (request.TokenHeader != Constants.JWTSecretToken)
                {
                    return new ErrorResponse(Constants.TokenInvalidSignature, HttpStatusCode.BadRequest);
                }

                object jsonJWT = DBManagerUtility.decodeJWT(request.TokenHeader);
                if (jsonJWT.GetType() == typeof(ErrorResponse))
                {
                    ErrorResponse error = (ErrorResponse)jsonJWT;
                    return new ErrorResponse(error.ErrorMessage, error.ErrorCode);
                }
                return jsonJWT;
            }
            else
            {
                if (request.TokenHeader == null)
                {
                    return new ErrorResponse(Constants.TokenInvalidSignature, HttpStatusCode.BadRequest);
                    //return ApiFailure("{}", StringConstants.Message.TokenInvalidSignature, HttpStatusCode.BadRequest, "null");
                }
                else
                {
                    object jsonJWT = DBManagerUtility.decodeJWT(request.TokenHeader);
                    if (jsonJWT.GetType() == typeof(ErrorResponse))
                    {
                        ErrorResponse error = (ErrorResponse)jsonJWT;
                        return new ErrorResponse(error.ErrorMessage, error.ErrorCode);
                        //return ApiFailure("{}", error.ErrorMessage, error.ErrorCode, "null");
                    }
                    if (ValidateToken(jsonJWT, request.RequestData, request.TokenHeader, request.RequestMethod))
                        return jsonJWT;
                    else
                    {
                        //Logger._log.Error("API - Token Validation Failed: " + request.RequestMethod + "\n" + request.TokenHeader);
                        return new ErrorResponse(Constants.TokenInvalidSignature, HttpStatusCode.BadRequest);
                    }
                    // return jsonJWT;
                }
            }
            //return request;
        }

        private bool ValidateToken(object jsonJWT, object requestData, string token, string requestMethod)
        {
            try
            {
                if (ConfigurationManager.AppSettings["TokenValidationAllowed"] == enActive.Y.ToString())
                {
                    JWTPayload payload = (JWTPayload)jsonJWT;

                    if (string.IsNullOrEmpty(payload.TokenNature) || string.IsNullOrEmpty(payload.ApplicationID) || string.IsNullOrEmpty(payload.AccessDateTime))
                        return false;

                    
                    //if (!ServiceLists.Instance.IsNonPatientService(requestMethod))
                    //{
                        if (string.IsNullOrEmpty(payload.DeviceID) || string.IsNullOrEmpty(payload.DeviceOS) || string.IsNullOrEmpty(payload.UserID))
                            return false;

                        //if (!RegisteredTokenManager.GetRegisteredTokenDetail(payload.DeviceID, payload.UserID, token))
                            //return false;

                        //if (payload.TokenNature == enTokenNature.D.ToString()) //isAdmin field is true in RegCard Table
                            //return true;

                        if (IsValidJson(requestData.ToString()))
                        {
                            //JWTPayloadValidationModel requestModel = JsonConvert.DeserializeObject<JWTPayloadValidationModel>(Convert.ToString(requestData));

                            //if (requestModel.UserID != null)
                            //    if (payload.UserID != requestModel.UserID)
                            //        return false;

                        }
                    //}
                    //else
                    //{
                        if (payload.TokenNature == enTokenNature.N.ToString())
                        {
                            DateTime tokentime = new DateTime(Convert.ToInt64(payload.AccessDateTime)).AddMinutes(5);
                            if (tokentime < DateTime.Now)
                                return false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(payload.DeviceID) || string.IsNullOrEmpty(payload.DeviceOS) || string.IsNullOrEmpty(payload.UserID))
                                return false;

                            //if (!RegisteredTokenManager.GetRegisteredTokenDetail(payload.DeviceID, payload.UserID, token))
                                //return false;

                        }
                    //}
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                //try
                //{
                    //JWTPayloadValidationModel requestModel = JsonConvert.DeserializeObject<JWTPayloadValidationModel>(strInput);
                    return true;
                //}
                //catch (Exception ex)
                //{
                //    return false;
                //}
            }
            else
            {
                return false;
            }
        }

        [Route("getimage")]
        [HttpGet]
        public async Task<Object> GetImage(string path, string requestmethod)
        {
            string tokenHeader = HttpContext.Current.Request.Headers[Constants.TokenKey];
            RequestModel request = new RequestModel(requestmethod, path, "", tokenHeader, null, false);
            return RouteToManager(request);
        }

        private bool IsMaliciousRequest(string requestText)
        {
            bool isRequestSafe = Constants.FALSE;
            try
            {
                requestText = requestText.Replace(" ", "").ToUpper();
                List<string> blackListedKeyWordsList = new List<string> { "HTML", "<SCRIPT", "'--" ,";--", ";#",
                                                                            "/*", "1=1", "DBMS_LOCK" , "UNIONALL", "INSERTINTO",
                                                                            "VALUES(", "*FROM", "DATABASE" , "ALERT(", "ONLOAD="};

                foreach (var item in blackListedKeyWordsList)
                {
                    if (requestText.Contains(item))
                    {
                        return Constants.TRUE;
                    }

                }
                // isRequestSafe = blackListedKeyWordsList.ForEach(x=> requestText.Contains(x);
            }
            catch (Exception ex)
            {
                //Logger._log.Error(ex.Message);
                isRequestSafe = Constants.FALSE;
            }
            return isRequestSafe;
        }
    }
}
