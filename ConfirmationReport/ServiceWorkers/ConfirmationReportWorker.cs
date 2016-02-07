using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;
using ConfirmRep.Repositories;

namespace ConfirmRep.ServiceWorkers
{
    public class ConfirmationReportWorker : IConfirmationReportWorker
    {
        private readonly IConfirmationReportRepository repo; 
        private readonly IMapper mapper;

        public ConfirmationReportWorker(IConfirmationReportRepository repo, IMapper mapper)
        {
            Contract.Requires<ArgumentNullException>(repo != null, "IConfirmationReportRepository");
            Contract.Requires<ArgumentNullException>(mapper != null, "mapper");
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ConfirmationReportViewModel> SaveDraft(ConfirmationReportViewModel report)
        {
            var domainReport = mapper.Map<ConfirmationReport>(report);
            await repo.SaveDraft(domainReport);
            var returnedReport = mapper.Map<ConfirmationReportViewModel>(domainReport);
            return returnedReport;
        }

        public async Task<ConfirmationReportViewModel> Save(ConfirmationReportViewModel report)
        {
            var domainReport = mapper.Map<ConfirmationReport>(report);
            await repo.Save(domainReport);
            var returnedReport = mapper.Map<ConfirmationReportViewModel>(domainReport);
            return returnedReport;
        }

        public async Task<ConfirmationReportViewModel> FindByNumber(int reportNumber)
        {
            ConfirmationReport report = await repo.FindByNumber(reportNumber);
            return mapper.Map<ConfirmationReportViewModel>(report);
        }

        public async Task<ConfirmationReportViewModel> FindById(int id)
        {
            ConfirmationReport report = await repo.FindById(id);
            return mapper.Map<ConfirmationReportViewModel>(report);
        }

        public async Task<List<ConfirmationReportViewModel>> FindAllByOwner(string ownerName, ReportStatus? status)
        {
            List<ConfirmationReport> reports = await repo.FindAllByOwner(ownerName, status).ToListAsync();
            return mapper.Map<List<ConfirmationReportViewModel>>(reports);
        }

        public async Task<int> FindNewReportNumber()
        {
            return await repo.FindNewReportNumber();
        }
    }
}