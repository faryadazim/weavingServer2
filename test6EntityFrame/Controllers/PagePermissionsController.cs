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
using test6EntityFrame.Models;

namespace test6EntityFrame.Controllers
{
    public class PagePermissionsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/PagePermissions/5
        [Route("api/PagePermissions")]
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
                                                  pagePermissionId = PrTable.PermissionId,
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


        // update by id: api/PagePermissions/5 
        [Route("api/updatePagePermissions")]
        public HttpResponseMessage PutPagePermission(string roleId, PagePermission pagePermissionP)
        {

            var entity = db.PagePermission.FirstOrDefault(e => e.PermissionId == pagePermissionP.PermissionId && e.RoleId == roleId && e.PageId == pagePermissionP.PageId);
            entity.AddPermission = pagePermissionP.AddPermission;
            entity.DelPermission = pagePermissionP.DelPermission;
            entity.EditPermission = pagePermissionP.EditPermission;
            entity.viewPermission = pagePermissionP.viewPermission;


            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Page Permission Updated");
        }
    
        [Authorize]
        [Route("api/PutPagePermissionAll")]
        public HttpResponseMessage PutPagePermissionAll(ClsPagePermissions ppr)
        {


            try
            {
                using (db_weavingEntities db = new db_weavingEntities())
                {
                    var roleEntity = db.AspNetRoles.FirstOrDefault(e => e.Id == ppr.roleId);
                    if (roleEntity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Role not found");
                    }
                    foreach (var pg in ppr.pages)
                    {
                        var entity = db.PagePermission.FirstOrDefault(e => e.PermissionId == pg.PermissionId && e.PageId == pg.PageId && e.RoleId == ppr.roleId);
                        if (entity == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Page ID or Permission ID not Found");
                        }
                        else
                        {
                            entity.EditPermission = pg.EditPermission;
                            entity.AddPermission = pg.AddPermission;
                            entity.viewPermission = pg.viewPermission;
                            entity.DelPermission = pg.DelPermission;
                            db.SaveChanges();
                        }

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "ppr");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }



        }

    }
}