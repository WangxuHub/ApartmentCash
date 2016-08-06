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
        public decimal? PayMoney { get; set; }
        
        [Display(Name = "备注")]
        public string PaySummary { get; set; }


        [Display(Name = "时间")]
        public DateTime? PayDate { get; set; }


        [Display(Name = "状态")]
        public string CheckStatus { get; set; }

        [Display(Name = "付款人ID")]
        public string UserID { get; set; }


        [Display(Name = "付款人")]
        public string UserName { get; set; }

        

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