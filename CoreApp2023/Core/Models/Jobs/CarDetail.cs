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
    
    public partial class CarDetail
    {
        public int Id { get; set; }
        public string Fuel { get; set; }
        public string Class { get; set; }
        public string Transmission { get; set; }
        public string Usage { get; set; }
        public string Passengers { get; set; }
        public string Remarks { get; set; }
        public int CarUnitId { get; set; }
    
        public virtual CarUnit CarUnit { get; set; }
    }
}
