using System;
using System.Collections.Generic;

namespace TestAPI.Models
{
    public class UserMasterRepository
    {
        public UserMaster ValidateUser(string username, string password, string type)
        {
            Dictionary<string, string> temp = UserEP.ValidateUser(username, password, type) as Dictionary<string, string>;
            UserMaster um = null;
            if (temp != null)
            {
                if (type == "USER")
                {
                    string UserId = "";
                    temp.TryGetValue("Email", out UserId);
                    string UserName = "";
                    temp.TryGetValue("UserName", out UserName);
                    string UserPassword = "";
                    temp.TryGetValue("Password", out UserPassword);
                    um = new UserMaster() {
                        UserId = UserId,
                        UserPassword = UserPassword,
                        UserName = UserName,
                        UserType = type,
                        UserRole = ""
                    };
                }
                else if (type == "ADMIN")
                {
                    string UserId = "";
                    temp.TryGetValue("AdminID", out UserId);
                    string UserName = "";
                    temp.TryGetValue("AdmUserId", out UserName);
                    string UserPassword = "";
                    temp.TryGetValue("Password", out UserPassword);
                    string UserRole = "";
                    temp.TryGetValue("AdmRole", out UserRole);
                    um = new UserMaster()
                    {
                        UserId = UserId,
                        UserPassword = UserPassword,
                        UserName = UserName,
                        UserType = type,
                        UserRole = UserRole
                    };
                }
                else
                {

                }
            }
            return um;
        }

    }
}