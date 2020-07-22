using System;
using System.Web.Http;
using MAAIF.Repository;
using MAAIF.Models;

namespace MAAIF.Controllers
{
    public class SmsController : ApiController
    {
        ResponseData response = new ResponseData();
        Repository.SMSRepository.SMSRepository SMSRepository = new Repository.SMSRepository.SMSRepository();
        #region SMS

        [Route("api/SingleSMSSingleNumber")]
        [HttpPost]
        public ResponseData SingleSMSSingleNumber(Sms obj)
        {
            ResponseData data = new ResponseData();
            try
            {
                response = SMSRepository.SingleSMSSingleNumber(obj);
                return response;
            }
            catch (Exception ex)
            {
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }



        [Route("api/SingelSMSMultipleNumbers")]
        [HttpPost]
        public ResponseData SingelSMSMultipleNumbers(Sms obj)
        {
            ResponseData data = new ResponseData();
            try
            {
                response = SMSRepository.SingelSMSMultipleNumbers(obj);
                return response;
            }
            catch (Exception ex)
            {
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }





        [Route("api/GetBatchStatus")]
        [HttpPost]
        public ResponseData GetBatchStatus(Sms obj)
        {
            ResponseData data = new ResponseData();
            try
            {
                response = SMSRepository.GetBatchStatus(obj);
                return response;
            }
            catch (Exception ex)
            {
                response.responseCode = "091";
                response.responseMessage = ex.Message;
                return response;
            }
        }

        #endregion
    }
}

