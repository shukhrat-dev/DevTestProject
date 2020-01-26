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
    public class ProjectCooperationsController : Controller
    {
        private readonly ProjectCooperationService _projectCooperaionsService = new ProjectCooperationService();
        private readonly ProjectsService _projectService = new ProjectsService();
        private readonly TeamsService _teamsService = new TeamsService();
        // GET: ProjectCooperations
        public ActionResult Index()
        {
            List<ProjectCooperaionsModel> projectCooperations = new List<ProjectCooperaionsModel>();


            try
            {
                projectCooperations = _projectCooperaionsService.GetAllCooperations();

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            ProjectCooperationsVm model = new ProjectCooperationsVm()
            {
                ProjectCooperations = projectCooperations,

            };
            return View("Index", model);
        }


        public ActionResult Create()
        {
            List<SelectListItem> projectList = GetProjectList();
            List<SelectListItem> teamList = GetTeamList();
            
            
            ProjectsVm model = new ProjectsVm()
            {
                
            };
            return View("Create", model);
        }



        private List<SelectListItem> GetProjectList()
        {
            List<ProjectsModel> projects = new List<ProjectsModel>();
            List<SelectListItem> projectList = new List<SelectListItem>();
            projects = _projectService.GetAllProjects();
            foreach (var project in projects)
            {
                projectList.Add(
                    new SelectListItem
                    {
                        Text = project.Id.ToString(),
                        Value = project.Id.ToString()
                    });
            }

            return projectList;
        }

        private List<SelectListItem> GetTeamList()
        {
            List<TeamsModel> teams = new List<TeamsModel>();
            List<SelectListItem> teamsList = new List<SelectListItem>();
            teams = _teamsService.GetTeams();
            foreach (var team in teams)
            {
                teamsList.Add(
                    new SelectListItem
                    {
                        Text = team.Id.ToString(),
                        Value = team.Id.ToString()
                    });
            }

            return teamsList;
        }
    }
}