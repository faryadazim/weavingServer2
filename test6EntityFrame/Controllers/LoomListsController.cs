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
    public class LoomListsController : ApiController
    {
    
        private db_weavingEntities db = new db_weavingEntities();


        
    
        [Route("api/LoomLists")]
        public HttpResponseMessage GetLoomList()
        {


            var joinGroup = from weavingUnitRow in db.weavingUnit
                            select new
                            {
                                weavingUnitRow.weavingUnit_id,
                                weavingUnitRow.weavingUnitName,
                                loomsList = from loomTable in db.LoomList
                                            where loomTable.weavingUnitId == weavingUnitRow.weavingUnit_id
                                            select new
                                            {
                                                loomTable.loom_id,
                                                loomTable.loomSize,
                                                loomTable.jacquard,
                                                loomTable.drawBox,
                                                loomTable.loomNumber,

                                            }
                            };






            return Request.CreateResponse(HttpStatusCode.OK, joinGroup);



            //(from PageTable in db.Pages
            // join PrTable in db.PagePermission on PageTable.page_id equals PrTable.PageId
            // where PrTable.RoleId == RoleID.FirstOrDefault() && PageTable.module_id == moduleRow.module_id
            // select new
            // {
            //     pageName = PageTable.page_name,
            //     pageID = PageTable.page_id,
            //     pageURL = PageTable.page_link,
            //     PageTable.page_id,
            //     //---- Permission Against Role 
            //     PrTable.AddPermission,
            //     PrTable.DelPermission,
            //     PrTable.EditPermission,
            //     PrTable.viewPermission

            // })
        }


        [Route("api/LoomListsCore")]
        public HttpResponseMessage GetLoomListCore()
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
        public HttpResponseMessage PutLoomList(LoomList loomList)
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
                        entity.weavingUnitId = loomList.weavingUnitId;
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
                var lastLoomIdInThatPartiularWeavingUnit = from loomTable in db.LoomList
                                                           where loomTable.weavingUnitId == loomListForPost.weavingUnitId
                                                           orderby loomTable.loom_id descending
                                                           select loomTable.loomNumber;
                string newListLoom = lastLoomIdInThatPartiularWeavingUnit.FirstOrDefault();

                int actualLoomNumber;
                if (newListLoom == null)
                {
                    actualLoomNumber = 1;
                }
                else
                {
                    string[] numberTo = newListLoom.Split('-');
                    actualLoomNumber = Int32.Parse(numberTo[2]) + 1;
                }

                var weavingUnitShortCode = from weavingUnitTable in
                        db.weavingUnit
                                           where weavingUnitTable.weavingUnit_id == loomListForPost.weavingUnitId
                                           select weavingUnitTable.weavingUnitShortCode;


                string GeneratedLoomNumber = "LN-" + weavingUnitShortCode.FirstOrDefault() + "-" + actualLoomNumber.ToString();
                var newLoomListWithCustomLoomNumber = new LoomList()
                {

                    loomNumber = GeneratedLoomNumber,
                    loomSize = loomListForPost.loomSize,
                    drawBox = loomListForPost.drawBox,
                    jacquard = loomListForPost.jacquard,
                    weavingUnitId = loomListForPost.weavingUnitId

                };
                db.LoomList.Add(newLoomListWithCustomLoomNumber);
                db.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, newLoomListWithCustomLoomNumber);
                message.Headers.Location = new Uri(Request.RequestUri + newLoomListWithCustomLoomNumber.loom_id.ToString());
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