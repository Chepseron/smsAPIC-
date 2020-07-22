using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAAIF.Models
{
    public class Sms
    {

        public string batchId { get; set; }
        public string type { get; set; }
        public string senderId { get; set; }
        public string number { get; set; }
        public string message { get; set; }
        public string numbers { get; set; }
        public string callback { get; set; }

    }
}