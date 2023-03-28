using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.Custom
{
    public class CustomerJobDetails
    {
        public int Id { get; set; }
        public string JobDate { get; set; }
        public string JobSvcDates { get; set; }
        public string Description { get; set; }
        public string SvcType { get; set; }
        public string SvcParticulars { get; set; }
        public string AgreedAmt { get; set; }
        public string NoOfDays { get; set; }
        public string NoOfPax { get; set; }
        public string StatusRemarks { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
    }
}
