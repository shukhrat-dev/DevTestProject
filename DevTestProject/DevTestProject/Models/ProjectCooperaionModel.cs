using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTestProject.Models
{
    public class ProjectCooperaionModel
    {
        public int Id { get; set; }
        public int Project_Id { get; set; }
        public int Team_Id { get; set; }
        public string DateAssigned { get; set; }
    }
}