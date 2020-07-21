using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MAAIF.Models;
using System.Web.Http;

namespace MAAIF.Repository.DataSupervisionRepository
{
    public class DataSupervisionRepository : ApiController
    {
        ResponseData response = new ResponseData();

        // GET UnsupervisedModules
        public ResponseData GetListOfUnsupervisedModules(DataSupervision obj)
        {
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetListOfUnsupervisedModules", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);

                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            using (DataSet dt = new DataSet())
                            {
                                da.Fill(dt);

                                response.payload = dt;
                                response.responseCode = "000";
                                response.responseMessage = "SUCCESS";
                                return response;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                return response;
            }

        }

        // GET Unsupervised Data
        public IDictionary<string, object> GetListOfUnsupervisedData(DataSupervision obj)
        {
            IDictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetListOfUnsupervisedData", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@UserGroup", obj.UserGroup);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        SqlDataReader reader = com.ExecuteReader();
                        List<List<object>> list = new List<List<object>>();
                        while (reader.Read())
                        {
                            List<object> o = new List<object>();
                            o.Add(reader.GetValue(1).ToString());
                            o.Add(reader.GetValue(2).ToString());
                            o.Add(reader.GetValue(4).ToString());
                            o.Add(reader.GetValue(3).ToString());
                            o.Add(reader.GetValue(0).ToString());
                            list.Add(o);
                        }
                        con.Close();
                        map.Add("aaData", list);
                        map.Add("iTotalRecords", list.Count);
                        map.Add("iTotalDisplayRecords", list.Count);
                        map.Add("sEcho", 1);
                        return map;
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                //  return BadRequest(ex.Message);
                map.Add("error", ex.Message);
                return map;

            }

        }

        // GET SupervisionDataDetail
        public DataSet GetSupervisionDataDetail(DataSupervision obj)
        {
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_GetSupervisionDataDetail", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@SearchKey", obj.SearchKey);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);

                        using (SqlDataAdapter da = new SqlDataAdapter(com))
                        {
                            using (DataSet dt = new DataSet())
                            {
                                da.Fill(dt);
                                IHttpActionResult result = Ok(dt);
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errLogger.LogError(Path.GetFileName("Agent.cs") + " : GetAgentBlockedDetail : " + ex.Message);
                response.responseCode = "091";
                response.responseMessage = "ERROR|" + ex.Message;
                return null;
            }

        }

        // Supervise Data
        public ResponseData SuperviseData(DataSupervision obj)
        {
            try
            {
                Connection connection = new Connection();
                string connStr = connection.MaaifDatabaseConnectionString();

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand com = new SqlCommand("p_SuperviseData", con))
                    {
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@UserID", obj.UserID);
                        com.Parameters.AddWithValue("@SuperviseAction", obj.SuperviseAction);
                        com.Parameters.AddWithValue("@SearchKey", obj.SearchKey);
                        com.Parameters.AddWithValue("@ModuleID", obj.ModuleID);
                        com.Parameters.AddWithValue("@Remarks", obj.Remarks);

                        using (SqlDataReader dr = com.ExecuteReader())
                        {
                            response.responseCode = "000";
                            if (dr.Read())
                            {
                                response.responseMessage = dr[0].ToString();
                            }
                        }
                    }
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

    }
}