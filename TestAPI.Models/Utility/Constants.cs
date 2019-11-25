using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAPI.Models.Utility
{
    public static class Constants
    {
        #region Date Constants
        // DateFormats 
        public static string DATE_FORMAT_DISPLAY = "MMM dd, yyyy";
        public static string DATETIME_FORMAT_DISPLAY = "MMM dd, yyyy HH:mm";
        public const string DATE_FORMAT = "dd/MM/yyyy";
        public const string DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        public const string DATE_FORMAT_ORACLE = "To_date('{0}','dd/mm/yyyy hh24:mi:ss')";
        public const string DATETIME_FORMAT_CYBERSOURCE = "yyyy-MM-dd'T'HH:mm:ss'Z'";

        public static string DATETIME_FORMAT_ddMMyyyyHHmm = "dd/MM/yyyy HH:mm";
        public static string DATETIME_FORMAT_ddMMyyyyHHmmss = "dd/MM/yyyy HH:mm:ss";
        public static string DATETIME_FORMAT_ddMMyyyy = "dd/MM/yyyy";
        public static string DATETIME_FORMAT_ddMMMyyyy = "dd MMM yyyy";
        public static string DATETIME_FORMAT_HHmm = "HH:mm";
        public static string DATETIME_FORMAT_ddMMMyyyyHHmm = "dd MMM yyyy HH:mm";


        // return Current DateTime
        public static string strCurrentDate { get { return System.DateTime.Now.ToString(DATE_FORMAT); } }
        public static string strCurrentDateTime { get { return System.DateTime.Now.ToString(DATETIME_FORMAT); } }
        public static string strCurrentUniversalDateTimeCYBERSOURCE { get { return System.DateTime.Now.ToUniversalTime().ToString(DATETIME_FORMAT_CYBERSOURCE); } }
        #endregion

        #region Key
        public const string JWTSecretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrkMnM";
        public const string JWTSecretToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJBY2Nlc3NEYXRlVGltZSI6NjM2ODYyNjYwNzMyMzIxMTU4LCJBcHBsaWNhdGlvbklEIjoiT05FVElNRVRPS0VOIiwiVG9rZW5OYXR1cmUiOiJOIiwiVG9rZW5OZWVkIjoiT25lVGltZVRva2VuQ2FsbCIsIlRva2VuS2V5IjoiMXFhejNlZGM1dGhuN3VqbTJ3ZGM0cmdiMGlodjc4Zng1ZXN6In0.vMtFFSEtO_CGIe5_y_jsCqcOk1SjAz7E8YY6ESbVadY";
        public const string JWTEmptyToken = "";
        #endregion

        #region Value
        public const string TokenKey = "_token";
        public const string RequestMethodKey = "RequestMethod";
        public const string RequestDataKey = "RequestData";
        public const string RequestDateTimeKey = "RequestDateTime";
        #endregion

        #region Code
        public const int iContinue = 100;
        public const int iSuccess = 200;
        public const int iBadRequest = 400;
        public const int iUnAuthorized = 401;
        public const int iNotFound = 404;
        #endregion

        #region Message
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
        #endregion

        #region Integer Constants
        public const int ZERO = 0;
        public const string strZERO = "0";
        public const int ONE = 1;
        public const string strONE = "1";
        #endregion

        #region Boolean Constants 
        public const bool TRUE = true;
        public const string strTRUE = "true";
        public const bool FALSE = false;
        public const string strFALSE = "false";
        #endregion

        #region General
        public const string ContentTypeAttachment = "attachment";
        #endregion

        #region Enums Constants 

        public enum enSignInStatus
        {
            Success = 0, //     Sign in was successful
            LockedOut = 1,  //     User is locked out
            AccessDenied = 2, //    User does not have rights
            Failure = 3, //     Sign in failed
        }

        public enum enYesNoDesc
        {
            YES,
            NO
        }

        public enum enActive
        {
            Y,
            N
        }

        public enum enHISApps
        {
            MYPATIENTS
        }

        public enum enTokenNature
        {
            Y,
            N,
            D
        }
        #endregion

        #region string
        public const string Unknown = "U";
        #endregion

        #region String Constants

        public const string EmptyString = "";
        public const string NotApplicable = "N/A";

        public const string Default_SaveSuccessTitle = "Saved";
        public const string Default_SaveFailureTitle = "Failed";

        public const string Default_SaveSuccessMessage = "Record has been saved successfully.";
        public const string Default_SaveFailureMessage = "Something went wrong. Please try again.";

        public const string Default_NoRecordFoundMessage = "Record Not Found.";
        public const string Default_NoReportFoundMessage = "No report found.";

        #endregion


        #region DDLs

        public class DDList
        {
            public DDList(string Text, object Value)
            {
                this.Text = Text;
                this.Value = Value;
            }
            public string Text { get; set; }
            public object Value { get; set; }
            public bool isSelected { get; set; }
        }
        
        public static List<DDList> getPropertyType()
        {
            return new List<DDList>()
            {{ new DDList("--Select Property Type--", "") },
                { new DDList("Developmental", "D") },
                { new DDList("Rental", "R") },
                { new DDList("Land", "L") }
            };
        }
        
        public static List<DDList> getSizeUnit()
        {
            return new List<DDList>()
            {{ new DDList("--Select size unit--", "") },
                { new DDList("Square Yards", "SQYD") },
                { new DDList("Square foot", "SQFT") },
                { new DDList("Land", "LAND") }
            };
        }
        
        public static List<DDList> getYN()
        {
            return new List<DDList>()
            { { new DDList("--Select an option--", "") },
                { new DDList("Yes", "Y") },
                { new DDList("No", "N") }  
            };
        }

        public static List<DDList> getCommercialResidential()
        {
            return new List<DDList>()
            { { new DDList("--Select an option--", "") },
                { new DDList("Commercial", "C") },
                { new DDList("Residential", "R") }
            };
        }

        public static List<DDList> getPropertyStatus()
        {
            return new List<DDList>()
            { { new DDList("--Select an option--", "") },
                { new DDList("On Hold", "H") },
                { new DDList("Open", "O") },
                { new DDList("Completed", "C") },
                { new DDList("Sold", "S") },
            };
        }

        public static List<DDList> getTimeUnit()
        {
            return new List<DDList>()
            { { new DDList("--Select an option--", "") },
                { new DDList("Year", "Y") },
                { new DDList("Month", "M") }
            };
        }
        
        public static List<DDList> getShareBreak()
        {
            return new List<DDList>()
            {
                { new DDList("--Select option--", 0) },
                { new DDList("0.1", 0.1) },
                { new DDList("0.2", 0.2) },
                { new DDList("0.3", 0.3) },
                { new DDList("0.4", 0.4) },
                { new DDList("0.5", 0.5) }
            };
        }
        
        #endregion


    }
}
