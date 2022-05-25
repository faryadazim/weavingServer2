//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
//using DAL;

//namespace test6EntityFrame.Controllers
//{
//    public class xxgrayProductListsController : ApiController
//    {
//        private db_weavingEntities db = new db_weavingEntities();

//        // GET: api/grayProductLists
//        public IQueryable<grayProductList> GetgrayProductList()
//        {
//            return db.grayProductList;
//        }

//        // GET: api/grayProductLists/5
//        [ResponseType(typeof(grayProductList))]
//        public IHttpActionResult GetgrayProductList(int id)
//        {
//            grayProductList grayProductList = db.grayProductList.Find(id);
//            if (grayProductList == null)
//            {
//                return NotFound();
//            }

//            return Ok(grayProductList);
//        }

//        // PUT: api/grayProductLists/5
//        [ResponseType(typeof(void))]
//        public HttpResponseMessage PutgrayProductList(grayProductList grayProductList)
//        {

//            try
//            {
//                using (db_weavingEntities db = new db_weavingEntities())
//                {
//                    var entity = db.grayProductList.FirstOrDefault(e => e.grayProduct_id == grayProductList.grayProduct_id);
//                    if (entity == null)
//                    {
//                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
//                    }
//                    else
//                    {
//                        entity.grayProduct_id = grayProductList.grayProduct_id;
//                        entity.itemName = grayProductList.itemName;
//                        entity.itemSize = grayProductList.itemSize;
//                        entity.PerPieceGrayWeightGram = grayProductList.PerPieceGrayWeightGram;
//                        entity.graySizeppLength = grayProductList.graySizeppLength;
//                        entity.graySizeppWidth = grayProductList.graySizeppWidth;
//                        entity.LoomNumbPieceInBorder76 = grayProductList.LoomNumbPieceInBorder76;
//                        entity.LoomNumbRatePerBorderWithDraw76 = grayProductList.LoomNumbRatePerBorderWithDraw76;
//                        entity.LoomNumbRatePerBorderWithoutDraw76 = grayProductList.LoomNumbRatePerBorderWithoutDraw76;
//                        entity.LoomNumbPieceInBorder96 = grayProductList.LoomNumbPieceInBorder96;
//                        entity.LoomNumbRatePerBorderWithDraw96 = grayProductList.LoomNumbRatePerBorderWithDraw96;
//                        entity.LoomNumbRatePerBorderWithoutDraw96 = grayProductList.LoomNumbRatePerBorderWithoutDraw96;
//                        entity.status = grayProductList.status;
//                        db.SaveChanges();
//                        return Request.CreateResponse(HttpStatusCode.OK, entity);

//                    }

//                }
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
//            }
//        }

//        // POST: api/grayProductLists
//        [ResponseType(typeof(grayProductList))]
//        public IHttpActionResult PostgrayProductList(grayProductList grayProductList)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.grayProductList.Add(grayProductList);
//            db.SaveChanges();

//            return CreatedAtRoute("DefaultApi", new { id = grayProductList.grayProduct_id }, grayProductList);
//        }

//        // DELETE: api/grayProductLists/5
//        [ResponseType(typeof(grayProductList))]
//        public IHttpActionResult DeletegrayProductList(int id)
//        {
//            grayProductList grayProductList = db.grayProductList.Find(id);
//            if (grayProductList == null)
//            {
//                return NotFound();
//            }

//            db.grayProductList.Remove(grayProductList);
//            db.SaveChanges();

//            return Ok(grayProductList);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool grayProductListExists(int id)
//        {
//            return db.grayProductList.Count(e => e.grayProduct_id == id) > 0;
//        }
//    }
//}