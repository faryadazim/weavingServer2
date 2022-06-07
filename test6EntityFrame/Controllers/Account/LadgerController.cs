using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace test6EntityFrame.Controllers.Account
{   
    [Authorize]
    public class LadgerController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();




        [Route("api/Ladger")]
        public HttpResponseMessage GetLadgerReport(DateTime dateFrom, DateTime dateTo, int empId)
        {

            var emp_chart_id = (from emp in db.employeeList where emp.employee_Id == empId select emp.chart_id).FirstOrDefault();
            //var openingBalance = //total debit -toal credit 
            var openingBalanceDebit = ((from financeMainTable in db.finance_main
                                        join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                        equals financeEntry.finance_main_id
                                        where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                        && financeEntry.chart_id == emp_chart_id && financeMainTable.voucher_date <= dateFrom
                                        select financeEntry.debit).ToList()).Sum();
            var openingBalanceCredit =((from financeMainTable in db.finance_main
                                 join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                 equals financeEntry.finance_main_id
                                 where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                 && financeEntry.chart_id == emp_chart_id && financeMainTable.voucher_date <= dateFrom
                                 select financeEntry.credit).ToList()).Sum(); 


            var itemToSend = new
            {
                openingBalance = openingBalanceDebit - openingBalanceCredit,
                ladgerData = from financeMainTable in db.finance_main
                             join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                             equals financeEntry.finance_main_id
                             where financeMainTable.finance_main_id == financeEntry.finance_main_id
                             && financeEntry.chart_id == emp_chart_id && financeMainTable.voucher_date >= dateFrom && financeMainTable.voucher_date <= dateTo
                             select new
                             {
                                 vocherInv = financeMainTable.voucher_inv,
                                 date = financeMainTable.voucher_date,
                                 description = financeMainTable.description,
                                 debit = financeEntry.debit,
                                 credit = financeEntry.credit,
                                 id = financeEntry.finance_entries_id
                             },
                     
        };
            return Request.CreateResponse(HttpStatusCode.OK, itemToSend);
        }

    }
}
