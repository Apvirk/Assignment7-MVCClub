//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCClub
{
    using System;
    using System.Collections.Generic;
    
    public partial class MemberShip
    {
        public int MembershipID { get; set; }
        public Nullable<int> ClubID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<decimal> RegistrationFee { get; set; }
    
        public virtual Club Club { get; set; }
        public virtual Student Student { get; set; }
    }
}