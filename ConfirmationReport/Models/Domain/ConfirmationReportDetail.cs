using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConfirmRep.Models.Domain
{
    public class ConfirmationReportDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime FromTime { get; set; }
        [Required]
        public DateTime ToTime { get; set; }
        [Required]
        public string Description { get; set; }
        public Int32 ReportId { get; set; }
        public virtual ConfirmationReport Report { get; set; }
    }
}