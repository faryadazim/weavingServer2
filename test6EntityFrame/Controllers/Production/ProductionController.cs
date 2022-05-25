using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using test6EntityFrame.Models.Production;

namespace test6EntityFrame.Controllers.Production
{
      [Authorize]
    public class ProductionController : ApiController
    {
        [Route("api/Production")]
        public HttpResponseMessage PostProduction(ClsProduction p)
        {

            return Request.CreateResponse(HttpStatusCode.OK, p);
        }

    }
}
