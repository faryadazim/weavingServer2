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
    public class employeeDesignationsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        [Route("api/employeeDesignations")]
        public HttpResponseMessage GetemployeeDesignation()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.employeeDesignation);
        }


        [Route("api/employeeDesignationsById")]
        public HttpResponseMessage GetemployeeDesignationById(int id)
        {
            employeeDesignation entity = db.employeeDesignation.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
            }


            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


        [Route("api/employeeDesignations")]
        [ResponseType(typeof(void))]
        public HttpResponseMessage PutemployeeDesignation(employeeDesignation employeeDesignation)
        {
            try
            {
                using (db_weavingEntities db = new db_weavingEntities())
                {
                    var entity = db.employeeDesignation.FirstOrDefault(e => e.designation_id == employeeDesignation.designation_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
                    }
                    else
                    {
                        entity.designation_id = employeeDesignation.designation_id;
                        entity.designationName = employeeDesignation.designationName;
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


        [Route("api/employeeDesignations")]
        public HttpResponseMessage PostemployeeDesignation(employeeDesignation employeeDesignationForPost)
        {
            try
            {
                db.employeeDesignation.Add(employeeDesignationForPost);
                db.SaveChanges();

                var message = Request.CreateResponse(HttpStatusCode.Created, employeeDesignationForPost);
                message.Headers.Location = new Uri(Request.RequestUri + employeeDesignationForPost.designation_id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("api/employeeDesignations")]
        public HttpResponseMessage DeleteemployeeDesignation(int id)
        {
            employeeDesignation entity = db.employeeDesignation.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
            }

            db.employeeDesignation.Remove(entity);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


    }
}