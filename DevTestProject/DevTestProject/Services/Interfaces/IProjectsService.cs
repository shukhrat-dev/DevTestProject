using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTestProject.Services.Interfaces
{
    interface IProjectsService
    {
        ProjectsModel GetProject(int project_id);
        List<ProjectsModel> GetAllProjects();
        Dictionary<string, string> GetTeamWorkedOnProject();
        bool Create(ProjectsModel project);
        bool Update(ProjectsModel project);
        bool Delete(int project_id);

    }
}
