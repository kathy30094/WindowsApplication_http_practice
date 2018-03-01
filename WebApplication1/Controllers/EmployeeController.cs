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
        private static List<Employee> employeeList = new List<Employee>();

        private static  DAOFunctions _DAOFunction  = new DAOFunctions();
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////why static thread

        ///GET  show all employees   /api/Employee
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();
            ///此處需要從DB撈出所有employee的資料
            ///存成DataTable，轉成List
            ///return出去
            ReturnValue Ret = _DAOFunction.GetAllEmployeeFromDB(ref employeeList);
            return employeeList;
        }  // end GetAllEmployee

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
        } // end GetEmployeeByID

        //POST   add employee
        public void PostEmployee([FromBody] Employee employeeToAdd)
        {
            _DAOFunction.AddEmployee(ref employeeToAdd);
        } // end PostEmployee

        //Delete
        public void Delete(int ID)
        {
            ReturnValue Ret = new ReturnValue();
            Ret = _DAOFunction.DeleteEmployee(ID);
        } // end Delete

        // PUT  update employee
        public void PutEmployee(int id,[FromBody]Employee employeeToPut)
        {
            ReturnValue returnValue = _DAOFunction.UpdateEmployee(employeeToPut);
        } // end PutEmployee
    }
}
