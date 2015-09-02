using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace iIECaB
{
    public class DAL
    {
        private static string DBName;
        private static string ServerName;
        internal static string GetConnectionString(string UserName, string Password)
        {
            return "Server=" + ServerName + ";database=" + DBName + ";uid=" + UserName + ";pwd=" + Password + ";";
        }
        static DAL()
        {
            DBName = ConfigurationManager.AppSettings["DBName"];
            ServerName = ConfigurationManager.AppSettings["DBServerName"];
        }
    }
}