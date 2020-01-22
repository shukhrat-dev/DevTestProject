using DevTestProject.Models;
using DevTestProject.Services.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace DevTestProject.Services.Classes
{
    public class EmployeesService : IEmployeesService
    {    
      
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        string TableName = "[dbo].[employees]";
        public void Create(EmployeesModel employee)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"INSERT INTO {TableName} (FirstName, MiddleName, LastName, Email, Team_Id)" +
                    $"VALUES ('{employee.FirstName}', '{employee.MiddleName}', '{employee.LastName}', '{employee.Email}', '{employee.Team_Id}');";
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                int number = command.ExecuteNonQuery();
            }
        }

        public EmployeesModel GetEmployee(int employee_id)
        {
            EmployeesModel employee = new EmployeesModel();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"SELECT * FROM {TableName} WHERE {TableName}.id = {employee_id};";
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                
                while(reader.Read())
                {

                    employee.Id = int.Parse(reader["Id"].ToString());
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.MiddleName = reader["MiddleName"].ToString(); //TODO Can be NULL CHECK IT!!!
                    employee.LastName = reader["LastName"].ToString();
                    employee.Email = reader["Email"].ToString();
                    employee.Team_Id = int.Parse(reader["Team_Id"].ToString());
                }                
            }
            return employee;
        }

        public List<EmployeesModel> GetAllEmployees()
        {
            List<EmployeesModel> employees = new List<EmployeesModel>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"SELECT * FROM {TableName};";
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    EmployeesModel employee = new EmployeesModel();
                    employee.Id = int.Parse(reader["Id"].ToString());
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.MiddleName = reader["MiddleName"].ToString(); //TODO Can be NULL CHECK IT!!!
                    employee.LastName = reader["LastName"].ToString();
                    employee.Email = reader["Email"].ToString();
                    employee.Team_Id = int.Parse(reader["Team_Id"].ToString());
                    employees.Add(employee);
                }

                return employees;
            }
        }


        public void Update(EmployeesModel employee)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"UPDATE {TableName} " +
                    $"SET FirstName = '{employee.FirstName}', " +
                    $"MiddleName = '{employee.MiddleName}', " +
                    $"LastName = '{employee.LastName}', " +
                    $"Email = '{employee.Email}' , " +
                    $"Team_Id = {employee.Team_Id}" +
                    $"WHERE {TableName}.Id = {employee.Id}";
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                int number = command.ExecuteNonQuery();
            }
        }

        public void Delete(EmployeesModel employee)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"DELETE FROM {TableName} WHERE {TableName}.Id = {employee.Id}";
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                int number = command.ExecuteNonQuery();
            }
        }
    }
}