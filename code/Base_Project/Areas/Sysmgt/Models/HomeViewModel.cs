using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Base_Project.Models.Home
{
    #region IndexViewModel
    public class IndexViewModel : BaseViewModel
    {
    }
    #endregion
    #region LoginViewModel
    public class LoginViewModel : BaseViewModel
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required(ErrorMessage = "請輸入帳號")]
        public string Acct { set; get; }
        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "請輸入密碼")]
        public string Pwd { set; get; }
        /// <summary>
        /// 驗證碼
        /// </summary>
        [Required(ErrorMessage = "請輸入驗證碼")]
        public string ValidateCode { set; get; }
    }
    #endregion
}
