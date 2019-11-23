using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using TestAPI.Models.Utility;

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
                var dev4 = db.Query("propertydetail").Where("propertytype", 1).Limit(4);
                var rent4 = db.Query("propertydetail").Where("propertytype", 2).Limit(4);

                var land4 = db.Query("propertydetail").Where("propertytype", 3).Limit(4);
                var temp = dev4.UnionAll(rent4).UnionAll(land4);
                var response = dev4.UnionAll(rent4).UnionAll(land4).Get();

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
                // You can register the QueryFactory in the IoC container
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
