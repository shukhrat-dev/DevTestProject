using DevTestProject.Models;
using DevTestProject.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.Utils
{
    public static class Helper
    {
        private static TeamsService _teamService = new TeamsService();
        private static EmployeesService _employeesService = new EmployeesService();
        private static ProjectsService _projectService = new ProjectsService();
        public static List<SelectListItem> GetTeamList()
        {
            List<TeamsModel> teams = new List<TeamsModel>();
            List<SelectListItem> teamList = new List<SelectListItem>();
            teams = _teamService.GetTeams();
            foreach (var team in teams)
            {
                teamList.Add(
                    new SelectListItem
                    {
                        Text = team.Id.ToString(),
                        Value = team.Id.ToString(),

                    });
            }
            return teamList;
        }
        public static List<SelectListItem> GetTeamList(int teamId)
        {
            List<TeamsModel> teams = new List<TeamsModel>();
            List<SelectListItem> teamList = new List<SelectListItem>();
            teams = _teamService.GetTeams();
            foreach (var team in teams)
            {
                if (teamId == team.Id)
                {
                    teamList.Add(
                    new SelectListItem
                    {
                        Text = team.Id.ToString(),
                        Value = team.Id.ToString(),
                        Selected = true
                    });
                }
                else
                {
                    teamList.Add(
                    new SelectListItem
                    {
                        Text = team.Id.ToString(),
                        Value = team.Id.ToString()
                    });
                }

            }
            return teamList;
        }

        public static List<SelectListItem> GetEmployeeList()
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




        public static List<SelectListItem> GetProjectsList()
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

    }
}