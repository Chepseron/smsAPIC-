using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MAAIF.Repository;
using MAAIF.Models;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace MAAIF.Controllers
{
    public class SmsController : ApiController
    {
        byte[] PasswordBytes = Encoding.UTF8.GetBytes("evms");
        ResponseData response = new ResponseData();
        Repository.UserRepository.UserRepository userRepository = new Repository.UserRepository.UserRepository();

        #region Auhtentication

     

       


        [Route("api/SingleSMSSingleNumber")]
        [HttpPost]
        public ResponseData SingleSMSSingleNumber(User obj)
        {
            ResponseData data = new ResponseData();
            try
            {

                response = userRepository.AddEditUser(obj);

                return response;
            }
            catch (Exception ex)
            {
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }



        [Route("api/SingleSMSSingleNumber")]
        [HttpPost]
        public ResponseData AddEditUser(User obj)
        {
            ResponseData data = new ResponseData();
            try
            {

                response = userRepository.AddEditUser(obj);

                return response;
            }
            catch (Exception ex)
            {
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }






        [Route("api/GetPerson")]
        [HttpPost]
        public ResponseData GetPerson(User obj)
        {
            try
            {
                var response2 = userRepository.GetPerson(obj.IDNumber, obj.DocumentID, obj.dob, obj.FirstName, obj.MiddleName, obj.LastName, ConfigurationManager.AppSettings["niraUserName"].ToString(), ConfigurationManager.AppSettings["niraPwd"].ToString());
                XmlDocument xmlDoc = new XmlDocument();


                xmlDoc.LoadXml(response2.responseMessage);  //loading soap message as string
                XmlNodeList matchingStatus = xmlDoc.GetElementsByTagName("matchingStatus");
                XmlNodeList cardStatus = xmlDoc.GetElementsByTagName("cardStatus");

                if (response2.responseMessage.ToString().Contains("Error"))
                {
                    XmlNodeList message = xmlDoc.GetElementsByTagName("message");
                    response.responseCode = "01";
                    response.responseMessage = message[0].InnerText;
                }
                else
                {
                    response.matchingStatus = matchingStatus[0].InnerText;
                    response.cardStatus = cardStatus[0].InnerText;
                    response.responseMessage = "Matching Status : " + response.matchingStatus + " card status : " + response.cardStatus;
                    response.responseCode = response.cardStatus;
                }
                return response;
            }
            catch (Exception rx)
            {
                response.responseMessage = rx.Message;
                response.responseCode = "01";
                return response;
            }
        }


        
        #endregion UserManagement
    }
}
