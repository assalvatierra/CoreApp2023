//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RealSys.CoreLib.Models.Erp
{
    using System;
    using System.Collections.Generic;
    
    public partial class RsvPayment
    {
        public int Id { get; set; }
        public System.DateTime DtPayment { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string PaypaPaymentId { get; set; }
        public int OnlineReservationId { get; set; }
    
        public virtual OnlineReservation OnlineReservation { get; set; }
    }
}