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
    
    public partial class tbl_member
    {
        public int Id { get; set; }
        public string member_name { get; set; }
        public string member_uniqueId { get; set; }
        public string member_age { get; set; }
        public string member_cell { get; set; }
        public string member_type { get; set; }
        public string member_fee { get; set; }
        public byte[] member_finger { get; set; }
        public Nullable<int> on_vacc { get; set; }
        public Nullable<System.DateTime> join_date { get; set; }
    }
}
