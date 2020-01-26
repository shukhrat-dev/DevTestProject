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
    public class WorkItemsService : IWorkItemsService
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private string TableName = "[dbo].[work_items]";
        private string Projects = "[dbo].[projects]";
        private string ProjectsCooperation = "[dbo].[project_cooperation]";
        private string Employees = "[dbo].[employees]";
        private string Teams = "[dbo].[teams]";
        public bool Create(WorkItemsModel workItem)
        {
            if (workItem == null)
            {
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string dataStart = workItem.DateStarted == null ? null : String.Format("{0}/{1}/{2}", workItem.DateStarted.Value.Year, workItem.DateStarted.Value.Month, workItem.DateStarted.Value.Day);
                    string dataFinished = workItem.DateFinished == null ? null : String.Format("{0}/{1}/{2}", workItem.DateFinished.Value.Year, workItem.DateFinished.Value.Month, workItem.DateFinished.Value.Day);
                    string dataCreated = String.Format("{0}/{1}/{2}", workItem.DateCreated.Year, workItem.DateCreated.Month, workItem.DateCreated.Day);
                    string dataDue = String.Format("{0}/{1}/{2}", workItem.DateDue.Year, workItem.DateDue.Month, workItem.DateDue.Day);

                    string queryString = $"INSERT INTO {TableName} (Name, Description, Project_Id, Employee_Id, DateCreated, DateDue, DateStarted, DateFinished) " +
                        $"VALUES (" +
                        $"'{workItem.Name}'," +
                        $"'{workItem.Description}', " +
                        $"{workItem.Project_Id}, " +
                        $"{workItem.Employee_Id}, " +
                        $"'{workItem.DateCreated}', " +
                        $"'{workItem.DateDue}', ";

                    if (string.IsNullOrEmpty(dataStart))
                        queryString += "NULL, ";
                    else
                        queryString += $"{dataStart}, ";
                    if (string.IsNullOrEmpty(dataFinished))
                        queryString += "NULL) ";
                    else
                        queryString += $"{dataFinished}') ";
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

        public bool Delete(int workItem_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"DELETE FROM {TableName} WHERE {TableName}.Id = {workItem_id}";
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

        public List<int> GetAllEmployeeFromProject(int project_id)
        {
            List<int> allEmployeesFromProject = new List<int>();
            try
            {
                using(SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = "SELECT empl.Id as employee_id " +
                                         $"FROM {Projects} proj " +
                                         $"JOIN {ProjectsCooperation} proj_coop  " +
                                         "ON proj.Id = proj_coop.Project_Id  " +
                                         $"JOIN {Teams} tms  " +
                                         "ON tms.Id = proj_coop.Team_Id  " +
                                         $"JOIN {Employees} empl  " +
                                         "ON empl.Team_Id = tms.Id " +
                                         $"WHERE proj.Id = {project_id}";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int employee_id = int.Parse(reader["employee_id"].ToString());
                        allEmployeesFromProject.Add(employee_id);
                    }
                }
            }
            catch (Exception)
            {
                return new List<int>();
            }
            return allEmployeesFromProject;
        }

        public WorkItemsModel GetWorkItem(int workItem_id)
        {
            WorkItemsModel workItem = new WorkItemsModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {TableName} WHERE {TableName}.id = {workItem_id};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            workItem.Id = int.Parse(reader["Id"].ToString());
                            workItem.Name = reader["Name"].ToString();
                            workItem.Description = reader["Description"].ToString();
                            workItem.Project_Id = int.Parse(reader["Project_Id"].ToString());
                            workItem.Employee_Id = int.Parse(reader["Employee_Id"].ToString());
                            workItem.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                            workItem.DateStarted = String.IsNullOrEmpty(reader["DateStarted"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["DateStarted"].ToString());
                            workItem.DateFinished = String.IsNullOrEmpty(reader["DateFinished"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["DateFinished"].ToString());
                            workItem.DateDue = DateTime.Parse(reader["DateDue"].ToString());
                        }
                        catch (Exception)
                        {
                            return new WorkItemsModel();
                        }

                    }
                }
            }
            catch (Exception)
            {
                return new WorkItemsModel();
            }

            return workItem;
        }

        public bool Update(WorkItemsModel workItem)
        {
            if (workItem is null)
            {
                return false;
            }
            try
            {
                string dataStart = workItem.DateStarted == null ?  null : String.Format("{0}/{1}/{2}", workItem.DateStarted.Value.Year, workItem.DateStarted.Value.Month, workItem.DateStarted.Value.Day);
                string dataFinished = workItem.DateFinished == null ? null : String.Format("{0}/{1}/{2}", workItem.DateFinished.Value.Year, workItem.DateFinished.Value.Month, workItem.DateFinished.Value.Day);
                
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"UPDATE {TableName} " +
                        $"SET " +
                        $"Name = '{workItem.Name}', " +
                        $"Description = '{workItem.Description}', " +
                        $"Project_Id = {workItem.Project_Id}, " +
                        $"Employee_Id = {workItem.Employee_Id}, " +
                        $"DateCreated = CAST('{workItem.DateCreated}' as DATETIME), " +
                        $"DateDue = CAST('{workItem.DateDue}' as DATETIME), ";
                    if (string.IsNullOrEmpty(dataStart))
                        queryString += "DateStarted = NULL ,";
                    else
                        queryString += $"DateStarted = CAST('{dataStart}' as DATETIME), ";
                    if (string.IsNullOrEmpty(dataFinished))
                        queryString += $"DateFinished = NULL ";
                    else
                        queryString += $"DateFinished = CAST('{dataFinished}' as DATETIME), ";
                         
                        queryString += $"WHERE {TableName}.Id = {workItem.Id}";
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
        public List<WorkItemsModel> GetAllWorkItems()
        {
            List<WorkItemsModel> workItems = new List<WorkItemsModel>();

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
                            WorkItemsModel workItem = new WorkItemsModel();
                            

                            
                            workItem.Id = int.Parse(reader["Id"].ToString());
                            workItem.Name = reader["Name"].ToString();
                            workItem.Description = reader["Description"].ToString();
                            workItem.Project_Id = int.Parse(reader["Project_Id"].ToString());
                            workItem.Employee_Id = int.Parse(reader["Employee_Id"].ToString());
                            workItem.DateCreated = DateTime.Parse(reader["DateCreated"].ToString());
                            workItem.DateDue = DateTime.Parse(reader["DateDue"].ToString());
                            workItem.DateStarted = String.IsNullOrEmpty(reader["DateStarted"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["DateStarted"].ToString());
                            workItem.DateFinished = String.IsNullOrEmpty(reader["DateFinished"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["DateFinished"].ToString());


                            workItems.Add(workItem);
                        }
                        catch (Exception e)
                        {
                            return new List<WorkItemsModel>();
                        }

                    }

                }
            }
            catch (Exception e)
            {
                return new List<WorkItemsModel>();
            }
            return workItems;
        }

    }
}