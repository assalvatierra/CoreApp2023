//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealSys.CoreLib.Models.SysDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntSetting
    {
        public int Id { get; set; }
        public int SysSetupTypeId { get; set; }
        public int EntBusinessId { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Remarks { get; set; }
    
        public virtual EntBusiness EntBusiness { get; set; }
        public virtual SysSetupType SysSetupType { get; set; }
    }
}
