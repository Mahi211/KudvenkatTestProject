
using Microsoft.EntityFrameworkCore;

namespace KudvenkatTestProject.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;


        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
            if (employee.Department != null)
            {
                _appDbContext.Entry(employee.Department).State = EntityState.Unchanged;  
            }
            var result = await _appDbContext.Employees.AddAsync(employee);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }
          
        public async Task DeleteEmployee(int employeeId)
        {
            var result = await _appDbContext.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            
            if (result != null)
            {
                _appDbContext.Employees.Remove(result);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            var result = await _appDbContext.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            return result;
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);

        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _appDbContext.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            IQueryable<Employee> query = _appDbContext.Employees;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
            if (gender != null)
            {
                query = query.Where(e => e.Gender == gender);
            }

            return await query.ToListAsync();
        }

        public async Task<Employee> UpdateEmployee(Employee employee)
        {
            var result = await _appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);
            if (result != null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Gender = employee.Gender;
                result.Email = employee.Email;
                result.DateOfBirth = employee.DateOfBirth;
                result.PhotoPath = employee.PhotoPath;
                if (employee.DepartmentId != 0)
                {
                    result.DepartmentId = employee.DepartmentId;
                }
                else if (employee.Department != null)
                {
                    result.DepartmentId = employee.Department.DepartmentId;
                        
                }
                

                await _appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
