//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoreLib.Inventory.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InvItemCustomSpec
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvItemCustomSpec()
        {
            this.Order = "0";
        }
    
        public int Id { get; set; }
        public int InvItemId { get; set; }
        public int InvCustomSpecId { get; set; }
        public string SpecValue { get; set; }
        public string Remarks { get; set; }
        public string Order { get; set; }
    
        public virtual InvItem InvItem { get; set; }
        public virtual InvCustomSpec InvCustomSpec { get; set; }
    }
}
