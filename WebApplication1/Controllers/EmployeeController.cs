using System; 
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EmployeeController : ApiController
    {
        static List<Employee> employees = new List<Employee>();

        //show all employees   //GET     /api/Employee
        public IEnumerable<Employee> GetAllEmployees()
        {
            return employees;
        }

        //show one employee by ID  //GET     /api/Employee/{id}
        public IHttpActionResult GetEmployeeByID(int ID)
        {
            var employee = employees.FirstOrDefault(em => em.ID == ID); ///Employee em
            if (employee == null)
            {
                return NotFound();
            }
            return  Ok(employee);
        }

        //POST
        public void PostEmployee([FromBody] Employee employeeToAdd)
        {
            bool duplicate = false;
            foreach (var employee in employees)
            {
                if (employee.ID == employeeToAdd.ID)
                {
                    duplicate = true;
                }
            }
            if (duplicate == false)
            {
                employees.Add(employeeToAdd);
            }
        }
       

        //Delete
        public IHttpActionResult Delete(int ID)
        {
            var employee = employees.FirstOrDefault(em => em.ID == ID);
            if (employee == null)//如果有此項目才做delete
            {
                return NotFound();
            }
            else
            {
                int IDindex = employees.FindIndex(em => em.ID == ID);
                employees.RemoveAt(IDindex);
                return null;
            }
           
        }

        // PUT
        public void PutEmployee(int id,[FromBody]Employee employeeToPut)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].ID == id)
                {
                    employees[i] = employeeToPut;
                }
            }
        }


        ///think after : how can find by name?
        //public IHttpActionResult GetEmployeeByName(string Name)
        //{
        //    var employee = employees.FirstOrDefault(em => em.name == Name); ///Employee em
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(employee);
        //}
    }
}
