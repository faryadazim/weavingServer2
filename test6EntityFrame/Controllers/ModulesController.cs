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
    public class ModulesController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/Modules
        public IQueryable<Modules> GetModules()
        {
            return db.Modules;
        }

        // GET: api/Modules/5
        [ResponseType(typeof(Modules))]
        public IHttpActionResult GetModules(string id)
        {
            Modules modules = db.Modules.Find(id);
            if (modules == null)
            {
                return NotFound();
            }

            return Ok(modules);
        }

        // PUT: api/Modules/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutModules(Modules modules)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

         

            db.Entry(modules).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModulesExists(modules.module_id))
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

        // POST: api/Modules
        [ResponseType(typeof(Modules))]
        public IHttpActionResult PostModules( Modules modules)
        {
            //            public string module_id { get; set; }
            //public string module_name { get; set; }
            //public string module_icon { get; set; }
            var customId = Guid.NewGuid().ToString("N");
            var newModule = new Modules()
            {
                module_id = customId,
                module_name = modules.module_name,
                module_icon = modules.module_icon

            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Modules.Add(newModule);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ModulesExists(modules.module_id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id =customId }, newModule);
        }

        // DELETE: api/Modules/5
        [ResponseType(typeof(Modules))]
        public IHttpActionResult DeleteModules(string id)
        {
            Modules modules = db.Modules.Find(id);
            if (modules == null)
            {
                return NotFound();
            }

            db.Modules.Remove(modules);
            db.SaveChanges();

            return Ok(modules);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ModulesExists(string id)
        {
            return db.Modules.Count(e => e.module_id == id) > 0;
        }
    }
}