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
    
    public partial class SupplierPoDtl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierPoDtl()
        {
            this.SupplierPoItems = new HashSet<SupplierPoItem>();
        }
    
        public int Id { get; set; }
        public int SupplierPoHdrId { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public int JobServicesId { get; set; }
    
        public virtual SupplierPoHdr SupplierPoHdr { get; set; }
        public virtual JobServices JobService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierPoItem> SupplierPoItems { get; set; }
    }
}