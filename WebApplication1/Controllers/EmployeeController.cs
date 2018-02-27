﻿using System; 
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

        //show all employees   //GET     /api/Employee
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();
            ReturnValue returnValue = _DAOFunction.GetAllEmployeeFromDB(ref employeeList);
            ///此處需要從DB撈出所有employee的資料
            ///存成DataTable，轉成List
            ///return出去
            return employeeList;
        }

        //show one employee by ID  //GET     /api/Employee/{id}
        public IHttpActionResult GetEmployeeByID(int ID)
        {
            var employee = employeeList.FirstOrDefault(em => em.ID == ID); ///Employee em
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
        public IHttpActionResult Delete(int ID)
        {
            var employee = employeeList.FirstOrDefault(em => em.ID == ID);
            if (employee == null)//如果有此項目才做delete
            {
                return NotFound();
            }
            else
            {
                int IDindex = employeeList.FindIndex(em => em.ID == ID);
                employeeList.RemoveAt(IDindex);
                return Ok(employee);
            }
           
        }

        // PUT
        public void PutEmployee(int id,[FromBody]Employee employeeToPut)
        {
            for (int i = 0; i < employeeList.Count; i++)
            {
                if (employeeList[i].ID == id)
                {
                    employeeList[i] = employeeToPut;
                }
            }
        }
    }
}
