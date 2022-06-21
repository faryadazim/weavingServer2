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
    public class RolesController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/Roles
        public IQueryable<AspNetRoles> GetAspNetRoles()
        {
            return db.AspNetRoles;
        }

        // GET: api/Roles/5
        [ResponseType(typeof(AspNetRoles))]
        public IHttpActionResult GetAspNetRoles(string id)
        {
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return NotFound();
            }

            return Ok(aspNetRoles);
        }

        // PUT: api/Roles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAspNetRoles(string id, string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = db.AspNetRoles.FirstOrDefault(e => e.Id == id);
            if(entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.Name = roleName;
            }
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetRolesExists(id))
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

        // POST: api/Roles 
        public IHttpActionResult PostAspNetRoles(string InputPageName)
        {
            var CustomId = Guid.NewGuid().ToString("N");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRole = new AspNetRoles()
            {
                Id = CustomId,
                Name = InputPageName

            };
            db.AspNetRoles.Add(newRole);
            // if we create any pages it permission will be assign to  all Role but what if
            //we create a new Role --- after that  new pages permission will bew assigned but wha about old pages

            //solution ---
            //bring list opf all table ;
            //add these table to that New Role
            //var ListOfRole = (from dataTAble in db.AspNetRoles select dataTAble.Id);
            var ListOfPages = from dataTable in db.Pages select dataTable.page_id;
            foreach (string ch in ListOfPages)
            {

                var customIdFor = Guid.NewGuid().ToString("P");
                var newPAgePermission = new PagePermission()
                {
                    PermissionId = customIdFor,

                    RoleId = CustomId,
                    PageId = ch,
                    EditPermission = "false",
                    viewPermission = "false",
                    DelPermission = "false",
                    AddPermission = "false"
                };
                db.PagePermission.Add(newPAgePermission);

            }
           

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            { 
                return NotFound();
            }

            return Ok();
        }

        // DELETE: api/Roles/5
        [ResponseType(typeof(AspNetRoles))]
        public IHttpActionResult DeleteAspNetRoles(string id)
        {
            AspNetRoles aspNetRoles = db.AspNetRoles.Find(id);
            if (aspNetRoles == null)
            {
                return NotFound();
            }

            db.AspNetRoles.Remove(aspNetRoles);

            var permissionIds = from prTable in db.PagePermission where prTable.RoleId == id select prTable.PermissionId;




 

            foreach (string ch in permissionIds)
            {
                 
                PagePermission prPermission = db.PagePermission.Find(ch);
                if (prPermission == null)
                {
                    return NotFound();
                }

                db.PagePermission.Remove(prPermission);


                

            }
 db.SaveChanges();
            return Ok(aspNetRoles);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetRolesExists(string id)
        {
            return db.AspNetRoles.Count(e => e.Id == id) > 0;
        }
    }
}