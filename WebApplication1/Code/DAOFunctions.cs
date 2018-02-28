using System.Web;
using WebApplication1.Models;
using ADOLib;
using LogLib;
using System.Data;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace WebApplication1.Code
{
    public class DAOFunctions : DAOBase
    {
        #region Stored Procedure Name
        private class StoredProcedure
        {
            public const string ADD_EMPLOYEE = "AddEmployee";
            /*AddEmployee SP
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE AddEmployee
@id int,
@Name nvarchar(15),
@Sex char(1),
@IsAdd nchar(1) ='' output
AS
if NOT EXISTS(select * from Company.dbo.Employee where Company.dbo.Employee.ID=@id)
BEGIN
	insert into Company.dbo.Employee (ID,Name,Sex) values (@id,@Name,@Sex)
END
else
BEGIN
	set @IsAdd='F'
	
END
GO


**/

            public const string GET_EMPLOYEE = "ReturnAllEmployees";
            /*ReturnAllEmployee SP
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE ReturnAllEmployee

AS
BEGIN
select * from Employee
END
GO
             * */

            public const string SEARCH_EMPLOYEE = "SearchEmployee";

            /* SearchEmployee SP
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE UpdateEmployee
	@id int
AS
BEGIN
	select * from Company.dbo.Employee where Company.dbo.Employee.ID=@id
END
GO
             * **/

            public const string UPDATE_EMPLOYEE = "UpdateEmployee";
            /*UpdateEmployee SP
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SearchEmployee
	@id int,
	@Name nvarchar(15),
	@Sex char(1),
	@IsUpdated nchar(1)=''
AS

if EXISTS (SELECT * FROM Company.dbo.Employee WHERE Company.dbo.Employee.ID = @id)
BEGIN
	update Company.dbo.Employee set Name=@Name,Sex=@Sex where ID = @id
END
else
BEGIN
	set @IsUpdated ='F'
END
GO
             * */

            public const string DELETE_EMPLOYEE = "DeleteEmployee";
            /* DeleteEmployee SP
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DeleteEmployee
@id int,
@IsDelete nchar(1) output
AS
if EXISTS(select * from Company.dbo.Employee where Company.dbo.Employee.ID=@id)
BEGIN
delete Company.dbo.Employee where Company.dbo.Employee.ID=@id
END
else
set @IsDelete='F'
GO
**/

        }//end StoredProcedure
        #endregion
        //public string DB_ConnctString = "Server=RD-006;Database=Company;User ID=sa;Password=t86-t86-";LAPTOP-7BPU7R88
        public string DB_ConnctString = "Server=LAPTOP-7BPU7R88;Database=Company;User ID=sa;Password=t86-t86-";
        #region  enum  _EmployeeField 欄位引索+
        private enum _EmployeeFields
        {
            ID = 0,
            Name,
            Sex,
        }//end _EmployeeFields
        #endregion

        public ReturnValue DeleteEmployee(int ID)
        {
            ReturnValue Ret = ReturnValue.NO_EXECUTE_RETURN;

            try
            {
                //設定sql參數，變數型態nchar(1),參數型別為輸出
                SqlParameter IsDelete = new SqlParameter("@IsDelete", SqlDbType.NChar, 1);
                IsDelete.Direction = ParameterDirection.Output;

                //建置Sqlparameter array並設置其內各parameter的格式，並把前面設置的IsUpdate放進去
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ID",SqlDbType.Int),
                    IsDelete
                };
                //填入值
                sqlParams[0].Value = ID;

                DataTable Dt = new DataTable();

                //連線參數
                _DAO = new DbAccessOperator(DB_ConnctString);

                //建立連線，成功連線並找到DB的Stored Procedure時DBReturn為0  
                //將sqlParams傳入DB，由其中的IsUpdate值判斷資料庫變更資料狀況
                int DBReturn = -1;
                DBReturn = _DAO.ExecuteNonQuery(StoredProcedure.DELETE_EMPLOYEE, ref sqlParams, true, true);

                if (DBReturn != 0)  //失敗
                {
                    Ret = ReturnValue.DB_EXECUTE_ERROR;
                    _Exception = _DAO.GetException();

                    //----------------------------------------
                    // DB Exception Error Log
                    //----------------------------------------
                    string LogMessage = String.Format(LogFormat.DB_EXCEPTION, ReturnValue.DB_EXCEPTION, "Test", StoredProcedure.DELETE_EMPLOYEE, _Exception.Message, _Exception.StackTrace);
                    LogMessage += string.Format("\tParams[{0}]", "");
                    LogUtility.ErrorLog(LogLevel.Low, LogMessage);
                }
                else
                {
                    if (IsDelete.Value.ToString() == "F")
                    {
                        //更新失敗
                        Ret = ReturnValue.DB_UPDATE_FAILURE;
                    }
                    else
                    {
                        Ret = ReturnValue.OK;
                    }// end if
                }//end if
            }
            catch (Exception Ex)//資料庫連接失敗

            {
                _Exception = Ex;
                Ret = ReturnValue.EXCEPTION_ERROR;

                //----------------------------------------
                // Exception Error Log
                //----------------------------------------
                string LogMessage = String.Format(LogFormat.EXCEPTION, ReturnValue.EXCEPTION_ERROR, Ex.Message, Ex.StackTrace);
                LogUtility.ErrorLog(LogLevel.Low, LogMessage);
            }
            finally
            {
                if (_DAO != null)
                    _DAO.Dispose();
            }//end try catch
            return Ret;
        }//end UpdateUser

        public ReturnValue UpdateEmployee(Employee employee)
        {
            ReturnValue Ret = ReturnValue.NO_EXECUTE_RETURN;

            if (employee == null)
                return Ret;
            try
            {

                //設定sql參數，變數型態nchar(1),參數型別為輸出
                SqlParameter IsUpdated = new SqlParameter("@IsUpdated", SqlDbType.NChar, 1);
                IsUpdated.Direction = ParameterDirection.Output;

                //建置Sqlparameter array並設置其內各parameter的格式，並把前面設置的IsUpdate放進去
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ID",SqlDbType.Int),
                    new SqlParameter("@Name",SqlDbType.NVarChar,15),
                    new SqlParameter("@Sex",SqlDbType.Char,1),
                    IsUpdated
                };
                //填入值
                int i = 0;
                sqlParams[i++].Value = employee.ID;
                sqlParams[i++].Value = employee.name;
                sqlParams[i++].Value = employee.sex;

                DataTable Dt = new DataTable();

                //連線參數
                _DAO = new DbAccessOperator(DB_ConnctString);

                //建立連線，成功連線並找到DB的Stored Procedure時DBReturn為0  
                //將sqlParams傳入DB，由其中的IsUpdate值判斷資料庫變更資料狀況
                int DBReturn = -1;
                DBReturn = _DAO.ExecuteNonQuery(StoredProcedure.UPDATE_EMPLOYEE, ref sqlParams, true, true);

                if (DBReturn != 0)  //失敗
                {
                    Ret = ReturnValue.DB_EXECUTE_ERROR;
                    _Exception = _DAO.GetException();

                    //----------------------------------------
                    // DB Exception Error Log
                    //----------------------------------------
                    string LogMessage = String.Format(LogFormat.DB_EXCEPTION, ReturnValue.DB_EXCEPTION, "Test", StoredProcedure.UPDATE_EMPLOYEE, _Exception.Message, _Exception.StackTrace);
                    LogMessage += string.Format("\tParams[{0}]", "");
                    LogUtility.ErrorLog(LogLevel.Low, LogMessage);
                }
                else
                {
                    if (IsUpdated.Value.ToString() == "F")
                    {
                        //更新失敗
                        Ret = ReturnValue.DB_UPDATE_FAILURE;
                    }
                    else
                    {
                        Ret = ReturnValue.OK;
                    }// end if
                }//end if
            }
            catch (Exception Ex)//資料庫連接失敗
            {
                _Exception = Ex;
                Ret = ReturnValue.EXCEPTION_ERROR;

                //----------------------------------------
                // Exception Error Log
                //----------------------------------------
                string LogMessage = String.Format(LogFormat.EXCEPTION, ReturnValue.EXCEPTION_ERROR, Ex.Message, Ex.StackTrace);
                LogUtility.ErrorLog(LogLevel.Low, LogMessage);
            }
            finally
            {
                if (_DAO != null)
                    _DAO.Dispose();
            }//end try catch
            return Ret;
        }//end UpdateUser

        public ReturnValue GetAllEmployeeFromDB(ref List<Employee> employeeList)
        {
            ReturnValue Ret = ReturnValue.NO_EXECUTE_RETURN;
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
                    Ret = ReturnValue.DB_EXECUTE_ERROR;
                    _Exception = _DAO.GetException();
                    string LogMessage = String.Format(LogFormat.DB_EXCEPTION, ReturnValue.DB_EXCEPTION, DB_ConnctString, StoredProcedure.GET_EMPLOYEE, _Exception.Message, _Exception.StackTrace);
                    LogMessage += string.Format("\tParams[{0}]", "");
                    LogUtility.ErrorLog(LogLevel.Low, LogMessage);
                }
                else //連接成功，並找到DB的Stored Procedure
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
                        Ret = ReturnValue.OK;
                    }
                    else //沒有資料
                    {
                        Ret = ReturnValue.DB_SEARCH_NODATA;
                    }//end if
                }//end if
            }
            catch (Exception Ex)//資料庫連接過程失敗
            {
                string logMsg = string.Format(LogFormat.EXCEPTION, ReturnValue.EXCEPTION_ERROR, Ex.Message, Ex.StackTrace);
                LogUtility.ErrorLog(LogLevel.Low, logMsg);
            }
            finally
            {
                if (_DAO != null)
                    _DAO.Dispose();
            }//end try catch
            return Ret;
        }//end GetAllEmployeeFromDB

        public ReturnValue SearchEmployeeByID(ref Employee employee,int ID)
        {
            ReturnValue Ret = ReturnValue.NO_EXECUTE_RETURN;

            try //conncet to DB
            {
                object[] ob = new object[3];

                SqlParameter[] sqlParams = new SqlParameter[]
                 {
                        new SqlParameter("@ID",SqlDbType.Int),
                 };
                sqlParams[0].Value = ID;

                //連接資料庫
                _DAO = new DbAccessOperator(DB_ConnctString);

                //連接成功則為 0 
                int DBReturn = -1;
                DBReturn = _DAO.ExecuteReaderSingleRow(ref ob , StoredProcedure.SEARCH_EMPLOYEE, ref sqlParams, true, true);
               
                if (DBReturn != 0)//失敗，找不到Stored Procedure
                {
                    Ret = ReturnValue.DB_EXECUTE_ERROR;
                    _Exception = _DAO.GetException();
                    string LogMessage = String.Format(LogFormat.DB_EXCEPTION, ReturnValue.DB_EXCEPTION, DB_ConnctString, StoredProcedure.SEARCH_EMPLOYEE, _Exception.Message, _Exception.StackTrace);
                    LogMessage += string.Format("\tParams[{0}]", "");
                    LogUtility.ErrorLog(LogLevel.Low, LogMessage);
                }
                else //連接成功，並找到DB的Stored Procedure
                {
                    if (ob!= null)//有資料
                    {
                        object TmpObj = null;

                        // ID
                        int TmpID;
                        TmpObj = ob[(int)_EmployeeFields.ID];
                        if (int.TryParse(TmpObj.ToString(), out TmpID))///TryParse : 將左邊的string 轉換為右邊的int，並回傳是否成功
                            employee.ID = TmpID;

                        //Name
                        TmpObj = ob[(int)_EmployeeFields.Name];
                        employee.name = TmpObj.ToString();

                        //Sex
                        TmpObj = ob[(int)_EmployeeFields.Sex];
                        employee.sex = TmpObj.ToString();

                        Ret = ReturnValue.OK;
                    }
                    else //沒有資料
                    {
                        Ret = ReturnValue.DB_SEARCH_NODATA;
                    }//end if
                }//end if
            }//end try
            catch (Exception Ex)//資料庫連接過程失敗
            {
                string logMsg = string.Format(LogFormat.EXCEPTION, ReturnValue.EXCEPTION_ERROR, Ex.Message, Ex.StackTrace);
                LogUtility.ErrorLog(LogLevel.Low, logMsg);
            } //end catch
            return Ret;
        }//end SearchEmployeeByID

        public ReturnValue AddEmployee(ref Employee employee)
        {
            ReturnValue Ret = ReturnValue.NO_EXECUTE_RETURN;

            try //conncet to DB
            {

                SqlParameter IsAdd = new SqlParameter("@IsAdd", SqlDbType.NChar, 1);
                IsAdd.Direction = ParameterDirection.Output;

                SqlParameter[] sqlParams = new SqlParameter[]
                 {
                    new SqlParameter("@ID",SqlDbType.Int),
                    new SqlParameter("@Name",SqlDbType.NVarChar,15),
                    new SqlParameter("@Sex",SqlDbType.Char,1),
                    IsAdd
                 };
                int i = 0;
                sqlParams[i++].Value = employee.ID;
                sqlParams[i++].Value = employee.name;
                sqlParams[i++].Value = employee.sex;

                //連接資料庫
                _DAO = new DbAccessOperator(DB_ConnctString);

                //連接成功則為 0 
                int DBReturn = -1;
                DBReturn = _DAO.ExecuteNonQuery(StoredProcedure.ADD_EMPLOYEE, ref sqlParams, true, true);
               
                if (DBReturn != 0)//失敗，找不到Stored Procedure
                {
                    Ret = ReturnValue.DB_EXECUTE_ERROR;
                    _Exception = _DAO.GetException();
                    string LogMessage = String.Format(LogFormat.DB_EXCEPTION, ReturnValue.DB_EXCEPTION, DB_ConnctString, StoredProcedure.SEARCH_EMPLOYEE, _Exception.Message, _Exception.StackTrace);
                    LogMessage += string.Format("\tParams[{0}]", "");
                    LogUtility.ErrorLog(LogLevel.Low, LogMessage);
                }
                else //連接成功，並找到DB的Stored Procedure
                {
                    if (IsAdd.Value.ToString() == "F")
                    {
                        //新增資料失敗
                        Ret = ReturnValue.DB_UPDATE_FAILURE;
                    }
                    else
                    {
                        Ret = ReturnValue.OK;
                    }// end if
                }//end if
            }//end try
            catch (Exception Ex)//資料庫連接過程失敗
            {
                string logMsg = string.Format(LogFormat.EXCEPTION, ReturnValue.EXCEPTION_ERROR, Ex.Message, Ex.StackTrace);
                LogUtility.ErrorLog(LogLevel.Low, logMsg);
            } //end catch
            return Ret;
        }


    }
}
