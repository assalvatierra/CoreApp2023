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
    
    public partial class CashExpense
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public System.DateTime DtExpense { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string RecievedBy { get; set; }
        public string ReleasedBy { get; set; }
    
        public virtual JobMain JobMain { get; set; }
    }
}