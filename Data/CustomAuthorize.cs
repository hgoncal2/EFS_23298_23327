using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using EFS_23298_23327.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Net;


//https://stackoverflow.com/questions/73555584/c-sharp-net-6-0-how-to-redirect-an-unauthorized-user-to-an-unauthorizedpage
// Obrigado ao utilizador GuyonM
namespace EFS_23298_23327.Data
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public String Roles { get; set; }
        [TempData]
        public bool NotAuth { get; set; }

        /// <summary>
        /// Este método é chamado sempre que se acede a qualquer página/método.Dá override da função default do atributo "[Authorize]
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            //Dá return se tiver [AllowAnonymous] na action que está a tentar aceder
            //https://stackoverflow.com/a/63940184
            if (filterContext.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;
            var controller = filterContext.Controller as Controller;
            var controllerName= filterContext.Controller.ToString().Split(".").Last();
            var isAPI = controllerName.ToLower().EndsWith("api");
            //Se o utilizador não estiver autenticado
            if (filterContext.HttpContext.User.Identity == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (isAPI)
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                    return;
                }
                //Se não estiver autenticado e não estiver na scope do Accountcontroller(pagina de login,register,etc),redireciona para login
                if (controllerName != "AccountController") {
                    controller.TempData["Autenticado"] = false;
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new RedirectToActionResult("Login", "Account", new { area = "", unauth = true });
                }
               
            } else {
                //Se já estiver na scope do AccountController
                if (controllerName == "AccountController") {
                    filterContext.Result = new RedirectToActionResult("Index", "Home", new { area = "" });
                    return;
                }
                //Se estiver autenticado
                var user = filterContext.HttpContext.User;
                
                bool authorized = false;
                //Lista de Roles especificados no [Authorize],ex [CustomAuthorize("Admin,Cliente")]
                List<string> RoleList = Roles.Split(",").ToList();
                //Se pertencer a qualquer um dos roles especificados(OR)
                foreach (var item in RoleList)
                {
                    if (user.IsInRole(item)) {
                        authorized = true;


                    } 
                }
                //Se não estiver autorizado
                if (!authorized) {
                    
                    filterContext.HttpContext.Response.StatusCode = 401;
                    //Referer/Action default vai ser "Home"
                    var referer = "Home";
                    try {
                        //Se o utilizador tiver feito o request de uma página que não tem referer/action visivel(ex "http://inicio.com/{Index}" ao invés de "http://inicio.com/example")
                        //Se falhar o try catch,é porque não tem referer,logo vamos redirecioná-lo para a homepage
                        //Nem sempre o referer vem no header do http request,se não vier,redirecionamos para a homepage
                     referer = filterContext.HttpContext.Request.Headers["Referer"].ToString().Split("/")[3];

                    }catch(Exception) { 
                    
                    }
                    if(referer == "") {
                        
                        controller.TempData["Auth"] = false;
                        filterContext.Result = new RedirectToActionResult("Index", "Home", new { area = "" });

                    } else {
                        controller.TempData["Auth"] = false;
                        filterContext.Result = new RedirectToActionResult("Index", referer, new { area = "" });
                    }
                   
                }

                
            }

            
        }
    }
}

   