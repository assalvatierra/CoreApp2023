//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealSys.CoreLib.Models.SysDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntAddress
    {
        public int Id { get; set; }
        public int EntCompanyId { get; set; }
        public int SysSetupTypeId { get; set; }
        public string add1 { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public string Add4 { get; set; }
        public string City { get; set; }
        public string Remarks { get; set; }
        public string Telno1 { get; set; }
        public string Telno2 { get; set; }
    
        public virtual EntBusiness EntCompany { get; set; }
        public virtual SysSetupType SysSetupType { get; set; }
    }
}
