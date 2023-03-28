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
    
    public partial class CustNotifRecipient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustNotifRecipient()
        {
            this.CustNotifActivities = new HashSet<CustNotifActivity>();
        }
    
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CustNotifId { get; set; }
        public int NotifRecipientId { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual CustNotif CustNotif { get; set; }
        public virtual CustNotifRecipientList CustNotifRecipientList { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustNotifActivity> CustNotifActivities { get; set; }
    }
}
