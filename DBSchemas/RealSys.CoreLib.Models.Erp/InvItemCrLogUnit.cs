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
    
    public partial class InvItemCrLogUnit
    {
        public int Id { get; set; }
        public int InvItemId { get; set; }
        public int CrLogUnitId { get; set; }
    
        public virtual InvItem InvItem { get; set; }
    }
}