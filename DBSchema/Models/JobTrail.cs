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
    
    public partial class JobTrail
    {
        public int Id { get; set; }
        public string RefTable { get; set; }
        public string RefId { get; set; }
        public System.DateTime dtTrail { get; set; }
        public string user { get; set; }
        public string Action { get; set; }
        public string IPAddress { get; set; }
    }
}
