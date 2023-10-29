using Microsoft.AspNetCore.Mvc;
using prjErpDemo.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace prjErpDemo.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckLoginStatusAPI : ControllerBase
    {
        // GET: api/<CheckLoginStatusAPIController>
        [HttpGet]
        public IActionResult Get()
        {

            if (HttpContext.Session.Keys.Contains("Login"))
            {
                string? jsonStr = HttpContext.Session.GetString("Login");
                if (jsonStr != null)
                {
                    User? loginUser = JsonSerializer.Deserialize<User>(jsonStr);
                    if (loginUser != null)
                    {
                        return Ok(new { 
                            status = "login",
                            permission = loginUser.Permission
                        });
                    }

                }
                return BadRequest(new { status = "No user found" });

            }
            else
            {
                return BadRequest(new { status = "No user found" });
            }
        }
    }
}
