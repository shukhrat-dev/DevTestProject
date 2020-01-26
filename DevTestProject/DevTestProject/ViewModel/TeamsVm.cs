using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTestProject.ViewModel
{
    public class TeamsVm : TeamsModel
    {
        public List<TeamsModel> TeamList { get; set; }
        public Dictionary<string, int> TeamProjects{ get; set; } 
    }
}