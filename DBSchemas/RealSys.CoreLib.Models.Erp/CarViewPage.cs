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
    
    public partial class CarViewPage
    {
        public int Id { get; set; }
        public int CarUnitId { get; set; }
        public string Viewname { get; set; }
    
        public virtual CarUnit CarUnit { get; set; }
    }
}
