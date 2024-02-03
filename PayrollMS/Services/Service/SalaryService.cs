using PayrollMS.Models;
using PayrollMS.Services.Interface;

namespace PayrollMS.Services.Service
{
    public class SalaryService : ISalaryService
    {
        private readonly ApplicationDbContext _dbContext;

        public SalaryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Salary> AddSalary(Salary salary)
        {
            _dbContext.Salarys.Add(salary);
            await _dbContext.SaveChangesAsync();
            return salary;
        }

        public async Task<SalaryRequest> AddSalaryApprovalRequest(SalaryRequest request)
        {
            _dbContext.SalaryRequests.Add(request);
            await _dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<SalaryRequest> GetSalaryApprovalRequest(int id)
        {
            return await _dbContext.SalaryRequests.FindAsync(id);
        }

        public async Task<bool> UploadFileByManager(int id, string filePath)
        {
            var request = await _dbContext.SalaryRequests.FindAsync(id);
            if (request == null)
            {
                return false;
            }

            request.ManagerFiles = filePath;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
