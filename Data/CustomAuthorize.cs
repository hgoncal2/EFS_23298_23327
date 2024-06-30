using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using EFS_23298_23306.Controllers;


//https://stackoverflow.com/questions/73555584/c-sharp-net-6-0-how-to-redirect-an-unauthorized-user-to-an-unauthorizedpage
// Obrigado ao utilizador GuyonM
namespace EFS_23298_23306.Data
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public String Roles { get; set; }
        [TempData]
        public bool NotAuth { get; set; }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if user isn't logged in.
            if (filterContext.HttpContext.User.Identity == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new RedirectToActionResult("Login", "Account", new { unauth = true });
            } else {
                var user = filterContext.HttpContext.User;
                //Check user rights here
                bool authorized = false;
                List<string> RoleList = Roles.Split(",").ToList();
                foreach (var item in RoleList)
                {
                    if (user.IsInRole(item)) {
                        authorized = true;


                    } 
                }
                if (!authorized) {
                    var s = filterContext.HttpContext.Request.HttpContext.Request.Path.ToString();
                    filterContext.HttpContext.Response.StatusCode = 401;
                    var controller = filterContext.Controller as Controller;
                    var referer = filterContext.HttpContext.Request.Headers["Referer"].ToString().Split("/")[3];
                    controller.TempData["Auth"] = false;
                    filterContext.Result = new RedirectToActionResult("Index", referer, new { });
                }

                
            }

            
        }
    }
}

   