using PayrollMS.Helpers;
using PayrollMS.Models;

namespace PayrollMS.Services.Interface
{
    public interface IEmployeeService
    {
        Task<Employeer> GetEmployeerById(int id);
      
        Task<List<Employeer>> GetAllEmployees();
    }
}
