using System;
using System.Linq;
using System.Threading.Tasks;
using ConfirmRep.Models.Domain;

namespace ConfirmRep.Repositories
{
    public interface IConfirmationReportRepository
    {
        Task SaveDraft(ConfirmationReport report);
        Task Save(ConfirmationReport report);
        // IQueryable: Async behaviours delegated to the worker in order to delay the query execution at the last moment
        Task<ConfirmationReport> FindByNumber(Int32 reportNumber);
        // TODO: move into the worker
        IQueryable<ConfirmationReport> FindAllByOwner(string ownerName, ReportStatus? status);
        IQueryable<ConfirmationReport> Reports { get; }
    }
}
