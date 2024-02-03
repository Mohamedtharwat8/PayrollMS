namespace PayrollMS.Models
{
    // Employee model

    public class Employeer  
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }
         public string AccountCode { get; set; }
        public string FullName { get; set; }
        public int PassportSerial { get; set; }
        public int PassportNumber { get; set; }
        public DateTime Birthdate { get; set; }
         public string Address { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual string Experiences { get; set; }

    }
}
