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
    
    public partial class SvcGroup
    {
        public int Id { get; set; }
        public int ServicesId { get; set; }
        public int SvcDetailId { get; set; }
    
        public virtual Services Service { get; set; }
        public virtual SvcDetail SvcDetail { get; set; }
    }
}
