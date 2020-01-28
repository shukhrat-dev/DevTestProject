using DevTestProject.Models;
using DevTestProject.Services.Classes;
using DevTestProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DevTestProject.Controllers
{
    public class TeamsController : Controller
    {
        private readonly TeamsService _teamService = new TeamsService();
        // GET: Teams
        public ActionResult Index()
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            List<TeamsModel> teams = new List<TeamsModel>();
            Dictionary<string, int> teamProjects = new Dictionary<string, int>();
            try
            {
                teams = _teamService.GetTeams();
                teamProjects = _teamService.GetTeamsProjectsCount();
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            TeamsVm model = new TeamsVm()
            {
                TeamList = teams,
                TeamProjects = teamProjects,
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
            TeamsVm model = new TeamsVm();
            model.ErrorMsg = errorMsg;
            return View("Create", model);
        }

        [HttpPost]
        public ActionResult Create(TeamsVm model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                TempData["error"] = $"You did not fill name. Name is required.";
                return RedirectToAction("Create");
            }
            TeamsModel team = new TeamsModel()
            {
                Name = model.Name
            };
            try
            {
                if(!_teamService.Create(team))
                {
                    TempData["error"] = $"Problems with create team (Service error \"Create\").";
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

        public ActionResult Edit(int team_id)
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            TeamsModel team = new TeamsModel();
            TeamsVm model = new TeamsVm();
            try
            {
                team = _teamService.GetTeam(team_id);

            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index");
            }
            model.Id = team.Id;
            model.Name = team.Name;
            
            return View("Edit", model);
        }

        public ActionResult SaveEdititngTeam(TeamsVm model)
        {
            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                TempData["error"] = $"You did not fill name. Name is required.";
                return RedirectToAction("Edit", new { team_id  = model.Id });
            }
            TeamsModel team = new TeamsModel()
            {
                Id = model.Id,
                Name = model.Name
            };

            try
            {
                if(!_teamService.Update(team))
                {
                    TempData["error"] = $"Problems with updating team (Service error \"Update/Edit\").";
                    return RedirectToAction("Edit", new { team_id = model.Id });
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Edit", new { team_id = model.Id });
            }
            return RedirectToAction("Index");
        }
            
        public ActionResult Delete(int team_id)
        { 
            try
            {
                if(!_teamService.Delete(team_id))
                {
                    TempData["error"] = $"Problems with deleting team (Service error \"Delete\"). Try to move employee to another team firstly, then try again";
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