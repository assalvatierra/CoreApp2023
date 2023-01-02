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
    
    public partial class Vehicle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle()
        {
            this.JobVehicles = new HashSet<JobVehicle>();
        }
    
        public int Id { get; set; }
        public int VehicleModelId { get; set; }
        public string YearModel { get; set; }
        public string PlateNo { get; set; }
        public string Conduction { get; set; }
        public string EngineNo { get; set; }
        public string ChassisNo { get; set; }
        public string Color { get; set; }
        public int CustomerId { get; set; }
        public int CustEntMainId { get; set; }
        public string Remarks { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobVehicle> JobVehicles { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CustEntMain CustEntMain { get; set; }
        public virtual VehicleModel VehicleModel { get; set; }
    }
}
