using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MAAIF.Repository
{
    public class ErrLogger
    {
        public static void LogError(string Message)
        {

            //StreamWriter sw = null;
            try
            {
                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                string sPathName = @"C:\MAAIF\Log\";

                if (!Directory.Exists(sPathName))
                {
                    Directory.CreateDirectory(sPathName);
                }

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                //open a text file with the errors for the day 
                // If it does not exist create it
                using (StreamWriter sw = new StreamWriter(sPathName + "Log_" + sErrorTime + ".txt", true))
                {
                    sw.WriteLine(sLogFormat + Message);
                }
                //sw = new StreamWriter(sPathName + "ErrorLog_" + sErrorTime + ".txt", true);

                //sw.WriteLine(sLogFormat + Message);
                //sw.Flush();

            }
            catch (Exception ex)
            {
                LogError("ErrLogger-" + ex.Message);
            }
        }
    }
}