using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace test6EntityFrame.Controllers.GatePass
{
    public class GatePassReportController : ApiController
    {

        private db_weavingEntities db = new db_weavingEntities();


        [Authorize]
        [Route("api/GatePassHistory")]
        public HttpResponseMessage GetGatePassReportHistory(DateTime dateFrom, DateTime dateTo)
        {

            var entity = from gpm in db.gate_pass_main
                         where gpm.created_datetime >= dateFrom && gpm.created_datetime <= dateTo
                         select new
                         {
                             Date = gpm.created_datetime,
                             gatePassNum = gpm.gate_pass_no,
                             gatePassID = gpm.gate_pass_id,
                             time = gpm.time
                         };



            return Request.CreateResponse(HttpStatusCode.OK, entity);
        }

        [Authorize]
        [Route("api/GatePassReport")]
        public HttpResponseMessage GetGatePassReport(int gpID)
        {

            var entity = from gpm in db.gate_pass_main
                         join prTb in db.partyList on gpm.party_id equals prTb.party_id
                         where gpm.gate_pass_id == gpID
                         select new
                         {
                             gatePassID=gpm.gate_pass_id,
                             poNo = gpm.gate_pass_no,
                             partyId = gpm.party_id,
                             partyName = prTb.party_name,
                             partyAddress = prTb.party_address,
                             partyCell = prTb.party_cell,
                             totalRolls = gpm.total_rolls,
                             totalPieces = gpm.total_pieces,
                             entires = from gpe in db.gate_pass_entries
                                       where gpe.gate_pass_id == gpID
                                       select new
                                       {
                                           border = (from brTb in db.BorderQuality where brTb.borderQuality_id == gpe.borderQuality_id select brTb.borderQuality1).FirstOrDefault(),
                                           size = (from bsTb in db.BorderSize where bsTb.borderSize_id == gpe.borderSize_id select bsTb.borderSize1).FirstOrDefault(),
                                           borderSize_id = (from bsTb in db.BorderSize where bsTb.borderSize_id == gpe.borderSize_id select bsTb.borderSize_id).FirstOrDefault(),
                                           borderQuality_id = (from brTb in db.BorderQuality where brTb.borderQuality_id == gpe.borderQuality_id select brTb.borderQuality_id).FirstOrDefault(),
                                           pieces = gpe.pieces,
                                           weight = gpe.weight,
                                           rollNo = new { label = (from prodTb in db.production where prodTb.production_id == gpe.roll_no select prodTb.roll_no).FirstOrDefault(), value = (from prodTb in db.production where prodTb.production_id == gpe.roll_no select prodTb.production_id).FirstOrDefault() },
                                       },
                             totalRoll = gpm.total_rolls,
                             totalWeight = gpm.total_weight,
                             totalSharingWeight = gpm.total_sharing_weight,
                             color =   (from colorTb in db.ColorConfig where colorTb.color_id == gpm.color select colorTb.color_name).FirstOrDefault(),
                             dyingProceess = gpm.dying_process,
                             dyingWeight = gpm.total_dying_weight,
                             remarks = gpm.remarks,
                             driverName = gpm.driver_name,
                             vehicleNumb = gpm.vehicle_no,
                             time = gpm.time,
                             createdTime = gpm.created_datetime,
                             createdBy = gpm.created_by,
                         };



            return Request.CreateResponse(HttpStatusCode.OK, entity.FirstOrDefault());
        }
    }
}
