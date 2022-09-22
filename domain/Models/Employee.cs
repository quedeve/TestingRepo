using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Models
{
    public class Employee
    {
        [Key]
        
        public int ID { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime HireDate { get; set; }
        public Decimal Salary { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool RowStatus { get; set; }

    }
}
