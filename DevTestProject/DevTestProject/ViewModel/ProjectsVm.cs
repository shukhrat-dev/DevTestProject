using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.ViewModel
{
    public class ProjectsVm : ProjectsModel
    {
        public List<ProjectsModel> ProjectList;
        public List<EmployeesModel> Employees { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public Dictionary<int, int> WorkItemsOnProject { get; set; }
        public string ErrorMsg { get; set; }
    }
}