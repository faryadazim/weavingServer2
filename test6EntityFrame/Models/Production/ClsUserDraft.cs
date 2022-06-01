using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace test6EntityFrame.Models.UserDraft
{
    public class ClsUserDraft
    {
        [Required]
        public string roll_no { get; set; }
        [Required]
        public DateTime production_date { get; set; }
        [Required]
        public decimal roll_weight { get; set; }
        [Required]
        public int loom_id { get; set; }
        [Required]
        public string loom_label { get; set; }
        [Required]
        public int borderSize_id { get; set; }
        [Required]
        public string borderSize_label { get; set; }
        [Required]
        public int borderQuality_id { get; set; }
        [Required]
        public string borderQuality_label { get; set; }
        [Required]
        public string programm_no { get; set; }
        [Required]
        public int grayProduct_id { get; set; }


        [Required]
        public decimal pile_to_pile_length { get; set; }
        [Required]
        public decimal pile_to_pile_width { get; set; }
        [Required]
        public decimal cut_piece_size { get; set; }
        [Required]
        public decimal cut_piece_weight { get; set; }
        public string remarks { get; set; }
        [Required]
        public decimal total_border { get; set; }
        [Required]
        public decimal total_pieces { get; set; }
        [Required]
        public decimal b_grade_pieces { get; set; }
        [Required]
        public decimal a_grade_pieces { get; set; } 
        [Required]
        public decimal current_per_piece_a_weight { get; set; }
        [Required]
        public decimal required_length_p_to_p { get; set; }
        [Required]
        public decimal required_width_p_to_p { get; set; }
        [Required]
        public decimal required_per_piece_a_weight { get; set; }
        public IEnumerable<ClsShiftsForDraft> shifts { get; set; }

    }

    public class ClsShiftsForDraft
    {
        [Required]
        public string shift_name { get; set; }
        [Required]
        public int weaver_employee_Id { get; set; }
        [Required]
        public string weaver_employee_label { get; set; }
        [Required]
        public decimal no_of_border { get; set; }
        [Required]
        public decimal total_pieces { get; set; }
        [Required]
        public decimal b_grade_piece { get; set; }
        [Required]
        public decimal a_grade_piece { get; set; }
        [Required]
        public decimal rate_per_border { get; set; }
        [Required]
        public decimal extra_amt { get; set; }
        public string extra_desc { get; set; }
        [Required]
        public decimal total_amt { get; set; }
        [Required]
        public int natting_employee_Id { get; set; }
        [Required]
        public string natting_employee_label { get; set; }
        public string known_faults_ids { get; set; }
        public string known_faults_labels { get; set; }

    }
}