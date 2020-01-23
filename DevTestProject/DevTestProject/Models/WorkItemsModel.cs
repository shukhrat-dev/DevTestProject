using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevTestProject.Models
{
    public class WorkItemsModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Project_Id { get; set; }
        public int Employee_Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateFinished { get; set; }
        public DateTime DateDue { get; set; }
    }
}