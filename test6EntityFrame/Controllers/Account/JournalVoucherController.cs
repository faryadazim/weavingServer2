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
        [Route("api/PostJV")]
        public HttpResponseMessage PostJV(int weaverId, ClsJV data)
        {

            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);



            var inv = (from fm in db.finance_main
                       join fe in db.finance_entries on fm.finance_main_id equals fe.finance_main_id
                       where fe.entry_type == "production"
                       orderby fm.finance_main_id descending
                       select fm.voucher_inv).FirstOrDefault();

            finance_main financeNew = new finance_main()
            {
                voucher_date = data.date,
                voucher_inv = inv,
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
                entry_type = data.description,
                finance_main_id = financeNew.finance_main_id,
            };
            db.finance_entries.Add(financeEntry);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Added Successfully");
        }

        [Authorize]
        [Route("api/JVReportHistory")]
        public HttpResponseMessage GetJVReportHistory(int weaverId, DateTime dateFrom, DateTime dateTo)
        {




            var employeeChartId = (from emp in db.employeeList where emp.employee_Id == weaverId select emp.chart_id).FirstOrDefault();

            var entity = from fm in db.finance_main
                         join fe in db.finance_entries on fm.finance_main_id equals fe.finance_main_id
                         where fe.chart_id == employeeChartId && fm.production_id == 0 && fm.voucher_date >= dateFrom && fm.voucher_date <= dateTo
                         select new
                         {
                             Voucherinv = fm.voucher_inv,
                             date = fm.voucher_date,
                             jvId = fm.finance_main_id,


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
                             devit = fe.debit,
                             entryType = fe.entry_type,
                             cretedBy = (from loginUser in db.AspNetUsers
                                         join fm in db.finance_main on loginUser.Id equals fm.created_by
                                         where
                                        loginUser.Id == fm.created_by
                                         select loginUser.UserName).FirstOrDefault(),
                         };


            return Request.CreateResponse(HttpStatusCode.OK, entity.FirstOrDefault());
        }

        [Authorize]
        public HttpRequestMessage PutJVReport (int financeMainId)

    }
}
