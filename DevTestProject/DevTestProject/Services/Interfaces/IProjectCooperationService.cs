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
        void Create(ProjectCooperaionModel employee);
        List<ProjectCooperaionModel> GetAllCooperations();
        ProjectCooperaionModel GetProjectCooperation(int projectCooperation_id);
        void Update(ProjectCooperaionModel projectCooperation);
        void Delete(ProjectCooperaionModel projectCooperation);
        
    }
}
