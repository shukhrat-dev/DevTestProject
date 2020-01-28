using DevTestProject.Models;
using DevTestProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DevTestProject.Services.Classes
{
    public class TeamsService : ITeamsService
    {
        private readonly string ConnectionString = Utils.Constants.ConnectionString;
        private readonly string TeamsTable = Utils.Constants.TEAMS_TABLE;
        private readonly string ProjectsCooperationTable = Utils.Constants.PROJECT_COOPERATION_TABLE;
        private readonly string ProjectsTable = Utils.Constants.PROJECTS_TABLE;
        public bool Create(TeamsModel team)
        {
            if (team == null)
            {
                return false;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"INSERT INTO {TeamsTable} (Name) " +
                        $"VALUES ('{team.Name}')";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    int number = command.ExecuteNonQuery();
                    return number > 0 ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int team_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"DELETE FROM {TeamsTable} WHERE {TeamsTable}.Id = {team_id}";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    int number = command.ExecuteNonQuery();
                    return number > 0 ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public TeamsModel GetTeam(int team_id)
        {
            TeamsModel team = new TeamsModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {TeamsTable} WHERE {TeamsTable}.id = {team_id};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            team.Id = int.Parse(reader["Id"].ToString());
                            team.Name = reader["Name"].ToString();

                        }
                        catch (Exception)
                        {
                            return new TeamsModel();
                        }

                    }
                }
            }
            catch (Exception)
            {
                return new TeamsModel();
            }

            return team;
        }

        public List<TeamsModel> GetTeams()
        {
            List<TeamsModel> teams = new List<TeamsModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {TeamsTable};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            TeamsModel team = new TeamsModel();
                            team.Id = int.Parse(reader["Id"].ToString());
                            team.Name = reader["Name"].ToString();

                            teams.Add(team);
                        }
                        catch (Exception)
                        {
                            return new List<TeamsModel>();
                        }

                    }

                }
            }
            catch (Exception)
            {
                return new List<TeamsModel>();
            }
            return teams;
        }




        public Dictionary<string, int> GetTeamsProjectsCount()
        {
            Dictionary<string, int> projectsAmount = new Dictionary<string, int>(); 
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString =  $"SELECT {TeamsTable}.Name as teams_name, count(proj.Id) as project_amount "
                                        + $"FROM {TeamsTable} "
                                        + $"JOIN {ProjectsCooperationTable} project_coop ON {TeamsTable}.Id = project_coop.Team_Id "
                                        + $"JOIN {ProjectsTable} proj ON project_coop.Project_Id = proj.Id "
                                        + $"GROUP BY {TeamsTable}.Name";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {      
                            string teamName = reader["teams_name"].ToString();
                            int projectsCount = int.Parse(reader["project_amount"].ToString());
                            projectsAmount.Add(teamName, projectsCount);
                        }
                        catch (Exception)
                        {
                            return new Dictionary<string, int>();
                        }

                    }

                }

            }
            catch (Exception)
            {

                return new Dictionary<string, int>();
            }

            return projectsAmount;
        }


        public bool Update(TeamsModel team)
        {
            if (team is null)
            {
                return false;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"UPDATE {TeamsTable} " +
                        $"SET Name = '{team.Name}' " +
                        $" WHERE {TeamsTable}.Id = {team.Id}"; ;
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    int number = command.ExecuteNonQuery();
                    return number > 0 ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}