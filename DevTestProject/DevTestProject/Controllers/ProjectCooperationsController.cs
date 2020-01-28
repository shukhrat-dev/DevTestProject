using DevTestProject.Models;
using DevTestProject.Services.Classes;
using DevTestProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DevTestProject.Controllers
{
    public class ProjectCooperationsController : Controller
    {
        private readonly ProjectCooperationService _projectCooperaionsService = new ProjectCooperationService();
        private readonly ProjectsService _projectService = new ProjectsService();
        private readonly TeamsService _teamsService = new TeamsService();
        // GET: ProjectCooperations
        public ActionResult Index()
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            List<ProjectCooperaionsModel> projectCooperations = new List<ProjectCooperaionsModel>();
            List<ProjectsModel> allProjects = new List<ProjectsModel>();
            List<TeamsModel> allTeams = new List<TeamsModel>();

            try
            {
                projectCooperations = _projectCooperaionsService.GetAllCooperations();
                allProjects = _projectService.GetAllProjects();
                allTeams = _teamsService.GetTeams();

            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            ProjectCooperationsVm model = new ProjectCooperationsVm()
            {
                ProjectCooperations = projectCooperations,
                AllProjects = allProjects,
                AllTeams = allTeams,
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
            List<SelectListItem> projectList = new List<SelectListItem>();
            List<SelectListItem> teamList = new List<SelectListItem>();
            try
            {
                projectList = Utils.Helper.GetProjectsList();
                teamList = Utils.Helper.GetTeamList();
            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }

            ProjectCooperationsVm model = new ProjectCooperationsVm()
            {
                ProjectList = projectList,
                TeamList = teamList,
                ErrorMsg = errorMsg
            };
            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Create(ProjectCooperationsVm model)
        {
            if (model is null || model.DateAssigned == DateTime.MinValue || model.Project_Id == 0 || model.Project_Id == null || model.Team_Id == 0 || model.Team_Id == null)
            {
                TempData["error"] = $"All data must be filled";
                return RedirectToAction("Create");
            }

            ProjectCooperaionsModel projectCooperations = new ProjectCooperaionsModel()
            {
                Project_Id = model.Project_Id,
                Team_Id = model.Team_Id,
                DateAssigned = model.DateAssigned,
            };
            try
            {
                
                if(!_projectCooperaionsService.CheckIfRecordExist(projectCooperations))
                {
                    if(!_projectCooperaionsService.Create(projectCooperations))
                    {
                        TempData["error"] = $"Problems with create project cooperation (Service error \"Create\").";
                        return RedirectToAction("Create");
                    }
                } 
                else
                {
                    TempData["error"] = $"This combination already exists. You are trying to duplicate (Service error \"Create\").";
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



        public ActionResult Edit(int projectCooperations_id)
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            ProjectCooperaionsModel projectCooperations = new ProjectCooperaionsModel();
            List<SelectListItem> projectList = new List<SelectListItem>();
            List<SelectListItem> teamList = new List<SelectListItem>();
            try
            {
                projectList = Utils.Helper.GetProjectsList();
                teamList = Utils.Helper.GetTeamList();
                projectCooperations = _projectCooperaionsService.GetProjectCooperation(projectCooperations_id);
            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }


            ProjectCooperationsVm model = new ProjectCooperationsVm()
            {
                ProjectList = projectList,
                TeamList = teamList,
                Id = projectCooperations.Id,
                Project_Id = projectCooperations.Project_Id,
                Team_Id = projectCooperations.Team_Id,
                DateAssigned = projectCooperations.DateAssigned,
                ErrorMsg = errorMsg
            };


            return View("Edit", model);
        }

        public ActionResult SaveEdititngProjectCooperations(ProjectCooperationsVm model)
        {

            if (model is null || model.DateAssigned == DateTime.MinValue || model.Project_Id == 0 || model.Project_Id == null || model.Team_Id == 0 || model.Team_Id == null)
            {
                TempData["error"] = $"Problems with saving information to database (services).";
                return RedirectToAction("Edit", new { projectCooperations_id = model.Id });
            }
            ProjectCooperaionsModel projectCooperations = new ProjectCooperaionsModel()
            {
                Id = model.Id,
                Project_Id = model.Project_Id,
                Team_Id = model.Team_Id,
                DateAssigned = model.DateAssigned
            };

            try
            {
                if(!_projectCooperaionsService.Update(projectCooperations))
                {
                    TempData["error"] = $"Problems with updating project cooperation (Service error \"Update/Edit\").";
                    return RedirectToAction("Edit", new { projectCooperations_id = model.Id });
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Edit", new { projectCooperations_id = model.Id });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int projectCooperations_id)
        {
            try
            {
                if(!_projectCooperaionsService.Delete(projectCooperations_id))
                {
                    TempData["error"] = $"Problems with deleting project cooperation (Service error \"Delete\").";
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
    }
}