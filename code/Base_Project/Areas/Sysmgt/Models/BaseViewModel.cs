using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Base_Project.Models
{
    #region BaseViewModel
    public class BaseViewModel
    {
        /// <summary>
        /// 系統使用者代號
        /// </summary>
        public string ActorSId { set; get; }
    }
    #endregion
}
