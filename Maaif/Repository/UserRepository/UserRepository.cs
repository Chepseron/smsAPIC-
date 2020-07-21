using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using MAAIF.Models;
using System.Web.Http;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace MAAIF.Repository.UserRepository
{
    public class UserRepository : ApiController
    {

        ResponseData response = new ResponseData();

        #region Authentication
        // User Login
        public ResponseData UserLogin(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                DataSet dt = new DataSet();

                // Get UserProfile
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("sp_GetPortalUser", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);

                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt == null)
                {
                    response.responseCode = "091";
                    response.responseMessage = "Something is NOT Right. Please Try Again Later";
                    //return response;
                }

                else if (dt.Tables[0].Rows.Count > 0)
                {
                    double pass = 0, pass2 = 0;

                    pass = fn.EncryptPassword(obj.Password, DateTime.Parse(dt.Tables[0].Rows[0]["ControlDate"].ToString()));
                    pass2 = double.Parse(dt.Tables[0].Rows[0]["Password"].ToString());
                    if (pass.Equals(pass2))
                    {
                        if (bool.Parse(dt.Tables[0].Rows[0]["IsLoginDisabled"].ToString()))
                        {
                            response.responseCode = "099";
                            response.responseMessage = "Account is Blocked. Please Contact Administrator";
                        }
                        else if (bool.Parse(dt.Tables[0].Rows[0]["IsLoggedIn"].ToString()))
                        {
                            response.responseCode = "099";
                            response.responseMessage = "You are Logged In Somewhere Else. Please logout and Try Again";
                        }
                        else if (bool.Parse(dt.Tables[0].Rows[0]["ChangePasswordAtNextLogon"].ToString()))
                        {
                            response.responseCode = "021";
                            response.payload = dt;
                            response.data = JsonConvert.SerializeObject(dt);
                        }
                        else
                        {
                            updateLoginStatus(obj, "00");
                            response.payload = dt;
                            response.responseCode = "000";
                            response.responseMessage = "SUCCESS";
                            response.data = JsonConvert.SerializeObject(dt);
                            //return response;
                        }
                    }
                    else
                    {
                        //  updateLoginStatus(obj, "99");
                        response.responseCode = "099";
                        response.responseMessage = "Invalid Credentials Provided. Please Try Again";
                        //return response;
                    }
                }

                else
                {
                    response.responseCode = "099";
                    response.responseMessage = "Invalid Credentials Provided. Please Try Again";
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                return response;
            }

        }

        // User Logout
        public ResponseData UserLogout(User obj)
        {
            try
            {
                updateLoginStatus(obj, "01");
                response.responseCode = "000";
                response.responseMessage = "User Successfully Logged Out.";
                return response;
            }
            catch
            {
                return response;
            }
        }








        // GET Regions
        public IDictionary<string, object> GetRegions()
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetRegions", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        //using (SqlDataAdapter da = new SqlDataAdapter(com))
                        //{
                        //    using (DataSet dt = new DataSet())
                        //    {
                        //        da.Fill(dt);

                        //        response.payload = dt;
                        //        response.responseCode = "000";
                        //        response.responseMessage = "SUCCESS";
                        //        return response;
                        //    }
                        //}
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(0).ToString());
                                o.Add(reader.GetValue(1).ToString());
                                o.Add(reader.GetValue(2).ToString());
                                o.Add(reader.GetValue(3).ToString());
                                o.Add(reader.GetValue(4).ToString());
                                o.Add(reader.GetValue(5).ToString());
                                o.Add(reader.GetValue(9).ToString());
                                o.Add(reader.GetValue(10).ToString());
                                o.Add(reader.GetValue(0).ToString());
                                list.Add(o);
                            }
                            con.Close();
                            map.Add("status", "000");
                            map.Add("aaData", list);
                            map.Add("iTotalRecords", list.Count);
                            map.Add("iTotalDisplayRecords", list.Count);
                            map.Add("sEcho", 1);
                        }
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }




        public IDictionary<string, object> GetCluster()
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("GetCluster", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(0).ToString());
                                o.Add(reader.GetValue(1).ToString());
                                o.Add(reader.GetValue(2).ToString());
                                list.Add(o);
                            }
                            con.Close();
                            map.Add("status", "000");
                            map.Add("aaData", list);
                            map.Add("iTotalRecords", list.Count);
                            map.Add("iTotalDisplayRecords", list.Count);
                            map.Add("sEcho", 1);
                        }
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }




        public IDictionary<string, object> GetDistrict()
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("GetDistrict", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(0).ToString());
                                o.Add(reader.GetValue(1).ToString());
                                o.Add(reader.GetValue(2).ToString());
                                list.Add(o);
                            }
                            con.Close();
                            map.Add("status", "000");
                            map.Add("aaData", list);
                            map.Add("iTotalRecords", list.Count);
                            map.Add("iTotalDisplayRecords", list.Count);
                            map.Add("sEcho", 1);
                        }
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }



        public void updateLoginStatus(User obj, string status)
        {
            try
            {
                Connection connection = new Connection();
                string connString = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    using (SqlCommand com = new SqlCommand("p_UpdateLogInStatus", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Operator", obj.UserID);
                        com.Parameters.AddWithValue("@Status", status);
                        com.Parameters.AddWithValue("@SessionID", obj.SessionID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        con.Open();
                        com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // USER change OWN Password
        public ResponseData ChangeUserPassword(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DateTime controlDate = DateTime.Now;
                string password = fn.EncryptPassword(obj.Password, controlDate).ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("sp_ChangeLoginPassword", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);

                        com.Parameters.AddWithValue("@Password", password);
                        com.Parameters.AddWithValue("@ControlDate", controlDate);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR";
                return response;
            }
        }

        public async System.Threading.Tasks.Task<ResponseData> ResetUserPassword(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.IPAddress = "0.0.0.0";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    HttpResponseMessage res = await client.PostAsJsonAsync(ConfigurationManager.AppSettings["maaifTest"] + "api/ResetUserPassword", obj);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);

                    if (res.IsSuccessStatusCode)
                    {
                        response.responseCode = "000";
                        response.responseMessage = data.responseMessage;
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    response.responseCode = "091";
                    response.responseMessage = error;
                    return response;
                }
            }
            response.responseCode = "091";
            response.responseMessage = "Server Error";
            return response;
        }


        public ResponseData ResendMessage(User obj)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.39.2:26000/EVMSAPI/api/SendPIN");
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Key=" + "AIzaSyDQ1-HTfbIlVRbsaX9bL2c7lCiyAA4Y7bw");
            httpWebRequest.Method = "POST";
            httpWebRequest.UseDefaultCredentials = true;
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\n" +
                                "\"MobileNumber\" : \"" + obj.MobileNumber + "\",\n" +
                              "}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //RootObject deserializedResults = JsonConvert.DeserializeObject<RootObject>(result);
                if (result.Contains("error"))
                {
                    response.responseCode = "091";
                    response.responseMessage = result;
                    return response;
                }
                else
                {
                    //return "000|" + result;
                    response.responseCode = "000";
                    response.responseMessage = result;
                    return response;
                }
            }
        }





        // USER Password Reset
        //public ResponseData ResetUserPassword(User obj)
        //{
        //    try
        //    {
        //        Functions fn = new Functions();
        //        Connection connection = new Connection();
        //        string constring = connection.MaaifDatabaseConnectionString();
        //        string statusDescription = "";

        //        string Pin = "";
        //        DateTime controlDate;

        //        Pin = fn.GeneratePIN();
        //        controlDate = DateTime.Now;

        //        obj.Ref1 = fn.Base64Encode(Pin);
        //        obj.Ref2 = fn.EncryptPassword(Pin, controlDate).ToString();
        //        obj.ControlDate = controlDate.ToString();

        //        DataTable dt = new DataTable();
        //        getPINResetRequestCollumns(dt);
        //        dt.Rows.Add(
        //            obj.UserID, obj.MobileNumber, obj.FirstName + " " + obj.LastName, obj.UserGroup, obj.ControlDate, obj.Ref1, obj.Ref2, obj.CreatedBy, obj.CreatedOn,
        //            obj.EventID, obj.ModuleID, obj.IPAddress
        //            );

        //        DataSet data = new DataSet();
        //        data.Tables.Add(dt);

        //        var FirstChild = XDocument.Parse(data.GetXml());
        //        var document = FirstChild.Root.Elements().First();
        //        document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
        //        obj.NewData = document.ToString();

        //        using (SqlConnection con = new SqlConnection(constring))
        //        {
        //            using (SqlCommand com = new SqlCommand("p_AddResetPinRequest", con))
        //            {
        //                con.Open();
        //                com.CommandType = CommandType.StoredProcedure;
        //                com.Parameters.AddWithValue("@UserID", obj.UserID);
        //                com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);

        //                com.Parameters.AddWithValue("@CustomerName", obj.FirstName + " " + obj.LastName);
        //                com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
        //                com.Parameters.AddWithValue("@ControlDate", obj.ControlDate);
        //                com.Parameters.AddWithValue("@Ref1", obj.Ref1);
        //                com.Parameters.AddWithValue("@Ref2", obj.Ref2);

        //                com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
        //                com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
        //                com.Parameters.AddWithValue("@EventID", obj.EventID);
        //                com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
        //                com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
        //                com.Parameters.AddWithValue("@DetailOld", obj.OldData);
        //                com.Parameters.AddWithValue("@DetailNew", obj.NewData);

        //                using (SqlDataReader dr = com.ExecuteReader())
        //                {

        //                    response.responseCode = "000";
        //                    if (dr.Read())
        //                    {
        //                        statusDescription = dr[0].ToString();
        //                        response.responseMessage = statusDescription;
        //                    }
        //                }
        //            }
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
        //        response.responseCode = "091";
        //        response.responseMessage = "ERROR";
        //        return response;
        //    }
        //}

        // CHECK User Rights to Module
        public ResponseData CheckUserRights(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();

                string responseCode = "091";
                DataSet ds = new DataSet();

                response.responseCode = "091";
                response.responseMessage = "Access Denied";

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUserRights", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@SessionID", obj.SessionID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);

                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            da.Fill(ds);
                        }
                    }
                }

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            bool res = false;

                            foreach (DataRow rs in ds.Tables[0].Rows)
                            {
                                responseCode = rs["Status"].ToString();
                                if (responseCode == "000")
                                {
                                    res = Boolean.Parse(rs[Rights(int.Parse(obj.EventID))].ToString());

                                    if (res)
                                    {
                                        response.responseCode = "000";
                                        response.responseMessage = "Access Allowed";
                                    }
                                }
                                else if (responseCode == "093")
                                {
                                    response.responseCode = "093";
                                    response.responseMessage = "Invalid Session";
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR";
                return response;
            }
        }


        protected string Rights(int Right)
        {
            string[] AllRights = { "AllowView", "AllowAdd", "AllowEdit", "AllowDelete", "AllowSupervision", "IsSupervisionRequired" };
            return AllRights[Right];
        }

        #endregion Authentication

        #region UserManagement
        // GET User
        public IDictionary<string, object> GetUser(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUser", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
                        com.Parameters.AddWithValue("@OperatorID", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(32).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(23).ToString());
                            o.Add(reader.GetValue(24).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }

        public IDictionary<string, object> GetEvoucherAccounts(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetEvoucherAccounts", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
                        com.Parameters.AddWithValue("@OperatorID", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(32).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(23).ToString());
                            o.Add(reader.GetValue(24).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }

        public IDictionary<string, object> GetResendMessage()
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetFailedPinDeliveries", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(0).ToString());

                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }
        public IDictionary<string, object> eTagSatisfaction(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("rw_EtagValidationLogs", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }
        public IDictionary<string, object> GetUserByGender(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUserByGender", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Gender", obj.Gender);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(7).ToString());
                            o.Add(reader.GetValue(24).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }
        public IDictionary<string, object> GetDealers(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUser", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
                        com.Parameters.AddWithValue("@OperatorID", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }

            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }
        public IDictionary<string, object> GetDashboard(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {



                string div = "<div class=\"progress thin\">\n"
                + "                                        <div class=\"progress-bar progress-bar-danger\" role=\"progressbar\" aria-valuenow=\"57\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: 57%\">\n"
                + "                                        </div>\n"
                + "                                        <div class=\"progress-bar progress-bar-warning\" role=\"progressbar\" aria-valuenow=\"43\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: 43%\">\n"
                + "                                        </div>\n"
                + "                                    </div>\n"
                + "                                    <span class=\"sr-only\">57%</span>";
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();
                DataSet dt = new DataSet();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_DashBoard_new", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        List<List<object>> list = new List<List<object>>(); // Totals
                        List<List<object>> list2 = new List<List<object>>(); // users by region
                        List<List<object>> list3 = new List<List<object>>(); // regions by Gender
                        List<List<object>> list4 = new List<List<object>>(); // Stock by Dealer
                        List<List<object>> list5 = new List<List<object>>(); // redemption per product
                        List<List<object>> list6 = new List<List<object>>(); //non redemption per product
                        List<List<object>> list7 = new List<List<object>>(); //number of farmers who have redeemed
                        List<List<object>> list8 = new List<List<object>>(); //male vs female
                        List<List<object>> list9 = new List<List<object>>(); //amount contributed by govt and farmer
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            da.Fill(dt);
                            if (dt != null)
                            {
                                foreach (DataRow row in dt.Tables[0].Rows)
                                {


                                    //TotalAgroDealearTransactions	| TotalStockItems | TotalPacedOrders | TotalAgroDealers | TotalRegisteredMale | TotalRegisteredFemale
                                    List<object> o = new List<object>();
                                    o.Add(row[0].ToString()); // TotalAgroDealearTransactions
                                    o.Add(row[1].ToString()); // TotalStockItems
                                    o.Add(row[2].ToString()); //  Total Placed Orders
                                    o.Add(row[3].ToString()); //  Total agrodelaers
                                    o.Add(row[4].ToString()); //  TotalRegisteredMale
                                    o.Add(row[5].ToString()); //  TotalRegisteredfeMale
                                    list.Add(o);
                                }

                                foreach (DataRow row in dt.Tables[1].Rows)
                                {
                                    List<object> o = new List<object>();
                                    o.Add(row[0].ToString()); // Region
                                    o.Add(row[1].ToString()); // male
                                    o.Add(row[2].ToString()); // female
                                    list2.Add(o);
                                }
                                foreach (DataRow row in dt.Tables[2].Rows)
                                {
                                    List<object> o = new List<object>();
                                    o.Add(row[0].ToString()); // Total farmer payments
                                    o.Add(row[1].ToString()); // Total govt payments
                                    list3.Add(o);
                                }
                                //foreach (DataRow row in dt.Tables[3].Rows)
                                //{
                                //    List<object> o = new List<object>();
                                //    o.Add(row[0].ToString()); // Dealer
                                //    o.Add(row[1].ToString()); // Stock

                                //    list4.Add(o);
                                //}
                                //foreach (DataRow row in dt.Tables[4].Rows)
                                //{
                                //    List<object> o = new List<object>();
                                //    o.Add(row[0].ToString()); // PRODUCT
                                //    o.Add(row[1].ToString()); // NUMBER

                                //    list5.Add(o);
                                //}
                                //foreach (DataRow row in dt.Tables[5].Rows)
                                //{
                                //    List<object> o = new List<object>();
                                //    o.Add(row[0].ToString()); // PRODUCT
                                //    o.Add(row[1]); // NUMBER
                                //    list6.Add(o);
                                //}

                                //foreach (DataRow row in dt.Tables[6].Rows)
                                //{
                                //    List<object> o = new List<object>();
                                //    o.Add(row[0].ToString()); // PRODUCT
                                //    o.Add(row[1]); // number male 
                                //    o.Add(row[2]); // number female
                                //    list7.Add(o);
                                //}

                                //foreach (DataRow row in dt.Tables[7].Rows)
                                //{
                                //    List<object> o = new List<object>();
                                //    o.Add(row[0]); // male
                                //    o.Add(row[1]); // female

                                //    list8.Add(o);
                                //}

                                //foreach (DataRow row in dt.Tables[8].Rows)
                                //{
                                //    List<object> o = new List<object>();
                                //    o.Add(row[0]); // government
                                //    o.Add(row[1]); // farmer
                                //    list9.Add(o);
                                //}
                            }
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("aaData2", list2);
                        map.Add("aaData3", list3);
                        //map.Add("aaData4", list4);
                        //map.Add("aaData5", list5);
                        //map.Add("aaData6", list6);
                        //map.Add("aaData7", list7);
                        //map.Add("aaData8", list8);
                        //map.Add("aaData9", list9);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }
                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }
        public IDictionary<string, object> GetUserByID(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUser", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
                        com.Parameters.AddWithValue("@OperatorID", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();
                        int i = 0;
                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(7).ToString());
                            o.Add(reader.GetValue(8).ToString());
                            o.Add(reader.GetValue(9).ToString());
                            o.Add(reader.GetValue(10).ToString());
                            o.Add(reader.GetValue(11).ToString());
                            o.Add(reader.GetValue(12).ToString());
                            o.Add(reader.GetValue(13).ToString());
                            o.Add(reader.GetValue(14).ToString());
                            o.Add(reader.GetValue(15).ToString());
                            o.Add(reader.GetValue(16).ToString());
                            o.Add(reader.GetValue(17).ToString());
                            o.Add(reader.GetValue(18).ToString());
                            o.Add(reader.GetValue(19).ToString());
                            o.Add(reader.GetValue(20).ToString());
                            o.Add(reader.GetValue(21).ToString());
                            o.Add(reader.GetValue(22).ToString());
                            o.Add(reader.GetValue(23).ToString());
                            o.Add(reader.GetValue(24).ToString());
                            o.Add(reader.GetValue(25).ToString());
                            o.Add(reader.GetValue(26).ToString());
                            o.Add(reader.GetValue(27).ToString());
                            o.Add(reader.GetValue(28).ToString());
                            o.Add(reader.GetValue(29).ToString());
                            o.Add(reader.GetValue(30).ToString());
                            o.Add(reader.GetValue(31).ToString());
                            o.Add(reader.GetValue(32).ToString());
                            list.Add(o);
                        }
                        con.Close();



                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }
        }
        public IDictionary<string, object> GetResendMessageByID(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetFailedPinDeliveries", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }
                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }
        }
        public async System.Threading.Tasks.Task<ResponseData> GetBalance(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.MobileNumber = obj.MobileNumber;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    string url = ConfigurationManager.AppSettings["maaifTest"].ToString();
                    HttpResponseMessage res = await client.PostAsJsonAsync(url + "api/GetBalance", obj);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);
                    if (res.IsSuccessStatusCode)
                    {
                        response.responseCode = "000";
                        response.responseMessage = data.responseMessage;
                        return response;
                    }
                }
                catch (HttpRequestException e)
                {
                    var error = e.InnerException.Message;
                    response.responseCode = "091";
                    response.responseMessage = error;
                    return response;
                }
            }
            response.responseCode = "091";
            response.responseMessage = "Server Error";
            return response;
        }

        //public ResponseData GetBalance(User obj)
        //{
        //    IDictionary<string, object> map = new Dictionary<string, object>();
        //    try
        //    {
        //        Connection connection = new Connection();
        //        string connStr = connection.MaaifDatabaseConnectionString();

        //        using (SqlConnection con = new SqlConnection(connStr))
        //        {
        //            using (SqlCommand com = new SqlCommand("p_GetBalance", con))
        //            {
        //                con.Open();
        //                com.CommandType = CommandType.StoredProcedure;
        //                com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
        //                List<List<object>> list = new List<List<object>>();
        //                SqlDataReader reader;
        //                reader = com.ExecuteReader();

        //                while (reader.Read())
        //                {
        //                    List<object> o = new List<object>();
        //                    o.Add(reader.GetValue(0).ToString());
        //                    response.responseCode = "000";
        //                    response.responseMessage = reader.GetValue(0).ToString();
        //                    list.Add(o);
        //                }
        //                con.Close();
        //                return response;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
        //        response.responseCode = "091";
        //        response.responseMessage = "ERROR|" + ex.Message;
        //        return response;
        //    }
        //}

        public IDictionary<string, object> GetResetPinUser(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUser", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
                        com.Parameters.AddWithValue("@OperatorID", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(30).ToString());
                            o.Add(reader.GetValue(23).ToString());
                            o.Add(reader.GetValue(24).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;


                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;

            }

        }
        // GET UserGroups
        public IDictionary<string, object> GetUserGroups(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUserGroup", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();

                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(0).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;

                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("error", ex.Message);
                return map;
            }

        }

        // GET Parish
        public IDictionary<string, object> GetParish(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetParish", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@SubCounty", obj.SubCounty);
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(0).ToString());
                                o.Add(reader.GetValue(1).ToString());
                             
                                list.Add(o);
                            }
                            con.Close();
                            map.Add("status", "000");
                            map.Add("aaData", list);
                            map.Add("iTotalRecords", list.Count);
                            map.Add("iTotalDisplayRecords", list.Count);
                            map.Add("sEcho", 1);
                        }
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }
        // GET Location
        public IDictionary<string, object> GetLocation(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetLocation", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Parish", obj.parish);
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(0).ToString());
                                o.Add(reader.GetValue(1).ToString());
                            
                                list.Add(o);
                            }
                            con.Close();
                            map.Add("status", "000");
                            map.Add("aaData", list);
                            map.Add("iTotalRecords", list.Count);
                            map.Add("iTotalDisplayRecords", list.Count);
                            map.Add("sEcho", 1);
                        }
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }
        // GET Sub County
        public IDictionary<string, object> GetSubCounty(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetSubCounty", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@District", obj.District);
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(0).ToString());
                                o.Add(reader.GetValue(1).ToString());
                           
                                list.Add(o);
                            }
                            con.Close();
                            map.Add("status", "000");
                            map.Add("aaData", list);
                            map.Add("iTotalRecords", list.Count);
                            map.Add("iTotalDisplayRecords", list.Count);
                            map.Add("sEcho", 1);
                        }
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }




        public IDictionary<string, object> p_GetDealerSupplyRegions(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetDealerSupplyRegions", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(0).ToString());
                                o.Add(reader.GetValue(1).ToString());
                                o.Add(reader.GetValue(2).ToString());
                                o.Add(reader.GetValue(3).ToString());

                                list.Add(o);
                            }
                            con.Close();
                            map.Add("status", "000");
                            map.Add("aaData", list);
                            map.Add("iTotalRecords", list.Count);
                            map.Add("iTotalDisplayRecords", list.Count);
                            map.Add("sEcho", 1);
                        }
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }



        // GET GetRole
        public IDictionary<string, object> GetRole(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetRole", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@RoleID", obj.RoleID);
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());

                            o.Add(reader.GetValue(0).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("error", ex.Message);
                return map;
            }

        }
        public IDictionary<string, object> GetRoleID(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetRole", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@RoleID", obj.RoleID);
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("error", ex.Message);
                return map;
            }

        }
        public async System.Threading.Tasks.Task<ResponseData> SendPIN(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.MobileNumber = obj.MobileNumber;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    string url = ConfigurationManager.AppSettings["maaifTest"].ToString();
                    HttpResponseMessage res = await client.PostAsJsonAsync(url + "api/SendPIN", obj);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);
                    if (res.IsSuccessStatusCode)
                    {

                        response.responseCode = "000";
                        response.responseMessage = data.responseMessage;
                        return response;
                    }
                }

                catch (HttpRequestException e)
                {
                    var error = e.InnerException.Message;
                    response.responseCode = "091";
                    response.responseMessage = error;
                    return response;
                }
            }
            response.responseCode = "091";
            response.responseMessage = "Server Error";
            return response;
        }
        public async System.Threading.Tasks.Task<ResponseData> sendSMS(string phoneNumber, string messageType)
        {

            User user = new Models.User();
            var data = new maaifPortal.DataTableObject();
            user.MobileNumber = phoneNumber;
            user.SMSType = messageType;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    string url = ConfigurationManager.AppSettings["maaifTest"].ToString();
                    HttpResponseMessage res = await client.PostAsJsonAsync(url + "api/Send", user);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);
                    if (res.IsSuccessStatusCode)
                    {
                        response.responseCode = "000";
                        response.responseMessage = data.responseMessage;
                        return response;
                    }
                }

                catch (HttpRequestException e)
                {
                    var error = e.InnerException.Message;
                    response.responseCode = "091";
                    response.responseMessage = error;
                    return response;
                }
            }
            response.responseCode = "091";
            response.responseMessage = "Server Error";
            return response;
        }

        // GET GetUserRole
        public IDictionary<string, object> GetUserRole(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUserRole", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@RoleID", obj.RoleID);

                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", ex.Message);
                return map;
            }

        }

        // GET GetRoleModule
        public IDictionary<string, object> GetRoleModule(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetRoleModule", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@RoleID", obj.RoleID);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);

                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(7).ToString());
                            o.Add(reader.GetValue(8).ToString());
                            o.Add(reader.GetValue(9).ToString());
                            o.Add(reader.GetValue(0).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("roles", list);
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", ex.Message);
                return map;
            }

        }
        public IDictionary<string, object> GetDistributionChannels(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetDistributionChains", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(0).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");

                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", ex.Message);
                return map;
            }

        }
        // GET GetBankModule
        public IDictionary<string, object> GetBankModule(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetBankModule", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", ex.Message);
                return map;
            }

        }

        // GET Blocked User
        public IDictionary<string, object> GetBlockedUnblockedUser(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {

                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUserBlockedDetail", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@OperatorID", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(5).ToString());
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(7).ToString());
                            o.Add(reader.GetValue(8).ToString());
                            o.Add(reader.GetValue(9).ToString());
                            o.Add(reader.GetValue(10).ToString());
                            o.Add(reader.GetValue(11).ToString());
                            o.Add(reader.GetValue(0).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", ex.Message);
                return map;
            }

        }
        public IDictionary<string, object> GetStreamWhitelist(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {

                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetInputStreamSubscribers", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(0).ToString());

                            list.Add(o);
                        }
                        con.Close();
                        map.Add("status", "000");
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                    }

                    return map;
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", ex.Message);
                return map;
            }

        }
        public ResponseData GetPerson(string nationalId, string DocumentID, DateTime dob, string firstName, string middleName, string lastName, string username, string password)
        {
            try
            {
                Maaif.WebReference.WebServiceSoapClient client = new Maaif.WebReference.WebServiceSoapClient();
                response.responseMessage = client.NiraIntegrationGetPerson(nationalId, DocumentID, firstName, middleName, dob, lastName, username, password);
                return response;
            }
            catch (Exception ex)
            {
                response.responseMessage = ex.Message;
                return response;
            }
        }
        public ResponseData ChangeNIRAPassword(string NewPassword, string username, string password)
        {
            try
            {
                Maaif.WebReference.WebServiceSoapClient client = new Maaif.WebReference.WebServiceSoapClient();
                response.responseMessage = client.NiraIntegrationChangePassword(NewPassword, username, password);
                return response;
            }
            catch (Exception ex)
            {
                response.responseMessage = ex.Message;
                return response;
            }
        }

        public ResponseData CreateBankAccount(string MobileNumber, string Title)
        {
            ResponseData data = new ResponseData();
            try
            {
                string baseURL = ConfigurationManager.AppSettings["maaifTest"] + "api/RecreateEvoucherAccount";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
                string jsonRequest = @"{
                                    ""Title"":""" + Title + @""",
                                    ""UserID"":""" + MobileNumber + @"""
                                }";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(jsonRequest);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic stuff = JObject.Parse(result);
                    data.responseCode = stuff.responseCode;
                    data.responseMessage = stuff.responseMessage;
                    return data;
                }

            }
            catch (Exception ex)
            {
                data.responseCode = "099";
                data.responseMessage = ex.Message;
                return data;
            }
        }

        // ADD/EDIT User
        public ResponseData AddEditUser(User obj)
        {
            ResponseData data = new ResponseData();
            try
            {

                if (obj.UserGroup.Equals("FARMERS"))
                {
                    obj.RoleID = "FARMER";
                }

                string baseURL = ConfigurationManager.AppSettings["maaifTest"] + "api/AddEditUser";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
                string jsonRequest = @"{
                                    ""UserID"":""" + obj.MobileNumber + @""",
                                    ""UserGroup"":""" + obj.UserGroup + @""",
                                    ""latitude"":""" + obj.latitude + @""",
                                    ""longitude"":""" + obj.longitude + @""",
                                    ""FirstName"":""" + obj.FirstName + @""",
                                    ""MiddleName"":""" + obj.MiddleName + @""",
                                    ""LastName"":""" + obj.LastName + @""",
                                    ""Address"":""" + obj.Address + @""",
                                    ""MobileNumber"":""" + obj.MobileNumber + @""",
                                    ""emailid"":""" + obj.EmailID + @""",
                                    ""Gender"":""" + obj.Gender + @""",
                                    ""IDNumber"":""" + obj.IDNumber + @""",
                                    ""RegionID"":""" + obj.RegionID + @""",
                                    ""FarmerGroup"":""" + obj.FarmerGroup + @""",
                                    ""Location"":""" + obj.Location + @""",
                                    ""SubCounty"":""" + obj.SubCounty + @""",
                                    ""Parish"":""" + obj.parish + @""",
                                    ""DOB"":""" + obj.dob + @""",
                                    ""Title"":""" + obj.cust_title_code + @""",
                                    ""RoleID"":""" + obj.RoleID + @""",
                                    ""RoleName"":""" + obj.RoleName + @""",
                                    ""AgrodealerCo"":""" + obj.AgrodealerCo + @""",
                                    ""AgroProduct"":""" + obj.AgroProduct + @""",
                                    ""AgroSignatories"":""" + obj.AgroSignatories + @""",
                                    ""OtherProducts"":""" + obj.OtherProducts + @""",
                                    ""SubDealerNumber"":""" + obj.SubDealerNumber + @""",
                                    ""SubDealerLocation"":""" + obj.SubDealerLocation + @""",
                                    ""PrimaryDesign"":""" + obj.PrimaryDesign + @""",
                                    ""PrimaryEmail"":""" + obj.PrimaryEmail + @""",
                                    ""PrimaryOfficeTelephone"":""" + obj.MobileNumber + @""",
                                    ""SecondaryName"":""" + obj.SecondaryName + @""",
                                    ""SecondaryDesign"":""" + obj.SecondaryDesign + @""",
                                    ""SecondaryOfficeTelephone"":""" + obj.SecondaryOfficeTelephone + @""",
                                    ""SecondaryMobile"":""" + obj.SecondaryMobile + @""",
                                    ""SecondaryEmail"":""" + obj.SecondaryEmail + @""",
                                    ""Remarks"":""" + obj.Remarks + @""",
                                    ""LanguageID"":""" + obj.LanguageID + @""",
                                    ""CreatedBy"":""" + obj.CreatedBy + @""",
                                    ""CreatedOn"":""" + obj.CreatedOn + @""",
                                    ""EventID"":""" + obj.EventID + @""",
                                    ""ModuleID"":""" + obj.ModuleID + @""",
                                    ""IPAddress"":""127.0.0.1""
                                }";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(jsonRequest);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic stuff = JObject.Parse(result);
                    data.responseCode = stuff.responseCode;
                    data.responseMessage = stuff.responseMessage;
                    return data;
                }

            }
            catch (Exception ex)
            {
                data.responseCode = "099";
                data.responseMessage = ex.Message;
                return data;
            }
        }
        //public ResponseData AddEditUser(User obj)
        //{
        //    try
        //    {
        //        Functions fn = new Functions();
        //        Connection connection = new Connection();
        //        string constring = connection.MaaifDatabaseConnectionString();
        //        string statusDescription = "";
        //        string statusCode = "";

        //        string Pin = "";
        //        DateTime controlDate;


        //        if (obj.EventID.ToString() == "1")
        //        {
        //            Pin = fn.GeneratePIN();
        //            controlDate = DateTime.Now;

        //            obj.Ref1 = fn.Base64Encode(Pin);
        //            obj.Ref2 = fn.EncryptPassword(Pin, controlDate).ToString();
        //            obj.ControlDate = controlDate.ToString();
        //        }
        //        DataTable dt = new DataTable();
        //        getUserCollumns(dt);
        //        dt.Rows.Add(
        //            obj.UserID, obj.UserGroup, obj.FirstName, obj.SubCounty, obj.FarmerGroup, obj.Location, obj.MiddleName, obj.LastName, obj.Address, obj.MobileNumber, obj.EmailID,
        //            obj.Gender, obj.IDNumber, obj.Ref1, obj.Ref2, obj.ControlDate, obj.Remarks, obj.LanguageID, obj.RegionID, obj.CreatedBy, DateTime.Now,
        //            obj.EventID, obj.ModuleID, obj.IPAddress, obj.AgroProduct, obj.parish
        //            );





        //        DataSet data = new DataSet();
        //        data.Tables.Add(dt);

        //        var FirstChild = XDocument.Parse(data.GetXml());
        //        var document = FirstChild.Root.Elements().First();
        //        document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
        //        obj.NewData = document.ToString();

        //        using (SqlConnection con = new SqlConnection(constring))
        //        {
        //            using (SqlCommand com = new SqlCommand("p_AddEditUsers", con))
        //            {
        //                con.Open();
        //                com.CommandType = CommandType.StoredProcedure;
        //                com.Parameters.AddWithValue("@UserID", obj.UserID);
        //                com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
        //                com.Parameters.AddWithValue("@FirstName", obj.FirstName);
        //                com.Parameters.AddWithValue("@SubCounty", obj.SubCounty);
        //                com.Parameters.AddWithValue("@FarmerGroup", obj.FarmerGroup);
        //                com.Parameters.AddWithValue("@Location", obj.Location);
        //                com.Parameters.AddWithValue("@MiddleName", obj.MiddleName);
        //                com.Parameters.AddWithValue("@LastName", obj.LastName);
        //                com.Parameters.AddWithValue("@RegionID", obj.RegionID);
        //                com.Parameters.AddWithValue("@Address", obj.Address);
        //                com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
        //                com.Parameters.AddWithValue("@EmailID", obj.EmailID);
        //                com.Parameters.AddWithValue("@TypeOfID", obj.TypeOfID);
        //                com.Parameters.AddWithValue("@IDNumber", obj.IDNumber);
        //                com.Parameters.AddWithValue("@Gender", obj.Gender);
        //                com.Parameters.AddWithValue("@Ref1", obj.Ref1);
        //                com.Parameters.AddWithValue("@Ref2", obj.Ref2);
        //                com.Parameters.AddWithValue("@ControlDate", obj.ControlDate);
        //                com.Parameters.AddWithValue("@Remarks", obj.Remarks);
        //                com.Parameters.AddWithValue("@LanguageID", obj.LanguageID);
        //                com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
        //                com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
        //                com.Parameters.AddWithValue("@EventID", obj.EventID);
        //                com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
        //                com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
        //                com.Parameters.AddWithValue("@DetailOld", obj.OldData);
        //                com.Parameters.AddWithValue("@Parish", obj.parish);
        //                com.Parameters.AddWithValue("@DOB", obj.dob);
        //                com.Parameters.AddWithValue("@DetailNew", obj.NewData);
        //                //com.Parameters.AddWithValue("@AgrodealerCo", obj.AgrodealerCo);
        //                com.Parameters.AddWithValue("@AgroProduct", obj.AgroProduct);
        //                using (SqlDataReader dr = com.ExecuteReader())
        //                {
        //                    response.responseCode = "000";
        //                    if (dr.Read())
        //                    {


        //                        if (obj.EventID.Equals("3"))
        //                        {
        //                            statusDescription = dr[0].ToString();
        //                            statusCode = dr[1].ToString();
        //                            response.responseMessage = statusDescription;
        //                            response.responseCode = statusCode;
        //                        }

        //                        else
        //                        {
        //                            statusDescription = dr[0].ToString();
        //                            statusCode = dr[1].ToString();
        //                            response.responseMessage = statusDescription;
        //                            response.responseCode = statusCode;
        //                            obj.UserID = obj.MobileNumber;
        //                            AddEditUserRole(obj);
        //                            //sendSMS(obj.MobileNumber, obj.SMSType);
        //                            if (statusDescription.Contains("New")) SendPIN(obj);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
        //        response.responseCode = "091";
        //        response.responseMessage = ex.Message;
        //        return response;
        //    }
        //}




        // ADD/EDIT User
        public ResponseData AddEditSpecialStreamSubscribers(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditSpecialStreamSubscribers", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@NIN", obj.IDNumber);
                        com.Parameters.AddWithValue("@Stream ", obj.Stream);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", "1");
                        com.Parameters.AddWithValue("@ModuleID", "2010");
                        com.Parameters.AddWithValue("@IPAddress", "127.0.0.1");
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }


        public ResponseData AddEditPrequalifiedFarmers(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";




                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_UploadInputStreamSubscribers", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Stream", obj.Stream);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailXML", obj.NewData);



                        //com.Parameters.AddWithValue("@AgrodealerCo", obj.AgrodealerCo);
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }



        public ResponseData AddEditRollout(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";




                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditRollout", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Code", obj.Code);
                        com.Parameters.AddWithValue("@Name", obj.Name);
                        com.Parameters.AddWithValue("@Status", obj.Status);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);
                        com.Parameters.AddWithValue("@ModifiedOn", obj.ModifiedOn);
                        com.Parameters.AddWithValue("@SupervisedBy", obj.SupervisedBy);
                        com.Parameters.AddWithValue("@SupervisedOn", obj.SupervisedOn);



                        //com.Parameters.AddWithValue("@AgrodealerCo", obj.AgrodealerCo);
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }



        public ResponseData AddEditClusterDistricts(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";




                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditClusterDistrict", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Cluster", obj.Cluster);
                        com.Parameters.AddWithValue("@District", obj.District);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);
                        com.Parameters.AddWithValue("@ModifiedOn", obj.ModifiedOn);
                        com.Parameters.AddWithValue("@SupervisedBy", obj.SupervisedBy);
                        com.Parameters.AddWithValue("@SupervisedOn", obj.SupervisedOn);



                        //com.Parameters.AddWithValue("@AgrodealerCo", obj.AgrodealerCo);
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }


        public ResponseData AddEditCluster(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";




                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditCluster", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Code", obj.Code);
                        com.Parameters.AddWithValue("@Name", obj.Name);
                        com.Parameters.AddWithValue("@Status", obj.Status);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);
                        com.Parameters.AddWithValue("@ModifiedOn", obj.ModifiedOn);
                        com.Parameters.AddWithValue("@SupervisedBy", obj.SupervisedBy);
                        com.Parameters.AddWithValue("@SupervisedOn", obj.SupervisedOn);



                        //com.Parameters.AddWithValue("@AgrodealerCo", obj.AgrodealerCo);
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }




        public ResponseData AddEditRollOutPlans(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";




                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditRollOutPlans", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Rollout", obj.Rollout);
                        com.Parameters.AddWithValue("@Cluster", obj.Cluster);
                        com.Parameters.AddWithValue("@Category", obj.Category);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@ModifiedBy", obj.ModifiedBy);
                        com.Parameters.AddWithValue("@ModifiedOn", obj.ModifiedOn);
                        com.Parameters.AddWithValue("@SupervisedBy", obj.SupervisedBy);
                        com.Parameters.AddWithValue("@SupervisedOn", obj.SupervisedOn);



                        //com.Parameters.AddWithValue("@AgrodealerCo", obj.AgrodealerCo);
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }





        public ResponseData AddEditDistributionChain(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";

                string Pin = "";
                DateTime controlDate;


                if (obj.EventID.ToString() == "1")
                {
                    Pin = fn.GeneratePIN();
                    controlDate = DateTime.Now;

                    obj.Ref1 = fn.Base64Encode(Pin);
                    obj.Ref2 = fn.EncryptPassword(Pin, controlDate).ToString();
                    obj.ControlDate = controlDate.ToString();
                }
                DataTable dt = new DataTable();
                getChainCollumns(dt);
                dt.Rows.Add(
                    obj.Address, obj.FirstName, obj.MobileNumber, obj.CreatedBy

                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditDistributionChain", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@Name", obj.FirstName);
                        com.Parameters.AddWithValue("@Address", obj.Address);
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@CreatedBy", obj.UserID);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                                obj.UserID = obj.MobileNumber;
                                AddEditUser(obj);
                                SendPIN(obj);
                            }
                        }



                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }





        public ResponseData p_AddDealerSupplyRegions(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                string statusCode = "";
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddDealerSupplyRegions", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        com.Parameters.AddWithValue("@RegionID", obj.RegionID);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@CreatedBy", obj.DealerID);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", "4014");
                        com.Parameters.AddWithValue("@IPAddress", "127.0.0.1");
                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                statusCode = dr[1].ToString();
                                response.responseMessage = statusDescription;
                                response.responseCode = statusCode;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }







        // ADD/EDIT User
        public ResponseData AddEditAccount(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                string Pin = "";
                DateTime controlDate;


                if (obj.EventID.ToString() == "1")
                {
                    Pin = fn.GeneratePIN();
                    controlDate = DateTime.Now;

                    obj.Ref1 = fn.Base64Encode(Pin);
                    obj.Ref2 = fn.EncryptPassword(Pin, controlDate).ToString();
                    obj.ControlDate = controlDate.ToString();
                }
                DataTable dt = new DataTable();
                getUserCollumns(dt);
                dt.Rows.Add(
                       obj.MobileNumber, obj.AccountNumber, obj.CreatedBy, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditAccount", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@AccountNumber", obj.AccountNumber);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);


                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR";
                return response;
            }
        }


        // ADD/EDIT UserGroups
        public ResponseData AddEditUserGroups(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getUserGroupCollumns(dt);
                dt.Rows.Add(
                        obj.UserGroup, obj.GroupName, obj.CreatedBy, obj.CreatedOn, obj.EventID, obj.ModuleID, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditUserGroups", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@GroupName", obj.GroupName);

                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailOld", obj.OldData);
                        com.Parameters.AddWithValue("@DetailNew", obj.NewData);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR";
                return response;
            }
        }



        public ResponseData AddEdiPhoneUserID(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getUserGroupCollumns(dt);
                dt.Rows.Add(
                        obj.UserGroup, obj.CreatedBy, obj.CreatedOn, obj.EventID, obj.ModuleID, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);
                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_UpdateMobileNumber ", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@NewMobileNumber", obj.NewMobileNumber);
                        com.Parameters.AddWithValue("@OldMobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@OperatorID", obj.CreatedBy);
                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr.GetString(1);
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }


        // ADD/EDIT Roles
        public ResponseData AddEditRole(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getRoleCollumns(dt);
                dt.Rows.Add(
                    obj.RoleID, obj.RoleName, obj.UserGroup, obj.CreatedBy, obj.CreatedOn, obj.EventID, obj.ModuleID, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditRoles", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@RoleID", obj.RoleID);
                        com.Parameters.AddWithValue("@RoleName", obj.RoleName);
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);

                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailOld", obj.OldData);
                        com.Parameters.AddWithValue("@DetailNew", obj.NewData);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR";
                return response;
            }
        }

        // ADD/EDIT User Roles
        public ResponseData AddEditUserRole(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getUserRoleCollumns(dt);
                dt.Rows.Add(
                    obj.UserID, obj.RoleID, obj.UserGroup, obj.CreatedBy, obj.CreatedOn, obj.EventID, obj.ModuleID, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditUserRole", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@RoleID", obj.RoleID);
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);

                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailOld", obj.OldData);
                        com.Parameters.AddWithValue("@DetailNew", obj.NewData);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }

        // ADD/EDIT Role Rights
        public ResponseData AddEditRoleRights(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getRoleRightCollumns(dt);
                dt.Rows.Add(
                    obj.UserGroup, obj.RoleID, obj.RoleModuleID, obj.AllowView, obj.AllowAdd, obj.AllowEdit, obj.AllowDelete, obj.IsSupervisionRequired,
                    obj.AllowSupervision, obj.CreatedBy, obj.CreatedOn, obj.EventID, obj.ModuleID, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditRoleModule", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@GroupID", obj.UserGroup);
                        com.Parameters.AddWithValue("@RoleID", obj.RoleID);
                        com.Parameters.AddWithValue("@RoleModuleID", obj.RoleModuleID);
                        com.Parameters.AddWithValue("@AllowView", obj.AllowView);
                        com.Parameters.AddWithValue("@AllowAdd", obj.AllowAdd);
                        com.Parameters.AddWithValue("@AllowEdit", obj.AllowEdit);
                        com.Parameters.AddWithValue("@AllowDelete", obj.AllowDelete);
                        com.Parameters.AddWithValue("@IsSupervisionRequired", obj.IsSupervisionRequired);
                        com.Parameters.AddWithValue("@AllowSupervision", obj.AllowSupervision);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailOld", obj.OldData);
                        com.Parameters.AddWithValue("@DetailNew", obj.NewData);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }

        // BlockUnblock User
        public ResponseData BlockUnblockUser(User obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getBlockedUnblockedUserCollumns(dt);
                dt.Rows.Add(
                    obj.UserID, obj.refNo, obj.blockedReason, obj.blockedDescrition, obj.CreatedBy, obj.CreatedOn, obj.EventID,
                    obj.ModuleID, obj.IPAddress, obj.OldData, obj.NewData, obj.SupervisedBy, obj.SupervisedOn
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditUserBlockedDetail", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@RefNo", obj.refNo);
                        com.Parameters.AddWithValue("@BlockedReasonID", obj.blockedReason);
                        com.Parameters.AddWithValue("@BlockedDescription", obj.blockedDescrition);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", obj.CreatedOn);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailOld", obj.OldData);
                        com.Parameters.AddWithValue("@DetailNew", obj.NewData);
                        com.Parameters.AddWithValue("@SupervisedBy", obj.SupervisedBy);
                        com.Parameters.AddWithValue("@SupervisedOn", obj.SupervisedOn);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {

                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                statusDescription = dr[0].ToString();
                                response.responseMessage = statusDescription;
                            }
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : AddEditAgent : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR";
                return response;
            }
        }

        #endregion UserManagement

        #region StaticFields
        protected void getUserCollumns(DataTable dt)
        {
            dt.Columns.Add("UserID");
            dt.Columns.Add("UserGroup");
            dt.Columns.Add("FirstName");

            dt.Columns.Add("SubCounty");
            dt.Columns.Add("FarmerGroup");
            dt.Columns.Add("Location");

            dt.Columns.Add("MiddleName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("Address");
            dt.Columns.Add("MobileNumber");
            dt.Columns.Add("EmailID");
            dt.Columns.Add("Gender");
            dt.Columns.Add("IDNumber");

            dt.Columns.Add("Ref1");
            dt.Columns.Add("Ref2");
            dt.Columns.Add("ControlDate");

            dt.Columns.Add("Remarks");
            dt.Columns.Add("LanguageID");
            dt.Columns.Add("RegionID");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
            //dt.Columns.Add("AgrodealerCo");
            dt.Columns.Add("AgroProduct");
            dt.Columns.Add("parish");
        }


        protected void getChainCollumns(DataTable dt)
        {
            dt.Columns.Add("FirstName");
            dt.Columns.Add("Address");
            dt.Columns.Add("MobileNumber");
            dt.Columns.Add("CreatedBy");
        }

        protected void getRoleRightCollumns(DataTable dt)
        {
            dt.Columns.Add("GroupID");
            dt.Columns.Add("RoleID");
            dt.Columns.Add("RoleModuleID");
            dt.Columns.Add("AllowView");
            dt.Columns.Add("AllowAdd");
            dt.Columns.Add("AllowEdit");
            dt.Columns.Add("AllowDelete");
            dt.Columns.Add("IsSupervisionRequired");
            dt.Columns.Add("AllowSupervision");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getUserRoleCollumns(DataTable dt)
        {
            dt.Columns.Add("UserID");
            dt.Columns.Add("RoleID");
            dt.Columns.Add("GroupID");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getRoleCollumns(DataTable dt)
        {

            dt.Columns.Add("RoleID");
            dt.Columns.Add("RoleName");
            dt.Columns.Add("GroupID");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getUserGroupCollumns(DataTable dt)
        {
            dt.Columns.Add("GroupID");
            dt.Columns.Add("GroupName");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getPINResetRequestCollumns(DataTable dt)
        {
            dt.Columns.Add("UserID");
            dt.Columns.Add("MobileNumber");

            dt.Columns.Add("CustomerName");
            dt.Columns.Add("GroupID");
            dt.Columns.Add("ControlDate");
            dt.Columns.Add("Ref1");
            dt.Columns.Add("Ref2");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getBlockedUnblockedUserCollumns(DataTable dt)
        {
            dt.Columns.Add("UserID");
            dt.Columns.Add("RefNo");
            dt.Columns.Add("BlockedReasonID");
            dt.Columns.Add("BlockedDescription");
            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
            dt.Columns.Add("OldData");
            dt.Columns.Add("NewData");
            dt.Columns.Add("SupervisedBy");
            dt.Columns.Add("SupervisedOn");




        }


        #endregion StaticFields

    }
}