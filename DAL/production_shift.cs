//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class production_shift
    {
        public int production_shift_id { get; set; }
        public string shift_name { get; set; }
        public int weaver_employee_Id { get; set; }
        public decimal no_of_border { get; set; }
        public decimal total_pieces { get; set; }
        public decimal b_grade_piece { get; set; }
        public decimal a_grade_piece { get; set; }
        public decimal rate_per_border { get; set; }
        public decimal extra_amt { get; set; }
        public string extra_desc { get; set; }
        public decimal total_amt { get; set; }
        public int natting_employee_Id { get; set; }
        public string known_faults_ids { get; set; }
        public Nullable<int> production_id { get; set; }
    }
}
