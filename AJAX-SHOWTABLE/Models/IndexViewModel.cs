using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AJAX_SHOWTABLE.Models
{
    public class DataEmp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EmpBirthday { get; set; }
        public string Gender { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime BuildDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime LogingDate { get; set; }
        public string JobTitle { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public int Seniority { get; set; }

        
        
        
        
    }
    public class IndexViewModel
    {
        public List<DataEmp> DataEmp;
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }

        public IndexViewModel()
        {
            DataEmp = new List<DataEmp>();
        }
    }
}