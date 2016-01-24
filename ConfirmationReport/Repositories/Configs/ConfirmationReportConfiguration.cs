using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ConfirmRep.Repositories.Configs
{
    public class ConfirmationReportConfiguration : EntityTypeConfiguration<ConfirmRep.Models.Domain.ConfirmationReport>
    {
        public ConfirmationReportConfiguration()
        {
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(a => a.ReportNumber).IsRequired();
            Property(a => a.ReportDate).IsRequired();
            Property(a => a.OwnerName).IsRequired();
            Property(a => a.CustomerName).IsRequired();
            Property(a => a.InterventionMode).IsRequired();
            Property(a => a.InterventionMode).IsRequired();
            
            HasKey(a => a.Id);
        }
    }
}