using application.Core;
using application.Interfaces;
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
    public class EmployeeDelete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ID { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail() && x.RowStatus == true, cancellationToken);
                if (user == null) return Result<Unit>.Failure("Your Account is not found please relogin");
                var employee = await _context.Employees.FindAsync(request.ID);
                if (employee == null) return null;

                employee.ModifiedBy = user.Email;
                employee.ModifiedDate = DateTime.UtcNow;
                employee.RowStatus = false;

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete employee");

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
