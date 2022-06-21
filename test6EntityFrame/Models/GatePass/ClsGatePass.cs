using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace test6EntityFrame.Models.GatePass
{
    public class ClsGatePass
    {
        [Required]
        public string gate_pass_no { get; set; }
        [Required]
        public string party_name { get; set; }
        [Required]
        public string party_cell { get; set; }
        public string party_address { get; set; }
        [Required]
        public int total_rolls { get; set; }
        [Required]
        public int total_pieces { get; set; }
        public decimal total_sharing_weight { get; set; }
        public int color { get; set; }  // its foreign key
        public string dying_process { get; set; }
        public decimal total_dying_weight { get; set; }
        public string remarks { get; set; }
        public string driver_name { get; set; }
        [Required]
        public string vehicle_no { get; set; }
        [Required]
        public DateTime time { get; set; }
        public IEnumerable<ClsGatePassEntries> gatePassEntries { get; set; }
    }
    public class ClsGatePassEntries
    {
        [Required]
        public int borderSize_id { get; set; }
        [Required]
        public int borderQuality_id { get; set; }
        [Required]
        public string roll_no { get; set; }
        public int pieces { get; set; }
        public decimal weight { get; set; } 


    }
}