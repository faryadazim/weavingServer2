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
    public class LoomListsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        

        [Route("api/LoomLists")]
        public HttpResponseMessage GetLoomList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.LoomList);
        }





        [Route("api/LoomListsById")]
        public HttpResponseMessage GetLoomListById(int id)
        {
            LoomList entity = db.LoomList.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


        [Route("api/LoomLists")]
        public HttpResponseMessage PutLoomList( LoomList loomList)
        {



            try
            {
                using (db_weavingEntities db = new db_weavingEntities())
                {
                    var entity = db.LoomList.FirstOrDefault(e => e.loom_id == loomList.loom_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
                    }
                    else
                    {
                        entity.loom_id = loomList.loom_id;
                        entity.loomSize = loomList.loomSize;
                        entity.loomNumber = loomList.loomNumber;
                        entity.jacquard = loomList.jacquard;
                        entity.drawBox = loomList.drawBox;
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
        [Route("api/LoomLists")]
        public HttpResponseMessage PostLoomList(LoomList loomListForPost)
        {
            try
            {
                db.LoomList.Add(loomListForPost);
                db.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, loomListForPost);
                message.Headers.Location = new Uri(Request.RequestUri + loomListForPost.loom_id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("api/LoomLists")]
        public HttpResponseMessage DeleteLoomList(int id)
        {
            LoomList entity = db.LoomList.Find(id); 
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
            }
            db.LoomList.Remove(entity);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        
    }
}