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
    
    public partial class InvPoItem
    {
        public int Id { get; set; }
        public int InvPoHdrId { get; set; }
        public int InvItemId { get; set; }
        public string ItemQty { get; set; }
        public int InvUomId { get; set; }
    
        public virtual InvPoHdr InvPoHdr { get; set; }
        public virtual InvItem InvItem { get; set; }
        public virtual InvUom InvUom { get; set; }
    }
}