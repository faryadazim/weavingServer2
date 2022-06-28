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
    public class ClsForPartyList
    {

        public string partyName { get; set; }
        public string partyCell { get; set; }
        public string partyCnic { get; set; }
        public string partyAddress { get; set; }

    }

    [Authorize]
    public class partyListsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        [Route("api/GetPartyList")]
        public HttpResponseMessage GetpartyList()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.partyList);
        }


        [Route("api/GetPartyListById")]
        public HttpResponseMessage GetpartyListById(int id)
        {
            var entity = (from pt in db.partyList where pt.party_id == id select pt).FirstOrDefault();
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Record Avaialble");
            }

            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        [Route("api/PutPartyList")]
        public HttpResponseMessage PutpartyList(int id, ClsForPartyList pr)
        {

            var entity = db.partyList.FirstOrDefault(e => e.party_id == id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Row not found");
            }
            else
            {
                entity.party_name = pr.partyName;
                entity.party_cnic = pr.partyCnic;
                entity.party_cell = pr.partyCell;
                entity.party_address = pr.partyAddress;
                db.SaveChanges();
            }
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        [Route("api/PostParty")]
        public HttpResponseMessage PostpartyList(ClsForPartyList pr)
        {
            partyList prE = new partyList()
            {
                party_name = pr.partyName,
                party_cell = pr.partyCell,
                party_cnic = pr.partyCnic,
                party_address = pr.partyAddress

            };

            db.partyList.Add(prE);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, prE);
        }

        [Route("api/DeleteParty")]
        public IHttpActionResult DeletepartyList(int id)
        {
            var entity = db.partyList.FirstOrDefault(e => e.party_id == id);
            if (entity == null)
            {
                return NotFound();
            }
            db.partyList.Remove(entity);
            db.SaveChanges();
            return Ok(entity);
        }

    }
}