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
    
    public partial class InvCarRcmdRequest
    {
        public int Id { get; set; }
        public int InvCarMntRcmdId { get; set; }
        public int InvCarRcmdStatusId { get; set; }
    
        public virtual InvCarMntRcmd InvCarMntRcmd { get; set; }
        public virtual InvCarRcmdStatus InvCarRcmdStatu { get; set; }
    }
}
