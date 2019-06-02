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
        #region 查詢條件
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string Keyword { get; set; }
        #region 分頁相關
        /// <summary>
        /// 當前頁數
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 總資料筆數
        /// </summary>
        public int TotalCount { get; set; } 
        #endregion
        #endregion
    }
    #endregion
}
