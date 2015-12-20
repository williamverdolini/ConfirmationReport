using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;
using ConfirmRep.ServiceWorkers;

namespace ConfirmRep.Controllers
{
    [RoutePrefix("api/Reports")]
    public class ConfirmationReportController : ApiController
    {
        private readonly IConfirmationReportWorker worker;

        public ConfirmationReportController(IConfirmationReportWorker worker)
        {
            Contract.Requires<ArgumentNullException>(worker != null, "worker");
            this.worker = worker;
        }

        [Route("SaveDraft")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> SaveDraft(ConfirmationReportViewModel report)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isNew = report.Id == 0;

            var returnedReport = await worker.SaveDraft(report);

            if (isNew)
                return Created(new Uri(Url.Link("FindById", new { id = report.Id })), report);
            else
                return Ok(report);
        }

        [Route("Save")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> Save(ConfirmationReportViewModel report)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isNew = report.Id == 0;

            var returnedReport = await worker.Save(report);

            if (isNew)
                return Created(new Uri(Url.Link("FindById", new { id = report.Id })), report);
            else
                return Ok(report);
        }

        [Route("{reportNumber:int}", Name = "FindByNumber")]
        [Authorize]
        [HttpGet]
        [ResponseType(typeof(ConfirmationReportViewModel))]
        public async Task<IHttpActionResult> FindByNumber(int reportNumber)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ConfirmationReportViewModel report = await worker.FindByNumber(reportNumber);
            if (report != null)
                return Ok(report);
            else
                return NotFound();
        }

        [Route("id/{id:int}", Name = "FindById")]
        [HttpGet]
        [ResponseType(typeof(ConfirmationReportViewModel))]
        public async Task<IHttpActionResult> FindById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ConfirmationReportViewModel report = await worker.FindById(id);

            return Ok(report);
        }

        [Route("FindAllByOwner")]
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(List<ConfirmationReportViewModel>))]
        public async Task<IHttpActionResult> FindAllByOwner(string ownerName, ReportStatus? status = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reports = await worker.FindAllByOwner(ownerName, status);

            return Ok(reports);
        }

        [Route("FindNewReportNumber")]
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(Int32))]
        public async Task<IHttpActionResult> FindNewReportNumber()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await worker.FindNewReportNumber());
        }
    }
}
