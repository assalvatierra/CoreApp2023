//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diagram001.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustFileRef
    {
        public int Id { get; set; }
        public string RefTable { get; set; }
        public int RefId { get; set; }
        public int CustFilesId { get; set; }
    
        public virtual CustFiles CustFile { get; set; }
    }
}
