using System;
using System.Collections.Generic;

namespace BatchTask.Models
{
    public partial class BatchTasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Parameters { get; set; }
        public int IntervalMinutes { get; set; }
        public bool IsRunning { get; set; }
        public DateTime LastEjecution { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
