using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTestProject.Models
{
    public class ProjectsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectManager_Id { get; set; }
        public string DateStart { get; set; }
        public string DateDue { get; set; }
    }
}