using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ConfirmRep.Infrastructure.Common;
using ConfirmRep.Models;
using ConfirmRep.Models.Domain;

namespace ConfirmRep.Repositories
{
    public class ConfirmationReportRepository : IConfirmationReportRepository, IDisposable
    {
        private readonly ConfirmReportContext db;

        public ConfirmationReportRepository(ConfirmReportContext context)
        {
            db = context;
        }

        private static void Update(ConfirmReportContext db, ConfirmationReport report)
        {
            //see: http://www.entityframeworktutorial.net/EntityFramework5/update-entity-graph-using-dbcontext.aspx
            var currentReport = db.Reports.AsNoTracking().FirstOrDefault(r => r.Id.Equals(report.Id));
            db.Reports.Attach(report);
            db.Entry(report).State = EntityState.Modified;
            report.Details.Where(d => d.Id > 0).ToList().ForEach(d => { db.Entry(d).State = EntityState.Modified; });
            report.Details.Where(d => d.Id.Equals(0)).ToList().ForEach(d => { db.Entry(d).State = EntityState.Added; });
            currentReport.Details.Where(d => !report.Details.Any(nr => nr.Id.Equals(d.Id))).ToList().ForEach(d => {
                var newD = new ConfirmationReportDetail { Id = d.Id };
                db.ReportDetails.Attach(newD);
                db.ReportDetails.Remove(newD); 
            });
        }

        public async Task SaveDraft(ConfirmationReport report)
        {
            Contract.Requires<ArgumentNullException>(report != null, "report");

            report.Status = ReportStatus.Draft;
            if (report.Id > 0)
            {
                Update(db, report);
            }
            else
            {
                db.Reports.Add(report);
            }
            await db.SaveChangesAsync();
        }

        public async Task Save(ConfirmationReport report)
        {
            Contract.Requires<ArgumentNullException>(report != null, "report");

            report.Status = ReportStatus.Completed;
            if (report.Id > 0)
            {
                Update(db, report);
            }
            else
            {
                db.Reports.Add(report);
            }
            await db.SaveChangesAsync();
        }

        public async Task<ConfirmationReport> FindByNumber(int reportNumber)
        {
            Contract.Requires<ArgumentException>(reportNumber > 0, "reportNumber");
            return await db.Reports.FindAsync(reportNumber);
        }

        public IQueryable<ConfirmationReport> FindAllByOwner(string ownerName, ReportStatus? status)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(ownerName), "ownerName");
            var reports = db.Reports.Where(r => r.OwnerName.Equals(ownerName));
            if (status != null)
                reports = reports.Where(r => r.Status == status.Value);
            return reports;
        }

        public IQueryable<ConfirmationReport> Reports
        {
            get { return db.Reports; }
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}