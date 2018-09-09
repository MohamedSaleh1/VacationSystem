using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelVacationSystem.Models
{
    public class modelForGrid
    {
        public Nullable<int> hours { get; set; }
        public int nDays { get; set; }
        public System.DateTime fromDate { get; set; }
        public Nullable<System.DateTime> toDate { get; set; }
        public Nullable<bool> type { get; set; }
        public string name { get; set; }
        public int emp_ssn { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> approval { get; set; }

    }
}