//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diagram001.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobPickup
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public System.DateTime puDate { get; set; }
        public string puTime { get; set; }
        public string puLocation { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
    
        public virtual JobMain JobMain { get; set; }
    }
}
