using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using prjErpDemo.Models;
using System.Text.Json;

namespace prjErpDemo.Filters
{
    public class CustomAuthorizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Session.Keys.Contains("Login"))
            {
                context.Result = new RedirectResult("/Home/Login");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            string? userStr = context.HttpContext.Session.GetString("Login");
            if (!string.IsNullOrEmpty(userStr))
            {
                User? user = JsonSerializer.Deserialize<User>(userStr);
                if (user != null)
                {
                    if (context.RouteData.Values["controller"] != null)
                    {
                        string? controllerName = context.RouteData.Values["controller"]?.ToString();
                        if (controllerName == "User")
                        {
                            if (user.Permission == 1 || user.Permission == 2)
                            {
                                context.Result = new RedirectResult("/Home/Index");
                            }
                            else
                            {
                                // permission granted    
                            }
                        }
                    }
                }
            }
        }

    }

}
