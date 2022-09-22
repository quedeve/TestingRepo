using application.Employees;
using Microsoft.AspNetCore.Mvc;

namespace main.Controllers
{
    public class EmployeeController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            return HandleResult(await Mediator.Send(new EmployeeList.Query()));
        }

        [HttpPost]
        public async Task<ActionResult> PostEmployee([FromBody]EmployeeDto employee)
        {
            return HandleResult(await Mediator.Send(new EmployeeCreate.Command { employee = employee }));  
        }
        [HttpPut]
        public async Task<ActionResult> PutEmployee([FromBody]EmployeeDto employee)
        {
            return HandleResult(await Mediator.Send(new EmployeeEdit.Command { employee = employee }));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            return HandleResult(await Mediator.Send(new EmployeeDelete.Command { ID = id }));
        }
    }
}
