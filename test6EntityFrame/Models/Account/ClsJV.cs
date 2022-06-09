using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace test6EntityFrame.Models.Account
{
    public class ClsJV
    {
      [Required]
       public DateTime date { get; set; }
        [Required]
        public string voucherInv { get; set; }
        [Required] 
public string description { get; set; }
       [Required] 
public decimal debit { get; set; }
       [Required]
        public decimal credit { get; set; } 


    }
}