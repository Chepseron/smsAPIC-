using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAAIF.Repository
{
    public class ResponseData
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }

        public string matchingStatus { get; set; }

        public string cardStatus { get; set; }
        public string photo { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string IDNumber { get; set; }
        public string DateOfBirth { get; set; }

        public string FarmerPayment { get; set; }


        public string Subsidy { get; set; }

        public string DealerAmount { get; set; }



        public object payload { get; set; }
        public string data { get; set; }


        public string response_string { get; set; }

        public string action { get; set; }

    }
}