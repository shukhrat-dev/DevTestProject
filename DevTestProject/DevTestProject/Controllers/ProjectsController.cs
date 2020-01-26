using DevTestProject.Models;
using DevTestProject.Services.Classes;
using DevTestProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectsService _projectService = new ProjectsService();
        private readonly EmployeesService _employeeService = new EmployeesService();
        // GET: Teams
        public ActionResult Index()
        {
            List<ProjectsModel> projects = new List<ProjectsModel>();
            Dictionary<string, string> teamsOnProject = new Dictionary<string, string>();
            try
            {
                projects = _projectService.GetAllProjects();
                teamsOnProject = _projectService.GetTeamWorkedOnProject();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            ProjectsVm model = new ProjectsVm()
            {
                ProjectList = projects,
                TeamsOnProject = teamsOnProject
            };
            return View("Index", model);
        }

        public ActionResult Create()
        {
            //List<EmployeesModel> employees = new List<EmployeesModel>();

            //employees = _employeeService.GetAllEmployees();
            //foreach (var employee in employees)
            //{
            //    employeeList.Add(
            //        new SelectListItem
            //        {
            //            Text = employee.Id.ToString(),
            //            Value = employee.Id.ToString()
            //        });
            //}
            List<SelectListItem> employeeList = GetEmployeeList();
            ProjectsVm model = new ProjectsVm()
            {
                EmployeeList = employeeList
            };
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(ProjectsVm model)
        {
            if (model is null || model.DateStart == DateTime.MinValue || model.DateDue == DateTime.MinValue || DateTime.Compare(model.DateStart, model.DateDue) > 0)
            {
                return RedirectToAction("Create");
            }
            ProjectsModel project = new ProjectsModel()
            {
                Name = model.Name,
                ProjectManager_Id = model.ProjectManager_Id,
                DateStart = model.DateStart.Date,
                DateDue = model.DateDue.Date
            };
            try
            {
                _projectService.Create(project);
            }
            catch (Exception)
            {

                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int project_id)
        {
            ProjectsModel project = new ProjectsModel();
            List<SelectListItem> employeeList = GetEmployeeList();
            ProjectsVm model = new ProjectsVm();
            try
            {
                project = _projectService.GetProject(project_id);
                model.Id = project.Id;
                model.Name = project.Name;
                model.ProjectManager_Id = project.ProjectManager_Id;
                model.DateStart = project.DateStart;
                model.DateDue = project.DateDue;
                model.EmployeeList = employeeList;
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }

        public ActionResult SaveEdititngProject(ProjectsVm model)
        {
            ProjectsModel project = new ProjectsModel()
            {
                Id = model.Id,
                Name = model.Name,
                ProjectManager_Id = model.ProjectManager_Id,
                DateStart = model.DateStart,
                DateDue = model.DateDue
            };

            try
            {
                _projectService.Update(project);
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int project_id)
        {
            try
            {
                _projectService.Delete(project_id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetEmployeeList()
        {
            List<EmployeesModel> employees = new List<EmployeesModel>();
            List<SelectListItem> employeeList = new List<SelectListItem>();
            employees = _employeeService.GetAllEmployees();
            foreach (var employee in employees)
            {
                employeeList.Add(
                    new SelectListItem
                    {
                        Text = employee.Id.ToString(),
                        Value = employee.Id.ToString()
                    });
            }

            return employeeList;
        }
    }
}
