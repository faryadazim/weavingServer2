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
    public class UserRolesController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/UserRoles
        public IQueryable<AspNetUserRoles> GetAspNetUserRoles()
        {
            return db.AspNetUserRoles;
        }

        // GET: api/UserRoles/5
        [ResponseType(typeof(AspNetUserRoles))]
        public IHttpActionResult GetAspNetUserRoles(string id)
        {
            var entity = from table in db.AspNetUserRoles where table.UserId == id select table;

            if (id == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        // PUT: api/UserRoles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAspNetUserRoles(string id, AspNetUserRoles aspNetUserRoles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspNetUserRoles.UserId)
            {
                return BadRequest();
            }

            db.Entry(aspNetUserRoles).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserRolesExists(id))
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

        // POST: api/UserRoles
        [ResponseType(typeof(AspNetUserRoles))]
        public IHttpActionResult PostAspNetUserRoles(AspNetUserRoles aspNetUserRoles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUserRoles.Add(aspNetUserRoles);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserRolesExists(aspNetUserRoles.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aspNetUserRoles.UserId }, aspNetUserRoles);
        }

        // DELETE: api/UserRoles/5
        [ResponseType(typeof(AspNetUserRoles))]
        public IHttpActionResult DeleteAspNetUserRoles(string id)
        {
            AspNetUserRoles aspNetUserRoles = db.AspNetUserRoles.Find(id);
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            db.AspNetUserRoles.Remove(aspNetUserRoles);
            db.SaveChanges();

            return Ok(aspNetUserRoles);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserRolesExists(string id)
        {
            return db.AspNetUserRoles.Count(e => e.UserId == id) > 0;
        }
    }
}