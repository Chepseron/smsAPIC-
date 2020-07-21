using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MAAIF.Repository
{
    public class Connection
    {
        public string MaaifDatabaseConnectionString()
        {
            try
            {
                string DataSource = ConfigurationManager.AppSettings["country"].ToString();
                string InitialCatalogue = ConfigurationManager.AppSettings["province"].ToString();
                string UserID = ConfigurationManager.AppSettings["district"].ToString();
                string Password = ConfigurationManager.AppSettings["division"].ToString();
                string ApplicationName = ConfigurationManager.AppSettings["dbApplicationName"].ToString();

                string conString = @"Data Source=" + @DataSource + ";" +
                                        "Initial Catalog=" + InitialCatalogue + ";" +
                                        "User ID=" + UserID + ";" +
                                        "Password=" + Password + ";" +
                                        "MultipleActiveResultSets=False;" +
                                        "Application Name=" + ApplicationName + "";

                return conString;
            }
            catch
            {
                return "";
            }
        }
    }
}