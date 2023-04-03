using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Companies
{
    public class CompanyJobsList
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string JobSvcDates { get; set; }
        public string SvcType { get; set; }
        public string SvcParticulars { get; set; }
        public string Customer { get; set; }
        public Decimal Amount { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public string PaymentStatus { get; set; }
    }
}
