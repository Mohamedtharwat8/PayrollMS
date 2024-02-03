using PayrollMS.Models;

namespace PayrollMS.Services.Interface
{
    public interface ISalaryService
    {
        Task<SalaryRequest> GetSalaryApprovalRequest(int id);
        Task<Salary> AddSalary(Salary salary);
        Task<SalaryRequest> AddSalaryApprovalRequest(SalaryRequest request);
         Task<bool> UploadFileByManager(int id, string filePath);
     }
}
