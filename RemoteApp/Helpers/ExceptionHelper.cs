using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace RemoteApp.Helpers
{
    public static class ExceptionHelper
    {
        private static Exception GetInnermostException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        public static bool IsUniqueConstraintViolation(Exception ex)
        {
             var innermost = GetInnermostException(ex);
             var sqlException = ex.InnerException as Microsoft.Data.SqlClient.SqlException;

             return sqlException != null && sqlException.Class == 14 && (sqlException.Number == 2601 || sqlException.Number == 2627);

        }

        public static bool IsRefDeleteViolation(Exception ex)
        {
            var innermost = GetInnermostException(ex);
            var sqlException = ex.InnerException as Microsoft.Data.SqlClient.SqlException;

            return sqlException != null && sqlException.Class == 16 && sqlException.Number == 547 ;

        }


        public static bool IsUniqueConstraintViolation44(Exception e)
        {

             var ex = e as Microsoft.Data.SqlClient.SqlException;
 

            bool res = false;
            if (((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2601 || ((Microsoft.Data.SqlClient.SqlException)ex.InnerException).Number == 2627) { 
                res =true;

            }

            return res;
        }
    }
}
