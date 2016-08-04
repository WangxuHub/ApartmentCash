using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ApartmentCash.Models
{
    public class CashViewModel
    {
        [Required]
        [Display(Name = "金额")]
        public decimal PayMoney { get; set; }
        
        [Display(Name = "备注")]
        public string PaySummary { get; set; }
        
    }


    public enum eCheckStatus {
        /// <summary>
        /// 申请
        /// </summary>
        apply,

        /// <summary>
        /// 通过
        /// </summary>
        adopt,

        /// <summary>
        /// 拒绝
        /// </summary>
        reject,

    }
}