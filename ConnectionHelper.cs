using System;
using System.Web.Configuration;

namespace TaskQuest
{
    public static class ConnectionHelper
    {
        public static string GetConnectionString()
        {
            string machineName = Environment.MachineName;

            // Check for the new user's machine (Eler)
            // Assuming the machine name starts with or matches the server name provided
            if (machineName.Equals("DESKTOP-LNCBN7V", StringComparison.OrdinalIgnoreCase) || 
                machineName.StartsWith("DESKTOP-LNCBN7V", StringComparison.OrdinalIgnoreCase))
            {
                var conn = WebConfigurationManager.ConnectionStrings["TodoAppDB_Eler"];
                if (conn != null) return conn.ConnectionString;
            }

            // Default to the existing connection string (Colleague's or Production)
            return WebConfigurationManager.ConnectionStrings["TodoAppDB"].ConnectionString;
        }
    }
}
