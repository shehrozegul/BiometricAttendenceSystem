//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Enrollment
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_attendence
    {
        public int Id { get; set; }
        public Nullable<int> memberid { get; set; }
        public string uniqueId { get; set; }
        public string name { get; set; }
        public string fee_paid { get; set; }
        public Nullable<System.DateTime> fee_date { get; set; }
        public Nullable<int> val { get; set; }
        public Nullable<System.DateTime> today_date { get; set; }
        public Nullable<int> month { get; set; }
        public Nullable<int> year { get; set; }
        public Nullable<int> fee { get; set; }
        public Nullable<int> day { get; set; }
    }
}
