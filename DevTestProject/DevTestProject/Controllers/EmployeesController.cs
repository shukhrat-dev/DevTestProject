using DevTestProject.Models;
using DevTestProject.Services.Classes;
using DevTestProject.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DevTestProject.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeesService _employeesService = new EmployeesService(); 
        private readonly TeamsService _teamService = new TeamsService();
        // GET: Employees
        public ActionResult Index()
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }

            List<EmployeesModel> employees = new List<EmployeesModel>();
            List<TeamsModel> teamNames = new List<TeamsModel>();
            try
            {
                employees = _employeesService.GetAllEmployees();
                teamNames = _teamService.GetTeams();
            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            
            EmployeesVm model = new EmployeesVm()
            {
                Employees = employees,
                TeamNames = teamNames,
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

            List<SelectListItem> teamList = new List<SelectListItem>();
            try
            {
                teamList = Utils.Helper.GetTeamList();
            }
            catch (Exception e)
            {

                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            
            EmployeesVm model = new EmployeesVm()
            {
                TeamList = teamList,
                ErrorMsg = errorMsg
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
            if(String.IsNullOrWhiteSpace(model.FirstName) || String.IsNullOrWhiteSpace(model.LastName) || String.IsNullOrWhiteSpace(model.Email))
            {
                TempData["error"] = $"It is necessary to fill everything. Middlename if possible.";
                return RedirectToAction("Create");
            }

            EmailAddressAttribute emailAdressAttr = new EmailAddressAttribute();

            if(!emailAdressAttr.IsValid(model.Email))
            {
                TempData["error"] = $"Email typed wrong";
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
            try
            {
                if(!_employeesService.Create(employee))
                {
                    TempData["error"] = $"Problems with create employee (Service error \"Create\").";
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
        public ActionResult Edit(int employee_id)
        {
            string errorMsg = String.Empty;
            if (TempData.ContainsKey("error"))
            {
                errorMsg = TempData["error"].ToString();
            }
            EmployeesModel employee = new EmployeesModel();
            List<SelectListItem> teamList = new List<SelectListItem>();
            try
            {
                employee = _employeesService.GetEmployee(employee_id);
                teamList = Utils.Helper.GetTeamList(employee.Team_Id);
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Index", "Home");
            }
            
            EmployeesVm employeeVm = new EmployeesVm()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Email = employee.Email,
                Team_Id = employee.Team_Id,
                TeamList = teamList,
                ErrorMsg = errorMsg

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

            if (String.IsNullOrWhiteSpace(model.FirstName) || String.IsNullOrWhiteSpace(model.LastName) || String.IsNullOrWhiteSpace(model.Email))
            {
                TempData["error"] = $"It is necessary to fill everything. Middlename if possible.";
                return RedirectToAction("Edit", new { employee_id = model.Id });
            }
            EmailAddressAttribute emailAdressAttr = new EmailAddressAttribute();

            if (!emailAdressAttr.IsValid(model.Email))
            {
                TempData["error"] = $"Email typed wrong";
                return RedirectToAction("Create");
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

            try
            {
                if (!_employeesService.Update(employee))
                {
                    TempData["error"] = $"Problems with updating employee (Service error \"Update/Edit\").";
                    return RedirectToAction("Edit", new { employee_id = model.Id });
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"Problems with getting information from database (services). {e.Message}";
                return RedirectToAction("Edit", new { employee_id = model.Id });
            }

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int employee_id)
        {
            EmployeesModel employee = new EmployeesModel();
            try
            {
                employee = _employeesService.GetEmployee(employee_id);
                if(!_employeesService.Delete(employee))
                {
                    TempData["error"] = $"Problems with deleting employee (Service error \"Delete\").";
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