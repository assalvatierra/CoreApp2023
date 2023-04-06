using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Companies
{
    public class cCompanyContact
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string SocialMedia { get; set; }
        public string Telephone { get; set; }
    }
}
