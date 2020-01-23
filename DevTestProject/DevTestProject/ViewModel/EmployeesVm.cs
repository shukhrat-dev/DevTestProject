using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.ViewModel
{
    public class EmployeesVm
    {
        public int Id { get; set; }
        public int Page { get; set; }
        public int ItemOnPage { get; set; }
        public int EmployeesForPage { get; set; }
        public List<EmployeesModel> Employees { get; set; }
        public EmployeesModel Employee { get; set; }       
        public List<SelectListItem> TeamList { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Team_Id { get; set; }
    }
}