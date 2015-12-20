using System.Collections.Generic;
using System.Threading.Tasks;
using ConfirmRep.Models.Domain;
using ConfirmRep.Models.View;

namespace ConfirmRep.ServiceWorkers
{
    public interface IConfirmationReportWorker
    {
        // Commands
        Task<ConfirmationReportViewModel> Save(ConfirmationReportViewModel report);
        Task<ConfirmationReportViewModel> SaveDraft(ConfirmationReportViewModel report);
        // Queries
        Task<ConfirmationReportViewModel> FindByNumber(int reportNumber);
        Task<ConfirmationReportViewModel> FindById(int id);
        Task<List<ConfirmationReportViewModel>> FindAllByOwner(string ownerName, ReportStatus? status);
        Task<int> FindNewReportNumber();
    }
}
