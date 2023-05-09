using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cUserActivity 
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Assigned { get; set; }
        public string ProjectName { get; set; }
        public string SalesCode { get; set; }
        public decimal? Amount { get; set; }
        public string Remarks { get; set; }
        public int CustEntMainId { get; set; }
        public string Type { get; set; }
        public string ActivityType { get; set; }
        public int CustEntActStatusId { get; set; }
        public int CustEntActActionCodesId { get; set; }
        public int? SalesLeadId { get; set; }
        public string Status { get; set; }
        public string Company { get; set; }
        public int Points { get; set; }

        //remove @email from user for display
        public string UserRemoveEmail(string input)
        {
            try
            {
                if (input != null)
                {
                    char ch = '@';
                    int idx = input.IndexOf(ch);
                    return input.Substring(0, idx);
                }
                return "";
            }
            catch
            {
                return "";
            }

        }
    }
}
