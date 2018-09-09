using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelVacationSystem.Models
{
    public class mappingClassForEmployees
    {
        public int emp_ssn { get; set; }
        public string name { get; set; }
        public string Email { get; set; }
        public Nullable<int> mgr_ssn { get; set; }
        public Nullable<int> Dno { get; set; }
        public int isManager { get; set; }
        public Nullable<decimal> Casual_vacation { get; set; }
        public Nullable<decimal> Regular_vacation { get; set; }
        public decimal Total_Available_days { get; set; }
        public bool Social_insurance { get; set; }
        public Nullable<System.DateTime> Hiring_Date { get; set; }

    }
}