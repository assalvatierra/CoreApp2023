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
    
    public partial class SupplierActActionCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierActActionCode()
        {
            this.SupplierActivities = new HashSet<SupplierActivity>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string SysCode { get; set; }
        public string IconPath { get; set; }
        public int DefaultActStatus { get; set; }
        public int SeqNo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierActivity> SupplierActivities { get; set; }
    }
}
