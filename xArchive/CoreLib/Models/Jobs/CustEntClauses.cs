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
    
    public partial class CustEntClauses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustEntClauses()
        {
            this.Desc2 = "250";
        }
    
        public int Id { get; set; }
        public int CustEntMainId { get; set; }
        public string Title { get; set; }
        public System.DateTime ValidStart { get; set; }
        public System.DateTime ValidEnd { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public System.DateTime DtEncoded { get; set; }
        public string EncodedBy { get; set; }
    
        public virtual CustEntMain CustEntMain { get; set; }
    }
}
