//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoreLib.Inventory.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvWarningLevel
    {
        public int Id { get; set; }
        public int InvItemId { get; set; }
        public decimal Level1 { get; set; }
        public decimal Level2 { get; set; }
        public int InvWarningTypeId { get; set; }
        public int InvUomId { get; set; }
    
        public virtual InvItem InvItem { get; set; }
        public virtual InvWarningType InvWarningType { get; set; }
        public virtual InvUom InvUom { get; set; }
    }
}