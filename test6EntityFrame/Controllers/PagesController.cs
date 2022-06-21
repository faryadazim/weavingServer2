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
    public class PagesController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/Pages
        public IHttpActionResult GetPages()
        {
            var joinGroup = (
                from pagesTable in db.Pages
                join modulesTable in db.Modules on pagesTable.module_id equals modulesTable.module_id
                where
                    pagesTable.module_id == modulesTable.module_id orderby pagesTable.page_name ascending
                select new
                {
                    id = pagesTable.page_id,
                    name = pagesTable.page_name,
                    pageUrl = pagesTable.page_link,
                    moduleId = modulesTable.module_id,
                    module = modulesTable.module_name
                });

            return Ok(joinGroup);
        }

        // GET: api/Pages/5
        [ResponseType(typeof(Pages))]
        public IHttpActionResult GetPages(string id)
        {
            Pages pages = db.Pages.Find(id);
            if (pages == null)
            {
                return NotFound();
            }

            return Ok(pages);
        }

        // PUT: api/Pages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPages( Pages pages)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           

            db.Entry(pages).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagesExists(pages.page_id))
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

        // POST: api/Pages
        [ResponseType(typeof(Pages))]
        public IHttpActionResult PostPages(Pages pages)
        {

            var customId = Guid.NewGuid().ToString("N");
            var newPages = new Pages()
            {
                page_id = customId,
                page_name =  pages.page_name,
                page_link =pages.page_link ,
            module_id = pages.module_id
            };

            db.Pages.Add(newPages);
            var ListOfRole = (from dataTAble in db.AspNetRoles select dataTAble.Id);

            

            foreach (string ch in ListOfRole)
            {

                var customIdFor = Guid.NewGuid().ToString("P");
                var newPAgePermission = new PagePermission()
                {
                       PermissionId  = customIdFor,

        RoleId = ch,
                    PageId = customId,
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
                if (PagesExists(customId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(newPages);
        }

        // DELETE: api/Pages/5
        [ResponseType(typeof(Pages))]
        public IHttpActionResult DeletePages(string id)
        {
            Pages pages = db.Pages.Find(id);
            if (pages == null)
            {
                return NotFound();
            }

            db.Pages.Remove(pages);
            var pagesPermissionDeleteAgainstPage = (from permissionTable in db.PagePermission where permissionTable.PageId == id
                                                   select permissionTable.PermissionId).ToList();

            
        
             
            foreach (var ch in pagesPermissionDeleteAgainstPage)
            {
              PagePermission pageToDelete = db.PagePermission.Find(ch);
             db.PagePermission.Remove(pageToDelete); 
            db.SaveChanges();
         }



            db.SaveChanges();

            return Ok(pages);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PagesExists(string id)
        {
            return db.Pages.Count(e => e.page_id == id) > 0;
        }
    }
}