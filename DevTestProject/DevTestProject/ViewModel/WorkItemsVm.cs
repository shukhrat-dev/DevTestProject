using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.ViewModel
{
    public class WorkItemsVm : WorkItemsModel
    {
        public List<WorkItemsModel> WorkItems { get; set; }
        public List<EmployeesModel> EmployeesList { get; set; }
        public List<SelectListItem> ProjectsList { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
    }
}