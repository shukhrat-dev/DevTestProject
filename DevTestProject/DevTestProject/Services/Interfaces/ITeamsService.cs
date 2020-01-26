using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTestProject.Services.Interfaces
{
    interface ITeamsService
    {
        bool Create(TeamsModel team);
        TeamsModel GetTeam(int team_id);
        Dictionary<string, int> GetTeamsProjectsCount();
        List<TeamsModel> GetTeams();
        bool Update(TeamsModel team);
        bool Delete(int team_id);
    }
}
