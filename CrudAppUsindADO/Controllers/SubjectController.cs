using CrudAppUsindADO.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudAppUsindADO.Controllers
{
    public class SubjectController : Controller
    {
        public IActionResult Index(SubjectDBContext subjectDBContext)
        {

            List<Subject> obj = subjectDBContext.GetSubjects();
            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Subject sub)
        {
            SubjectDBContext context = new SubjectDBContext();
            bool check = context.AddSubject(sub);
            if (check == true)
            {
                TempData["InsertMessage"] = "Data has been inserted ";
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int Id)
        {
            SubjectDBContext context = new SubjectDBContext();
            var subject = context.GetSubjects().Where(x => x.SubjectID == Id).FirstOrDefault();
            return View(subject);
        }
        [HttpPost]
        public IActionResult Edit(Subject sub)
        {
            SubjectDBContext context = new SubjectDBContext();
            bool check = context.UpdateSubject(sub);
            if (check)
            {
                TempData["UpdateMessage"] = "Data has been updated";
                return RedirectToAction("Index");
            }

            return View(sub);
        }
        public ActionResult Delete(int Id)
        {
            SubjectDBContext context = new SubjectDBContext();
            var subject = context.GetSubjects().Where(x => x.SubjectID == Id).SingleOrDefault();
            return View(subject);
        }
        [HttpPost]
        public IActionResult Delete(int id,Subject sub)
        {
            SubjectDBContext context =new SubjectDBContext();
            bool check = context.DeleteSubjects(id);
            if (check == true)
            {
                return RedirectToAction("Index");
            }

            return View(sub);
        }
    }
}
