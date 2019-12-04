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
    public class TestModuleEP
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
                var response = db.Query("User").Get();  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();
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

        public static object GetCustomList(RequestModel request)
        {
            //var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));

            // Setup the connection and compiler
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
                // You can register the QueryFactory in the IoC container
                //var response = db.Query("jpadmin").Get();  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();
                //var response = db.Select("select * from jpadmin");
                string strRawQuery = @"
                    (select * from jpopening where Department = 'Human Resource' limit 1)
                    union all
                    (select * from jpopening where Department = 'Marketing' limit 1)
                    union all
                    (select * from jpopening where Department = 'IT' limit 1)
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
        public static object GetSharePercentage(RequestModel request)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));
            object _PropertyID;
            test.TryGetValue("PropertyID", out _PropertyID);
            object Amount;
            test.TryGetValue("Amount", out Amount);
            int _Amount = Convert.ToInt32(Amount);
            // Setup the connection and compiler
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            int _totalShareQuantity=0;
            int _CurrentValue=0; 
            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
                // You can register the QueryFactory in the IoC container
                //var response = db.Query("jpadmin").Get();  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();
                //var response = db.Select("select * from jpadmin");
                IDictionary<string, object>  response = db.Query("PropertyShare")
                                                          .Where("PropertyID", _PropertyID)
                                                          .Get()
                                                          .Cast<IDictionary<string, object>>().First(); ;  //db.Query("jpexperience").Where("ExpId", 6).Where("ProfileId", 4).First();
                
                bool hasData = (response != null) ? true : false;
                if (hasData)
                {
                   
                        _totalShareQuantity = Convert.ToInt32(response["TotalShareQuantity"]);
                    
                }
                string sql = @"select * from propertyprediction where propertyid = {0} and year(predictionyear)=year(current_date()) ;";
                sql = string.Format(sql, _PropertyID);
                IDictionary<string, object> predresponse = db.Select(sql).Cast<IDictionary<string, object>>().First();


                /*("PropertyPrediction")
                .Where("PropertyID", _PropertyID).Where()
                .Get()
                .Cast<IDictionary<string, object>>().First();*/

                hasData = (response != null) ? true : false;
                if (hasData)
                {

                   _CurrentValue = Convert.ToInt32(predresponse["CurrentValue"]);

                }
                double numOfShares = _CurrentValue / _totalShareQuantity;
                double user_shares = _Amount / numOfShares;
                double percentvalue = user_shares/(double)(_totalShareQuantity) ;
                percentvalue *= 100;
                percentvalue = Math.Floor(percentvalue);
               // float percentvalue = (_Amount / (_CurrentValue / float(_totalShareQuantity)))/float(_totalShareQuantity) *100;
                Dictionary<string, object> res = new Dictionary<string, object>() { { "Percentage", percentvalue } };
                successResponseModel = new SuccessResponse(res, hasData);

            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }

            return successResponseModel;

        }
        public static object SaveDemoRecord(RequestModel request)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(request.RequestData));

            // Setup the connection and compiler
            // var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var conn = "Database =PropertyInvestment; Data Source = localhost; User Id = root; Password = gsmgms12";
            // var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var connection = new MySqlConnection(conn);

            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);

            SuccessResponse successResponseModel = new SuccessResponse();

            try
            {
              //  You can register the QueryFactory in the IoC container
               var response = db.Query("propertydetail").Insert(test);
                bool hasData = true;//(response != null) ? true : false;
                successResponseModel = new SuccessResponse(response, hasData);
            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
            }

            return successResponseModel;
        }
    }
}
