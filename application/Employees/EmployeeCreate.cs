using application.Core;
using application.Interfaces;
using AutoMapper;
using domain.Models;
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
    public class EmployeeCreate
    {
        public class Command : IRequest<Result<Unit>>
        {
            public EmployeeDto employee { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.employee).SetValidator(new EmployeeValidator());
            }
        }
        
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail() && x.RowStatus == true ,cancellationToken);
                if (user == null) return Result<Unit>.Failure("Your Account is not found please relogin");
                if(FinDuplicate(request.employee)) return Result<Unit>.Failure("Duplicated email or phone number");

                Employee employee = new Employee();
                _mapper.Map(request.employee, employee);
                employee.CreatedBy = _userAccessor.GetEmail();
                employee.CreatedDate = DateTime.UtcNow;
                employee.RowStatus = true;
                

                _context.Employees.Add(employee);
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to create activity");

                return Result<Unit>.Success(Unit.Value);

            }

            private bool FinDuplicate(EmployeeDto employee)
            {
                return _context.Employees.Any(x => x.RowStatus
                && (x.Email == employee.Email || x.PhoneNumber == employee.PhoneNumber));
                
            }
        }

        
    }
}
