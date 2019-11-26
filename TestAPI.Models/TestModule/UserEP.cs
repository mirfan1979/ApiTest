using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Routing;
using TestAPI.Models.Utility;

namespace TestAPI.Models
{
    public class UserEP
    {
        public static object ValidateUser(string username, string password, string type)
        {
            // Setup the connection and compiler
            var connection = new MySqlConnection(ConfigurationManager.AppSettings["MySqlDBConn"].ToString());
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            
            try
            {
                // You can register the QueryFactory in the IoC container
                if(type == "USER")
                {
                    object response = db.Query("User").Where(q=> q.Where("Email", username).OrWhere("Username", username) ).Where("Password", password).First();
                    var strResponse = response.ToString().Replace("DapperRow,", "").Replace("=", ":");
                    Dictionary<string,string> temp =  JsonConvert.DeserializeObject< Dictionary < string,string>>(strResponse);
                    return temp;
                }
                else if (type == "ADMIN")
                {
                    object response = db.Query("Admin").Where("AdmUserId", username).Where("Password", password).First();
                    var strResponse = response.ToString().Replace("DapperRow,", "").Replace("=", ":");
                    Dictionary<string, string> temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(strResponse);
                    return temp;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                //Logger.WriteErrorLog(ex);
                //return new ErrorResponse(ex.Message, HttpStatusCode.BadRequest);
                return null;
            }

        }
    }
}
