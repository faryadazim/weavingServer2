using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test6EntityFrame.Models
{
    public class FileUpload
    {

        public int imageid
        {
            get;
            set;
        }
        public string imagename
        {
            get;
            set;
        }
        public byte[] imagedata
        {
            get;
            set;
        }
    }

}