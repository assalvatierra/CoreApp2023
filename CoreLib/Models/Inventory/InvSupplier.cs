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
    
    public partial class InvSupplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvSupplier()
        {
            this.InvSupplierItems = new HashSet<InvSupplierItem>();
            this.InvPoHdrs = new HashSet<InvPoHdr>();
            this.InvRecHdrs = new HashSet<InvRecHdr>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvSupplierItem> InvSupplierItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvPoHdr> InvPoHdrs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvRecHdr> InvRecHdrs { get; set; }
    }
}
