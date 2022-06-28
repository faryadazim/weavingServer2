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
    [Authorize]
    public class ColorConfigsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/ColorConfigs
        [Route("api/GetColor")]
        public HttpResponseMessage GetColor()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.ColorConfig);
        }


        [Route("api/GetColorByID")]
        public HttpResponseMessage GetColorByID(int id)
        {
            var entity = (from cl in db.ColorConfig where cl.color_id == id select cl).FirstOrDefault();
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Record Avaialble");
            }

            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        [Route("api/PutColor")]
        public HttpResponseMessage PutColor(int id, string colorName)
        {

            var entity = db.ColorConfig.FirstOrDefault(e => e.color_id == id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Row not found");
            }
            else
            {

                entity.color_name = colorName;
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        [Route("api/PostColor")]
        public HttpResponseMessage PostColor(string color)
        {
            ColorConfig clC = new ColorConfig()
            {

                color_name = color,

            };

            db.ColorConfig.Add(clC);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, clC);
        }

        [Route("api/DeleteColor")]
        public IHttpActionResult DeleteColor(int id)
        {
            var entity = db.ColorConfig.FirstOrDefault(e => e.color_id == id);
            if (entity == null)
            {
                return NotFound();
            }
            db.ColorConfig.Remove(entity);
            db.SaveChanges();
            return Ok(entity);
        }

    }
}