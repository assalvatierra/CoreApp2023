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
    
    public partial class CustEntCat
    {
        public int Id { get; set; }
        public int CustEntMainId { get; set; }
        public int CustCategoryId { get; set; }
    
        public virtual CustEntMain CustEntMain { get; set; }
        public virtual CustCategory CustCategory { get; set; }
    }
}
