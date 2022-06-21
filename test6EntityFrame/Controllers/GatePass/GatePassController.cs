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
using test6EntityFrame.Models.GatePass;

namespace test6EntityFrame.Controllers.GatePass
{
    public class GatePassController : ApiController
    {
        private db_weavingEntities db = new db_weavingEntities();
        [Authorize]
        [Route("api/GetAvailableRoll")]
        public HttpResponseMessage GetAvailableRoll(int borderID , int sizeID)
        {
            var rollAvailable = from ptb in db.production
                                where ptb.borderQuality_id == borderID && ptb.borderSize_id == sizeID
                                 && ptb.status == 0
                                select new
                                {
                                        rollNo=ptb.roll_no,
                                        productionId = ptb.production_id,
                                };
            return Request.CreateResponse(HttpStatusCode.OK, rollAvailable);
        }

        [Authorize]
        [Route("api/GetGatePassRollData")]
        public HttpResponseMessage GetGatePassRollData(int productionID)
        {
            var prductionData = from prTb in db.production
                                where prTb.production_id == productionID
                                select new
                                {
                                  totalPieces=  prTb.total_pieces,
                                  rollWeight=  prTb.roll_weight, 
                                };

            return Request.CreateResponse(HttpStatusCode.OK, prductionData);
        }




        [Authorize]
        [Route("api/GatePassNo")]
        public HttpResponseMessage GetGatePassNo()
        {
            var lastGatePassNo = (from gpm in db.gate_pass_main
                                  orderby gpm.gate_pass_id descending
                                  select gpm.gate_pass_no).FirstOrDefault();

            int actuallGatePassNo;
            if (lastGatePassNo == null)
            {
                actuallGatePassNo = 1;
            }
            else
            {
                string[] numberTo = lastGatePassNo.Split('-');
                actuallGatePassNo = Int32.Parse(numberTo[1]) + 1;
            }
            string GeneratedGatePassNo = "GP-" + actuallGatePassNo.ToString(); 
         return   Request.CreateResponse(HttpStatusCode.OK, GeneratedGatePassNo);
        }

        [Authorize]
        [Route("api/GatePass")]
        public HttpResponseMessage PostGatePass(ClsGatePass gp)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var LogIn = (identity.Claims
                 .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);




            DbContextTransaction transaction = db.Database.BeginTransaction();
            try
            {

                gate_pass_main gpd = new gate_pass_main()
                {


                    gate_pass_no = gp.gate_pass_no,
                    party_name = gp.party_name,
                    party_cell = gp.party_cell,
                    party_address = gp.party_address,
                    total_rolls = gp.total_rolls,
                    total_pieces = gp.total_pieces,
                    total_sharing_weight = gp.total_sharing_weight,
                    color = gp.color,
                    dying_process = gp.dying_process,
                    total_dying_weight = gp.total_dying_weight,
                    remarks = gp.remarks,
                    driver_name = gp.driver_name,
                    vehicle_no = gp.vehicle_no,
                    time = gp.time,
                    created_datetime = StaticValues.PakDateTime,
                    modified_datetime = StaticValues.PakDateTime,
                    created_by = LogIn,
                    modified_by = LogIn,

                };

                db.gate_pass_main.Add(gpd);
                db.SaveChanges();



                int gate_pass_id = gpd.gate_pass_id;

                //creating shift and faults 
                foreach (var item in gp.gatePassEntries)
                {
                    gate_pass_entries gpe = new gate_pass_entries()
                    {
                        borderQuality_id = item.borderQuality_id,
                        borderSize_id = item.borderSize_id,
                        roll_no = item.roll_no,
                        pieces = item.pieces,
                        weight = item.weight,
                        gate_pass_id = gate_pass_id

                    };
                    db.gate_pass_entries.Add(gpe);
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

            return Request.CreateResponse(HttpStatusCode.OK, gp);
        }

    }
}
