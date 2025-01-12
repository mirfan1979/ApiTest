﻿using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;


using TestAPI.Models.Utility;


namespace TestAPI.Models
{
    public class PropertyModuleEP
    {
        public static object GetTop4(RequestModel request)
        {
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
            // Setup the connection and compiler
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();
            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
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
        public static object GetPropertyByID(RequestModel request)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));
            object _PropertyID;
            test.TryGetValue("PropertyID", out _PropertyID);

            // Setup the connection and compiler
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());


            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
                // You can register the QueryFactory in the IoC container
                IEnumerable<IDictionary<string, object>> response;
                response = db.Query("propertydetail")
                    .Where("PropertyID", _PropertyID)
                    .Get()
                    .Cast<IDictionary<string, object>>();  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();

                bool hasData = (response != null) ? true : false;
                if (hasData)
                {

                    foreach (var row in response)
                    {
                        var _PropertyType = row["PropertyType"];
                        if ((_PropertyType.ToString() == "D"))
                        {
                            object response_dev = db.Query("developmental").Where("PropertyID", _PropertyID).Get().Cast<IDictionary<string, object>>();
                            row.Add("developmental", response_dev);
                            object response_dev_pred = db.Query("developmentalprediction").Where("PropertyID", _PropertyID).Get().Cast<IDictionary<string, object>>();
                            row.Add("developmentalprediction", response_dev_pred);

                        }
                        else if ((_PropertyType.ToString() == "R"))
                        {
                            object response_rent = db.Query("rentalproperty").Where("PropertyID", _PropertyID).Get().Cast<IDictionary<string, object>>();
                            row.Add("rental", response_rent);
                            

                        }
                       
                            object response_pred = db.Query("propertyprediction").Where("PropertyID", _PropertyID).Get().Cast<IDictionary<string, object>>();
                            row.Add("propertyprediction", response_pred);

                        

                    }

                }

                successResponseModel = new SuccessResponse(response, hasData);
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }

            return successResponseModel;


        }
        public static object GetPropertyEditableByID(RequestModel request)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));
            object _PropertyID;
            test.TryGetValue("PropertyID", out _PropertyID);
            
            // Setup the connection and compiler
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
          

            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
                // You can register the QueryFactory in the IoC container
                IEnumerable<IDictionary<string, object>> response;
                response = db.Query("propertydetail")
                    .Select("PropertyID","PropertyPics","PropertyVideos", "PStatus" , "DisplayProperty", "Documents", 
                    "MinimumInvestmentAmount", "PropertyType")
                    .Where("PropertyID",_PropertyID )
                    .Get()
                    .Cast<IDictionary<string, object>>();  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();

               bool hasData = (response != null) ? true : false;
                 if (hasData)
                 {

                    foreach (var row in response)
                    {
                        var _PropertyType = row["PropertyType"]; 
                        if ((_PropertyType.ToString() == "D") )
                          {
                              object response_dev = db.Query("developmental").Where("PropertyID",_PropertyID).Get().Cast<IDictionary<string, object>>();
                              row.Add("developmental", response_dev);
                              object response_dev_pred = db.Query("developmentalprediction").Where("PropertyID", _PropertyID).Get().Cast<IDictionary<string, object>>();
                              row.Add("developmentalprediction", response_dev_pred);

                        }
                        else if ((_PropertyType.ToString() == "R") )
                        {
                            object response_rent = db.Query("rentalproperty").Where("PropertyID", _PropertyID).Get().Cast<IDictionary<string, object>>();
                            row.Add("RentalProperty", response_rent);
                          

                        }
                       
                         object response_pred = db.Query("propertyprediction").Where("PropertyID", _PropertyID).Get().Cast<IDictionary<string, object>>();
                         row.Add("propertyprediction", response_pred);

                        
                       
                    }

                 }
                 
                successResponseModel = new SuccessResponse(response, hasData);
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }

            return successResponseModel;


        }

        public static object EditProperty(RequestModel request)
        {
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();
            db.Connection.Open();
            using (var scope = db.Connection.BeginTransaction())
            {
                try
                {
                    var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));
                    object PropertyID;
                    test.TryGetValue("PropertyID", out PropertyID);
                    int _PropertyID = Convert.ToInt32(PropertyID);

                    object PropertyType;
                    test.TryGetValue("PropertyType", out PropertyType);
                    string _PropertyType = PropertyType.ToString();
                    test.Remove("PropertyType");
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
                    if (_PropertyType== "D")
                    {
                        object _Developmental;
                        test.TryGetValue("Developmental", out _Developmental);
                        test.Remove("Developmental");
                        Dictionary<string, object> DevelopmentalProperty = JsonConvert
                                           .DeserializeObject<Dictionary
                                           <string, object>>(_Developmental.ToString());
                       

                        if (DevelopmentalProperty != null)
                        {

                            //foreach (var dev in DevelopmentalProperty)
                            //{
                            DevelopmentalProperty.Add("PropertyID", _PropertyID);
                                  
                            var resdevprop = db.Query("developmental").Where("PropertyID", _PropertyID).Update(DevelopmentalProperty);
                           // }


                        }
                        object DevelopmentalPrediction;
                        test.TryGetValue("DevelopmentalPrediction", out DevelopmentalPrediction);
                        test.Remove("DevelopmentalPrediction");
                        List<Dictionary<string, object>> _DevelopmentalPrediction = JsonConvert
                                           .DeserializeObject<List<Dictionary
                                           <string, object>>>(DevelopmentalPrediction.ToString());

                        if (_DevelopmentalPrediction != null)
                        {
                            foreach (var pd in _DevelopmentalPrediction)
                            {
                                object _PredictionID;
                                pd.TryGetValue("PredictionID", out _PredictionID);
                                if (_PredictionID == null)
                                {
                                    pd.Add("PropertyID", _PropertyID);
                                    var resPdRN = db.Query("developmentalprediction").Insert(pd);
                                }

                            }

                        }


                    }
                    else if (_PropertyType== "R")
                    {
                        object RentalProperty;
                        test.TryGetValue("RentalProperty", out RentalProperty);
                        test.Remove("RentalProperty");
                        List<Dictionary<string, object>> _RentalProperty = JsonConvert
                                            .DeserializeObject<List<Dictionary
                                            <string, object>>>(RentalProperty.ToString());
                        if (_RentalProperty != null)
                        {   

                            foreach (var ren in _RentalProperty)
                            {
                                object _RentalPropertyid;
                                ren.TryGetValue("RentalPropertyID", out _RentalPropertyid);
                                if(_RentalPropertyid==null)
                                {
                                    object RentalContract;
                                    ren.TryGetValue("RentalContract", out RentalContract);
                                    List<string> _RentalContract = RentalContract as List<string>;
                                    ren.Remove("RentalContract");
                                    if (_RentalContract != null && _RentalContract.Count > 0)
                                    {
                                        //convert and add key    
                                    }
                                    ren.Add("PropertyID", _PropertyID);
                                    var resRental = db.Query("RentalProperty").Insert(ren);
                                }


                            }
                            
                            
                        }
                        


                    }
                    object PropertyPrediction;
                    test.TryGetValue("PropertyPrediction", out PropertyPrediction);
                    test.Remove("PropertyPrediction");
                    List<Dictionary<string, object>> PredictionRentalProperty = JsonConvert
                                        .DeserializeObject<List<Dictionary
                                        <string, object>>>(PropertyPrediction.ToString());
                    if (PredictionRentalProperty != null)
                    {
                        foreach (var pd in PredictionRentalProperty)
                        {
                            object _PredictionID;
                            pd.TryGetValue("PredictionID", out _PredictionID);
                            if (_PredictionID == null)
                            {
                                pd.Add("PropertyID", _PropertyID);
                                var resPdRN = db.Query("propertyprediction").Insert(pd);
                            }

                        }

                    }




                    var query = db.Query("propertydetail").Where("PropertyID", _PropertyID).Update(test);


                    bool hasData = true;//(response != null) ? true : false;
                    scope.Commit();
                    successResponseModel = new SuccessResponse("", hasData, "Record Edited");

                }
                catch (Exception ex)
                {
                    scope.Rollback();
                    return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
                }

                return successResponseModel;


            }
        }
    }
}
