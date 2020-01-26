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
    public class WorkItemsController : Controller
    {
        private readonly WorkItemsService _workItemService = new WorkItemsService();
        private readonly ProjectsService _projectService = new ProjectsService();
        private readonly EmployeesService _employeesService = new EmployeesService();
        // GET: WorkItems
        public ActionResult Index()
        {
            List<WorkItemsModel> workItems = new List<WorkItemsModel>();

            try
            {
                workItems = _workItemService.GetAllWorkItems();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            WorkItemsVm model = new WorkItemsVm()
            {
                 WorkItems = workItems
            };
            return View("Index", model);
        }


        public ActionResult Create()
        {
            List<SelectListItem> projectsList = GetProjectsList();
            List<SelectListItem> employeeList = new List<SelectListItem>();
            WorkItemsVm model = new WorkItemsVm()
            {
                ProjectsList = projectsList,
                EmployeeList = employeeList
            };
            

            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(WorkItemsVm model)
        {
            if (model is null)
            {
                return RedirectToAction("Create");
            }
            WorkItemsModel workItem = new WorkItemsModel()
            {
                Name = model.Name,
                Description = model.Description,
                Project_Id = model.Project_Id,
                Employee_Id = model.Employee_Id,
                DateCreated = model.DateCreated,
                DateStarted = model.DateStarted,
                DateFinished = model.DateFinished,
                DateDue = model.DateDue
            };
            try
            {
                _workItemService.Create(workItem);
            }
            catch (Exception)
            {

                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int workItem_id)
        {
            WorkItemsModel workItem = new WorkItemsModel();
            List<SelectListItem> projectsList = GetProjectsList();
            List<SelectListItem> employeeList = GetEmployeeList();
            
            WorkItemsVm model = new WorkItemsVm();
            try
            {
                workItem = _workItemService.GetWorkItem(workItem_id);
                model.Id = workItem.Id;
                model.Name = workItem.Name;
                model.Description = workItem.Description;
                model.Project_Id = workItem.Project_Id;
                model.Employee_Id = workItem.Employee_Id;
                model.DateCreated = workItem.DateCreated;
                model.DateStarted = workItem.DateStarted;
                model.DateFinished = workItem.DateFinished;
                model.DateDue = workItem.DateDue;
                model.ProjectsList = projectsList;
                model.EmployeeList = employeeList;

            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }
        
        public ActionResult SaveEditingWorkItem(WorkItemsVm model)
        {
            WorkItemsVm workItem = new WorkItemsVm()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Project_Id = model.Project_Id,
                Employee_Id = model.Employee_Id,
                DateCreated = model.DateCreated,
                DateStarted = model.DateStarted,
                DateFinished = model.DateFinished,
                DateDue = model.DateDue
        };

            try
            {
                _workItemService.Update(workItem);
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int workItem_id)
        {
            try
            {
                _workItemService.Delete(workItem_id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetEmployeesFromProject(string project_id)
        {
            if(string.IsNullOrWhiteSpace(project_id))
            {
                return Json( new { success = false}); 
            }
            int projectId = int.Parse(project_id);
            List<int> employeesId = _workItemService.GetAllEmployeeFromProject(projectId);
            return Json(new { empId = employeesId });
        }


        private List<SelectListItem> GetProjectsList()
        {
            List<ProjectsModel> projects = new List<ProjectsModel>();
            List<SelectListItem> projectsList = new List<SelectListItem>();
            projects = _projectService.GetAllProjects();
            foreach (var project in projects)
            {
                projectsList.Add(
                    new SelectListItem
                    {
                        Text = project.Id.ToString(),
                        Value = project.Id.ToString()
                    });
            }

            return projectsList;
        }

        private List<SelectListItem> GetEmployeeList()
        {
            List<EmployeesModel> employees = new List<EmployeesModel>();
            List<SelectListItem> employeeList = new List<SelectListItem>();
            employees = _employeesService.GetAllEmployees();
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