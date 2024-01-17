using KudvenkatTestProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace KudvenkatTestProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;                

        public EmployeesController(IEmployeeRepository employeeRepository) 
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
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {

                    return BadRequest();
                }
                var emp = _employeeRepository.GetEmployeeByEmail(employee.Email);
                if (emp != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return BadRequest(ModelState);
                }


                var createdEmployee = await _employeeRepository.AddEmployee(employee);

                return CreatedAtAction(nameof(GetEmployee),
                    new { id = createdEmployee.EmployeeId }, createdEmployee);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error Creating New employee record");
            }


        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                {
                    return BadRequest();
                }
                var employeeToUpdate = await _employeeRepository.GetEmployee(id);

                if (employeeToUpdate == null)
                {
                    return NotFound($"Employee with id = {id} was not found");
                }

                return await _employeeRepository.UpdateEmployee(employee);


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Updating employee record with id = {id}");
            }

        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {                
                var employeeToDelete = await _employeeRepository.GetEmployee(id);
                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with id = {id} was not found");
                }
                
                await _employeeRepository.DeleteEmployee(id);
                
                return Ok($"Employee with id = {id} was deleted");
            }
            catch (Exception exa)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error Deleting employee record with id = {id}");
            }
        }


    }
}   