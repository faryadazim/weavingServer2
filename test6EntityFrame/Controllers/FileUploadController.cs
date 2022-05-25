using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using test6EntityFrame.Models;

namespace test6EntityFrame.Controllers
{
    public class FileUploadController : ApiController
    {

        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostFileUpload()
        {

            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection  
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];
                if (httpPostedFile != null)
                {
                    string n = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);

                    FileUpload imgupload = new FileUpload();
                    int length = httpPostedFile.ContentLength;
                    imgupload.imagedata = new byte[length]; //get imagedata  
                    httpPostedFile.InputStream.Read(imgupload.imagedata, 0, length);
                    imgupload.imagename = Path.GetFileName(httpPostedFile.FileName);

                    //db.FileUpload.Add(imgupload);
                    //db.SaveChanges();
                    string extension1 = Path.GetExtension(httpPostedFile.FileName);
                    string filename = httpPostedFile.FileName;
                    string[] tokens = filename.Split('.');
                    Random rand = new Random();
                    long randnum2 = (long)(rand.NextDouble() * 90000000) + 10000000;
                    string fileName = randnum2 + n + extension1;

                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles"), fileName);
                    // Save the uploaded file to "UploadedFiles" folder  
                    httpPostedFile.SaveAs(fileSavePath);



                    return Request.CreateResponse(HttpStatusCode.OK, "/UploadedFiles/" + fileName);

                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "File Upload Error Please try again");

        }

    }
}
