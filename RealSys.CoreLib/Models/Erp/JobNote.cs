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
    
    public partial class JobNote
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public Nullable<int> Sort { get; set; }
        public string Note { get; set; }
    
        public virtual JobMain JobMain { get; set; }
    }
}
