//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobsV1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobItinerary
    {
        public int Id { get; set; }
        public int JobMainId { get; set; }
        public int DestinationId { get; set; }
        public Nullable<decimal> ActualRate { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> ItiDate { get; set; }
        public Nullable<int> SvcId { get; set; }
    
        public virtual JobMain JobMain { get; set; }
        public virtual Destination Destination { get; set; }
    }
}
