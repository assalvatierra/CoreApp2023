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
    
    public partial class SysAccessUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SysMenuId { get; set; }
        public int Seqno { get; set; }
    
        public virtual SysMenu SysMenu { get; set; }
    }
}
