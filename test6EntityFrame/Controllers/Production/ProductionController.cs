using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using test6EntityFrame.Models;
using test6EntityFrame.Models.Production;

namespace test6EntityFrame.Controllers.Production
{
    [Authorize]
    public class ProductionController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();

        //simple api created for WPF firststep jfor role name [fetch on load]
        [Route("api/RoleName")]
        public HttpResponseMessage GetNewRoleName()
        {

            var lastRoleCreated = (from productionTable in db.production
                                   orderby productionTable.production_id descending
                                   select productionTable.roll_no).FirstOrDefault();


            int actuallRoleNo;
            if (lastRoleCreated == null)
            {
                actuallRoleNo = 1;
            }
            else
            {
                string[] numberTo = lastRoleCreated.Split('-');
                actuallRoleNo = Int32.Parse(numberTo[2]) + 1;
            }
            string currentDate = string.Format("{0:yyyy}", DateTime.Now);
            string GeneratedRollNo = "RL-" + currentDate + "-" + actuallRoleNo.ToString();

            return Request.CreateResponse(HttpStatusCode.OK, GeneratedRollNo);
        }


        //production Report history api 

        // productionPost api
        [Route("api/Production")]
        public HttpResponseMessage PostProduction(ClsProduction p)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);




            DbContextTransaction transaction = db.Database.BeginTransaction();
            try
            {

                production pr = new production()
                {
                    roll_no = p.roll_no,
                    production_date = p.production_date,
                    roll_weight = p.roll_weight,
                    loom_id = p.loom_id,
                    borderSize_id = p.borderSize_id,
                    borderQuality_id = p.borderQuality_id,
                    programm_no = p.programm_no,
                    grayProduct_id = p.grayProduct_id,
                    pile_to_pile_length = p.pile_to_pile_length,
                    pile_to_pile_width = p.pile_to_pile_width,
                    cut_piece_size = p.cut_piece_size,
                    cut_piece_wieght = p.cut_piece_weight,
                    remarks = p.remarks,
                    total_border = p.total_border,
                    total_pieces = p.total_pieces,
                    b_grade_pieces = p.b_grade_pieces,
                    a_grade_pieces = p.a_grade_pieces,
                    piece_in_one_border = p.piece_in_one_border, //newly Created
                    current_per_piece_a_weight = p.current_per_piece_a_weight,
                    required_length_p_to_p = p.required_length_p_to_p,
                    required_width_p_to_p = p.required_width_p_to_p,
                    required_per_piece_a_weight = p.required_per_piece_a_weight,
                    cretad_datetime = StaticValues.PakDateTime,
                    modified_datetime = StaticValues.PakDateTime,
                    created_by = LogIn,
                    modified_by = LogIn,

                };

                db.production.Add(pr);
                db.SaveChanges();



                int production_id = pr.production_id;
                finance_main fm = new finance_main()
                {
                    voucher_date = p.production_date,
                    voucher_inv = p.roll_no,
                    voucher_type_id = 10,
                    description = p.remarks,
                    production_id = production_id,
                    created_datetime = StaticValues.PakDateTime,
                    modified_datetime = StaticValues.PakDateTime,
                    created_by = LogIn,
                    modified_by = LogIn,
                };
                db.finance_main.Add(fm);
                db.SaveChanges();
                int finance_main_id = fm.finance_main_id;
                //creating shift and faults 
                foreach (var item in p.shifts)
                {
                    //add new known faults

                    string[] knowFaults = item.known_faults_ids.Split(',');
                    foreach (var fault in knowFaults)
                    {
                        var knownFault = (from fault1 in db.shiftFaults where fault1.fault_title == fault select fault1.fault_id).FirstOrDefault();
                        if (knownFault == 0)
                        {
                            shiftFaults sf = new shiftFaults()
                            {
                                fault_title = fault

                            };
                            db.shiftFaults.Add(sf);
                            db.SaveChanges();

                        }
                    }




                    production_shift sh = new production_shift()
                    {

                        shift_name = item.shift_name,
                        weaver_employee_Id = item.weaver_employee_Id,
                        no_of_border = item.no_of_border,
                        total_pieces = item.total_pieces,
                        b_grade_piece = item.b_grade_piece,
                        a_grade_piece = item.a_grade_piece,
                        rate_per_border = item.rate_per_border,
                        extra_amt = item.extra_amt,
                        extra_desc = item.extra_desc,
                        total_amt = item.total_amt,
                        natting_employee_Id = item.natting_employee_Id,
                        known_faults_ids = item.known_faults_ids,
                        // known_faults_names = item.known_faults_names,
                        production_id = production_id

                    };
                    db.production_shift.Add(sh);
                    db.SaveChanges();

                    //weaver finance entries
                    var emp_chart_id = (from emp in db.employeeList where emp.employee_Id == item.weaver_employee_Id select emp.chart_id).FirstOrDefault();
                    if (emp_chart_id == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Employee chart_id not found");
                    }
                    finance_entries fe = new finance_entries()
                    {
                        chart_id = emp_chart_id,
                        credit = item.total_amt,
                        debit = 0,
                        entry_type = "production",
                        description = item.extra_desc,
                        finance_main_id = finance_main_id,
                    };
                    db.finance_entries.Add(fe);
                    db.SaveChanges();
                }
                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var message = Request.CreateResponse(HttpStatusCode.InternalServerError);

                return message;
            }

            return Request.CreateResponse(HttpStatusCode.OK, p);
        }



        //public HttpResponseMessage PostProduction(ClsProduction p)
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var LogIn = (identity.Claims
        //         .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);




        //    DbContextTransaction transaction = db.Database.BeginTransaction();
        //    try
        //    {

        //        production pr = new production()
        //        {
        //            roll_no = p.roll_no,
        //            production_date = p.production_date,
        //            roll_weight = p.roll_weight,
        //            loom_id = p.loom_id,
        //            borderSize_id = p.borderSize_id,
        //            borderQuality_id = p.borderQuality_id,
        //            programm_no = p.programm_no,
        //            grayProduct_id = p.grayProduct_id,
        //            pile_to_pile_length = p.pile_to_pile_length,
        //            pile_to_pile_width = p.pile_to_pile_width,
        //            cut_piece_size = p.cut_piece_size,
        //            cut_piece_wieght = p.cut_piece_weight,
        //            remarks = p.remarks,
        //            total_border = p.total_border,
        //            total_pieces = p.total_pieces,
        //            b_grade_pieces = p.b_grade_pieces,
        //            a_grade_pieces = p.a_grade_pieces,

        //            current_per_piece_a_weight = p.current_per_piece_a_weight,
        //            required_length_p_to_p = p.required_length_p_to_p,
        //            required_width_p_to_p = p.required_width_p_to_p,
        //            required_per_piece_a_weight = p.required_per_piece_a_weight,
        //            cretad_datetime = StaticValues.PakDateTime,
        //            modified_datetime = StaticValues.PakDateTime,
        //            created_by = LogIn,
        //            modified_by = LogIn,

        //        };

        //        db.production.Add(pr);
        //        db.SaveChanges();
        //        int production_id = pr.production_id;

        //        foreach (var item in p.shifts)
        //        {
        //            production_shift sh = new production_shift()
        //            {

        //                shift_name = item.shift_name,
        //                weaver_employee_Id = item.weaver_employee_Id,
        //                no_of_border = item.no_of_border,
        //                total_pieces = item.total_pieces,
        //                b_grade_piece = item.b_grade_piece,
        //                a_grade_piece = item.a_grade_piece,
        //                rate_per_border = item.rate_per_border,
        //                extra_amt = item.extra_amt,
        //                extra_desc = item.extra_desc,
        //                total_amt = item.total_amt,
        //                natting_employee_Id = item.natting_employee_Id,
        //                known_faults_ids = item.known_faults_ids,
        //                production_id = pr.production_id

        //            };

        //            db.production_shift.Add(sh);
        //            db.SaveChanges();
        //        }

        //        transaction.Commit();
        //        return Request.CreateResponse(HttpStatusCode.OK, pr);



        //    }
        //    catch (Exception ex)
        //    {
        //        transaction.Rollback();
        //        var message = Request.CreateResponse(HttpStatusCode.InternalServerError);

        //        return message;
        //    }

        //}


        [Route("api/UpdateProduction")]
        public HttpResponseMessage PutProductionData(ClsProduction pp, int id)
        {//update prroduction detail and replace shift data

            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);




            DbContextTransaction transaction = db.Database.BeginTransaction();
            try
            {

                var entity = db.production.FirstOrDefault(e => e.production_id == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Row not found");
                }
                else
                {
                    //updating production table 
                    // roll name will be same
                    entity.production_date = pp.production_date;
                    entity.roll_weight = pp.roll_weight;
                    entity.loom_id = pp.loom_id;
                    entity.borderSize_id = pp.borderSize_id;
                    entity.borderQuality_id = pp.borderQuality_id;
                    entity.programm_no = pp.programm_no;
                    entity.grayProduct_id = pp.grayProduct_id;
                    entity.pile_to_pile_length = pp.pile_to_pile_length;
                    entity.pile_to_pile_width = pp.pile_to_pile_width;
                    entity.cut_piece_size = pp.cut_piece_size;
                    entity.cut_piece_wieght = pp.cut_piece_weight;
                    entity.remarks = pp.remarks;
                    entity.total_border = pp.total_border;
                    entity.total_pieces = pp.total_pieces;
                    entity.b_grade_pieces = pp.b_grade_pieces;
                    entity.a_grade_pieces = pp.a_grade_pieces;
                    entity.current_per_piece_a_weight = pp.current_per_piece_a_weight;
                    entity.required_length_p_to_p = pp.required_length_p_to_p;
                    entity.required_width_p_to_p = pp.required_width_p_to_p;
                    entity.required_per_piece_a_weight = pp.required_per_piece_a_weight;
                    entity.piece_in_one_border = pp.piece_in_one_border;
                    entity.modified_by = LogIn;
                    entity.modified_datetime = StaticValues.PakDateTime;
                    db.SaveChanges();



                    //removing/deleting old shiftTable data against selected id
                    var shiftTableAgainstParticularId = from shiftTable in db.production_shift where shiftTable.production_id == id select shiftTable;
                    foreach (var item in shiftTableAgainstParticularId)
                    {
                        db.production_shift.Remove(item);
                        db.SaveChanges();
                    };


                    //adding shiftTable data against selected id


                    var finance_main_entity = db.finance_main.FirstOrDefault(e => e.production_id == id);
                    finance_main_entity.voucher_date = pp.production_date;
                    finance_main_entity.description = pp.remarks;
                    finance_main_entity.modified_datetime = StaticValues.PakDateTime;
                    finance_main_entity.modified_by = LogIn;
                    db.SaveChanges();


                    int finance_main_id = finance_main_entity.finance_main_id;
                    //creating shift and faults 
                    foreach (var item in pp.shifts)
                    {





                        //here add code to create new fault and save them in shift table 






                        production_shift sh = new production_shift()
                        {

                            shift_name = item.shift_name,
                            weaver_employee_Id = item.weaver_employee_Id,
                            no_of_border = item.no_of_border,
                            total_pieces = item.total_pieces,
                            b_grade_piece = item.b_grade_piece,
                            a_grade_piece = item.a_grade_piece,
                            rate_per_border = item.rate_per_border,
                            extra_amt = item.extra_amt,
                            extra_desc = item.extra_desc,
                            total_amt = item.total_amt,
                            natting_employee_Id = item.natting_employee_Id,
                            known_faults_ids = item.known_faults_ids, //here add new generated ids 
                            production_id = id

                        };
                        db.production_shift.Add(sh);
                        db.SaveChanges();















                        //weaver finance entries
                        var emp_chart_id = (from emp in db.employeeList where emp.employee_Id == item.weaver_employee_Id select emp.chart_id).FirstOrDefault();
                        if (emp_chart_id == 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, "Employee chart_id not found");
                        }


                        var financeEntries = db.finance_entries.FirstOrDefault(e => e.chart_id == emp_chart_id);
                        db.finance_entries.Remove(financeEntries);
                        db.SaveChanges();


                        finance_entries fe = new finance_entries()
                        {
                            chart_id = emp_chart_id,
                            credit = item.total_amt,
                            debit = 0,
                            entry_type = "production",
                            description = item.extra_desc,
                            finance_main_id = finance_main_id,
                        };
                        db.finance_entries.Add(fe);
                        db.SaveChanges();
                    }


                }

                transaction.Commit();
                return Request.CreateResponse(HttpStatusCode.OK, "updated successfully");



            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var message = Request.CreateResponse(HttpStatusCode.InternalServerError);

                return message;
            }
        }

        [Route("api/DeleteProduction")]
        public HttpResponseMessage DeleteProductionData(int id)
        {
            production pr = db.production.Find(id);
            if (pr == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Record not Found");
            }

            db.production.Remove(pr);
            //db.SaveChanges();

            var shiftLoop = from productionShiftData in db.production_shift
                            where productionShiftData.production_id == id
                            select productionShiftData;


            foreach (var item in shiftLoop)
            {


                db.production_shift.Remove(item);

            }


            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "deleted successfully");
        }



        //Production Report --------
        [Route("api/ProductionHistory")]
        public HttpResponseMessage GetProductionHistory(DateTime dateFrom, DateTime dateTo)
        {


            var productionReportHistory = from productionTable in db.production
                                          where productionTable.production_date >= dateFrom
    && productionTable.production_date <= dateTo
                                          orderby productionTable.production_id
                                          select new
                                          {
                                              productionId = productionTable.production_id,
                                              rollName = productionTable.roll_no,
                                              productionDate = productionTable.production_date,

                                          };

            if (productionReportHistory == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.OK, productionReportHistory);
            }
        }

        [Route("api/GetProductById")]
        public HttpResponseMessage GetProductionById(int id)
        {

            var joinGroup = from prouctionTable in db.production
                            where prouctionTable.production_id == id
                            select new
                            {

                                production_id = prouctionTable.production_id,
                                roll_no = prouctionTable.roll_no,
                                production_date = prouctionTable.production_date,
                                roll_weight = prouctionTable.roll_weight,
                                loomLabelId = (from loomTable in db.LoomList
                                               where
                                               loomTable.loom_id == prouctionTable.loom_id
                                               select new
                                               {
                                                   label = loomTable.loomNumber,
                                                   value = loomTable.loom_id,
                                                   loomSize = loomTable.loomSize,
                                                   loomJacquard = loomTable.jacquard,
                                                   loomDrawBox = loomTable.drawBox,

                                               }).FirstOrDefault(),

                                piece_in_one_border = prouctionTable.piece_in_one_border,
                                borderSizeLabelId = (from borderSizeTable in db.BorderSize
                                                     where
                                                     borderSizeTable.borderSize_id == prouctionTable.borderSize_id
                                                     select new
                                                     {
                                                         label = borderSizeTable.borderSize1,
                                                         value = borderSizeTable.borderSize_id
                                                     }).FirstOrDefault(),
                                // borderQuality_id = prouctionTable.borderQuality_id,
                                borderQualityLabelId = (from borderQualityTable in db.BorderQuality
                                                        where
                                                        borderQualityTable.borderQuality_id == prouctionTable.borderQuality_id
                                                        select new
                                                        {
                                                            label = borderQualityTable.borderQuality1,
                                                            value = borderQualityTable.borderQuality_id,
                                                        }).FirstOrDefault(),
                                programm_no = prouctionTable.programm_no,
                                grayProduct_id = prouctionTable.grayProduct_id,
                                pile_to_pile_length = prouctionTable.pile_to_pile_length,
                                pile_to_pile_width = prouctionTable.pile_to_pile_width,
                                cut_piece_weight = prouctionTable.cut_piece_wieght,
                                cut_piece_size = prouctionTable.cut_piece_size,
                                remarks = prouctionTable.remarks,
                                total_border = prouctionTable.total_border,
                                total_pieces = prouctionTable.total_pieces,
                                b_grade_pieces = prouctionTable.b_grade_pieces,
                                a_grade_pieces = prouctionTable.a_grade_pieces,
                                current_per_piece_a_weight = prouctionTable.current_per_piece_a_weight,
                                required_length_p_to_p = prouctionTable.required_length_p_to_p,
                                required_width_p_to_p = prouctionTable.required_width_p_to_p,
                                required_per_piece_a_weight = prouctionTable.required_per_piece_a_weight,
                                shiftData = from productionShiftData in db.production_shift
                                            where productionShiftData.production_id == id
                                            select new
                                            {
                                                shift_name = productionShiftData.shift_name,
                                                weaver_EmployeeDNameId = (from employeeListTable in db.employeeList
                                                                          where
                                                                          employeeListTable.employee_Id == productionShiftData.weaver_employee_Id
                                                                          select new
                                                                          {
                                                                              label = employeeListTable.name,
                                                                              value = employeeListTable.employee_Id
                                                                          }).FirstOrDefault(),
                                                no_of_border = productionShiftData.no_of_border,
                                                total_pieces = productionShiftData.total_pieces,
                                                b_grade_piece = productionShiftData.b_grade_piece,
                                                a_grade_piece = productionShiftData.a_grade_piece,
                                                rate_per_border = productionShiftData.rate_per_border,
                                                extra_amt = productionShiftData.extra_amt,
                                                extra_des = productionShiftData.extra_desc,
                                                total_amt = productionShiftData.total_amt,
                                                natting_EmployeeNameId = (from employeeListTable in db.employeeList
                                                                          where
                                                                          employeeListTable.employee_Id == productionShiftData.natting_employee_Id
                                                                          select new
                                                                          {
                                                                              label = employeeListTable.name,
                                                                              value = employeeListTable.employee_Id
                                                                          }).FirstOrDefault(),
                                                known_faults_ids = productionShiftData.known_faults_ids,
                                            }



                            };
            return Request.CreateResponse(HttpStatusCode.OK, joinGroup.FirstOrDefault());
        }
        //Weaver Wise Report --------
        [Route("api/WeaverWiseReport")]
        public HttpResponseMessage GetWeaverWiseReport(int w_id, DateTime dateFrom, DateTime dateTo)
        {
            var dateTime = dateFrom;
            var entity = from productionShiftTable in db.production_shift
                         join productionTable in db.production on
productionShiftTable.production_id equals productionTable.production_id
                         where productionShiftTable.production_id == productionShiftTable.production_id && productionShiftTable.weaver_employee_Id == w_id
                         && productionTable.production_date >= dateFrom && productionTable.production_date <= dateTo
                         select new
                         {



                             productDate = productionTable.production_date,
                             rollNumber = productionTable.roll_no,
                             product = "Unknown",
                             size = (from BorderSizeTable in db.BorderSize where BorderSizeTable.borderSize_id == productionTable.borderSize_id select BorderSizeTable.borderSize1).FirstOrDefault(),
                             border = (from BorderQualityTable in db.BorderQuality where BorderQualityTable.borderQuality_id == productionTable.borderQuality_id select BorderQualityTable.borderQuality1).FirstOrDefault(),

                             bGradePiece = productionTable.b_grade_pieces,
                             aGradePieces = productionTable.a_grade_pieces,
                             ratePerBorder = productionShiftTable.rate_per_border,
                             extraAmount = productionShiftTable.extra_amt,
                             totalAmount = productionShiftTable.total_amt,
                             paidAmount = (from financeEntriesTable in db.finance_entries
                                           join employeeListTable in db.employeeList on
                                       financeEntriesTable.chart_id equals employeeListTable.chart_id
                                           where employeeListTable.chart_id == financeEntriesTable.chart_id
                                           orderby financeEntriesTable.finance_entries_id
                                           select financeEntriesTable.credit).FirstOrDefault(), //it should 4 again 57 but its 16 mean total --also how to calculate them here //for now here im using first value
                                                                                                //here to gwt chart id we use join cant we use any var from this obj



                         };
            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }




    }
}
