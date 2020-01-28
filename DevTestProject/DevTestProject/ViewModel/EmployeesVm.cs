using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.ViewModel
{
    public class EmployeesVm : EmployeesModel
    {
        public int Id { get; set; }
        
        public List<EmployeesModel> Employees { get; set; }
        public EmployeesModel Employee { get; set; }       
        public List<SelectListItem> TeamList { get; set; }
        public List<TeamsModel> TeamNames { get; set; }
        public string ErrorMsg { get; set; }
    }
}