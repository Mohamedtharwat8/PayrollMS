namespace PayrollMS.Models
{
    public class Salary    
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public int BasicSalary { get; set; }
        public int Premium { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsApproved { get; set; }
        public DateTime Month { get; set; }
        public int WorkDays { get; set; }
        public Guid EmployeerID { get; set; }
        public virtual List<Employeer> Employeer { get; set; }


    }

}
