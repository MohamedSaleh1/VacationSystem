//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExcelVacationSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vacation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vacation()
        {
            this.requests = new HashSet<request>();
        }
    
        public int vac_ID { get; set; }
        public int num_days { get; set; }
        public System.DateTime fromDate { get; set; }
        public Nullable<System.DateTime> toDate { get; set; }
        public string Reason { get; set; }
        public bool isCasual { get; set; }
        public bool isRegular { get; set; }
        public Nullable<bool> isPermission { get; set; }
        public Nullable<int> hours { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<request> requests { get; set; }
    }
}