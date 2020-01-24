using DevTestProject.Models;
using DevTestProject.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DevTestProject.Services.Classes
{
    public class ProjectsService : IProjectsService
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private string TableName = "[dbo].[projects]";
        public bool Create(ProjectsModel project)
        {
            {
                if (project == null)
                {
                    return false;
                }
                try
                {   
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        string queryString = $"INSERT INTO {TableName} (Name, ProjectManager_Id, DateStart, DateDue) " +
                                             $"VALUES ('{project.Name}', " +
                                             $"'{project.ProjectManager_Id}', " +
                                             $"(convert(datetime, {project.DateStart}, 1)), " +
                                             $"(convert(datetime, {project.DateDue}, 1))";
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
        }

        public bool Delete(int project_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"DELETE FROM {TableName} WHERE {TableName}.Id = {project_id}";
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

        public bool Update(ProjectsModel project)
        {
            if (project is null)
            {
                return false;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"UPDATE {TableName} " +
                                            $"SET Name = '{project.Name}', " +
                                            $"ProjectManager_Id = '{project.ProjectManager_Id}', " +
                                            $"DateStart = (convert(datetime, {project.DateStart}, 1)), " +
                                            $"DateDue = (convert(datetime, {project.DateDue}, 1))" +
                                            $" WHERE {TableName}.Id = {project.Id}";
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

        public List<ProjectsModel> GetAllProjects()
        {
            List<ProjectsModel> projects = new List<ProjectsModel>();

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
                            ProjectsModel project = new ProjectsModel();
                            project.Id = int.Parse(reader["Id"].ToString());
                            project.Name = reader["Name"].ToString();
                            project.ProjectManager_Id = int.Parse(reader["ProjectManager_Id"].ToString());
                            project.DateStart = DateTime.Parse(reader["DateStart"].ToString());
                            project.DateDue = DateTime.Parse(reader["DateDue"].ToString());

                            projects.Add(project);
                        }
                        catch (Exception e)
                        {
                            return new List<ProjectsModel>();
                        }

                    }

                }
            }
            catch (Exception e)
            {
                return new List<ProjectsModel>();
            }
            return projects;
        }

        public ProjectsModel GetProject(int project_id)
        {
            ProjectsModel project = new ProjectsModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {TableName} WHERE {TableName}.id = {project.Id};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            project.Id = int.Parse(reader["Id"].ToString());
                            project.Name = reader["Name"].ToString();
                            project.ProjectManager_Id = int.Parse(reader["ProjectManager_Id"].ToString());
                            project.DateStart = DateTime.Parse(reader["DateStart"].ToString());
                            project.DateDue = DateTime.Parse(reader["DateDue"].ToString());
                        }
                        catch (Exception e)
                        {
                            return new ProjectsModel();
                        }

                    }

                }
            }
            catch (Exception)
            {

                return new ProjectsModel();
            }

            return project;

        }
    }
}