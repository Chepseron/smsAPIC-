using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MAAIF.Repository
{
    public class Functions
    {
        public string GeneratePIN()
        {
            try
            {
                string _allowedChars = "0123456789";
                Random randNum = new Random();
                char[] chars = new char[4];
                int allowedCharCount = _allowedChars.Length;
                for (int i = 0; i < 4; i++)
                {
                    chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                }
                string pwd = new string(chars);
                return pwd;
            }
            catch (Exception ex)
            {
                //string activity = Path.GetFileName(Request.Path) + " : " + ex.Message;
                //ErrLogger.WriteTextLogs(hdbankid.Value.ToString() + "-" + txtUser.Text, "ELMAPORTAL", DateTime.Now.ToString() + " >> Response / ", txtUser.Text, txtCountry.Text, "Error => " + activity);
                return null;
            }
        }

        public string GenerateReference()
        {
            try
            {
                string _allowedChars = "012345678ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                Random randNum = new Random();
                char[] chars = new char[10];
                int allowedCharCount = _allowedChars.Length;
                for (int i = 0; i < 10; i++)
                {
                    chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                }
                string pwd = new string(chars);
                return pwd;
            }
            catch (Exception ex)
            {
                //string activity = Path.GetFileName(Request.Path) + " : " + ex.Message;
                //ErrLogger.WriteTextLogs(hdbankid.Value.ToString() + "-" + txtUser.Text, "ELMAPORTAL", DateTime.Now.ToString() + " >> Response / ", txtUser.Text, txtCountry.Text, "Error => " + activity);
                return null;
            }
        }

        public string GenerateTXReference()
        {
            try
            {
                string _allowedChars1 = "0123456789";
                string _allowedChars2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                Random randNum = new Random();
                Random randNum2 = new Random();

                char[] chars = new char[4];
                char[] chars2 = new char[5];

                for (int i = 0; i < 4; i++)
                {
                    chars[i] = _allowedChars2[(int)((_allowedChars2.Length) * randNum.NextDouble())];
                }

                for (int i = 0; i < 5; i++)
                {
                    chars2[i] = _allowedChars1[(int)((_allowedChars1.Length) * randNum.NextDouble())];
                }

                string str1 = new string(chars);
                string str2 = new string(chars2);
                string pwd = str1 + str2;

                return pwd;
            }
            catch (Exception ex)
            {
                //string activity = Path.GetFileName(Request.Path) + " : " + ex.Message;
                //ErrLogger.WriteTextLogs(hdbankid.Value.ToString() + "-" + txtUser.Text, "ELMAPORTAL", DateTime.Now.ToString() + " >> Response / ", txtUser.Text, txtCountry.Text, "Error => " + activity);
                return null;
            }
        }

        //Base64Encode
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public double EncryptPassword(string Password, DateTime PWUpdatedOn)
        {
            double lngN;
            int intI;
            double days;
            double Qtr;

            DateTime StartDate = new DateTime(PWUpdatedOn.Year, 01, 01);
            TimeSpan ts = new TimeSpan(PWUpdatedOn.Ticks - StartDate.Ticks);
            //Response.Write(ts.Days+1); 
            days = ts.Days + 1;
            lngN = 0;


            switch (PWUpdatedOn.Month)
            {
                case 1:
                case 2:
                case 3:
                    Qtr = 1;
                    break;
                case 4:
                case 5:
                case 6:
                    Qtr = 2;
                    break;
                case 7:
                    Qtr = 3;
                    break;
                case 8:
                    Qtr = 3;
                    break;
                case 9:
                    Qtr = 3;
                    break;
                case 10:
                    Qtr = 4;
                    break;
                case 11:
                    Qtr = 4;
                    break;
                case 12:
                    Qtr = 4;
                    break;
                default:
                    Qtr = 1;
                    break;
            }
            if (Password.Length <= 8)
                Password = Password.PadRight(8);
            else
                Password = Password.PadRight(20);

            for (intI = 0; intI < Password.Length; intI++)
            {
                char ch = Password[intI];
                lngN = lngN + ((int)ch * (intI + 1)) + PWUpdatedOn.Day + PWUpdatedOn.Month + Qtr + PWUpdatedOn.Year - 2000;
            }
            return lngN;
        }

    }
}