using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using MAAIF.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MAAIF.Repository.ProductRepository
{
    public class ProductRepository
    {
        ResponseData response = new ResponseData();

        #region Products Management

        // GET Product Categories
        public IDictionary<string, object> GetProductCategories(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetProductCategories", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
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
                                o.Add(reader.GetValue(4).ToString());
                                o.Add(reader.GetValue(5).ToString());
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

        public string UNBS(string tag)
        {
            try
            {
                Maaif.WebReference.WebServiceSoapClient client = new Maaif.WebReference.WebServiceSoapClient();
                string response = "";
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IDictionary<string, object> GetTotalProductPrice(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetTotalProductPrice", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        //com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@Quantity", obj.Quantity);
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
                                map.Add("totalPrice", reader.GetValue(1).ToString());
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


        public IDictionary<string, object> GetDealerProductsByRegion(User obj)
        {



            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetProductsByRegion", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Category", obj.Category);
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
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
                                o.Add(reader.GetValue(4).ToString());
                                o.Add(reader.GetValue(5).ToString());
                                o.Add(reader.GetValue(6).ToString());
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


        public IDictionary<string, object> GetStreams(User obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetPaymentStreams", con))
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

        public IDictionary<string, object> GetProducts(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetProducts", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@Category", obj.Category);

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
                                o.Add(reader.GetValue(4).ToString());
                                o.Add(reader.GetValue(5).ToString());
                                o.Add(reader.GetValue(6).ToString());
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


        public async System.Threading.Tasks.Task<ResponseData> NotifyFarmer(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.IPAddress = "0.0.0.0";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    HttpResponseMessage res = await client.PostAsJsonAsync(ConfigurationManager.AppSettings["maaifTest"] + "api/NotifyFarmer", obj);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);

                    if (res.IsSuccessStatusCode)
                    {
                        obj.FarmerPayment = data.payload.Table[0].FarmerPayment;
                        obj.Subsidy = data.payload.Table[0].Subsidy;


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

        public IDictionary<string, object> GetDealerProducts(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetDealerProducts", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();

                            o.Add(reader.GetValue(1).ToString());//product ID
                            o.Add(reader.GetValue(2).ToString());//name
                            o.Add(reader.GetValue(4).ToString());//description
                            o.Add(reader.GetValue(8).ToString());//created On
                            o.Add(reader.GetValue(6).ToString());//unit price
                            o.Add(reader.GetValue(5).ToString());//unit of measure
                            o.Add(reader.GetValue(7).ToString());//created by
                            o.Add(reader.GetValue(3).ToString());//picture
                            o.Add(reader.GetValue(1).ToString());//product ID



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

        // GET Dealer Product Price Bands
        public IDictionary<string, object> GetProductPriceBands(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetProductPriceBands", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);

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
                            o.Add(reader.GetValue(8).ToString());
                            o.Add(reader.GetValue(9).ToString());
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
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                map.Add("status", "091");
                return map;
            }


        }




        public IDictionary<string, object> GetStock(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetStock", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());//stock id
                            o.Add(reader.GetValue(1).ToString());//product ID
                            o.Add(reader.GetValue(2).ToString());//serial number
                            o.Add(reader.GetValue(3).ToString());//quantity
                            o.Add(reader.GetValue(12).ToString());//unit of measure
                            o.Add(reader.GetValue(4).ToString());//created by
                            o.Add(reader.GetValue(5).ToString());//created on
                            o.Add(reader.GetValue(11).ToString());//product name
                            o.Add(reader.GetValue(1).ToString());//product ID






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




        public IDictionary<string, object> fetchProductPrices(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetProductDealersAndPrices", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());//product id
                            o.Add(reader.GetValue(1).ToString());//dealer ID
                            o.Add(reader.GetValue(2).ToString());//dealer name
                            o.Add(reader.GetValue(3).ToString());//unit price
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



        public IDictionary<string, object> ViewCart(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUserCart", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);

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


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();

                            o.Add(reader.GetValue(0).ToString());
                            string request = "<div class=\"act-time\">\n" +
"                                                    <div class=\"activity-body act-in\">\n" +
"                                                        <span class=\"arrow\"></span>\n" +
"                                                        <div class=\"text\">\n" +

"<table class=\"table-active table datatable-responsive datatable-basic table-striped table-hover\" id=\"cartTable\" style=\"border-collapse:collapse\">\n" +
"                            <thead>\n" +
"                                <tr>\n" +
"                                    <th><a href =\"#\" class=\"activity-img\"><img src=\"/" + ConfigurationManager.AppSettings["folder"] + "/img/" + reader.GetValue(11).ToString() + "\" alt=\"\" height=\"200\"  width=\"200\"></a>\n</th>" +
"                                    <th>" +
"                                                            <p class=\"attribution\"><a href=\"#\" style = \"color:black; font-weight:bold\">" + reader.GetValue(1).ToString() + " " + reader.GetValue(14).ToString() + " </a></p>\n" +
"                                                            <p> " + reader.GetValue(13).ToString() + "</p>\n" +
"</th>" +
"                                                        </div>\n" +

                            "                            <a href=\"javascript:void(0)\"  class=\"removeCart\" style=\"width:200px\" >\n" +
"                               <p style=\"color:#79c879; font-weight:bold\"> <i class=\"fa fa-recycle removeCart\"></i> Remove</p> \n" +
"                            </a>\n" +
"      </tr>\n" +
                "                            </thead>\n" +
               "                        </table>" +
"                                                    </div>\n" +
"                                                </div>";
                            o.Add(request);
                            string quantity = "<select id=\"quantity\" name=\"quantity\">\n" +
                                             "</select>";
                            o.Add(reader.GetValue(4).ToString() + "    " + quantity + "   " + reader.GetValue(12).ToString());
                            o.Add("<div class=\"text\"><p class=\"attribution\"  style = \"color:#79c879; font-weight:bold\">UGX " + reader.GetValue(5).ToString() + "</p>");
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




        public IDictionary<string, object> GetUnpaidOrders(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUnpaidOrders", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@UniqueID", obj.UniqueID);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        com.Parameters.AddWithValue("@OrderID", obj.OrderID);


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            StringBuilder br = new StringBuilder();

                            br.Append("<div class=\"nav-collapse\">");
                            br.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"fa fa-align-justify\"></span><span class=\"fa fa-caret-down\"></span></a>");
                            br.Append("<ul class=\"dropdown-menu dropdown-menu-right\" role=\"menu\">");
                            br.Append("<li><a href=\"javascript:void(0)\" class=\"payorder\" data-index=\"' + i + '\"><i class=\"fa fa-google-wallet\" aria-hidden=\"true\"></i>Pay for item</a></li></ul></div>");


                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(0).ToString());
                            o.Add(reader.GetValue(22).ToString());
                            o.Add(reader.GetValue(24).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(23).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(25).ToString());
                            o.Add(reader.GetValue(9).ToString());
                            o.Add(br.ToString());

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



        public IDictionary<string, object> GetUnpaidOrdersFarmer(Product obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUnpaidOrders", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@UniqueID", obj.UniqueID);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
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


                        List<List<object>> list = new List<List<object>>();
                        SqlDataReader reader;
                        reader = com.ExecuteReader();

                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(4).ToString());

                            o.Add(reader.GetValue(6).ToString());
                            o.Add(reader.GetValue(9).ToString());
                            o.Add(reader.GetValue(14).ToString());
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



        public async System.Threading.Tasks.Task<ResponseData> CompleteOrder(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.IPAddress = "0.0.0.0";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    HttpResponseMessage res = await client.PostAsJsonAsync(ConfigurationManager.AppSettings["maaifTest"] + "api/VeryfyOrder", obj);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);

                    if (res.IsSuccessStatusCode)
                    {
                        obj.FarmerPayment = data.payload.Table[0].FarmerPayment;
                        obj.Subsidy = data.payload.Table[0].Subsidy;

                        res = await client.PostAsJsonAsync(ConfigurationManager.AppSettings["maaifTest"] + "api/CompleteOrder", obj);
                        jsonStr = res.Content.ReadAsStringAsync().Result;
                        var data2 = new maaifPortal.DataTableObject();
                        data2 = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);

                        if (data2.responseCode.Equals("000"))
                        {
                            response.responseCode = "000";
                            response.responseMessage = data2.responseMessage + "\n Your summary; \n\n\nFarmer Payment : " + data.payload.Table[0].FarmerPayment + "\nSubsidy : " + data.payload.Table[0].Subsidy + "\nTotal Cost : " + data.payload.Table[0].TotalCost;
                            return response;
                        }
                        else
                        {
                            response.responseMessage = data2.responseMessage;
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





        public async System.Threading.Tasks.Task<ResponseData> VerifyOrder(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.IPAddress = "0.0.0.0";
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    HttpResponseMessage res = await client.PostAsJsonAsync(ConfigurationManager.AppSettings["maaifTest"] + "api/VeryfyOrder", obj);
                    var jsonStr = res.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<maaifPortal.DataTableObject>(jsonStr);

                    if (res.IsSuccessStatusCode)
                    {
                        response.responseCode = "000";
                        response.FarmerPayment = data.payload.Table[0].FarmerPayment;
                        response.Subsidy = data.payload.Table[0].Subsidy;
                        response.DealerAmount = data.payload.Table[0].TotalCost;
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





        public ResponseData GetTotalPrice(Product obj)
        {
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetUnpaidOrdersTotalPrice", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@UniqueID", obj.UniqueID);

                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            using (DataSet dt = new DataSet())
                            {
                                da.Fill(dt);

                                response.payload = dt;
                                response.responseCode = "000";
                                response.responseMessage = dt.Tables[1].Rows[0]["TotalPrice"].ToString();
                                return response;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }

        }


        public async System.Threading.Tasks.Task<ResponseData> MakePayment(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.IPAddress = "0.0.0.0";
            obj.TrxType = "D";
            obj.ExtraDetails = "";
            obj.CustomerID = obj.MobileNumber;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);
                    string url = ConfigurationManager.AppSettings["maaifTest"].ToString();
                    HttpResponseMessage res = await client.PostAsJsonAsync(url + "api/MakePayment", obj);
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




        public async System.Threading.Tasks.Task<ResponseData> topup(User obj)
        {
            var data = new maaifPortal.DataTableObject();
            obj.IPAddress = "0.0.0.0";
            obj.TrxType = "C";
            obj.ExtraDetails = "";
            obj.CustomerID = obj.MobileNumber;
            obj.ServiceAccountID = obj.MobileNumber;
            
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["maaifTest"]);

                    string url = ConfigurationManager.AppSettings["payment"].ToString();
                    HttpResponseMessage res = await client.PostAsJsonAsync(url + "api/MakePayment", obj);
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


        public ResponseData AddEditShoppingCart(Product obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";


                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditShoppingCart", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@OrderID", obj.OrderID);
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        com.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        com.Parameters.AddWithValue("@TotalPrice", obj.TotalPrice);
                        com.Parameters.AddWithValue("@PaymentMode", obj.PaymentMethod);
                        com.Parameters.AddWithValue("@Status", obj.Status);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
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




        public ResponseData AddEditOrder(Product obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";


                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditOrders", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@OrderID", obj.OrderID);
                        //com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@MobileNumber", obj.MobileNumber);
                        //com.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        // com.Parameters.AddWithValue("@TotalPrice", obj.TotalPrice);
                        com.Parameters.AddWithValue("@PaymentMode", obj.PaymentMethod);
                        com.Parameters.AddWithValue("@Status", obj.Status);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
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

        // ADD/EDIT Product Category
        public ResponseData AddEditProductCategory(Product obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getProductCategoryCollumns(dt);
                dt.Rows.Add(
                        obj.Category, obj.Name, obj.Description, obj.Picture,
                        obj.CreatedBy, DateTime.Now, obj.EventID, obj.ModuleID, obj.IPAddress
                    );

                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddProductCategory", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@CategoryID", obj.Category);
                        com.Parameters.AddWithValue("@Name", obj.Name);
                        com.Parameters.AddWithValue("@Description", obj.Description);
                        com.Parameters.AddWithValue("@Picture", obj.Picture);

                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
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

        // ADD/EDIT Product
        public ResponseData AddEditProduct(Product obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getProductCollumns(dt);
                dt.Rows.Add(
                        obj.ProductID, obj.SerialNo, obj.Name, obj.Description, obj.Category, obj.UnitOfMeasure,
                        obj.CreatedBy, DateTime.Now, obj.EventID, obj.ModuleID, obj.IPAddress
                    );


                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditProduct", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@SerialNo", obj.SerialNo);
                        com.Parameters.AddWithValue("@Name", obj.Name);
                        com.Parameters.AddWithValue("@Description", obj.Description);
                        com.Parameters.AddWithValue("@ProductCategory", obj.Category);
                        com.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        com.Parameters.AddWithValue("@UnitPrice", obj.UnitPrice);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@IPAddress", obj.IPAddress);
                        com.Parameters.AddWithValue("@DetailOld", obj.OldData);
                        com.Parameters.AddWithValue("@DetailNew", obj.NewData);
                        com.Parameters.AddWithValue("@SupervisedBy", obj.SupervisedBy);
                        com.Parameters.AddWithValue("@SupervisedOn", obj.@SupervisedOn);
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


        public ResponseData AddEditStock(Product obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getStockCollumns(dt);
                dt.Rows.Add(
                        obj.StockID, obj.ProductID, obj.SerialNo, obj.Quantity, obj.DealerID, obj.UnitOfMeasure,
                        obj.CreatedBy, DateTime.Now, obj.EventID, obj.ModuleID, obj.IPAddress
                    );


                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditStock", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@StockID", obj.StockID);
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@SerialNumber", obj.SerialNo);
                        com.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@EventID", obj.EventID);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
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

        // Validate ETAG
        public ResponseData ValidateEtag(Product obj)
        {
            try
            {
                //Maaif.MAAIF_Integration.ServiceTemplate etag = new Maaif.MAAIF_Integration.ServiceTemplate();
                //string resp = etag.UnbsGetProductPerTag(obj.Etag);
                string resp = "000|{'id': 1975,'tag': {'id': 2062,'tag_code': '71972044611695','tag_details': 'Basic tag','tag_status': 'ASSIGNED','update_time': '2018-08-22T15:33:17.497260+03:00','manufacturer': null,'tag_type': 1, 'roll': 16,'update_by': null }, 'product': { 'id': 3, 'relevant_uganda_standard_number': '123434', 'product_name': 'Fanta', 'product_description': 'Fanta 500ml', 'product_unit': null, 'product_status': true, 'shelf_days': 120, 'update_time': '2018-08-22T15:27:16.842679+03:00', 'manufacturer': 1, 'product_category': 1, 'update_by': 2 } }";
                string[] res = resp.Split('|');
                if (res[0].ToString() == "000")
                {
                    dynamic stuff = JObject.Parse(res[1].ToString());
                    dynamic tag = stuff.tag;
                    dynamic product = stuff.product;

                    string tagDetails = tag.tag_details;
                    string tagStatus = tag.tag_status;

                    string productName = product.product_name;
                    string productDescription = product.product_description;

                    response.responseCode = "000";
                    response.responseMessage = productName + "|" + productDescription;
                }
                else
                {


                    response.responseCode = "091";
                    response.responseMessage = "ETag Validation Failed. Please try Again Later";
                }



                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_LogEtagRequests", con))
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Tag", obj.Etag);
                        com.Parameters.AddWithValue("@Status", res[0].ToString());
                        com.Parameters.AddWithValue("@ExtraDetails", resp);
                        con.Open();
                        com.ExecuteNonQuery();
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


        // ADD/EDIT Dealer Product
        public ResponseData AddEditDealerProduct(Product obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getDealerProductCollumns(dt);
                dt.Rows.Add(
                     obj.Etag, obj.Quantity, obj.ProductID, obj.DealerID, obj.Picture, obj.Description, obj.UnitOfMeasure, obj.UnitPrice,
                        obj.CreatedBy, DateTime.Now, obj.EventID, obj.ModuleID, obj.IPAddress
                    );


                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                if (obj.Etag.ToString() != "")
                {
                    // Maaif.MAAIF_Integration.ServiceTemplate etag = new Maaif.MAAIF_Integration.ServiceTemplate();
                    string resp = "000|{'id': 1975,'tag': {'id': 2062,'tag_code': '71972044611695','tag_details': 'Basic tag','tag_status': 'ASSIGNED','update_time': '2018-08-22T15:33:17.497260+03:00','manufacturer': null,'tag_type': 1, 'roll': 16,'update_by': null }, 'product': { 'id': 3, 'relevant_uganda_standard_number': '123434', 'product_name': 'Fanta', 'product_description': 'Fanta 500ml', 'product_unit': null, 'product_status': true, 'shelf_days': 120, 'update_time': '2018-08-22T15:27:16.842679+03:00', 'manufacturer': 1, 'product_category': 1, 'update_by': 2 } }";

                    string[] res = resp.Split('|');
                    if (res[0].ToString() == "000")
                    {
                        //response.responseCode = "000";
                        //response.responseMessage = productName + "|" + productDescription;
                    }
                    else
                    {
                        response.responseCode = "091";
                        response.responseMessage = "ETag Validation Failed. Please Provide a Valid E-Tag";
                        return response;
                    }
                }
                else
                {
                    response.responseCode = "091";
                    response.responseMessage = "ETag Validation Failed. Please Provide a Valid E-Tag";
                    return response;
                }

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditDealerProduct", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@Etag", obj.Etag);
                        com.Parameters.AddWithValue("@Quantity", obj.Quantity);
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        com.Parameters.AddWithValue("@Picture", obj.Picture);
                        com.Parameters.AddWithValue("@Description", obj.Description);
                        com.Parameters.AddWithValue("@UnitPrice", obj.UnitPrice);
                        com.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);
                        //com.Parameters.AddWithValue("@District", obj.District);
                        //com.Parameters.AddWithValue("@Cluster", obj.Cluster);
                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", DateTime.Now.ToShortDateString());
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


                                if (statusDescription.Contains("FAILED"))
                                {
                                    response.responseCode = "091";
                                    response.responseMessage = statusDescription;
                                }
                                else
                                {
                                    response.responseCode = "000";
                                    response.responseMessage = statusDescription;
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

        // ADD/EDIT Dealer Product
        public ResponseData AddEditProductPriceBands(Product obj)
        {
            try
            {
                Functions fn = new Functions();
                Connection connection = new Connection();
                string constring = connection.MaaifDatabaseConnectionString();
                string statusDescription = "";

                DataTable dt = new DataTable();
                getProductPriceBandsCollumns(dt);
                dt.Rows.Add(
                        obj.RowID, obj.ProductID, obj.DealerID, obj.UnitOfMeasure, obj.UnitPrice, obj.MinUnits, obj.MaxUnits,
                        obj.CreatedBy, DateTime.Now, obj.EventID, obj.ModuleID, obj.IPAddress
                    );


                DataSet data = new DataSet();
                data.Tables.Add(dt);

                var FirstChild = XDocument.Parse(data.GetXml());
                var document = FirstChild.Root.Elements().First();
                document.Descendants().Where(e => e.IsEmpty || String.IsNullOrWhiteSpace(e.Value)).Remove();
                obj.NewData = document.ToString();

                using (SqlConnection con = new SqlConnection(constring))
                {
                    using (SqlCommand com = new SqlCommand("p_AddEditProductPriceBands", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@RowID", obj.RowID);
                        com.Parameters.AddWithValue("@ProductID", obj.ProductID);
                        com.Parameters.AddWithValue("@DealerID", obj.DealerID);
                        com.Parameters.AddWithValue("@MinUnits", obj.MinUnits);
                        com.Parameters.AddWithValue("@MaxUnits", obj.MaxUnits);
                        com.Parameters.AddWithValue("@UnitPrice", obj.UnitPrice);
                        com.Parameters.AddWithValue("@UnitOfMeasure", obj.UnitOfMeasure);

                        com.Parameters.AddWithValue("@CreatedBy", obj.CreatedBy);
                        com.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
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


        #endregion Products Management


        #region Static Fields

        protected void getProductCategoryCollumns(DataTable dt)
        {
            dt.Columns.Add("CategoryID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("Picture");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getProductCollumns(DataTable dt)
        {
            dt.Columns.Add("ProductID");
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("Name");
            dt.Columns.Add("Description");
            dt.Columns.Add("ProductCategory");
            dt.Columns.Add("UnitOfMeasure");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getStockCollumns(DataTable dt)
        {


            dt.Columns.Add("StockID");
            dt.Columns.Add("ProductID");
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("DealerID");
            dt.Columns.Add("UnitOfMeasure");
            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }

        protected void getDealerProductCollumns(DataTable dt)
        {
            dt.Columns.Add("Etag");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("ProductID");
            dt.Columns.Add("DealerID");
            dt.Columns.Add("Picture");
            dt.Columns.Add("Description");
            dt.Columns.Add("UnitOfMeasure");
            dt.Columns.Add("UnitPrice");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }


        protected void getProductPriceBandsCollumns(DataTable dt)
        {
            dt.Columns.Add("RowID");
            dt.Columns.Add("ProductID");
            dt.Columns.Add("DealerID");
            dt.Columns.Add("UnitOfMeasure");
            dt.Columns.Add("UnitPrice");
            dt.Columns.Add("MinUnits");
            dt.Columns.Add("MaxUnits");

            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("CreatedOn");
            dt.Columns.Add("EventID");
            dt.Columns.Add("ModuleID");
            dt.Columns.Add("IPAddress");
        }


        #endregion Static Fields
    }
}