using DevTestProject.Models;
using DevTestProject.Services.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System;

namespace DevTestProject.Services.Classes
{
    public class EmployeesService : IEmployeesService
    {    
      
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private string TableName = "[dbo].[employees]";
        
        public bool Create(EmployeesModel employee)
        {
            
            if (employee == null)
            {                
                return false;
            }
            
            try
            {                
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"INSERT INTO {TableName} (FirstName, MiddleName, LastName, Email, Team_Id) " +
                        $"VALUES ('{employee.FirstName}'," +
                        $"'{employee.MiddleName}', " +
                        $"'{employee.LastName}', " +
                        $"'{employee.Email}', " +
                        $"'{employee.Team_Id}');";
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

        public EmployeesModel GetEmployee(int employee_id)
        {            
            EmployeesModel employee = new EmployeesModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {TableName} WHERE {TableName}.id = {employee_id};";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            employee.Id = int.Parse(reader["Id"].ToString());
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.MiddleName = reader["MiddleName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.Email = reader["Email"].ToString();
                            employee.Team_Id = int.Parse(reader["Team_Id"].ToString());
                        }
                        catch (Exception e)
                        {
                            return new EmployeesModel();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                return new EmployeesModel();
            }

            return employee;
        }

        public List<EmployeesModel> GetAllEmployees()
        {
            List<EmployeesModel> employees = new List<EmployeesModel>();
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
                            EmployeesModel employee = new EmployeesModel();
                            employee.Id = int.Parse(reader["Id"].ToString());
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.MiddleName = reader["MiddleName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.Email = reader["Email"].ToString();
                            employee.Team_Id = int.Parse(reader["Team_Id"].ToString());
                            employees.Add(employee);
                        }
                        catch (Exception e)
                        {
                            return new List<EmployeesModel>();
                        }

                    }

                }
            }
            catch (Exception e)
            {
                return new List<EmployeesModel>();
            }
            return employees;
            
        }

        public List<EmployeesModel> GetAllEmployeesByPage(int page, int item_on_page)
        {
            List<EmployeesModel> employees = new List<EmployeesModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM {TableName} ORDER BY {TableName}.Id OFFSET {page}*{item_on_page} ROWS FETCH NEXT {item_on_page} ROWS ONLY;";
                    connection.Open();
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Prepare();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            EmployeesModel employee = new EmployeesModel();
                            employee.Id = int.Parse(reader["Id"].ToString());
                            employee.FirstName = reader["FirstName"].ToString();
                            employee.MiddleName = reader["MiddleName"].ToString();
                            employee.LastName = reader["LastName"].ToString();
                            employee.Email = reader["Email"].ToString();
                            employee.Team_Id = int.Parse(reader["Team_Id"].ToString());
                            employees.Add(employee);
                        }
                        catch (Exception)
                        {
                            return new List<EmployeesModel>();
                        }
                        
                    }

                    return employees;
                }
            }
            catch (Exception)
            {
                return new List<EmployeesModel>();
            }
            
            
        }


        public bool Update(EmployeesModel employee)
        {
          
            if (employee == null)
            {
                return false;
            }            
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"UPDATE {TableName} " +
                        $"SET FirstName = '{employee.FirstName}', " +
                        $"MiddleName = '{employee.MiddleName}', " +
                        $"LastName = '{employee.LastName}', " +
                        $"Email = '{employee.Email}' , " +
                        $"Team_Id = {employee.Team_Id}" +
                        $" WHERE {TableName}.Id = {employee.Id}";
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

        public bool Delete(EmployeesModel employee)
        {
            if (employee == null)
            {
                return false;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"DELETE FROM {TableName} WHERE {TableName}.Id = {employee.Id}";
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