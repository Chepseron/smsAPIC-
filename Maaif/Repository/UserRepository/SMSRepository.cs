using System;
using MAAIF.Models;
using System.Web.Http;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace MAAIF.Repository.SMSRepository
{
    public class SMSRepository : ApiController
    {
        ResponseData response = new ResponseData();
        // SINGLE SMS SINGLE NUMBER
        public ResponseData SingleSMSSingleNumber(Sms obj)
        {
            ResponseData data = new ResponseData();
            try
            {
                string baseURL = ConfigurationManager.AppSettings["smsNode"] + "/SingleSMSSingleNumber";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
                string jsonRequest = @"{
                                    ""batchId"":""" + obj.batchId + @""",
                                    ""type"":""" + obj.type + @""",
                                    ""senderId"":""" + obj.senderId + @""",
                                    ""messages"":""[{"",
                                    ""number"":""" + obj.number + @""",
                                    ""message"":""" + obj.message + @""" +""}]"",
                                    ""callback"":""" + obj.callback + @"""
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



        // SINGLE SMS MULTIPLE NUMBERS
        public ResponseData SingelSMSMultipleNumbers(Sms obj)
        {
            ResponseData data = new ResponseData();
            try
            {
                string baseURL = ConfigurationManager.AppSettings["smsNode"] + "/SingelSMSMultipleNumbers";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
                string jsonRequest = @"{
                                    ""batchId"":""" + obj.batchId + @""",
                                    ""type"":""" + obj.type + @""",
                                    ""senderId"":""" + obj.senderId + @""",
                                    ""numbers"":""[""+""" + obj.numbers + @"""+""]"",
                                    ""message"":""" + obj.message + @""",
                                    ""callback"":""" + obj.callback + @"""
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



        // GET BATCH STATUS
        public ResponseData GetBatchStatus(Sms obj)
        {
            ResponseData data = new ResponseData();
            try
            {
                string baseURL = ConfigurationManager.AppSettings["smsNode"] + "/getBatchStatus";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(baseURL);
                string jsonRequest = @"{
                                    ""batchId"":""" + obj.batchId + @"""
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

    }
}