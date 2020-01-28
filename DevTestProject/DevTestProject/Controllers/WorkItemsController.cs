using DevTestProject.Models;
using DevTestProject.Services.Classes;
using DevTestProject.ViewModel;
using System;
using System.Collections.Generic;
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
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            List<WorkItemsModel> workItems = new List<WorkItemsModel>();
            try
            {
                workItems = _workItemService.GetAllWorkItems();
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            WorkItemsVm model = new WorkItemsVm()
            {
                 WorkItems = workItems,
                 ErrorMsg = errorMsg
            };
            return View("Index", model);
        }


        public ActionResult Create()
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            List<SelectListItem> projectsList = new List<SelectListItem>();
            List<SelectListItem> employeeList = new List<SelectListItem>();
            try
            {
                projectsList = Utils.Helper.GetProjectsList();

            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }           

            if(TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            WorkItemsVm model = new WorkItemsVm()
            {
                ProjectsList = projectsList,
                EmployeeList = employeeList,
                ErrorMsg = errorMsg
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
            if (String.IsNullOrWhiteSpace(model.Name) ||
                model.Project_Id == null || model.Project_Id == 0 ||
                model.Employee_Id == null || model.Employee_Id == 0 ||
                model.DateCreated == DateTime.MinValue || 
                (model.DateCreated == DateTime.MinValue && model.DateDue == DateTime.MinValue) || 
                model.DateCreated == DateTime.MinValue || model.DateDue == DateTime.MinValue || 
                DateTime.Compare(model.DateCreated, model.DateDue) > 0)
            {
                TempData["error"] = "You did not enter information correctly. Check the information and try again.";
                return RedirectToAction("Create");
            }

            if (model.DateStarted != null && model.DateFinished != null)
            {
                if (model.DateStarted.Value != DateTime.MinValue && model.DateFinished.Value != DateTime.MinValue)
                {
                    if (DateTime.Compare(model.DateStarted.Value, model.DateFinished.Value) > 0)
                    {
                        TempData["error"] = "You did not enter dates correctly. Check the dates and try again.";
                        return RedirectToAction("Create");
                    }
                }
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
                if(!_workItemService.Create(workItem))
                {
                    TempData["error"] = $"Problems with create work item (Service error \"Create\").";
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

        public ActionResult Edit(int workItem_id)
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            WorkItemsModel workItem = new WorkItemsModel();
            List<SelectListItem> projectsList = new List<SelectListItem>();
            List<SelectListItem> employeeList = new List<SelectListItem>();
            try
            {
                projectsList = Utils.Helper.GetProjectsList();
                employeeList = Utils.Helper.GetEmployeeList();
                workItem = _workItemService.GetWorkItem(workItem_id);
            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }


            WorkItemsVm model = new WorkItemsVm()
            {
                Id = workItem.Id,
                Name = workItem.Name,
                Description = workItem.Description,
                Project_Id = workItem.Project_Id,
                Employee_Id = workItem.Employee_Id,
                DateCreated = workItem.DateCreated,
                DateStarted = workItem.DateStarted,
                DateFinished = workItem.DateFinished,
                DateDue = workItem.DateDue,
                ProjectsList = projectsList,
                EmployeeList = employeeList,
                ErrorMsg = errorMsg
            };
            return View("Edit", model);
        }
        
        public ActionResult SaveEditingWorkItem(WorkItemsVm model)
        {
            if (model is null)
            {
                return RedirectToAction("Edit", new { workItem_id  = model.Id} );
            }
            if (String.IsNullOrWhiteSpace(model.Name) ||
                model.Project_Id == null || model.Project_Id == 0 ||
                model.Employee_Id == null || model.Employee_Id == 0 ||
                model.DateCreated == DateTime.MinValue ||
                (model.DateCreated == DateTime.MinValue && model.DateDue == DateTime.MinValue) ||
                model.DateDue == DateTime.MinValue || DateTime.Compare(model.DateCreated, model.DateDue) > 0)
            {
                TempData["error"] = "You did not enter information correctly. Check the information and try again.";
                return RedirectToAction("Edit", new { workItem_id = model.Id });
            }


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
                if(!_workItemService.Update(workItem))
                {
                    TempData["error"] = $"Problems with updating work item (Service error \"Update/Edit\").";
                    return RedirectToAction("Edit", new { workItem_id = model.Id });
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Edit", new { workItem_id = model.Id });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int workItem_id)
        {
            try
            {
                if(!_workItemService.Delete(workItem_id))
                {
                    TempData["error"] = $"Problems with deleting work item (Service error \"Delete\").";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services) or deleting information. {e.Message}";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Method for getting all employees who are working on project via AJAX
        /// </summary>
        /// <param name="project_id"></param>
        /// <returns></returns>
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

    }
}