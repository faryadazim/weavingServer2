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
using test6EntityFrame.Models.UserDraft;
 

namespace test6EntityFrame.Controllers
{
    public class userDraftController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();


        [Route("api/userDraft")]
        public HttpResponseMessage GetuserDraft(string userId)
        {
            var entity = from draftTable in db.user_draft where draftTable.user_id == userId select draftTable;

            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }



        // POST: 
        [Route("api/userDraft")]
        public IHttpActionResult Postuser_draft(user_draft userDraft  )
        {
            var lastCreatedDraftName = (from draftTable in db.user_draft where draftTable.user_id == userDraft.user_id
                                        orderby draftTable.draft_id descending select draftTable.draft_name).FirstOrDefault();
       

            int lastDraftNumberAgainstParticularUSer;
            if (lastCreatedDraftName == null)
            {
                lastDraftNumberAgainstParticularUSer = 1;
            }
            else
            {
                string[] numberTo = lastCreatedDraftName.Split('-');
                lastDraftNumberAgainstParticularUSer = Int32.Parse(numberTo[4]) + 1;
            };



            string currentDate = string.Format("{0:dd-MM-yyyy}", DateTime.Now);
            string customCreatedDraftName = "Draft-" + currentDate + "-" + lastDraftNumberAgainstParticularUSer;
            var userDraftWithCustomName = new user_draft()
            {
                draft_name = customCreatedDraftName,
                user_id = userDraft.user_id,
                page_name = userDraft.page_name,
                draft_json = userDraft.draft_json
            };

            db.user_draft.Add(userDraftWithCustomName);
            db.SaveChanges();
            return Ok(userDraftWithCustomName);
           // return CreatedAtRoute("DefaultApi", new { id = userDraft.draft_id }, userDraftWithCustomName);
        }

        // DELETE: api/userDraft/5
        [Route("api/userDraft")]
        public IHttpActionResult Deleteuser_draft(int id)
        {
            user_draft user_draft = db.user_draft.Find(id);
            if (user_draft == null)
            {
                return NotFound();
            }

            db.user_draft.Remove(user_draft);
            db.SaveChanges();

            return Ok(user_draft);
        }


    }
}