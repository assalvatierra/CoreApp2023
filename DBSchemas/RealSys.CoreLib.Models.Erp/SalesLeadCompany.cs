//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealSys.CoreLib.Models.Erp
{
    using System;
    using System.Collections.Generic;
    
    public partial class SalesLeadCompany
    {
        public int Id { get; set; }
        public int SalesLeadId { get; set; }
        public int CustEntMainId { get; set; }
    
        public virtual SalesLead SalesLead { get; set; }
        public virtual CustEntMain CustEntMain { get; set; }
    }
}
