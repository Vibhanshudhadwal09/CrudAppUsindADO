using CrudAppUsindADO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CrudAppUsindADO.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
           TeacherDbContext dbContext = new TeacherDbContext();
            List<Teachers> obj = dbContext.GetTeachers();
            return View(obj);
        }

        public IActionResult Create()
        {
            TeacherDbContext context =new TeacherDbContext();
            var students=context.GetAllStudents();
            var viewModel = new TeacherCreateViewModel
            {
                Teachers = new Teachers(),
                AllStudents = students
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(TeacherCreateViewModel viewModel)
        {

            string salt = GenerateSalt();
            string hashedPassword = HashPasswords(viewModel.Teachers.Password, salt);


            viewModel.Teachers.HashPassword= hashedPassword;
            viewModel.Teachers.Salt= salt;

            TeacherDbContext teacherDbContext = new TeacherDbContext();
            bool check = teacherDbContext.AddTeacher(viewModel.Teachers);
            if (check ) 
            {
                foreach (var studentId in viewModel.SelectedEmployeeIds)
                {
                    teacherDbContext.AssignStudentToTeacher(viewModel.Teachers.TeacherId,studentId);
                }
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            TeacherDbContext teacherDbContext = new TeacherDbContext();
            var teacher = teacherDbContext.GetTeachers().Where(x => x.TeacherId == id).SingleOrDefault();
            var students = teacherDbContext.GetAllStudents();
            var selectedStudentIds = teacherDbContext.GetTeacherStudents()
                                                     .Where(x => x.TeachersName == teacher.TeacherName)
                                                     .Select(x => x.StudentId)
                                                     .ToList();

            var viewModel = new TeacherCreateViewModel
            {
                Teachers = teacher,
                AllStudents = students,
                SelectedEmployeeIds = selectedStudentIds
            };

            return View(viewModel);
        }


        [HttpPost]
        [HttpPost]
        public IActionResult Edit(TeacherCreateViewModel viewModel)
        {
            TeacherDbContext teacherDbContext = new TeacherDbContext();
            bool check = teacherDbContext.EditTeacher(viewModel.Teachers);
            if (check)
            {
               
                teacherDbContext.RemoveStudentsFromTeacher(viewModel.Teachers.TeacherId);

                foreach (var studentId in viewModel.SelectedEmployeeIds)
                {
                    teacherDbContext.AssignStudentToTeacher(viewModel.Teachers.TeacherId, studentId);
                }

                ModelState.Clear();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public IActionResult Delete(int id)
        {
            TeacherDbContext teacherDbContext = new TeacherDbContext();
            var teacher = teacherDbContext.GetTeachers().Where(x => x.TeacherId == id).SingleOrDefault();
            return View(teacher);
        }
        [HttpPost]
        public IActionResult Delete(Teachers teachers,int id)
        {
            TeacherDbContext teacherDbContext = new TeacherDbContext();
            bool check= teacherDbContext.DeleteTeacher(id);
            if(check == true)
            {
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            return View(teachers);
        }

        public IActionResult Details(int id)
        {
            TeacherDbContext teacherDbContext = new TeacherDbContext();
            var teacher = teacherDbContext.GetTeachers().Where(x => x.TeacherId == id).SingleOrDefault();
            var teachersubjects= teacherDbContext.GetTeacherStudents().Where(x=>x.TeachersName==teacher.TeacherName).ToList();

            var viewModel = new TeachersDetailViewModel
            {
                Teacher = teacher,
                Students = teachersubjects
            };
            return View(viewModel);
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
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(bytes);
            }
        }

    }
}
