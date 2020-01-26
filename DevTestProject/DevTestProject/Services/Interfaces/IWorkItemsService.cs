using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTestProject.Services.Interfaces
{
    interface IWorkItemsService
    {
        bool Create(WorkItemsModel team);
        WorkItemsModel GetWorkItem(int workItem_id);
        List<WorkItemsModel> GetAllWorkItems();
        List<int> GetAllEmployeeFromProject(int project_id);
        bool Update(WorkItemsModel workItem);
        bool Delete(int team_id);
    }
}
