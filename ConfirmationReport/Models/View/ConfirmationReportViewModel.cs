using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConfirmRep.Models.Domain;

namespace ConfirmRep.Models.View
{
    public class ConfirmationReportViewModel
    {
        public Int32 Id { get; set; }
        [Required]
        public Int32 ReportNumber { get; set; }
        [Required]
        public DateTime ReportDate { get; set; }
        [Required]
        public string OwnerName { get; set; }
        public string OwnerCompleteName { get; set; }
        public string OtherInterventionOps { get; set; }
        [Required]
        public string CustomerName { get; set; }
        public string CustomerRepresentative { get; set; }
        [Required]
        public InterventionMode InterventionMode { get; set; }
        public string OtherInterventionMode { get; set; }
        public string Notes { get; set; }
        public ReportStatus Status { get; set; }
        public IList<ConfirmationReportDetailViewModel> Details { get; set; }
    }
}