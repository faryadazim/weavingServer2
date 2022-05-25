using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;

namespace test6EntityFrame.Controllers
{
    public class BorderSizesController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        [Route("api/BorderSizes")]
        public HttpResponseMessage GetBorderSize()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.BorderSize);
        }

        [Route("api/BorderSizesById")]

        public HttpResponseMessage GetBorderSizeById(int id)
        {
            BorderSize entity = db.BorderSize.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


        [Route("api/BorderSizes")]
        public HttpResponseMessage PutBorderSize(BorderSize borderSize)
        {
            try
            {
                using (db_weavingEntities db = new db_weavingEntities())
                {
                    var entity = db.BorderSize.FirstOrDefault(e => e.borderSize_id == borderSize.borderSize_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
                    }
                    else
                    {
                        entity.borderSize_id = borderSize.borderSize_id;
                        entity.borderSize1 = borderSize.borderSize1;
                        db.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("api/BorderSizes")]
        public HttpResponseMessage PostBorderSize(BorderSize borderSizeForPost)
        {

            try
            {
                db.BorderSize.Add(borderSizeForPost);
                db.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, borderSizeForPost);
                message.Headers.Location = new Uri(Request.RequestUri + borderSizeForPost.borderSize_id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/BorderSizes")]
        public HttpResponseMessage DeleteBorderSize(int id)
        {
            BorderSize entity = db.BorderSize.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
            }
            db.BorderSize.Remove(entity);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


    }
}