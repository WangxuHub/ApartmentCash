//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApartmentCash.DBModel
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// AspNetUsers
    /// </summary>
    public partial class AspNetUsers
    {
    
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool EmailConfirmed { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string PasswordHash { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string SecurityStamp { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public bool LockoutEnabled { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public int AccessFailedCount { get; set; }
    
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
    }
}
