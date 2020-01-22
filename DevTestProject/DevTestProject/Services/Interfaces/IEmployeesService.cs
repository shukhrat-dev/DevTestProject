using DevTestProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTestProject.Services.Interfaces
{
    interface IEmployeesService
    {
        void Create(EmployeesModel employee);
        List<EmployeesModel> GetAllEmployees();
        EmployeesModel GetEmployee(int employee_id);
        void Update(EmployeesModel employee);
        void Delete(EmployeesModel employee);
    }
}
