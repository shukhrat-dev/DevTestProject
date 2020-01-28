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
        bool Create(EmployeesModel employee);
        EmployeesModel GetEmployee(int employee_id);
        List<EmployeesModel> GetAllEmployees();

        bool Update(EmployeesModel employee);
        bool Delete(EmployeesModel employee);
    }
}
