using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTestProject.Services.Interfaces
{
    interface IProjectCooperationService
    {
        bool Create(ProjectCooperaionsModel projectCooperaion);
        List<ProjectCooperaionsModel> GetAllCooperations();
        ProjectCooperaionsModel GetProjectCooperation(int projectCooperation_id);
        bool Update(ProjectCooperaionsModel projectCooperation);
        bool Delete(int projectCooperaion_id);
        bool CheckIfRecordExist(ProjectCooperaionsModel projectCooperations);


    }
}
