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
    
    public partial class JobPostSale
    {
        public int Id { get; set; }
        public System.DateTime DtPost { get; set; }
        public string DoneBy { get; set; }
        public string Remarks { get; set; }
        public int JobServicesId { get; set; }
        public Nullable<System.DateTime> DtDone { get; set; }
        public int JobPostSalesStatusId { get; set; }
    
        public virtual JobServices JobService { get; set; }
        public virtual JobPostSalesStatus JobPostSalesStatu { get; set; }
    }
}