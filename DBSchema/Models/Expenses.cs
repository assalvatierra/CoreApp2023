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
    
    public partial class Expenses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Expenses()
        {
            this.JobExpenses = new HashSet<JobExpenses>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public int SeqNo { get; set; }
        public int ExpensesCategoryId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobExpenses> JobExpenses { get; set; }
        public virtual ExpensesCategory ExpensesCategory { get; set; }
    }
}
