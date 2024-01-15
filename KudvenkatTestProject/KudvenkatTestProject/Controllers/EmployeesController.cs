using KudvenkatTestProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace KudvenkatTestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController: ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;                  // Min controller funkar inte alls

        public EmployeesController(IEmployeeRepository employeeRepository) // varför är det Iemployeerepository här men appdbcontext på den andra
        {
            
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
         public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
            

        }

    }
}
