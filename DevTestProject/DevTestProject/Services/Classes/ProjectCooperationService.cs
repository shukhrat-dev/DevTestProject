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
    public class ProjectCooperationService : IProjectCooperationService
    {
        private readonly string ConnectionString = Utils.Constants.ConnectionString;
        private readonly string ProjectCooperationsTable = Utils.Constants.PROJECT_COOPERATION_TABLE;
        public bool Create (ProjectCooperaionsModel projectCooperation)
        {
            if (projectCooperation == null)
            {
                return false;
            }
            try
            {
                string dateAssigned = String.Format("{0}/{1}/{2}", projectCooperation.DateAssigned.Year, projectCooperation.DateAssigned.Month, projectCooperation.DateAssigned.Day);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"INSERT INTO {ProjectCooperationsTable} (Project_Id, Team_Id, DateAssigned) " +
                        $"VALUES ({projectCooperation.Project_Id}, {projectCooperation.Team_Id}, '{dateAssigned}')";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    int number = command.ExecuteNonQuery();
                    return number > 0 ? true : false;
                }
            }
            catch (Exception )
            {
                return false;
            }
        }

        public bool Delete(int projectCooperaion_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"DELETE FROM {ProjectCooperationsTable} WHERE {ProjectCooperationsTable}.Id = {projectCooperaion_id}";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    int number = command.ExecuteNonQuery();
                    return number > 0 ? true : false;
                }
            }
            catch (Exception )
            {
                return false;
            }
        }

        public List<ProjectCooperaionsModel> GetAllCooperations()
        {
            List<ProjectCooperaionsModel> projectCooperations = new List<ProjectCooperaionsModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {ProjectCooperationsTable};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            ProjectCooperaionsModel projectCooperation = new ProjectCooperaionsModel();
                            projectCooperation.Id = int.Parse(reader["Id"].ToString());
                            projectCooperation.Project_Id = int.Parse(reader["Project_Id"].ToString());
                            projectCooperation.Team_Id = int.Parse(reader["Team_Id"].ToString());
                            projectCooperation.DateAssigned = DateTime.Parse(reader["DateAssigned"].ToString());
                            projectCooperations.Add(projectCooperation);
                        }
                        catch (Exception )
                        {
                            return new List<ProjectCooperaionsModel>();
                        }

                    }

                }
            }
            catch (Exception )
            {
                return new List<ProjectCooperaionsModel>();
            }
            return projectCooperations;
        }

        public ProjectCooperaionsModel GetProjectCooperation(int projectCooperation_id)
        {
            ProjectCooperaionsModel projectCooperation = new ProjectCooperaionsModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {ProjectCooperationsTable} WHERE {ProjectCooperationsTable}.id = {projectCooperation_id};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            projectCooperation.Id = int.Parse(reader["Id"].ToString());
                            projectCooperation.Project_Id = int.Parse(reader["Project_Id"].ToString());
                            projectCooperation.Team_Id = int.Parse(reader["Team_Id"].ToString());
                            projectCooperation.DateAssigned = DateTime.Parse(reader["DateAssigned"].ToString());

                        }
                        catch (Exception )
                        {
                            return new ProjectCooperaionsModel();
                        }

                    }
                }
            }
            catch (Exception )
            {
                return new ProjectCooperaionsModel();
            }

            return projectCooperation;
        }

        public bool Update(ProjectCooperaionsModel projectCooperation)
        {
            if (projectCooperation is null || projectCooperation.DateAssigned ==  DateTime.MinValue)
            {
                return false;
            }
            try
            {
                string dateAssigned = String.Format("{0}/{1}/{2}", projectCooperation.DateAssigned.Year, projectCooperation.DateAssigned.Month, projectCooperation.DateAssigned.Day);

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    
                    string queryString = $"UPDATE {ProjectCooperationsTable} " +
                                            $"SET Project_Id = {projectCooperation.Project_Id}, " +
                                            $"Team_Id = {projectCooperation.Team_Id}, " +
                                            $"DateAssigned = CAST('{dateAssigned}' as DATETIME) " +
                                            $"WHERE {ProjectCooperationsTable}.Id = {projectCooperation.Id}";
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
        public bool CheckIfRecordExist(ProjectCooperaionsModel projectCooperations)
        {            
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {ProjectCooperationsTable} WHERE {ProjectCooperationsTable}.Project_Id = {projectCooperations.Project_Id} AND {ProjectCooperationsTable}.Team_Id = {projectCooperations.Team_Id};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();

                    if(reader.HasRows)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception )
            {
                return false; ;
            }

        }
    }
}