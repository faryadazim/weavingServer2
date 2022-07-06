using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace test6EntityFrame.Models
{
    public class ClsPagePermissions
    {

        [Required]
        public string roleId { get; set; }
        public IEnumerable<ClsPages> pages { get; set; }
    }
    public class ClsPages
    {

        public string PermissionId { get; set; }
        public string PageId { get; set; }
        public string AddPermission { get; set; }
        public string viewPermission { get; set; }
        public string EditPermission { get; set; }
        public string DelPermission { get; set; }
        



    }
}