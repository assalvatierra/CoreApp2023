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
    
    public partial class InvCarMntRcmd
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvCarMntRcmd()
        {
            this.InvCarRcmdRequests = new HashSet<InvCarRcmdRequest>();
        }
    
        public int Id { get; set; }
        public string Recommendation { get; set; }
        public System.DateTime DateRec { get; set; }
        public bool IsDone { get; set; }
        public int InvItemId { get; set; }
        public int InvCarMntPriorityId { get; set; }
        public Nullable<System.DateTime> DateDue { get; set; }
        public string Remarks { get; set; }
    
        public virtual InvItem InvItem { get; set; }
        public virtual InvCarMntPriority InvCarMntPriority { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvCarRcmdRequest> InvCarRcmdRequests { get; set; }
    }
}
