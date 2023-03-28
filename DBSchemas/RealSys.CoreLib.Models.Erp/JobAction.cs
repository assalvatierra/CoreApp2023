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
    
    public partial class JobAction
    {
        public int Id { get; set; }
        public int JobServicesId { get; set; }
        public string AssignedTo { get; set; }
        public string PerformedBy { get; set; }
        public Nullable<System.DateTime> DtAssigned { get; set; }
        public Nullable<System.DateTime> DtPerformed { get; set; }
        public int SrvActionItemId { get; set; }
        public string Remarks { get; set; }
    
        public virtual JobServices JobService { get; set; }
        public virtual SrvActionItem SrvActionItem { get; set; }
    }
}
