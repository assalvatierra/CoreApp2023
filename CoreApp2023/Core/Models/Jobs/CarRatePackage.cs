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
    
    public partial class CarRatePackage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarRatePackage()
        {
            this.CarRateUnitPackages = new HashSet<CarRateUnitPackage>();
            this.CarRateGroups = new HashSet<CarRateGroup>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public decimal DailyMeals { get; set; }
        public decimal DailyRoom { get; set; }
        public int DaysMin { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarRateUnitPackage> CarRateUnitPackages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarRateGroup> CarRateGroups { get; set; }
    }
}
