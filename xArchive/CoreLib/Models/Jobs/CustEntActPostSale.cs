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
    
    public partial class CustEntActPostSale
    {
        public int Id { get; set; }
        public string By { get; set; }
        public System.DateTime DateEncoded { get; set; }
        public string Remarks { get; set; }
        public string SalesCode { get; set; }
        public int CustEntActPostSaleStatusId { get; set; }
    
        public virtual CustEntActPostSaleStatus CustEntActPostSaleStatu { get; set; }
    }
}
