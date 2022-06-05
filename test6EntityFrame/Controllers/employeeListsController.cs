using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;
using DAL;
using test6EntityFrame.Models;

namespace test6EntityFrame.Controllers
{
    public class employeeListsController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        //All  Employee List 
        [Route("api/employeeLists")]
        public HttpResponseMessage GetemployeeList()
        {
            var entity = from employeeTable in db.employeeList
                         join designationTable in db.employeeDesignation on employeeTable.designation equals designationTable.designation_id
                         where employeeTable.designation == designationTable.designation_id
                         select new
                         {
                             employee_Id = employeeTable.employee_Id,
                             name = employeeTable.name,
                             fatherName = employeeTable.fatherName,
                             phoneNum1 = employeeTable.phoneNum1,
                             phoneNum2 = employeeTable.phoneNum2,
                             phoneNum3 = employeeTable.phoneNum3,
                             homePhoneNum = employeeTable.homePhoneNum,
                             cnicNum = employeeTable.cnicNum,
                             address = employeeTable.address,
                             referenceName = employeeTable.referenceName,
                             referencePhoneNum = employeeTable.referencePhoneNum,
                             jobStatus = employeeTable.jobStatus,
                             designation = employeeTable.designation,
                             employeePic1 = employeeTable.employeePic1,
                             employeePic2 = employeeTable.employeePic2,
                             employeeCnicFront = employeeTable.employeeCnicFront,
                             employeeCnicBsck = employeeTable.employeeCnicBsck,
                             recruitmentType = employeeTable.recruitmentType,
                             weeklySalary = employeeTable.weeklySalary,
                             monthlySalary = employeeTable.monthlySalary,
                             designationName = designationTable.designationName
                         };
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        // Employee By Ids
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


        // Employee Only Weaver 
        [Route("api/employeeWeaverListWithName")]
        public HttpResponseMessage GetAllWeaver()
        {
            int weaverId = (db.employeeDesignation.FirstOrDefault(c => c.designationName == "Weaver").designation_id);
            var entity = from employeeListTable in db.employeeList
                         where employeeListTable.designation == weaverId
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


        // Employee Only Nativing
        [Route("api/employeeNativingListWithName")]
        public HttpResponseMessage GetAllNativing()
        {
            int nativingId = (db.employeeDesignation.FirstOrDefault(c => c.designationName == "Nativing").designation_id);
            var entity = from employeeListTable in db.employeeList
                         where employeeListTable.designation == nativingId
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

        // Employee List Name nd id only
        [Route("api/employeeListsName")]
        public HttpResponseMessage GetEmployeeListName()
        {
            var employeeList = from employeeTable in db.employeeList select new { employeeTable.name, employeeTable.employee_Id };
            return Request.CreateResponse(HttpStatusCode.OK, employeeList);
        }



        //updated employee list + chart Account
        [Authorize]
        [Route("api/employeeLists")]
        public HttpResponseMessage PutemployeeList(employeeList employeeList)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

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

                        //updating EmployeeData in employeeList table
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
                        entity.chart_id = employeeList.chart_id;



                        //Getting chartAccount against particular employee
                        var chartAccountEmployee = db.chart_of_accounts.FirstOrDefault(e => e.chart_id == employeeList.chart_id);
                        //updating chart account employee 
                        chartAccountEmployee.chart_id = chartAccountEmployee.chart_id;
                        chartAccountEmployee.account_name= employeeList.name;
                        chartAccountEmployee.account_code = chartAccountEmployee.account_code;
                        chartAccountEmployee.description = chartAccountEmployee.description;
                        chartAccountEmployee.created_datetime = chartAccountEmployee.created_datetime;
                        chartAccountEmployee.created_by = chartAccountEmployee.created_by;
                        chartAccountEmployee.modified_by = LogIn;
                        chartAccountEmployee.modified_datetime = StaticValues.PakDateTime; 
                    } 
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, entity);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //post 
        [Authorize]
        [Route("api/employeeLists")]
        public HttpResponseMessage PostemployeeList(employeeList employeeListForPost)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            //  int accountTypeId = db.account_type.First(e => e.type_name == "Employees").account_type_id; 
            var accountShortCode = (from accountType in db.account_type where accountType.account_type_id == 1 select accountType.type_code).FirstOrDefault();
            var lastEmployeeCode = (from chartAccountTable in db.chart_of_accounts
                                    where chartAccountTable.account_type_id == 1
                                    orderby chartAccountTable.chart_id descending
                                    select chartAccountTable.account_code).FirstOrDefault();


            int actuallEmployeeCode;
            if (lastEmployeeCode == null)
            {
                actuallEmployeeCode = 1;
            }
            else
            {
                string[] numberTo = lastEmployeeCode.Split('-');
                actuallEmployeeCode = Int32.Parse(numberTo[1]) + 1;
            }
            string GeneratedEmployeeCode = accountShortCode + "-" + actuallEmployeeCode.ToString();
            var employeeInChartsAccount = new chart_of_accounts()
            {
                account_name = employeeListForPost.name,
                account_code = GeneratedEmployeeCode,
                description = "",
                created_datetime = StaticValues.PakDateTime,
                created_by = LogIn,
                modified_datetime = StaticValues.PakDateTime,
                modified_by = LogIn,
                account_type_id = 1,

            };
            db.chart_of_accounts.Add(employeeInChartsAccount);
            db.SaveChanges();

            try
            {
                var newEmployeeForPost = new employeeList()
                {
                    name = employeeListForPost.name,
                    fatherName = employeeListForPost.fatherName,
                    phoneNum1 = employeeListForPost.phoneNum1,
                    phoneNum2 = employeeListForPost.phoneNum2,
                    phoneNum3 = employeeListForPost.phoneNum3,
                    homePhoneNum = employeeListForPost.homePhoneNum,
                    cnicNum = employeeListForPost.cnicNum,
                    address = employeeListForPost.address,
                    referenceName = employeeListForPost.referenceName,
                    referencePhoneNum = employeeListForPost.referencePhoneNum,
                    jobStatus = employeeListForPost.jobStatus,
                    designation = employeeListForPost.designation,
                    employeePic1 = employeeListForPost.employeePic1,
                    employeePic2 = employeeListForPost.employeePic2,
                    employeeCnicFront = employeeListForPost.employeeCnicFront,
                    employeeCnicBsck = employeeListForPost.employeeCnicBsck,
                    recruitmentType = employeeListForPost.recruitmentType,
                    weeklySalary = employeeListForPost.weeklySalary,
                    monthlySalary = employeeListForPost.monthlySalary,
                    chart_id = employeeInChartsAccount.chart_id
                };

                db.employeeList.Add(newEmployeeForPost);
                db.SaveChanges();
                //var message = Request.CreateResponse(HttpStatusCode.Created, employeeListForPost);
                //message.Headers.Location = new Uri(Request.RequestUri + employeeListForPost.employee_Id.ToString());
                //return message

                return Request.CreateResponse(HttpStatusCode.OK, newEmployeeForPost);
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