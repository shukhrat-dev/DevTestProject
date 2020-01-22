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
        public int Employee_id { get; set; }
        public string DateCreated { get; set; }
        public string DateStarted { get; set; }
        public string DateFinished { get; set; }
        public string DateDue { get; set; }
    }
}