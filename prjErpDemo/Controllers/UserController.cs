using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjErpDemo.Filters;
using prjErpDemo.Models;

namespace prjErpDemo.Controllers
{
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    public class UserController : Controller
    {
        private readonly db_ErpDemoContext _db;
        public UserController(db_ErpDemoContext db)
        {
            _db = db;
        }

        public IActionResult UserList()
        {
            IEnumerable<User> users = from u in _db.Users
                                      where u.Permission != 0
                                      select u;
            return View(users);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User userParam)
        {
            if (String.IsNullOrEmpty(userParam.AccountName) ||
                String.IsNullOrEmpty(userParam.Password) ||
                (userParam.Permission != 1 && userParam.Permission != 2))
            {
                return BadRequest("Please provide correct information");
            }
            User? userCheck = _db.Users.FirstOrDefault(u => u.AccountName == userParam.AccountName);

            if (userCheck != null)
            {
                return BadRequest("The account name is already taken.");
            }

            try
            {
                userParam.Password = CommonFn.ComputeSHA256Hash(userParam.Password);
                _db.Add(userParam);
                _db.SaveChanges();
                return RedirectToAction("UserList", "User");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong with the database: " + ex);
            }
        }

        // update user's permission
        public IActionResult UpdateUser(int id)
        {
            User? userToUpdate = _db.Users.FirstOrDefault(u => u.UserID == id);
            if (userToUpdate != null)
            {
                return View(userToUpdate);
            }

            return RedirectToAction("UserList", "User");
        }
        [HttpPost]
        public IActionResult UpdateUser(User userParam)
        {
            User? user = _db.Users.FirstOrDefault(u => u.UserID == userParam.UserID);
            if (user != null)
            {

                if (userParam.Permission != 1 && userParam.Permission != 2)
                {
                    return BadRequest("Please provide correct information");
                }


                try
                {
                    user.Permission = userParam.Permission;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(("Something went wrong with the database: " + ex));
                }

            }
            return RedirectToAction("UserList", "User");
        }
    }
}
