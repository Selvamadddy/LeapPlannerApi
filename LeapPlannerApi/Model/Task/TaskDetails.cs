using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapPlannerApi.Model.Task
{
    public class TaskDetails
    {
        public int Id { get; set; }
        public string TaskText { get; set; }
        public bool IsChecked { get; set; }
    }
}
