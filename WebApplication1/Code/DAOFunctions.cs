using System.Web;
using WebApplication1.Models;
using ADOLib;
using LogLib;
using System.Data;
using System.Collections.Generic;
using System;

namespace WebApplication1.Code
{
    public class DAOFunctions : DAOBase
    {
        #region Stored Procedure Name
        private class StoredProcedure
        {

            public const string GET_EMPLOYEE = "ReturnAllEmployees";
            public const string UPDATE_EMPLOYEE = "";
        }//end StoredProcedure
        #endregion
        public string DB_ConnctString = "Server=RD-006;Database=Company;User ID=sa;Password=t86-t86-";

        #region  enum  _EmployeeField 欄位引索+
        private enum _EmployeeFields
        {
            ID = 0,
            Name,
            Sex,
        }//end _EmployeeFields
        #endregion
        public ReturnValue GetAllEmployeeFromDB(ref List<Employee> employeeList)
        {

            ReturnValue returnValue = ReturnValue.NO_EXECUTE_RETURN;
            /////
            if (employeeList == null)
                employeeList = new List<Employee>();
            try //conncet to DB
            {
                DataTable Dt = new DataTable();

                //連接資料庫
                _DAO = new DbAccessOperator(DB_ConnctString);

                //連接成功則為 0 
                int DBReturn = -1;
                DBReturn = _DAO.ExecuteReader(ref Dt, StoredProcedure.GET_EMPLOYEE, true, true);

                if (DBReturn != 0)//失敗，找不到Stored Procedure
                {
                    returnValue = ReturnValue.DB_EXECUTE_ERROR;
                    _Exception = _DAO.GetException();
                    string LogMessage = String.Format(LogFormat.DB_EXCEPTION, ReturnValue.DB_EXCEPTION, DB_ConnctString, StoredProcedure.GET_EMPLOYEE, _Exception.Message, _Exception.StackTrace);
                    LogMessage += string.Format("\tParams[{0}]", "");
                    LogUtility.ErrorLog(LogLevel.Low, LogMessage);
                }
                else //連接成功並找到Stored Procedure
                {
                    if (Dt.Rows.Count > 0)//有資料
                    {
                        object TmpObj = null;
                        for (int i = 0; i < Dt.Rows.Count; i++)
                        {
                            Employee employee = new Employee();

                            // ID
                            int TmpID;
                            TmpObj = Dt.Rows[i][(int)_EmployeeFields.ID];
                            if (int.TryParse(TmpObj.ToString(), out TmpID))///TryParse : 將左邊的string 轉換為右邊的int，並回傳是否成功
                                employee.ID = TmpID;

                            //Name
                            TmpObj = Dt.Rows[i][(int)_EmployeeFields.Name];
                            employee.name = TmpObj.ToString();

                            //Sex
                            TmpObj = Dt.Rows[i][(int)_EmployeeFields.Sex];
                            employee.sex = TmpObj.ToString();
                            employeeList.Add(employee);
                        }//end for
                        returnValue = ReturnValue.OK;
                    }
                    else //沒有資料
                    {
                        returnValue = ReturnValue.DB_SEARCH_NODATA;
                    }//end if
                }//end if
            }//end try
            catch(Exception Ex)//資料庫連接過程失敗
            {
                string logMsg = string.Format(LogFormat.EXCEPTION, ReturnValue.EXCEPTION_ERROR, Ex.Message, Ex.StackTrace);
                LogUtility.ErrorLog(LogLevel.Low, logMsg);
            } //end catch
            return returnValue;
        }//end GetAllEmployeeFromDB
    }
}
