using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MAAIF.Models;
using System.Xml.Linq;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
namespace MAAIF.Repository.PaymentRepository
{
    public class PaymentRepository
    {
        ResponseData response = new ResponseData();

        #region ManagePayments

        // GET Payment Methods
        public IDictionary<string, object> GetPaymentMethods()
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetPaymentMethods", con))
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
                                o.Add(reader.GetValue(1).ToString());
                                o.Add(reader.GetValue(2).ToString());
                                o.Add(reader.GetValue(3).ToString());

                                o.Add(reader.GetValue(7).ToString());
                                o.Add(reader.GetValue(8).ToString());
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
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }

        }




        public async System.Threading.Tasks.Task<ResponseData> MakePaymentWeb(Payment obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.IPAddress = "0.0.0.0";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    HttpResponseMessage res = await client.PostAsJsonAsync(ConfigurationManager.AppSettings["maaifTest"] + "api/MakePaymentWeb", obj);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);

                    if (res.IsSuccessStatusCode)
                    {
                        if (data.responseCode.Equals("000"))
                        {
                            response.responseCode = "000";
                            response.responseMessage = data.responseMessage;
                            return response;
                        }
                        else
                        {
                            response.responseMessage = data.responseMessage;
                            response.responseCode = "091";
                            return response;
                        }
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




        public IDictionary<string, object> GetOrders(Payment obj)
        {

            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetOrders", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@OrderRef", obj.OrderRef);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(1).ToString());
                                o.Add(reader.GetValue(2).ToString());
                                o.Add(reader.GetValue(3).ToString());
                                o.Add(reader.GetValue(6).ToString());
                                o.Add(reader.GetValue(16).ToString());
                                o.Add(reader.GetValue(17).ToString());
                                o.Add(reader.GetValue(15).ToString());
                                o.Add(reader.GetValue(4).ToString());
                                o.Add(reader.GetValue(6).ToString());
                                o.Add(reader.GetValue(9).ToString());
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




        public IDictionary<string, object> GetTransactions(Payment obj)
        {

            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetPayments", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@OrderRef", obj.OrderRef);



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
                                o.Add(reader.GetValue(6).ToString());
                                o.Add(reader.GetValue(7).ToString());
                                o.Add(reader.GetValue(8).ToString());
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






        public IDictionary<string, object> GetFarmerPayments(Payment obj)
        {

            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetPayments", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@OrderRef", obj.OrderRef);

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
                                o.Add(reader.GetValue(6).ToString());
                                o.Add(reader.GetValue(7).ToString());
                                o.Add(reader.GetValue(12).ToString());

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




        public IDictionary<string, object> GetDealerPayments(Payment obj)
        {

            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetPayments", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;

                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@OrderRef", obj.OrderRef);




                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(5).ToString());
                                o.Add(reader.GetValue(6).ToString());
                                o.Add(reader.GetValue(8).ToString());
                                o.Add(reader.GetValue(9).ToString());
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




        public IDictionary<string, object> GetMaaifContributions(Payment obj)
        {

            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetPayments", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@OrderRef", obj.OrderRef);

                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            List<List<object>> list = new List<List<object>>();
                            SqlDataReader reader;
                            reader = com.ExecuteReader();
                            while (reader.Read())
                            {
                                List<object> o = new List<object>();
                                o.Add(reader.GetValue(7).ToString());
                                o.Add(reader.GetValue(9).ToString());
                                o.Add(reader.GetValue(11).ToString());

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







        // ADD EDIT Payment Methods
        public ResponseData AddEditPaymentMethod(Payment obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getPaymentMethodCollumns(dt);
                dt.Rows.Add(
                    obj.PaymentID, obj.PaymentName, obj.PaymentType, obj.BankShortName, obj.SortCode, obj.ExtraDetails,
                    obj.CreatedBy, DateTime.Now, obj.EventID, obj.ModuleID, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.DetailNew = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditPaymentMethod", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@PaymentID", obj.PaymentID);
                        com.Parameters.AddWithValue("@PaymentName", obj.PaymentName);
                        com.Parameters.AddWithValue("@PaymentType", obj.PaymentType);
                        com.Parameters.AddWithValue("@BankShortName", obj.BankShortName);
                        com.Parameters.AddWithValue("@SortCode", obj.SortCode);
                        com.Parameters.AddWithValue("@ExtraDetails", obj.ExtraDetails);

                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailOld", obj.DetailOld);
                        com.Parameters.AddWithValue("@DetailNew", obj.DetailNew);

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


        public void log(string msg, string msisdn)
        {
            try
            {

                string directory = ConfigurationManager.AppSettings["log_file_path"];
                var MyLogsDir = directory + String.Format("{0:yyyyMMdd}", DateTime.Now);
                Directory.CreateDirectory(MyLogsDir);
                string logfile = string.Format(MyLogsDir + "\\" + msisdn + ".log.{0:yyyy-MM-dd}", DateTime.Now);
                StreamWriter sw = File.AppendText(logfile);
                string logLine = String.Format(
                    "{0:G}: {1}.", DateTime.Now, msg);
                sw.WriteLine(logLine);
                sw.Close();

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }


     



        public async System.Threading.Tasks.Task<ResponseData> InitiateFarmerPayment(Payment obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.OrderID = obj.OrderID;
            obj.MobileNumber = obj.MobileNumber;
            obj.DealerID = obj.DealerID;
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    string url = ConfigurationManager.AppSettings["maaifTest"].ToString();
                    HttpResponseMessage res = await client.PostAsJsonAsync(url + "api/InitiateFarmerPayment", obj);
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





        // MAKE Payment 
        public ResponseData MakePayment(Payment obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";
                DataSet dt = new DataSet();
                // Get UserProfile
                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("sp_GeqtPortalUser", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);

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
                        obj.TXReference = fn.GenerateTXReference();
                        obj.MobileNumber = dt.Tables[0].Rows[0]["MobileNumber"].ToString();

                        using (SqlConnection con = new SqlConnection(constring))
                        {
                            using (SqlCommand com = new SqlCommand("p_AddPayment", con))
                            {
                                con.Open();
                                com.CommandType = CommandType.StoredProcedure;
                                com.Parameters.AddWithValue("@TXReference", obj.TXReference);
                                com.Parameters.AddWithValue("@TXReference1", obj.TXReference1);
                                com.Parameters.AddWithValue("@TXReference2", obj.TXReference2);
                                com.Parameters.AddWithValue("@CustomerID", obj.CustomerID);
                                com.Parameters.AddWithValue("@MerchantCode", obj.MerchantCode);
                                com.Parameters.AddWithValue("@MerchantID", obj.MerchantID);
                                com.Parameters.AddWithValue("@ServiceAccountID", obj.ServiceAccountID);

                                com.Parameters.AddWithValue("@Amount", obj.Amount);
                                com.Parameters.AddWithValue("@TipAmount", obj.TipAmount);
                                com.Parameters.AddWithValue("@DiscountAmount", obj.DiscountAmount);
                                com.Parameters.AddWithValue("@ChargesToCustomer", obj.ChargesToCustomer);
                                com.Parameters.AddWithValue("@ChargesToMerchant", obj.ChargesToMerchant);
                                com.Parameters.AddWithValue("@PaymentDetails", obj.PaymentDetails);
                                com.Parameters.AddWithValue("@BankAccountID", obj.BankAccountID);

                                com.Parameters.AddWithValue("@ToBankAccountID", obj.ToBankAccountID);
                                com.Parameters.AddWithValue("@Status", obj.Status);
                                com.Parameters.AddWithValue("@ExtraDetails", obj.ExtraDetails);
                                com.Parameters.AddWithValue("@UniqueID", obj.UniqueID);
                                com.Parameters.AddWithValue("@TrxType", obj.TrxType);

                                using (SqlDataReader dr = com.ExecuteReader())
                                {

                                    response.responseCode = "000";
                                    if (dr.Read())
                                    {
                                        statusDescription = dr[0].ToString();
                                        if (statusDescription.Contains("091"))
                                        {
                                            response.responseCode = "091";
                                            response.responseMessage = "FAILED. Unable to Submit the Transaction Request";
                                        }
                                        else
                                        {
                                            response.responseMessage = statusDescription;
                                        }
                                    }
                                }
                            }
                        }

                    }

                }





                return response;
            }
            catch (Exception ex)
            {
                response.responseCode = "091";
                response.responseMessage = "ERROR";
                return response;
            }
        }


        #endregion ManagePayments 

        #region staticFields
        protected void getPaymentMethodCollumns(DataTable dt)
        {
            dt.Columns.Add("PaymentID");
            dt.Columns.Add("PaymentName");
            dt.Columns.Add("PaymentType");
            dt.Columns.Add("BankShortName");
            dt.Columns.Add("SortCode");
            dt.Columns.Add("ExtraDetails");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        #endregion staticFields
    }
}