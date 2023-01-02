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
    
    public partial class SupplierContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string SkypeId { get; set; }
        public string ViberId { get; set; }
        public string Remarks { get; set; }
        public int SupplierId { get; set; }
        public string WhatsApp { get; set; }
        public string Email { get; set; }
        public int SupplierContactStatusId { get; set; }
        public string WeChat { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    
        public virtual Supplier Supplier { get; set; }
        public virtual SupplierContactStatus SupplierContactStatu { get; set; }
    }
}
