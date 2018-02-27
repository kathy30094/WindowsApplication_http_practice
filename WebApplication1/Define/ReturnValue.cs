using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public enum ReturnValue
{
    // 執行成功
    OK = 0,

    //----------------------------------------------------
    // 共用 Error Code (20000~21999)
    //----------------------------------------------------

    COMM_ERROR_CODE_START = 20000,

    // 沒有執行就返回
    NO_EXECUTE_RETURN = 20001,

    // 例外錯誤
    EXCEPTION_ERROR = 20002,

    DB_EXCEPTION = 20003,

    // 程式錯誤
    PROGRAM_ERROR = 20004,



    // DB
    DB_EXECUTE_ERROR,
    DB_DELETE_FAILURE,
    DB_ADD_FAILURE,
    DB_UPDATE_FAILURE,
    DB_SEARCH_FAILURE,
    DB_SEARCH_NODATA,
}
