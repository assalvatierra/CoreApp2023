//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBSchema.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobExpenses
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public int JobMainId { get; set; }
        public int ExpensesId { get; set; }
        public int JobServicesId { get; set; }
        public Nullable<System.DateTime> DtExpense { get; set; }
        public Nullable<bool> IsReleased { get; set; }
        public Nullable<bool> ForRelease { get; set; }
    
        public virtual Expenses Expens { get; set; }
        public virtual JobServices JobService { get; set; }
    }
}
