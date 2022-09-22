using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Employees
{
    public class EmployeeValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().MaximumLength(200)
                .WithMessage("Name should not be empty and maximum 200 characters");
            RuleFor(x => x.Email).NotEmpty().MaximumLength(250).EmailAddress()
                .WithMessage("Email should not be empty and maximum 250 character and email format");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone Number should not be empty");
            RuleFor(x => x.HireDate).NotEmpty().WithMessage("Hire date should not be empty");
            RuleFor(x => x.Salary).NotEmpty().WithMessage("Salary should not be empty");
        }
    }
}
