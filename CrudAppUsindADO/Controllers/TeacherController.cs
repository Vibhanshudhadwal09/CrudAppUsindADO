using CrudAppUsindADO.Models;
using Microsoft.AspNetCore.Mvc;

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
            TeacherDbContext teacherDbContext=new TeacherDbContext();
            var teacher = teacherDbContext.GetTeachers().Where(x => x.TeacherId == id).SingleOrDefault();
            return View(teacher);
        }

        [HttpPost]
        public IActionResult Edit(Teachers teachers)
        {
            TeacherDbContext teacherDbContext = new TeacherDbContext();
            bool check = teacherDbContext.EditTeacher(teachers);
            if(check == true)
            {
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            return View(teachers);
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


    }
}
