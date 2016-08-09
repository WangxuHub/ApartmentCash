using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ApartmentCash.Models
{
    public class DateWashViewModel
    {
        public string DateStr;

        public string UserID;
        public string UserName;
        public string IsFinish;
    }

    public class CreateDateModel
    {

        [Required]
        [Display(Name = "开始时间"),DataType(DataType.Date)]
        public DateTime DateStart { get; set; }

        [Required]
        [Display(Name = "结束时间"),DataType(DataType.Date)]
        public DateTime DateEnd { get; set; }


    }
}