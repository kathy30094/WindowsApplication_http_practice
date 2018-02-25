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

        //GET     /api/Employee
        public IEnumerable<Employee> GetAllEmployees()
        {
            return employees;
        }

        //GET     /api/Employee/{id}
        public IHttpActionResult GetEmployeeByID(int ID)
        {
            var employee = employees.FirstOrDefault(em => em.ID == ID); ///Employee em
            if (employee == null)
            {
                return NotFound();
            }
            return  Ok(employee);
        }

        ////POST 
        //public IHttpActionResult PostEmployee(int ID,[FromBody]Employee employeeToAdd)
        //{
        //    var employee = employees.FirstOrDefault(em => em.ID == ID);
        //    if (employee == null)//如果已經有此項目就不增加
        //    {
        //        employees.Add(employeeToAdd);
        //        return Ok(employee);
        //    }
        //    else return null;

        //}

        // POST api/values  
        public void Post([FromBody]Employee value)
        {
            employees.Add(value);
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
        public IHttpActionResult PutEmployee(int ID, [FromBody] Employee employeeToPut)
        {
            var employee = employees.FirstOrDefault(em => em.ID == ID);
            if (employee == null)
            {
                return null;
            }
            else
            {
                employees[ID] = employeeToPut;
                return NotFound() ;
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
