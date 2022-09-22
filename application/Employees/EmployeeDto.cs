using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Employees
{
    public class EmployeeDto
    {
        public int? ID { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}
