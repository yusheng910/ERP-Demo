using Newtonsoft.Json;
using prjErpDemo.Models;

namespace prjErpDemo.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Session.Keys.Contains("Login"))
            {
                await _next(context); // proceed handling request
            }
            else
            {
                context.Response.Redirect("/Home/Login"); // No Session, redirect to login
            }
        }
    }
}
