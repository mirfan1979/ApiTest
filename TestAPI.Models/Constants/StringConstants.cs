using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI.Models
{
    public static class StringConstants
    {
        public static class Key
        {
            public const string JWTSecretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrkMnM";
            public const string JWTSecretToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJBY2Nlc3NEYXRlVGltZSI6NjM2ODYyNjYwNzMyMzIxMTU4LCJBcHBsaWNhdGlvbklEIjoiT05FVElNRVRPS0VOIiwiVG9rZW5OYXR1cmUiOiJOIiwiVG9rZW5OZWVkIjoiT25lVGltZVRva2VuQ2FsbCIsIlRva2VuS2V5IjoiMXFhejNlZGM1dGhuN3VqbTJ3ZGM0cmdiMGlodjc4Zng1ZXN6In0.vMtFFSEtO_CGIe5_y_jsCqcOk1SjAz7E8YY6ESbVadY";
            public const string JWTEmptyToken = "";
        }

        public static class RequestMethod
        {
            public const string Login = "MyPatientsManager.Login";
            public const string Register = "UserManager.Register";
            public const string AdminLogin = "AdminManager.Login";
            public const string GeneratePasswordResetCodeAndEmail = "UserManager.GeneratePasswordResetCodeAndEmail";
            public const string VerifyPasswordResetCodeAndUpdatePassword = "UserManager.VerifyPasswordResetCodeAndUpdatePassword";
            public const string GetApplicationParameter = "MyPatientsManager.GetApplicationParameters";
            public const string GetOneTimeCode = "MyPatientsManager.GetOneTimeCode";
        }

        public static class Value
        {
            //  public const string ManagerNamespace         = "MyPatientsAKUAPI.APIManagers";
            public const string InitializeManager = "InitializeManager";
            public const string TokenKey = "_token";
            public const string RequestMethodKey = "RequestMethod";
            public const string RequestDataKey = "RequestData";
            public const string RequestDateTimeKey = "RequestDateTime";
            // public const string DataServiceKey           = "MyPatientsAKUAPIService";
            public const string CybersourceTransactionTypeKey = "CybersourceTransactionType";
            public const string CybersourceAccessKey = "CybersourceAccessKey";
            public const string CybersourceProfileIDKey = "CybersourceProfileID";
            public const string CybersourceSecretKey = "CybersourceSecretKey";
            public const string CybersourcePayURLKey = "CybersourcePayURL";
        }

        public static class Code
        {
            public const int Continue = 100;
            public const int Success = 200;
            public const int BadRequest = 400;
            public const int UnAuthorized = 401;
            public const int NotFound = 404;
        }

        public static class Message
        {
            public const string Success = "Success";
            public const string Failure = "Failure";
            public const string MethodNotFound = "Requested method not found";
            public const string UserNotFound = "User not found";
            public const string InvalidCredentials = "Invalid credentials, please try again.";
            public const string TokenExpired = "Token has expired.";
            public const string TokenInvalidSignature = "Token has invalid signature.";
            public const string FailureMessageTitle = "Unexpected Error!";
            public const string FailureMessage = "Something went wrong. Please try again.";
            public const string RecordNotFound = "No record found";
        }

        public static class General
        {
            public const string ContentTypeAttachment = "attachment";
        }
    }
}
