using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringTask.Models.ViewModel
{
    public class TaskViewModel
    {
        public System.Guid Id { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public string Priority { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateNotif { get; set; }
        public string PIC { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        
    }

    public class TaskViewModelObj
    {
        public List<TaskViewModel> Tasks { get; set; }
        public string FilterName { get; set; }
        public string SortBy { get; set; }
    }


}