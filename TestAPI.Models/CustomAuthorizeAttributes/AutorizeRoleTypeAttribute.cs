using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace TestAPI.Models
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AutorizeRoleTypeAttribute : AuthorizationFilterAttribute
    {
        public string RoleType { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity);
            var claim = claims.Claims.Where(x => x.Type == "RoleType").FirstOrDefault();
            if (claim != null)
            {
                string privilegeLevels = string.Join("", claim.Value);
                if (privilegeLevels.Contains(this.RoleType) == false)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>() { { "Message", "You are not allowed access on this resource." } });
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new Dictionary<string, string>() { { "Message", "Authorization has been denied for this request." } });
            }
        }
    }
}