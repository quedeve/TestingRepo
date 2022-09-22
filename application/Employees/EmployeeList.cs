using application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class EmployeeList
    {
        public class Query : IRequest<Result<List<EmployeeDto>>>
        {
            
        }

        public class Handler: IRequestHandler<Query, Result<List<EmployeeDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;

                _context = context;
            }

            public async Task<Result<List<EmployeeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var employees = await _context.Employees.Where(x => x.RowStatus).ProjectTo<EmployeeDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return Result<List<EmployeeDto>>.Success(employees);
            }
        }
    }
}
