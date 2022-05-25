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
    public class employeeListsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

     
        [Route("api/employeeLists")]
            public HttpResponseMessage GetemployeeList()
           {
            //on PageTable.page_id equals PrTable.PageId
                                         
            var entity = from employeeTable in db.employeeList
                         join designationTable in db.employeeDesignation  on employeeTable.designation equals designationTable.designation_id
                         where employeeTable.designation == designationTable.designation_id select new
            {
                             employee_Id=employeeTable.employee_Id,
                             name = employeeTable.name,
                             fatherName=employeeTable.fatherName,
                             phoneNum1=employeeTable.phoneNum1,
                             phoneNum2=employeeTable.phoneNum2,
                             phoneNum3=employeeTable.phoneNum3,
                             homePhoneNum=employeeTable.homePhoneNum,
                             cnicNum=employeeTable.cnicNum,
                             address=employeeTable.address,
                             referenceName=employeeTable.referenceName,
                             referencePhoneNum=employeeTable.referencePhoneNum,
                             jobStatus=employeeTable.jobStatus,
                             designation=employeeTable.designation,
                             employeePic1=employeeTable.employeePic1,
                             employeePic2=employeeTable.employeePic2,
                             employeeCnicFront=employeeTable.employeeCnicFront,
                             employeeCnicBsck=employeeTable.employeeCnicBsck,
                             recruitmentType=employeeTable.recruitmentType,
                             weeklySalary=employeeTable.weeklySalary,
                             monthlySalary=employeeTable.monthlySalary,
                             designationName = designationTable.designationName
            };
             return Request.CreateResponse(HttpStatusCode.OK, entity);
             }

        // GET: api/employeeLists/5
        [Route("api/employeeListsById")]
        public HttpResponseMessage GetemployeeListById(int id)
        {
            employeeList entity = db.employeeList.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record Not Found");
            }
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }



        //        [Route("api/employeeListsById")]


       //All Weaver  Available list with Name
       [Route ("api/employeeWeaverListWithName")]
       public HttpResponseMessage GetAllWeaver()
        {
            var entity = from employeeListTable in db.employeeList
                         where employeeListTable.designation == 6
                         select new
                         {
                           employeeId=  employeeListTable.employee_Id,
                          employeeName=   employeeListTable.name,
                         };

            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
        }

        [Route("api/employeeNativingListWithName")]
        public HttpResponseMessage GetAllNativing()
        {
            var entity = from employeeListTable in db.employeeList
                         where employeeListTable.designation == 22
                         select new
                         {
                             employeeId = employeeListTable.employee_Id,
                             employeeName = employeeListTable.name,
                         };

            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Found");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
        }



        [Route("api/employeeListsName")]
        public HttpResponseMessage GetEmployeeListName()
        {
            var employeeList = from employeeTable in db.employeeList select new { employeeTable.name, employeeTable.employee_Id };

            return Request.CreateResponse(HttpStatusCode.OK, employeeList);
        }







        [Route("api/employeeLists")]
        public HttpResponseMessage PutemployeeList(employeeList employeeList)
        {

            try
            {
                using (db_weavingEntities db = new db_weavingEntities())
                {
                    var entity = db.employeeList.FirstOrDefault(e => e.employee_Id == employeeList.employee_Id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
                    }
                    else
                    {
                        entity.employee_Id = employeeList.employee_Id;
                        entity.name = employeeList.name;
                        entity.fatherName = employeeList.fatherName;
                        entity.phoneNum1 = employeeList.phoneNum1;
                        entity.phoneNum2 = employeeList.phoneNum2;
                        entity.phoneNum3 = employeeList.phoneNum3;
                        entity.homePhoneNum = employeeList.homePhoneNum;
                        entity.cnicNum = employeeList.cnicNum;
                        entity.address = employeeList.address;
                        entity.referenceName = employeeList.referenceName;
                        entity.referencePhoneNum = employeeList.referencePhoneNum;
                        entity.jobStatus = employeeList.jobStatus;
                        entity.designation = employeeList.designation;
                        entity.employeePic1 = employeeList.employeePic1;
                        entity.employeePic2 = employeeList.employeePic1;
                        entity.employeeCnicFront = employeeList.employeeCnicFront;
                        entity.employeeCnicBsck = employeeList.employeeCnicBsck;
                        entity.recruitmentType = employeeList.recruitmentType;
                        entity.weeklySalary = employeeList.weeklySalary;
                        entity.monthlySalary = employeeList.monthlySalary;
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


        //post 
        [Route("api/employeeLists")]
        public HttpResponseMessage PostemployeeList(employeeList employeeListForPost)
        {
            try
            {
                db.employeeList.Add(employeeListForPost);
                db.SaveChanges();
                var message = Request.CreateResponse(HttpStatusCode.Created, employeeListForPost);
                message.Headers.Location = new Uri(Request.RequestUri + employeeListForPost.employee_Id.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }







        // DELETE: api/employeeLists/5 
        [Route("api/employeeLists")]
        public HttpResponseMessage DeleteemployeeList(int id)
        {
            employeeList entity = db.employeeList.Find(id);
            if (entity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
            }
            db.employeeList.Remove(entity);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }


    }
}