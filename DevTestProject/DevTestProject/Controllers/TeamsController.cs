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
    public class TeamsController : Controller
    {
        private readonly TeamsService _teamService = new TeamsService();
        // GET: Teams
        public ActionResult Index()
        {
            List<TeamsModel> teams = new List<TeamsModel>();
            Dictionary<string, int> teamProjects = new Dictionary<string, int>();
            try
            {
                teams = _teamService.GetTeams();
                teamProjects = _teamService.GetTeamsProjectsCount();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
            TeamsVm model = new TeamsVm()
            {
                TeamList = teams,
                TeamProjects = teamProjects
            };
            return View("Index", model);
        }

        public ActionResult Create()
        {
            TeamsVm model = new TeamsVm();
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(TeamsVm model)
        {
            if (model is null)
            {
                return RedirectToAction("Create");
            }
            TeamsModel team = new TeamsModel()
            {
                Name = model.Name
            };
            try
            {
                _teamService.Create(team);
            }
            catch (Exception)
            {

                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int team_id)
        {
            TeamsModel team = new TeamsModel();
            TeamsVm model = new TeamsVm();
            try
            {
                team = _teamService.GetTeam(team_id);
                model.Id = team.Id;
                model.Name = team.Name;
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }

        public ActionResult SaveEdititngTeam(TeamsVm model)
        {
            TeamsModel team = new TeamsModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            try
            {
                _teamService.Update(team);
            }
            catch (Exception)
            {
                return RedirectToAction("Edit", model);
            }
            return RedirectToAction("Index");
        }
            
        public ActionResult Delete(int team_id)
        { 
            try
            {
                _teamService.Delete(team_id);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}