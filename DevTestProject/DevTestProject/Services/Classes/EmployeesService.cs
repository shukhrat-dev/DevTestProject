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

        private readonly string ConnectionString = Utils.Constants.ConnectionString;
        private readonly string EmployeesTable = Utils.Constants.EMPLOYEES_TABLE;
        
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
                    string queryString = $"INSERT INTO {EmployeesTable} (FirstName, MiddleName, LastName, Email, Team_Id) " +
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
            catch (Exception)
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
                    string queryString = $"SELECT * FROM {EmployeesTable} WHERE {EmployeesTable}.id = {employee_id};";
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
                        catch (Exception)
                        {
                            return new EmployeesModel();
                        }

                    }
                }
            }
            catch (Exception)
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
                    string queryString = $"SELECT * FROM {EmployeesTable};";
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

                }
            }
            catch (Exception)
            {
                return new List<EmployeesModel>();
            }
            return employees;
            
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
                    string queryString = $"UPDATE {EmployeesTable} " +
                        $"SET FirstName = '{employee.FirstName}', " +
                        $"MiddleName = '{employee.MiddleName}', " +
                        $"LastName = '{employee.LastName}', " +
                        $"Email = '{employee.Email}' , " +
                        $"Team_Id = {employee.Team_Id}" +
                        $" WHERE {EmployeesTable}.Id = {employee.Id}";
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
                    string queryString = $"DELETE FROM {EmployeesTable} WHERE {EmployeesTable}.Id = {employee.Id}";
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