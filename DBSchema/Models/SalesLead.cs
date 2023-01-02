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
    
    public partial class SalesLead
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SalesLead()
        {
            this.SalesActivities = new HashSet<SalesActivity>();
            this.SalesLeadCategories = new HashSet<SalesLeadCategory>();
            this.SalesStatus = new HashSet<SalesStatus>();
            this.SalesLeadLinks = new HashSet<SalesLeadLink>();
            this.SalesLeadCompanies = new HashSet<SalesLeadCompany>();
            this.SalesLeadItems = new HashSet<SalesLeadItems>();
            this.SalesProcStatus = new HashSet<SalesProcStatus>();
            this.SalesLeadSupActivities = new HashSet<SalesLeadSupActivity>();
            this.SalesLeadFiles = new HashSet<SalesLeadFile>();
        }
    
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Details { get; set; }
        public string Remarks { get; set; }
        public int CustomerId { get; set; }
        public string CustName { get; set; }
        public System.DateTime DtEntered { get; set; }
        public string EnteredBy { get; set; }
        public decimal Price { get; set; }
        public string AssignedTo { get; set; }
        public string CustPhone { get; set; }
        public string CustEmail { get; set; }
        public string SalesCode { get; set; }
        public string ItemWeight { get; set; }
        public string Commodity { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesActivity> SalesActivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadCategory> SalesLeadCategories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesStatus> SalesStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadLink> SalesLeadLinks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadCompany> SalesLeadCompanies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadItems> SalesLeadItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesProcStatus> SalesProcStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadSupActivity> SalesLeadSupActivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SalesLeadFile> SalesLeadFiles { get; set; }
    }
}
