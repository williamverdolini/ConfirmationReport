using System;
using System.Linq;
using System.Threading.Tasks;
using ConfirmRep.Models.Domain;

namespace ConfirmRep.Repositories
{
    public interface IConfirmationReportRepository
    {
        // Commands
        Task SaveDraft(ConfirmationReport report);
        Task Save(ConfirmationReport report);
        // Queries
        Task<int> FindNewReportNumber();
        Task<ConfirmationReport> FindByNumber(Int32 reportNumber);
        Task<ConfirmationReport> FindById(Int32 reportId);
        // IQueryable: Async behaviours delegated to the worker in order to delay the query execution at the last moment
        IQueryable<ConfirmationReport> FindAllByOwner(string ownerName, ReportStatus? status);
    }
}
