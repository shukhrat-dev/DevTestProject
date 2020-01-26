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
        private string ProjectsCooperation = "[dbo].[project_cooperation]";
        private string Teams = "[dbo].[teams]";
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
                        string queryString = $"INSERT INTO {TableName} (Name, ProjectManager_Id, DateStart, DateDue) " +
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
                string dateStart = String.Format("{0}/{1}/{2}", project.DateStart.Year, project.DateStart.Month, project.DateStart.Day);
                string dateDue = String.Format("{0}/{1}/{2}", project.DateDue.Year, project.DateDue.Month, project.DateDue.Day);
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"UPDATE {TableName} " +
                                            $"SET Name = '{project.Name}', " +
                                            $"ProjectManager_Id = {project.ProjectManager_Id}, " +
                                            $"DateStart = CAST('{dateStart}' as DATETIME), " +
                                            $"DateDue = CAST('{dateDue}' as DATETIME) " +
                                            $"WHERE {TableName}.Id = {project.Id}";
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
                    string queryString = $"SELECT * FROM {TableName} WHERE {TableName}.id = {project_id};";
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

        public Dictionary<string, string> GetTeamWorkedOnProject()
        {
            Dictionary<string, string> teamOnProject = new Dictionary<string, string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT {TableName}.Name, team.Name as teams_name "
                                        + $"FROM {TableName} "
                                        + $"JOIN {ProjectsCooperation} project_coop ON {TableName}.Id = project_coop.Team_Id "
                                        + $"JOIN {Teams} team ON project_coop.Project_Id = team.Id;";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            string projectName = reader["Name"].ToString();
                            string teamName = reader["teams_name"].ToString();
                            teamOnProject.Add(teamName, projectName);
                        }
                        catch (Exception e)
                        {
                            return new Dictionary<string, string>();
                        }

                    }

                }

            }
            catch (Exception)
            {

                return new Dictionary<string, string>();
            }

            return teamOnProject;
        }
    }
}