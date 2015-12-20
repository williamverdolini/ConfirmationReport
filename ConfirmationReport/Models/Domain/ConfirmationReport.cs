using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConfirmRep.Infrastructure.Models;

namespace ConfirmRep.Models.Domain
{
    public enum ReportStatus
    {
        Draft = 0,
        Completed = 1
    }

    public enum InterventionMode
    {
        OnSite = 0,
        FromOffice = 1,
        ByPhone = 2,
        Other = 3
    }

    public class ConfirmationReport : IAuditable
    {
        public ConfirmationReport()
        {
            this.Details = new List<ConfirmationReportDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        [Required]
        public Int32 ReportNumber { get; set; }
        [Required]
        public DateTime ReportDate { get; set; }
        [Required]
        public string OwnerName { get; set; }
        public string OtherInterventionOps { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string CustomerRepresentative { get; set; }
        [Required]
        public InterventionMode InterventionMode { get; set; }
        public string OtherInterventionMode { get; set; }
        public string Notes { get; set; }
        public ReportStatus Status { get; set; }
        public virtual ICollection<ConfirmationReportDetail> Details { get; protected set; }
        
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}