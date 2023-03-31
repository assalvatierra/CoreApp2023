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
    
    public partial class CarUnit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarUnit()
        {
            this.CarRates = new HashSet<CarRate>();
            this.CarReservations = new HashSet<CarReservation>();
            this.CarImages = new HashSet<CarImage>();
            this.CarViewPages = new HashSet<CarViewPage>();
            this.CarRateUnitPackages = new HashSet<CarRateUnitPackage>();
            this.CarUnitMetas = new HashSet<CarUnitMeta>();
            this.CarDetails = new HashSet<CarDetail>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public int CarCategoryId { get; set; }
        public Nullable<int> SelfDrive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public string Status { get; set; }
    
        public virtual CarCategory CarCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarRate> CarRates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarReservation> CarReservations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarImage> CarImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarViewPage> CarViewPages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarRateUnitPackage> CarRateUnitPackages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarUnitMeta> CarUnitMetas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarDetail> CarDetails { get; set; }
    }
}