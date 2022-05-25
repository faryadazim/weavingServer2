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
    public class grayProductListsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        // GET: api/grayProductLists
        public HttpResponseMessage GetgrayProductList()
        {

            var joinGroup = from productListTable in db.grayProductList
                            join borderNameTable in db.BorderQuality on
                            productListTable.itemName
                            equals borderNameTable.borderQuality_id
                            join borderSizeTable in db.BorderSize on productListTable.itemSize equals borderSizeTable.borderSize_id
                            select new
                            {
                                productListTable.grayProduct_id,
                                itemName = borderNameTable.borderQuality1,
                                itemNameId = borderNameTable.borderQuality_id,
                                itemSizeId = productListTable.itemSize,
                                itemSize = borderSizeTable.borderSize1,
                                productListTable.PerPieceGrayWeightGram,
                                productListTable.graySizeppLength,
                                productListTable.graySizeppWidth,
                                productListTable.LoomNumbPieceInBorder76,
                                productListTable.LoomNumbRatePerBorderWithDraw76,
                                productListTable.LoomNumbRatePerBorderWithoutDraw76,
                                productListTable.LoomNumbPieceInBorder96,
                                productListTable.LoomNumbRatePerBorderWithDraw96,
                                productListTable.LoomNumbRatePerBorderWithoutDraw96,
                                productListTable.status,

                            };
            return Request.CreateResponse(HttpStatusCode.OK, joinGroup);

        }




        //Api created for weaving production form to fetch loom detail specially to find number of  pieces in  border for step one and
        // rate per border for both draw boxes

        [Route("api/loomDetailWPF")]
        public HttpResponseMessage GetLoomDetailWPF(string LoomSize, int BorderSizeId, int BorderQualityId)
        {
            if (LoomSize == "76")
            {


                var numbOfPieceInOneBorder76 = from grayProductTable76 in db.grayProductList
                                               where grayProductTable76.itemName == BorderQualityId
                                               && grayProductTable76.itemSize == BorderSizeId
                                               select  new {
                                                   noOfPieceInOneBorder = grayProductTable76.LoomNumbPieceInBorder76,
                                          
                                                   rateDrawBox = grayProductTable76.LoomNumbRatePerBorderWithDraw76,
                                                   rateWithoutDrawBox = grayProductTable76.LoomNumbRatePerBorderWithoutDraw76,
                                               };
                                               return Request.CreateResponse(HttpStatusCode.OK, numbOfPieceInOneBorder76.FirstOrDefault());
            }
            else if (LoomSize == "96")
            {
                var numberOfPieceInOneBorder96 = from grayProductTable96 in db.grayProductList
                                                 where grayProductTable96.itemName == BorderQualityId
                                                 && grayProductTable96.itemSize == BorderSizeId
                                                 select new {
                                                     noOfPieceInOneBorder  = grayProductTable96.LoomNumbPieceInBorder96,
                                                  
                                                     rateDrawBox=grayProductTable96.LoomNumbRatePerBorderWithDraw96,
                                                     rateWithoutDrawBox =grayProductTable96.LoomNumbRatePerBorderWithoutDraw96,
                                                     
                                                 };

            return Request.CreateResponse(HttpStatusCode.OK, numberOfPieceInOneBorder96.FirstOrDefault());
            }


              else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
            }



        }

// GET: api/grayProductLists/5
[ResponseType(typeof(grayProductList))]
public IHttpActionResult GetgrayProductList(int id)
{
    grayProductList grayProductList = db.grayProductList.Find(id);
    if (grayProductList == null)
    {
        return NotFound();
    }

    return Ok(grayProductList);
}

// PUT: api/grayProductLists/5
[ResponseType(typeof(void))]
public IHttpActionResult PutgrayProductList(int id, grayProductList grayProductList)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    if (id != grayProductList.grayProduct_id)
    {
        return BadRequest();
    }

    db.Entry(grayProductList).State = EntityState.Modified;

    try
    {
        db.SaveChanges();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!grayProductListExists(id))
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

// POST: api/grayProductLists
[ResponseType(typeof(grayProductList))]
public IHttpActionResult PostgrayProductList(grayProductList grayProductList)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    db.grayProductList.Add(grayProductList);
    db.SaveChanges();

    return CreatedAtRoute("DefaultApi", new { id = grayProductList.grayProduct_id }, grayProductList);
}

// DELETE: api/grayProductLists/5
[ResponseType(typeof(grayProductList))]
public IHttpActionResult DeletegrayProductList(int id)
{
    grayProductList grayProductList = db.grayProductList.Find(id);
    if (grayProductList == null)
    {
        return NotFound();
    }

    db.grayProductList.Remove(grayProductList);
    db.SaveChanges();

    return Ok(grayProductList);
}

protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        db.Dispose();
    }
    base.Dispose(disposing);
}

private bool grayProductListExists(int id)
{
    return db.grayProductList.Count(e => e.grayProduct_id == id) > 0;
}
    }
}