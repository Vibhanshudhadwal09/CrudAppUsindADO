﻿using CrudAppUsindADO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CrudAppUsindADO.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            EmployeeDbContext employeeDbContext = new EmployeeDbContext();
            List<Employee> obj = employeeDbContext.GetEmployees();

            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            bool check = context.AddEmployee(emp);
            if (check)
            {
                TempData["InsertMessage"] = "Data has been inserted ";
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            var employee = context.GetEmployees().Where(x => x.Id == id).SingleOrDefault();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            bool check = context.UpdateEmployee(emp);
            if (check)
            {
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
        public IActionResult Delete(Employee emp,int id)
        {
            EmployeeDbContext context = new EmployeeDbContext();
            bool check = context.DeleteEmployee(id);
            if (check)
            {
                TempData["UpdateMessage"] = "Data has been updated";
                return RedirectToAction("Index");
            }

            return View(emp);
        }

    }
}