using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MonitoringTask.Models.DB
{
    [MetadataType(typeof(TaskMetadata))] // Associate with a metadata class
    public partial class Task
    {
    }

    public class TaskMetadata
    {
        [Required(ErrorMessage = "The Due Date field is required.")]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "The Priority field is required.")]
        public string Priority { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        [MaxLength(1000, ErrorMessage = "The Description field must be less than 200 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The PIC (Person in Charge) field is required.")]
        public string PIC { get; set; }
    }
}