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
    
    public partial class InvWarningType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvWarningType()
        {
            this.InvWarningLevels = new HashSet<InvWarningLevel>();
        }
    
        public int Id { get; set; }
        public string Desc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvWarningLevel> InvWarningLevels { get; set; }
    }
}
