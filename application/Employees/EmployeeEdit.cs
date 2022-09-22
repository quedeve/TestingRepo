using application.Core;
using application.Interfaces;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Employees
{
    public class EmployeeEdit
    {
        public class Command : IRequest<Result<EmployeeDto>>
        {
            public EmployeeDto employee { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {

            public CommandValidator()
            {
                RuleFor(x=>x.employee).SetValidator(new EmployeeValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<EmployeeDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<EmployeeDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail() && x.RowStatus == true,cancellationToken);
                if (user == null) return Result<EmployeeDto>.Failure("Your Account is not found please relogin");
                var employee = await _context.Employees.FindAsync(request.employee.ID);
                if(employee ==null) return Result<EmployeeDto>.Failure("Failure to update employee. There is no data");
                if(findDuplicate(request.employee)) return Result<EmployeeDto>.Failure("Duplicated email or phone number");
                
                _mapper.Map(request.employee, employee);

                employee.ModifiedBy = user.Email;
                employee.ModifiedDate = DateTime.UtcNow;
                employee.CreatedBy = user.Email;
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<EmployeeDto>.Failure("Failure to update employee");

                EmployeeDto response = new EmployeeDto();
                _mapper.Map(employee, response);
                return Result<EmployeeDto>.Success(response);

            }

            private bool findDuplicate(EmployeeDto employee)
            {
                return _context.Employees.Any(x => x.ID != employee.ID
                && x.RowStatus && (x.Email == employee.Email || x.PhoneNumber == employee.PhoneNumber));
            }

        }
        
    }   

  
}
