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
    public class PagePermissionsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/PagePermissions/5
        [ResponseType(typeof(PagePermission))]
        public IHttpActionResult GetPagePermission(string roleId)
        {



            var pagePermission = from moduleRow in db.Modules
                            select new
                            {
                                moduleRow.module_name,
                                moduleRow.module_id,
                                pages = (from PageTable in db.Pages
                                         join PrTable in db.PagePermission on PageTable.page_id equals PrTable.PageId
                                         where PrTable.RoleId == roleId && PageTable.module_id == moduleRow.module_id
                                         orderby PageTable.page_name ascending  
                                         select new
                                         {
                                             pageName = PageTable.page_name,
                                             pageID = PageTable.page_id,
                                             //moduleName = moduleRow.module_name,
                                             //moduleID = moduleRow.module_id, 
                                             pageURL = PageTable.page_link,
                                             PageTable.page_id,
                                             //---- Permission Against Role 
                                             PrTable.AddPermission,
                                             PrTable.DelPermission,
                                             PrTable.EditPermission,
                                             PrTable.viewPermission

                                         })
                            }; 
            return Ok(pagePermission);

        }


        // PUT: api/PagePermissions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPagePermission(string id, PagePermission pagePermission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pagePermission.PermissionId)
            {
                return BadRequest();
            }

            db.Entry(pagePermission).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagePermissionExists(id))
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



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PagePermissionExists(string id)
        {
            return db.PagePermission.Count(e => e.PermissionId == id) > 0;
        }
    }
}