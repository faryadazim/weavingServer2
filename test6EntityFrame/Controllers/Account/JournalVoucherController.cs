using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using test6EntityFrame.Models;
using test6EntityFrame.Models.Account;

namespace test6EntityFrame.Controllers.Account
{
    public class JournalVoucherController : ApiController
    {

        private db_weavingEntities db = new db_weavingEntities();
        [Authorize]
        [Route("api/LadgerBalance")]
        public HttpResponseMessage GetLadgerBalance(int weaverId)
        {

            var emp_chart_id = (from emp in db.employeeList where emp.employee_Id == weaverId select emp.chart_id).FirstOrDefault();

            var lastBalanceDebit = ((from financeMainTable in db.finance_main
                                     join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                     equals financeEntry.finance_main_id
                                     where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                     && financeEntry.chart_id == emp_chart_id
                                     select financeEntry.debit).ToList()).Sum();
            var lastBalanceCredit = ((from financeMainTable in db.finance_main
                                      join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                      equals financeEntry.finance_main_id
                                      where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                      && financeEntry.chart_id == emp_chart_id
                                      select financeEntry.credit).ToList()).Sum();

            var ladgerBalance = lastBalanceDebit - lastBalanceCredit;

            var inv = (from fm in db.finance_main
                       join fe in db.finance_entries on fm.finance_main_id equals fe.finance_main_id
                       where fe.entry_type == "production"
                       orderby fm.finance_main_id descending
                       select fm.voucher_inv).FirstOrDefault();
            var joinEntity = new
            {
                ladgerBalance = ladgerBalance,
                voucherNumber = inv

            };
            return Request.CreateResponse(HttpStatusCode.OK, joinEntity);

        }

        [Authorize]
        [Route("api/VoucherName")]
        public HttpResponseMessage GetVoucherName()
        {


            var inv = (from fm in db.finance_main
                       join fe in db.finance_entries on fm.finance_main_id equals fe.finance_main_id
                       where fe.entry_type == "jv"
                       orderby fm.finance_main_id descending
                       select fm.voucher_inv).FirstOrDefault();
  
            int actuallVoucherNo;
            if (inv == null)
            {
                actuallVoucherNo = 1;
            }
            else
            {
                string[] numberTo = inv.Split('-');
                actuallVoucherNo = Int32.Parse(numberTo[2]) + 1;
            }
            string currentDate = string.Format("{0:yyyy}", DateTime.Now);
            string GeneratedRollNo = "JV-" + currentDate + "-" + actuallVoucherNo.ToString();

            return Request.CreateResponse(HttpStatusCode.OK, GeneratedRollNo);
        }













        [Authorize]
        [Route("api/PostJV")]
        public HttpResponseMessage PostJV(int weaverId, ClsJV data)
        {

            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

             

            finance_main financeNew = new finance_main()
            {
                voucher_date = data.date,
                voucher_inv = data.voucherInv,
                voucher_type_id = 10,
                description = data.description,
                production_id = 0, // its only for production
                created_datetime = StaticValues.PakDateTime,
                modified_datetime = StaticValues.PakDateTime,
                created_by = LogIn,
                modified_by = LogIn,
            };
            db.finance_main.Add(financeNew);
            db.SaveChanges();



            var employeeChartId = (from emp in db.employeeList where emp.employee_Id == weaverId select emp.chart_id).FirstOrDefault();
            if (employeeChartId == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Employee chart Id not Exist");
            }
            finance_entries financeEntry = new finance_entries()
            {
                chart_id = employeeChartId,
                credit = data.credit,
                debit = data.debit,
                description = data.description,
                entry_type ="jv",
                finance_main_id = financeNew.finance_main_id,
            };
            db.finance_entries.Add(financeEntry);
            db.SaveChanges();
            var addedBody = new {
                financeNew,
                financeEntry ,
                createdBy = from ut in db.AspNetUsers where ut.Id == financeNew.created_by select ut.UserName,
            };

            return Request.CreateResponse(HttpStatusCode.OK, addedBody);
        }

        [Authorize]
        [Route("api/JVReportHistory")]
        public HttpResponseMessage GetJVReportHistory( DateTime dateFrom, DateTime dateTo)
        {




            
            var entity = from fm in db.finance_main
                         join fe in db.finance_entries on fm.finance_main_id equals fe.finance_main_id
                         where  fm.production_id == 0 && fm.voucher_date >= dateFrom && fm.voucher_date <= dateTo
                         select new
                         {
                             Voucherinv = fm.voucher_inv,
                             date = fm.voucher_date,
                             jvId = fm.finance_main_id,
                             weaverId= (from empT in db.employeeList where empT.chart_id==fe.chart_id select empT.employee_Id).FirstOrDefault(),
                             debit = fe.debit,
                             credit = fe.credit // it will only return value which one is not zero bcuz 2nd value have 0 will not effect it


                         };


            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        [Authorize]
        [Route("api/JVReport")]
        public HttpResponseMessage GetJVReport(int weaverId, int financeMainId)
        {




            var employeeChartId = (from emp in db.employeeList where emp.employee_Id == weaverId select emp.chart_id).FirstOrDefault();

            var entity = from fm in db.finance_main
                         join fe in db.finance_entries on fm.finance_main_id equals fe.finance_main_id
                         where fe.chart_id == employeeChartId && fm.production_id == 0 && fm.finance_main_id == financeMainId
                         select new
                         {
                             weaverName = (from emp in db.employeeList where emp.employee_Id == weaverId select emp.name).FirstOrDefault(),
                             date = fm.voucher_date,
                             Voucherinv = fm.voucher_inv,
                             description = fm.description,
                             credit = fe.credit,
                             debit = fe.debit,
                             
                             cretedBy = (from loginUser in db.AspNetUsers
                                         join fm in db.finance_main on loginUser.Id equals fm.created_by
                                         where
                                        loginUser.Id == fm.created_by
                                         select loginUser.UserName).FirstOrDefault(),
                         };


            return Request.CreateResponse(HttpStatusCode.OK, entity.FirstOrDefault());
        }

        [Authorize]
        [Route("api/PutJVReport")]
        public HttpResponseMessage PutJVReport(int financeMainId,int weaverId ,  ClsJV data)
        {

            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);




            var fmEntity = db.finance_main.FirstOrDefault(e => e.finance_main_id == financeMainId);
            if (fmEntity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Row not found");
            }
            else
            {
                fmEntity.description = data.description; 
                fmEntity.modified_by = LogIn;
                fmEntity.modified_datetime = StaticValues.PakDateTime;
                fmEntity.voucher_date = data.date; 
                db.SaveChanges();



                var employeeChartId = (from emp in db.employeeList where emp.employee_Id == weaverId select emp.chart_id).FirstOrDefault();
                if (employeeChartId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Employee chart Id not Exist");
                }

                var feEntity = db.finance_entries.FirstOrDefault(e => e.finance_main_id == financeMainId);
                db.finance_entries.Remove(feEntity);
                finance_entries financeEntry = new finance_entries()
                {
                    chart_id = employeeChartId,
                    credit = data.credit,
                    debit = data.debit,
                    description = data.description,
                    entry_type = "jv",
                    finance_main_id = financeMainId,
                };
                db.finance_entries.Add(financeEntry);
                db.SaveChanges();


            }
                return Request.CreateResponse(HttpStatusCode.OK, "updated");
        }

    }
}
