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
    
    public partial class InvCustomSpec
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvCustomSpec()
        {
            this.Order = 0;
            this.InvItemCustomSpecs = new HashSet<InvItemCustomSpec>();
            this.InvCatCustomSpecs = new HashSet<InvCatCustomSpec>();
        }
    
        public int Id { get; set; }
        public string SpecName { get; set; }
        public int InvCustomSpecTypeId { get; set; }
        public int Order { get; set; }
        public string Measurement { get; set; }
        public string Remarks { get; set; }
    
        public virtual InvCustomSpecType InvCustomSpecType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvItemCustomSpec> InvItemCustomSpecs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvCatCustomSpec> InvCatCustomSpecs { get; set; }
    }
}
