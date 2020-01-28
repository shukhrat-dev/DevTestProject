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
        private readonly string ConnectionString = Utils.Constants.ConnectionString;
        private readonly string ProjectsTable = Utils.Constants.PROJECTS_TABLE;
        private readonly string ProjectsCooperationTable = Utils.Constants.PROJECT_COOPERATION_TABLE;
        private readonly string TeamsTable = Utils.Constants.TEAMS_TABLE;
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
                        string dateStart = String.Format("{0}/{1}/{2}", project.DateStart.Year, project.DateStart.Month, project.DateStart.Day);
                        string dateDue = String.Format("{0}/{1}/{2}", project.DateDue.Year, project.DateDue.Month, project.DateDue.Day);
                        string queryString = $"INSERT INTO {ProjectsTable} (Name, ProjectManager_Id, DateStart, DateDue) " +
                                             $"VALUES (" +
                                             $"'{project.Name}', " +
                                             $"{project.ProjectManager_Id}, " +
                                             $"'{dateStart}', '{dateDue}')";
                        

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

        public bool Delete(int project_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"DELETE FROM {ProjectsTable} WHERE {ProjectsTable}.Id = {project_id}";
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

        public bool Update(ProjectsModel project)
        {
            if (project is null)
            {
                return false;
            }
            try
            {
                string dateStart = String.Format("{0}/{1}/{2}", project.DateStart.Year, project.DateStart.Month, project.DateStart.Day);
                string dateDue = String.Format("{0}/{1}/{2}", project.DateDue.Year, project.DateDue.Month, project.DateDue.Day);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"UPDATE {ProjectsTable} " +
                                            $"SET Name = '{project.Name}', " +
                                            $"ProjectManager_Id = {project.ProjectManager_Id}, " +
                                            $"DateStart = CAST('{dateStart}' as DATETIME), " +
                                            $"DateDue = CAST('{dateDue}' as DATETIME) " +
                                            $"WHERE {ProjectsTable}.Id = {project.Id}";
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
                    string queryString = $"SELECT * FROM {ProjectsTable};";
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
                        catch (Exception)
                        {
                            return new List<ProjectsModel>();
                        }

                    }

                }
            }
            catch (Exception)
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
                    string queryString = $"SELECT * FROM {ProjectsTable} WHERE {ProjectsTable}.id = {project_id};";
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
                        catch (Exception)
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