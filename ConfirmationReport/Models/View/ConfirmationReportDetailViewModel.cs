using System;
using System.ComponentModel.DataAnnotations;

namespace ConfirmRep.Models.View
{
    public class ConfirmationReportDetailViewModel
    {
        public Int32 Id { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "From Time")]
        public DateTime FromTime { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "To Time")]
        public DateTime ToTime { get; set; }
        [Required]
        public string Description { get; set; }
        public Int32 ReportId { get; set; }
    }
}