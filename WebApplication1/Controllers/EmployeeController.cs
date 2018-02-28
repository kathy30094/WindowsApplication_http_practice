using System; 
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http; 
using WebApplication1.Models;
using WebApplication1.Code;

namespace WebApplication1.Controllers
{
    public class EmployeeController : ApiController
    {
        static List<Employee> employeeList = new List<Employee>();

        private DAOFunctions _DAOFunction  = new DAOFunctions();

        ///GET  show all employees   /api/Employee
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();
            ReturnValue Ret = _DAOFunction.GetAllEmployeeFromDB(ref employeeList);
            ///此處需要從DB撈出所有employee的資料
            ///存成DataTable，轉成List
            ///return出去
            return employeeList;
        }

        ///GET    show one employee by ID   /api/Employee/{id}
        public IHttpActionResult GetEmployeeByID(int ID)
        {
            Employee employee = new Employee();
            ReturnValue Ret =  _DAOFunction.SearchEmployeeByID(ref employee, ID);
            //var employee = employeeList.FirstOrDefault(em => em.ID == ID); ///Employee em
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        //POST   add employee
        public void PostEmployee([FromBody] Employee employeeToAdd)
        {
            bool duplicate = false;
            foreach (var employee in employeeList)
            {
                if (employee.ID == employeeToAdd.ID)
                {
                    duplicate = true;
                }
            }
            if (duplicate == false)
            {
                employeeList.Add(employeeToAdd);
            }
        }

        //Delete
        public void Delete(int ID)
        {
            ReturnValue Ret = new ReturnValue();
            Ret = _DAOFunction.DeleteEmployee(ID);
        }

        // PUT  update employee
        public void PutEmployee(int id,[FromBody]Employee employeeToPut)
        {

            ReturnValue returnValue = _DAOFunction.UpdateEmployee(employeeToPut);
            //for (int i = 0; i < employeeList.Count; i++)
            //{
            //    if (employeeList[i].ID == id)
            //    {
            //        employeeList[i] = employeeToPut;
            //    }
            //}
        }
    }
}
