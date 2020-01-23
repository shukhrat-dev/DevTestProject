using DevTestProject.Models;
using DevTestProject.Services.Classes;
using DevTestProject.Services.Interfaces;
using DevTestProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _employeesService = new EmployeesService(); 
        private readonly ITeamsService _teamService = new TeamsService();
        // GET: Employees
        public ActionResult Index(int page = 1, int itemOnPage = 1, string message = null)
        {
            #region Pager
            int Page = page <= 1 ? 1 : page;
            int ItemOnPage = itemOnPage;
            #endregion

            List<EmployeesModel> employees = new List<EmployeesModel>();
            employees = _employeesService.GetAllEmployees();
            if (!employees.Any() || employees is null)
            {
                throw new NullReferenceException();
            }
            int employeesAmount = employees.Count();
            int employeesForOnePage = employeesAmount / itemOnPage;
            employees = employees.Take(employeesForOnePage).ToList();

            EmployeesVm model = new EmployeesVm()
            {
                Employees = employees,
                EmployeesForPage = employeesForOnePage,
                ItemOnPage = itemOnPage,
                Page = page
                
            };
            return View("Index", model);
        }
        public ActionResult Create()
        {
            List<SelectListItem> teamList = GetTeamList();
            EmployeesVm model = new EmployeesVm()
            {
                TeamList = teamList
            };

            return View("Create", model);
        }
        [HttpPost]
        public ActionResult Create(EmployeesVm model)
        {
            if (model is null)
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(model.Email)) //TODO RENDER SOME TEXT FOR SHOWING WHY USER CANT CREATE IT
            {
                return RedirectToAction("Create");
            }

            EmployeesModel employee = new EmployeesModel()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Team_Id = model.Team_Id,
            };
            _employeesService.Create(employee);
            return RedirectToAction("Index");

        }
        public ActionResult Edit(int employee_id)
        {
            
            EmployeesModel employee = _employeesService.GetEmployee(employee_id);
            List<SelectListItem> teamList = GetTeamList(employee.Team_Id);
            EmployeesVm employeeVm = new EmployeesVm()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Email = employee.Email,
                Team_Id = employee.Team_Id,
                TeamList = teamList

            };
            return View("Edit", employeeVm);
        }
        [HttpPost]
        public ActionResult SaveEditingEmployee(EmployeesVm model)
        {
            if (model is null)
            {
                return RedirectToAction("Index");
            }

            EmployeesModel employee = new EmployeesModel()
            {
                Id = model.Id,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Team_Id = model.Team_Id
            };

            _employeesService.Update(employee);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int employee_id)
        {
            EmployeesModel employee = _employeesService.GetEmployee(employee_id);
            _employeesService.Delete(employee);
            return RedirectToAction("Index");
        }


        #region Helper
        private List<SelectListItem> GetTeamList()
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
        private List<SelectListItem> GetTeamList(int teamId)
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
        #endregion
    }
}