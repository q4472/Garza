using Nskd;
using System;
using System.Data;

namespace Garza.Data
{
    public class HomeData
    {
        public class Home
        {
            public static void CreateSession(Guid sessionId, String userHostAddress)
            {
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[phs_s].[dbo].[session_insert]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter ( "id", sessionId.ToString() ),
                    new RequestParameter ( "user_host_address", userHostAddress )
                };
                ExecuteInSql(rqp);
            }
            public static void UpdateSession(
                String userToken,
                Guid sessionId,
                String cryptKey
                )
            {
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[phs_s].[dbo].[session_update]";
                DataTable dt = new DataTable();
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter ( "user_token", userToken ),
                    new RequestParameter ( "session_id", sessionId.ToString() ),
                    new RequestParameter ( "crypt_key", cryptKey )
                };
                ExecuteInSql(rqp);
            }
            public static DataTable GetSessionById(Guid id)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[dbo].[session_get_by_id]";
                rqp.Parameters = new RequestParameter[] {
                    new RequestParameter ("id", id)
                };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetClientData(String filter)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[dbo].[oc_клиенты_select]";
                rqp.Parameters = new RequestParameter[] {
                new RequestParameter ( "DESCR", filter)
            };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
            public static DataTable GetManagerData(String filter)
            {
                DataTable dt = null;
                RequestPackage rqp = new RequestPackage();
                rqp.Command = "[dbo].[oc_сотрудники_select]";
                rqp.Parameters = new RequestParameter[] {
                new RequestParameter ("DESCR", filter)
            };
                dt = GetFirstTable(Execute(rqp));
                return dt;
            }
        }
        private static String dataServicesHost = "127.0.0.1"; // by default localhost
        public static ResponsePackage ExecuteInSql(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11002/");
            return rsp;
        }
        public static DataSet Execute(RequestPackage rqp)
        {
            ResponsePackage rsp = rqp.GetResponse("http://" + dataServicesHost + ":11002/");
            return rsp.Data;
        }
        public static DataTable GetFirstTable(DataSet ds)
        {
            DataTable dt = null;
            if ((ds != null) && (ds.Tables.Count > 0))
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
    }
}