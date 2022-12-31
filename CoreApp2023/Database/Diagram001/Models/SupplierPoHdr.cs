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
    
    public partial class SupplierPoHdr
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierPoHdr()
        {
            this.SupplierPoDtls = new HashSet<SupplierPoDtl>();
        }
    
        public int Id { get; set; }
        public System.DateTime PoDate { get; set; }
        public string Remarks { get; set; }
        public int SupplierId { get; set; }
        public int SupplierPoStatusId { get; set; }
        public string RequestBy { get; set; }
        public System.DateTime DtRequest { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> DtApproved { get; set; }
    
        public virtual Supplier Supplier { get; set; }
        public virtual SupplierPoStatus SupplierPoStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierPoDtl> SupplierPoDtls { get; set; }
    }
}
