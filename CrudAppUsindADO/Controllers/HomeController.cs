using CrudAppUsindADO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CrudAppUsindADO.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _ImageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            EmployeeDbContext employeeDbContext = new EmployeeDbContext();
            List<Employee> obj = employeeDbContext.GetEmployees();

            return View(obj);
        }

        public IActionResult Create()
        {
            EmployeeDbContext context = new EmployeeDbContext();
            var subjects = context.GetAllSubjects();
            var viewModel = new EmployeeCreateViewModel
            {
                Employee = new Employee(),
                AllSubjects = subjects
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel viewModel)
        {
            
            string salt = GenerateSalt();
            string hashedPassword = HashPasswords(viewModel.Employee.Password, salt);


            viewModel.Employee.HashPassword = hashedPassword;
            viewModel.Employee.Salt = salt;

            EmployeeDbContext context = new EmployeeDbContext();

            bool check = context.AddEmployee(viewModel.Employee);

            if (check)
            {
                foreach (var SubjectId in viewModel.SelectedSubjectIds)
                {
                    context.AssignSubjectToEmployee(viewModel.Employee.Id, SubjectId);
                }

                TempData["InsertMessage"] = "Data has been inserted";
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("Employee.Id", "An employee with this ID already exists. Please enter a different ID.");

                return View(viewModel);
            }
        }



        public ActionResult Edit(int id)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            var employee = context.GetEmployees().Where(x => x.Id == id).SingleOrDefault();
            var allSubjects = context.GetAllSubjects();
            var employeeSubjects = context.GetEmployeeSubjects().Where(x => x.EmployeeName == employee.Name).Select(x => x.SubjectName).ToList();
            var viewModel = new EmployeeCreateViewModel
            {
                Employee = employee,
                AllSubjects = allSubjects,
                SelectedSubjectIds = employeeSubjects.Select(s => allSubjects.First(subject => subject.SubjectName == s).SubjectID).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeCreateViewModel emp)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            bool check = context.UpdateEmployee(emp.Employee);
            if (check)
            {
                context.DeleteEmployeeSubjects(emp.Employee.Id);
                foreach (var subjectId in emp.SelectedSubjectIds)
                {
                    context.AssignSubjectToEmployee(emp.Employee.Id, subjectId);
                }
                TempData["UpdateMessage"] = "Data has been updated";
                return RedirectToAction("Index");
            }

            return View(emp);
        }
        public ActionResult Details(int id)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            var employee = context.GetEmployees().Where(x => x.Id == id).SingleOrDefault();
            var employeeSubjects = context.GetEmployeeSubjects().Where(x => x.EmployeeName == employee.Name).ToList();

            var viewModel = new EmployeeDetailsViewModel
            {
                Employee = employee,
                Subjects = employeeSubjects
            };

            return View(viewModel);
        }

        public ActionResult Delete(int id)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            var employee = context.GetEmployees().Where(x => x.Id == id).SingleOrDefault();
            return View(employee);
        }
        [HttpPost]
        public IActionResult Delete(Employee emp, int id)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            bool check = context.DeleteEmployee(id);
            if (check == true)
            {
                TempData["UpdateMessage"] = "Data has been updated";
                return RedirectToAction("Index");
            }

            return View(emp);
        }

        public IActionResult addData()
        {
            return View();
        }

        //Hasing & Salting code 

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
                byte[] bytes=sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
