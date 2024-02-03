namespace PayrollMS.Models
{

    // SalaryRequest model
    public class SalaryRequest  
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsApprovedByManager { get; set; }
        public bool IsApprovedByAccountant { get; set; }
        public decimal TotalAmount { get; set; }
        public string ManagerComments { get; set; }
        public string? ManagerFiles { get; set; }
        public string AccountantComments { get; set; }
         
        public List<Employeer> Employees { get; set; }

    }
}
