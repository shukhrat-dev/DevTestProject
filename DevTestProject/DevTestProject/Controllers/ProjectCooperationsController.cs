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
            ProjectCooperationsVm model = new ProjectCooperationsVm()
            {
                ProjectList = projectList,
                TeamList = teamList
            };
            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Create(ProjectCooperationsVm model)
        {
            //TODO ОБРАБОТАТЬ ВХОДЯЩИЙ ЗАПРОС И ПРОВЕРИТЬ ЕСЛИ ВХОДЯЩИЙ ЗАПРОС УЖЕ ЕСТЬ В БД ТО ПЕРЕЕДРЕСОВАТЬ ОБРАТНО
            //(ТО ЕСТЬ ПОСМОТРЕТЬ ЕСЛИ КОМАНДА УЖЕ РАБОТАЕТ НАД ЭТИМ ПРОЕКТОМ)

            if (model is null || model.DateAssigned == DateTime.MinValue)
            {
                return RedirectToAction("Create");
            }
            ProjectCooperaionsModel projectCooperations = new ProjectCooperaionsModel()
            {
                Project_Id = model.Project_Id,
                Team_Id = model.Team_Id,
                DateAssigned = model.DateAssigned
            };
            try
            {
                
                _projectCooperaionsService.Check(projectCooperations); // Проверить существует ли уже данная комбинация проект - команда, если да, то редирект обратно
                _projectCooperaionsService.Create(projectCooperations);
            }
            catch (Exception)
            {

                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");

            return Redirect("Index");
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