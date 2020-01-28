using DevTestProject.Models;
using DevTestProject.Services.Classes;
using DevTestProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DevTestProject.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ProjectsService _projectService = new ProjectsService();
        private readonly EmployeesService _employeeService = new EmployeesService();
        private readonly WorkItemsService _workItemsService = new WorkItemsService();
        // GET: Teams
        public ActionResult Index()
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            List<ProjectsModel> projects = new List<ProjectsModel>();
            List<WorkItemsModel> workItems = new List<WorkItemsModel>();
            List<EmployeesModel> employees = new List<EmployeesModel>();
            Dictionary<int, int> workItemsOnProject = new Dictionary<int, int>();
            try
            {
                projects = _projectService.GetAllProjects();
                workItems = _workItemsService.GetAllWorkItems();
                employees = _employeeService.GetAllEmployees();
                
                foreach (var workItem in workItems) 
                {
                    foreach(var project in projects)
                    {

                        if (project.Id == workItem.Project_Id)
                        {
                            if(workItemsOnProject.ContainsKey(project.Id))
                            {
                                workItemsOnProject[project.Id] += 1;
                            }
                            else
                            {
                                workItemsOnProject.Add(project.Id, 1);
                            }

                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            ProjectsVm model = new ProjectsVm()
            {
                ProjectList = projects,
                WorkItemsOnProject = workItemsOnProject,
                Employees = employees,
                ErrorMsg = errorMsg
            };
            return View("Index", model);
        }


        public ActionResult Create()
        {
            string errorMsg = String.Empty;
            if(TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            List<SelectListItem> employeeList = new List<SelectListItem>();
            try
            {
                employeeList = Utils.Helper.GetEmployeeList();
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            ProjectsVm model = new ProjectsVm()
            {
                EmployeeList = employeeList,
                ErrorMsg = errorMsg
            };
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(ProjectsVm model)
        {
            if (model is null || String.IsNullOrWhiteSpace(model.Name) || 
                model.ProjectManager_Id == 0 || model.ProjectManager_Id == null || 
                model.DateStart == DateTime.MinValue || model.DateDue == DateTime.MinValue || 
                DateTime.Compare(model.DateStart, model.DateDue) > 0)
            {
                TempData["error"] = "You did not enter dates correctly or you did not fill in some fields. All fields are required.. Check the dates and try again.";
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
                if(!_projectService.Create(project))
                {
                    TempData["error"] = $"Problems with create project (Service error \"Create\").";
                    return RedirectToAction("Create");
                }
            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with saving information to database (services). {e.Message}";
                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int project_id)
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            ProjectsModel project = new ProjectsModel();
            List<SelectListItem> employeeList = new List<SelectListItem>();
            try
            {
                employeeList = Utils.Helper.GetEmployeeList();
                project = _projectService.GetProject(project_id);
            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            ProjectsVm model = new ProjectsVm();
            model.Id = project.Id;
            model.Name = project.Name;
            model.ProjectManager_Id = project.ProjectManager_Id;
            model.DateStart = project.DateStart;
            model.DateDue = project.DateDue;
            model.EmployeeList = employeeList;
            model.ErrorMsg = errorMsg;
            return View("Edit", model);
        }

        public ActionResult SaveEdititngProject(ProjectsVm model)
        {
            if (model is null || String.IsNullOrWhiteSpace(model.Name) ||
                model.ProjectManager_Id == 0 || model.ProjectManager_Id == null ||
                model.DateStart == DateTime.MinValue || model.DateDue == DateTime.MinValue ||
                DateTime.Compare(model.DateStart, model.DateDue) > 0)
            {
                TempData["error"] = "You did not enter dates correctly or you did not fill in some fields. All fields are required.. Check the dates and try again.";
                return RedirectToAction("Edit", new { project_id = model.Id });
            }
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
                if (!_projectService.Update(project))
                {
                    TempData["error"] = $"Problems with updating project info (Service error \"Update/Edit\").";
                    return RedirectToAction("Edit", new { project_id = model.Id});
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Edit", model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int project_id)
        {
            try
            {
                if(!_projectService.Delete(project_id))
                {
                    TempData["error"] = $"Problems with deleting project (Service error \"Delete\"). To delete a project, you must remove/move the working teams(employees) from this project";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


    }
}
