//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealSys.CoreLib.Models.SysDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class SysService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SysService()
        {
            this.EntServices = new HashSet<EntServices>();
            this.SysServiceMenus = new HashSet<SysServiceMenu>();
        }
    
        public int Id { get; set; }
        public string SysCode { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string IconPath { get; set; }
        public int SeqNo { get; set; }
        public string IconFA { get; set; }
        public string UrlPath { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntServices> EntServices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SysServiceMenu> SysServiceMenus { get; set; }
    }
}
