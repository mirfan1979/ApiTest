using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using TestAPI.Models.Utility;
using System.Transactions;

namespace TestAPI.Models
{
    public class PropertyModuleEP
    {
        public static object GetDemoList(RequestModel request)
        {
            //var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));

            // Setup the connection and compiler
            var conn = "Database =PropertyInvestment; Data Source = localhost; User Id = root; Password = gsmgms12";
            // var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var connection = new MySqlConnection(conn);
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
                // You can register the QueryFactory in the IoC container
                var response = db.Query("propertydetail").Get();  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();
                bool hasData = (response != null) ? true : false;
                successResponseModel = new SuccessResponse(response, hasData);
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }

            return successResponseModel;

        }

        public static object GetTop4(RequestModel request)
        {
            //var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));

            // Setup the connection and compiler
            var conn = "Database =PropertyInvestment; Data Source = localhost; User Id = root; Password = gsmgms12";
            // var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var connection = new MySqlConnection(conn);
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
             
            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
                // You can register the QueryFactory in the IoC container

                //var response = db.Query("propertydetail").Select("propertyid", "propertytype").GroupBy("PropertyType").GroupBy("propertyid").Get();  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();
                /*var dev4 = db.Query("propertydetail").Where("propertytype", 1).Limit(4);
                var rent4 = db.Query("propertydetail").Where("propertytype", 2).Limit(4);

                var land4 = db.Query("propertydetail").Where("propertytype", 3).Limit(4);
                var temp = dev4.UnionAll(rent4).UnionAll(land4);
                var response = dev4.UnionAll(rent4).UnionAll(land4).Get();*/

                string strRawQuery = @"
                    (select * from propertydetail where propertytype = 1 limit 4)
                    union all
                    (select * from propertydetail where propertytype = 2 limit 4)
                    union all
                    (select * from propertydetail where propertytype = 3 limit 4)
                ";

                var response = db.Select(strRawQuery);
                
                bool hasData = (response != null) ? true : false;
                successResponseModel = new SuccessResponse(response, hasData);
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }

            return successResponseModel;

        }

        public static object SaveAddNewProperty(RequestModel request)
        {
            //validate form auth token
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));

            object PropertyType;
            test.TryGetValue("PropertyType", out PropertyType);
            string _PropertyType = PropertyType.ToString();

            object PropertyPics;
            test.TryGetValue("PropertyPics", out PropertyPics);
            List<string> _PropertyPics = PropertyPics as List<string>;
            test.Remove("PropertyPics");
            if (_PropertyPics != null && _PropertyPics.Count > 0)
            {
                //convert and add key    
            }

            object PropertyVideos;
            test.TryGetValue("PropertyVideos", out PropertyVideos);
            List<string> _PropertyVideos = PropertyVideos as List<string>;
            test.Remove("PropertyVideos");
            if (_PropertyVideos != null && _PropertyVideos.Count > 0)
            {
                //convert and add key    
            }

            object Documents;
            test.TryGetValue("Documents", out Documents);
            List<string> _Documents = Documents as List<string>;
            test.Remove("Documents");
            if (_Documents != null && _Documents.Count > 0)
            {
                //convert and add key    
            }

            object PropertyShare;
            test.TryGetValue("PropertyShare", out PropertyShare);
            test.Remove("PropertyShare");

            object PropertyPrediction;
            test.TryGetValue("PropertyPrediction", out PropertyPrediction);
            test.Remove("PropertyPrediction");

            object Developmental;
            test.TryGetValue("Developmental", out Developmental);
            test.Remove("Developmental");

            object DevelopmentalPrediction;
            test.TryGetValue("DevelopmentalPrediction", out DevelopmentalPrediction);
            test.Remove("DevelopmentalPrediction");

            object RentalProperty;
            test.TryGetValue("RentalProperty", out RentalProperty);
            test.Remove("RentalProperty");
            // Setup the connection and compiler
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            // conn = "Database =hrms; Data Source = localhost; User Id = root; Password = gsmgms12";
            // var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            //var connection = new MySqlConnection(conn);

            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();
            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
                    // You can register the QueryFactory in the IoC container

                    var query = db.Query("propertydetail").AsInsert(test);

                    SqlKata.SqlResult compiledQuery = compiler.Compile(query);

                    //Inject the Identity in the Compiled Query SQL object
                    var sql = compiledQuery.Sql + "; SELECT @@IDENTITY as ID;";

                    //Name Binding house the values that the insert query needs 
                    var IdentityKey = db.Select<string>(sql, compiledQuery.NamedBindings).FirstOrDefault();

                    Dictionary<string, object> _PropertyShare = JsonConvert.DeserializeObject<Dictionary<string, object>>(PropertyShare.ToString());
                    if (_PropertyShare != null)
                    {
                        _PropertyShare.Add("PropertyId", IdentityKey);
                        var resPropertyShare = db.Query("PropertyShare").Insert(_PropertyShare);
                    }

                    List<Dictionary<string, object>> _PropertyPrediction = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(PropertyPrediction.ToString());
                    if (_PropertyPrediction != null)
                    {
                        foreach (var pp in _PropertyPrediction)
                        {
                            pp.Add("PropertyId", IdentityKey);
                            var resPropertyShare = db.Query("PropertyPrediction").Insert(pp);
                        }
                    }
                    if (_PropertyType == "D")
                    {
                        Dictionary<string, object> _Developmental = JsonConvert.DeserializeObject<Dictionary<string, object>>(Developmental.ToString());
                        if (_Developmental != null)
                        {
                            _Developmental.Add("PropertyId", IdentityKey);
                            var resDevelopmental = db.Query("Developmental").Insert(_Developmental);
                        }

                        List<Dictionary<string, object>> _DevelopmentalPrediction = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(DevelopmentalPrediction.ToString());
                        if (_DevelopmentalPrediction != null)
                        {
                            foreach (var dp in _DevelopmentalPrediction)
                            {
                                dp.Add("PropertyId", IdentityKey);
                                var resDevelopmentalPrediction = db.Query("DevelopmentalPrediction").Insert(dp);
                            }
                        }
                    }

                    if (_PropertyType == "R")
                    {
                        Dictionary<string, object> _RentalProperty = JsonConvert.DeserializeObject<Dictionary<string, object>>(RentalProperty.ToString());
                        if (_RentalProperty != null)
                        {
                            object RentalContract;
                            _RentalProperty.TryGetValue("RentalContract", out RentalContract);
                            List<string> _RentalContract = RentalContract as List<string>;
                            _RentalProperty.Remove("RentalContract");
                            if (_RentalContract != null && _RentalContract.Count > 0)
                            {
                                //convert and add key    
                            }

                            _RentalProperty.Add("PropertyId", IdentityKey);
                            var resRentalProperty = db.Query("RentalProperty").Insert(_RentalProperty);
                        }
                    }
                    bool hasData = true;//(response != null) ? true : false;
                    scope.Commit();
                    successResponseModel = new SuccessResponse("", hasData, "Record Saved");
                }
                catch (Exception ex)
                {
                    //Logger.WriteErrorLog(ex);
                    scope.Rollback();
                    return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
                }
            }
            return successResponseModel;
            
        }

        public static object GetAddNewProperty(RequestModel request)
        {
            //add forms auth token here
            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
                Dictionary<string, object> response = new Dictionary<string, object>()
                {
                    { "FormAuthToken", "encryptformauthtoken" },
                    { "ddlPropertyType", Constants.getPropertyType() },
                    { "ddlSizeUnits", Constants.getSizeUnit() },
                    { "ddlIsCommercialOrResidential", Constants.getCommercialResidential() },
                    { "ddlPropertyStatus", Constants.getPropertyStatus() },
                    { "ddlShareBreak", Constants.getShareBreak() },
                    { "ddlDisplayProperty", Constants.getYN() },
                    { "ddlTimeUnit", Constants.getTimeUnit() }
                };
                successResponseModel = new SuccessResponse(response, true);
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }

            return successResponseModel;

        }

        //(private) validate AddProperty save --function needs to be added
        
        //(public) Edit Property Values (decide on which values should be editable)

        //(public) Change property status

        //(public) list all properties for Admin


    }
}
