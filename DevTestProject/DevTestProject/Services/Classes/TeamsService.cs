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
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private string TableName = "[dbo].[teams]";
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
                    string queryString = $"INSERT INTO {TableName} (Name) " +
                        $"VALUES ('{team.Name}')";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    int number = command.ExecuteNonQuery();
                    return number > 0 ? true : false;
                }
            }
            catch (Exception e)
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
                    string queryString = $"DELETE FROM {TableName} WHERE {TableName}.Id = {team_id}";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    int number = command.ExecuteNonQuery();
                    return number > 0 ? true : false;
                }
            }
            catch (Exception e)
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
                    string queryString = $"SELECT * FROM {TableName} WHERE {TableName}.id = {team_id};";
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
                        catch (Exception e)
                        {
                            return new TeamsModel();
                        }

                    }
                }
            }
            catch (Exception e)
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
                    string queryString = $"SELECT * FROM {TableName};";
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
                        catch (Exception e)
                        {
                            return new List<TeamsModel>();
                        }

                    }

                }
            }
            catch (Exception e)
            {
                return new List<TeamsModel>();
            }
            return teams;
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
                    string queryString = $"UPDATE {TableName} " +
                        $"SET Name = '{team.Name}' " +
                        $" WHERE {TableName}.Id = {team.Id}"; ;
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