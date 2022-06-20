using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace test6EntityFrame.Controllers
{
    public class productListController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();


        [Authorize]
        [Route("api/ProductList")]
        public HttpResponseMessage GetProductList()
        { 
                var productList = from grayProductListTb in db.grayProductList select new
                { 
                    productName = (from BorderQualityTable in db.BorderQuality where BorderQualityTable.borderQuality_id == grayProductListTb.itemName select BorderQualityTable.borderQuality1).FirstOrDefault() + "-" + (from BorderSizeTable in db.BorderSize where BorderSizeTable.borderSize_id == grayProductListTb.itemSize select BorderSizeTable.borderSize1).FirstOrDefault(),
                    productId = grayProductListTb.itemName + "-" + grayProductListTb.itemSize,
                };


            return Request.CreateResponse(HttpStatusCode.OK, productList);
        }


    }
}
