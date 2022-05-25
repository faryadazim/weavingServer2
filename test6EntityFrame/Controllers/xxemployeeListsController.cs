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
//    public class xxemployeeListsController : ApiController
//    {
//        private db_weavingEntities db = new db_weavingEntities();

//        [Route("api/employeeLists")]
//        public HttpResponseMessage GetemployeeList()
//        {
//            return Request.CreateResponse(HttpStatusCode.OK, db.employeeList);
//        }

//        [Route("api/employeeListsById")]
//        public HttpResponseMessage GetemployeeListById(int id)
//        {
//            employeeList entity = db.employeeList.Find(id);
//            if (entity == null)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
//            }
//            return Request.CreateResponse(HttpStatusCode.OK, entity);
//        }

//        [Route("api/employeeLists")]
//        public HttpResponseMessage PutemployeeList(employeeList employeeList)
//        {

//            try
//            {
//                using (db_weavingEntities db = new db_weavingEntities())
//                {
//                    var entity = db.employeeList.FirstOrDefault(e => e.employee_Id == employeeList.employee_Id);
//                    if (entity == null)
//                    {
//                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
//                    }
//                    else
//                    {
//                        entity.employee_Id = employeeList.employee_Id;
//                        entity.name = employeeList.name;
//                        entity.fatherName = employeeList.fatherName;
//                        entity.phoneNum1 = employeeList.phoneNum1;
//                        entity.phoneNum2 = employeeList.phoneNum2;
//                        entity.phoneNum3 = employeeList.phoneNum3;
//                        entity.homePhoneNum = employeeList.homePhoneNum;
//                        entity.cnicNum = employeeList.cnicNum;
//                        entity.address = employeeList.address;
//                        entity.referenceName = employeeList.referenceName;
//                        entity.referencePhoneNum = employeeList.referencePhoneNum;
//                        entity.jobStatus = employeeList.jobStatus;
//                        entity.designation = employeeList.designation;
//                        entity.employeePic1 = employeeList.employeePic1;
//                        entity.employeePic2 = employeeList.employeePic1;
//                        entity.employeeCnicFront = employeeList.employeeCnicFront;
//                        entity.employeeCnicBsck = employeeList.employeeCnicBsck;
//                        entity.recruitmentType = employeeList.recruitmentType;
//                        entity.weeklySalary = employeeList.weeklySalary;
//                        entity.monthlySalary = employeeList.monthlySalary;
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

//        [Route ("api/employeeListsName")]
//        public HttpResponseMessage GetEmployeeListName()
//        {
//            var employeeList = from employeeTable in db.employeeList select new { employeeTable.name, employeeTable.employee_Id};

//            return Request.CreateResponse(HttpStatusCode.OK, employeeList);
//        }



//        [Route("api/employeeLists")]
//        public HttpResponseMessage PostemployeeList(employeeList employeeListForPost)
//        {
//            try
//            {
//                db.employeeList.Add(employeeListForPost);
//                db.SaveChanges();
//                var message = Request.CreateResponse(HttpStatusCode.Created, employeeListForPost);
//                message.Headers.Location = new Uri(Request.RequestUri + employeeListForPost.employee_Id.ToString());
//                return message;
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
//            }
//        }

//        [Route("api/employeeLists")]
//        public HttpResponseMessage DeleteemployeeList(int id)
//        {
//            employeeList entity = db.employeeList.Find(id);
//            if (entity == null)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
//            }
//            db.employeeList.Remove(entity);
//            db.SaveChanges();
//            return Request.CreateResponse(HttpStatusCode.OK, entity);
//        }


//    }
//}