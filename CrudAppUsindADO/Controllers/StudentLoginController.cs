using CrudAppUsindADO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CrudAppUsindADO.Controllers
{
    public class StudentLoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Valid()
        {
            StudentLoginDBContext dbContext = new StudentLoginDBContext();
            var model = new StudentLogin
            {
                Roles = dbContext.GetRoles()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Valid(StudentLogin model)
        {
            StudentLoginDBContext context = new StudentLoginDBContext();
            StudentLogin user = context.GetUserByEmailAndRole(model.Email, model.SelectedRoleId);

            if (user == null)
            {
                ViewBag.Message = "User not found!";
                model.Roles = context.GetRoles();
                return View(model);
            }
            string hashedPassword = HashPasswords(model.Password, user.Salt);
            Console.WriteLine($"Entered Password Hash: {hashedPassword}");
            Console.WriteLine($"Stored Password Hash: {user.Password}");

            if (hashedPassword == user.Password)
            {
               
                TempData["Email"] = user.Email;
                TempData["Role"] = model.SelectedRoleId == 1 ? "Admin" :
                                   model.SelectedRoleId == 2 ? "Student" : "Teacher";

                Console.WriteLine(hashedPassword);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Invalid password!";
                Console.WriteLine(hashedPassword);
            }

            model.Roles = context.GetRoles();
            return View(model);
        }


        private static string GenerateSalt(int size = 16)
        {
            byte[] salt = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private static string HashPasswords(string password, string salt)
        {
            var saltedPassword = password + salt;
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}

