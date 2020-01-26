using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevTestProject.ViewModel
{
    public class ProjectCooperationsVm : ProjectCooperaionsModel
    {
        public List<ProjectCooperaionsModel> ProjectCooperations { get; set; }
        public List<SelectListItem> ProjectList { get; set; }
        public List<SelectListItem> TeamList { get; set; }
    }
}