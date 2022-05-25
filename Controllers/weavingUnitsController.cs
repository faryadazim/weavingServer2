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
    public class weavingUnitsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/weavingUnits
        public IQueryable<weavingUnit> GetweavingUnit()
        {
            return db.weavingUnit;
        }

        // GET: api/weavingUnits/5
        [ResponseType(typeof(weavingUnit))]
        public IHttpActionResult GetweavingUnit(int id)
        {
            weavingUnit weavingUnit = db.weavingUnit.Find(id);
            if (weavingUnit == null)
            {
                return NotFound();
            }

            return Ok(weavingUnit);
        }

        // PUT: api/weavingUnits/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutweavingUnit(int id, weavingUnit weavingUnit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weavingUnit.weavingUnit_id)
            {
                return BadRequest();
            }

            db.Entry(weavingUnit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!weavingUnitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/weavingUnits
        [ResponseType(typeof(weavingUnit))]
        public IHttpActionResult PostweavingUnit(weavingUnit weavingUnit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.weavingUnit.Add(weavingUnit);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = weavingUnit.weavingUnit_id }, weavingUnit);
        }

        // DELETE: api/weavingUnits/5
        [ResponseType(typeof(weavingUnit))]
        public IHttpActionResult DeleteweavingUnit(int id)
        {
            weavingUnit weavingUnit = db.weavingUnit.Find(id);
            if (weavingUnit == null)
            {
                return NotFound();
            }

            db.weavingUnit.Remove(weavingUnit);
            db.SaveChanges();

            return Ok(weavingUnit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool weavingUnitExists(int id)
        {
            return db.weavingUnit.Count(e => e.weavingUnit_id == id) > 0;
        }
    }
}