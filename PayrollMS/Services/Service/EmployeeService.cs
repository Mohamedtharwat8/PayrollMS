using PayrollMS.Helpers;
using PayrollMS.Models;
using PayrollMS.Services.Interface;

namespace PayrollMS.Services.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //public   IEnumerable<Employeer> GetAllEmployees()
        //{
        //    return   _dbContext.Employees.ToList();
        //}

        public async Task<Employeer> GetEmployeerById(int id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task<List<Employeer>> GetAllEmployees()
        {
                 return   _dbContext.Employees.ToList();
        }
    }
}
