using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Activities
{
    public class cUserActivity : CustEntActivity
    {
        public string Company { get; set; }
        public int Points { get; set; }

        //remove @email from user for display
        public string UserRemoveEmail(string input)
        {
            try
            {
                char ch = '@';
                int idx = input.IndexOf(ch);
                return input.Substring(0, idx);
            }
            catch
            {
                return input;
            }

        }
    }
}
