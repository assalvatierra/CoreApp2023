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
    
    public partial class RateGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RateGroup()
        {
            this.CarRateGroups = new HashSet<CarRateGroup>();
        }
    
        public int Id { get; set; }
        public string GroupName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarRateGroup> CarRateGroups { get; set; }
    }
}
