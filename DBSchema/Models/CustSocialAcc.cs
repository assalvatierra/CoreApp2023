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
    
    public partial class CustSocialAcc
    {
        public int Id { get; set; }
        public string Facebook { get; set; }
        public string Viber { get; set; }
        public string Skype { get; set; }
        public int CustomerId { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
