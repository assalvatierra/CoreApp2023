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
    
    public partial class SalesLeadQuotedItem
    {
        public int Id { get; set; }
        public int SalesLeadItemsId { get; set; }
        public int SupplierItemRateId { get; set; }
        public int SalesLeadQuotedItemStatusId { get; set; }
    
        public virtual SalesLeadItems SalesLeadItem { get; set; }
        public virtual SupplierItemRate SupplierItemRate { get; set; }
        public virtual SalesLeadQuotedItemStatus SalesLeadQuotedItemStatu { get; set; }
    }
}
