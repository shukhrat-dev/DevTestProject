﻿using DevTestProject.Models;
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
        public Dictionary<string, string> TeamsOnProject { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
    }
}