using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using ConfirmRep.Infrastructure.Models;
using ConfirmRep.Models.Domain;
using ConfirmRep.Repositories.Migrations;

namespace ConfirmRep.Repositories
{
    public class ConfirmReportContext : DbContext
    {
        public ConfirmReportContext() : base("ConfirmReport") {
            var octx = (this as IObjectContextAdapter).ObjectContext;
            octx.SavingChanges += (s, e) => this.OnSavingChanges(e);
        }

        public virtual DbSet<ConfirmationReport> Reports { get; set; }
        public virtual DbSet<ConfirmationReportDetail> ReportDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ConfirmReportContext, ConfirmReportConfiguration>());
        }

        protected virtual void OnSavingChanges(EventArgs e)
        {
            foreach (var auditable in this.ChangeTracker.Entries().Where(en => en.State.Equals(EntityState.Added)).Select(x => x.Entity).OfType<IAuditable>())
            {
                auditable.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                auditable.CreatedAt = DateTime.Now;
            }
            foreach (var auditable in this.ChangeTracker.Entries().Where(en => en.State.Equals(EntityState.Modified)).Select(x => x.Entity).OfType<IAuditable>())
            {
                this.Entry(auditable).Property(a => a.CreatedAt).IsModified = false;
                this.Entry(auditable).Property(a => a.CreatedBy).IsModified = false;
                auditable.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;
                auditable.UpdatedAt = DateTime.Now;
            }
        }


    }
}