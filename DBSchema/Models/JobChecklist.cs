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
    
    public partial class JobChecklist
    {
        public int Id { get; set; }
        public System.DateTime dtEntered { get; set; }
        public System.DateTime dtDue { get; set; }
        public System.DateTime dtNotification { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> RefId { get; set; }
        public int Status { get; set; }
    }
}
