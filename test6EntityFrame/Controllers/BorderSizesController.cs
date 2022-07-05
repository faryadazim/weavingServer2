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
    public class BorderSizesController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        [Route("api/BorderSizes")]
        public HttpResponseMessage GetBorderSize()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.BorderSize);
        }


        public class SizeList
        {
            public int borderSize_id { get; set; }
            public string borderSize { get; set; }
        }

        [Route("api/BorderSizesStatus")]
        public HttpResponseMessage GetBorderSizeStatus()
        {
            //            var entity = from bQ in db.BorderSize
            //                         join pL in db.grayProductList 
            //on bQ.borderSize_id equals pL.itemSize 
            //                         where pL.status == "Activate" group bQ by bQ.borderSize_id into y
            //                         select new
            //                         {
            //                             borderSize1 = y.Sum(z=>z.borderSize1),
            //                             borderSize_id = y.Sum(x => x.borderSize_id)
            //                         };

            var entity = db.Database.SqlQuery<SizeList>("select a.borderSize_id,a.borderSize from BorderSize a join grayProductList b on a.borderSize_id = b.itemSize where b.status='Activate' group by  a.borderSize,a.borderSize_id").ToList();
         

            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }



        [Route("api/BorderSizesById")]

        public HttpResponseMessage GetBorderSizeById(int id)
        {
            BorderSize entity = db.BorderSize.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


        [Route("api/BorderSizes")]
        public HttpResponseMessage PutBorderSize(BorderSize borderSize)
        {
            try
            {
                using (db_weavingEntities db = new db_weavingEntities())
                {
                    var entity = db.BorderSize.FirstOrDefault(e => e.borderSize_id == borderSize.borderSize_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
                    }
                    else
                    {
                        entity.borderSize_id = borderSize.borderSize_id;
                        entity.borderSize1 = borderSize.borderSize1;
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

        [Route("api/BorderSizes")]
        public HttpResponseMessage PostBorderSize(BorderSize borderSizeForPost)
        {

            try
            {
                db.BorderSize.Add(borderSizeForPost);
                db.SaveChanges();


                var listOfBorderDesign = (from borderDesignTable in db.BorderQuality select borderDesignTable).ToList();

                foreach (var item in listOfBorderDesign)
                {


                    grayProductList gpl = new grayProductList()
                    {

                        itemName = item.borderQuality_id ,
                        itemSize = borderSizeForPost.borderSize_id,
                        PerPieceGrayWeightGram = 0,
                        graySizeppWidth = 0,
                        graySizeppLength = 0,
                        LoomNumbPieceInBorder76 = 0,
                        LoomNumbRatePerBorderWithDraw76 = 0,
                        LoomNumbRatePerBorderWithoutDraw76 = 0,
                        LoomNumbPieceInBorder96 = 0,
                        LoomNumbRatePerBorderWithDraw96 = 0,
                        LoomNumbRatePerBorderWithoutDraw96 = 0,
                        nativingRate76=0,
                        nativingRate96=0,
                        status = "Activate"

                    };
                    db.grayProductList.Add(gpl);
                    db.SaveChanges();




                };














                var message = Request.CreateResponse(HttpStatusCode.Created, borderSizeForPost);
                message.Headers.Location = new Uri(Request.RequestUri + borderSizeForPost.borderSize_id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("api/BorderSizes")]
        public HttpResponseMessage DeleteBorderSize(int id)
        {
            BorderSize entity = db.BorderSize.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
            }
            db.BorderSize.Remove(entity);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


    }
}