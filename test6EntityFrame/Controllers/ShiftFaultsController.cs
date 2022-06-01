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
    public class ShiftFaultsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

 
        public HttpResponseMessage GetshiftFaults()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.shiftFaults);

        }

 
        public IHttpActionResult GetshiftFaults(int id)
        {
            shiftFaults shiftFaults = db.shiftFaults.Find(id);
            if (shiftFaults == null)
            {
                return NotFound();
            }

            return Ok(shiftFaults);
        }


    
        public HttpResponseMessage PostshiftFaults(shiftFaults shiftFaults)
        {

            db.shiftFaults.Add(shiftFaults);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, shiftFaults);
        }

   
        public IHttpActionResult DeleteshiftFaults(int id)
        {
            shiftFaults shiftFaults = db.shiftFaults.Find(id);
            if (shiftFaults == null)
            {
                return NotFound();
            }

            db.shiftFaults.Remove(shiftFaults);
            db.SaveChanges();

            return Ok(shiftFaults);
        }


    }
}