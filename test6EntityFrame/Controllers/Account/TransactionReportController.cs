using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace test6EntityFrame.Controllers.Account
{
    public class TransactionReportController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();


        //Get Avvount type :
        [Authorize]
        [Route("api/accountType")]
        public HttpResponseMessage GetAccountType()
        { var accountType = from cofTb in db.chart_of_accounts select new
        {
            accountType = cofTb.account_name,
            accountCode = cofTb.account_code,
            chartId = cofTb.chart_id
            };

            return Request.CreateResponse(HttpStatusCode.OK, accountType); 

        }

      [Authorize]
        [Route("api/TransactionReport")]
        public HttpResponseMessage GetTransactionReport(DateTime dateFrom, DateTime dateTo, int emp_chart_id)
        {
             
            var openingBalanceDebit = ((from financeMainTable in db.finance_main
                                        join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                        equals financeEntry.finance_main_id
                                        where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                        && financeEntry.chart_id == emp_chart_id && financeMainTable.voucher_date < dateFrom
                                        select financeEntry.debit).ToList()).Sum();
            var openingBalanceCredit = ((from financeMainTable in db.finance_main
                                         join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                         equals financeEntry.finance_main_id
                                         where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                         && financeEntry.chart_id == emp_chart_id && financeMainTable.voucher_date < dateFrom
                                         select financeEntry.credit).ToList()).Sum();




            var closingBalanceDebit = ((from financeMainTable in db.finance_main
                                        join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                        equals financeEntry.finance_main_id
                                        where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                        && financeEntry.chart_id == emp_chart_id && financeMainTable.voucher_date <= dateTo
                                        select financeEntry.debit).ToList()).Sum();
            var closingBalanceCredit = ((from financeMainTable in db.finance_main
                                         join financeEntry in db.finance_entries on financeMainTable.finance_main_id
                                         equals financeEntry.finance_main_id
                                         where financeMainTable.finance_main_id == financeEntry.finance_main_id
                                         && financeEntry.chart_id == emp_chart_id && financeMainTable.voucher_date <= dateTo
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
                                 accountName= (from cofAccTb in db.chart_of_accounts where cofAccTb.chart_id== emp_chart_id select cofAccTb.account_name).FirstOrDefault(),
                                 description = financeMainTable.description,
                                 debit = financeEntry.debit,
                                 credit = financeEntry.credit,
                                 id = financeEntry.finance_entries_id
                             },
                closingBalance = closingBalanceDebit - closingBalanceCredit,


            };
            return Request.CreateResponse(HttpStatusCode.OK, itemToSend);
        }


    }

}
