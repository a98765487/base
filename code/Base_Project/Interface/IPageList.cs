using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Project.Interface
{
    public interface IPageList
    {
        #region 查詢條件
        /// <summary>
        /// 關鍵字
        /// </summary>
        string Keyword { get; set; }
        #region 分頁相關
        /// <summary>
        /// 當前頁數
        /// </summary>
        int PageIndex { get; set; }
        /// <summary>
        /// 每頁筆數
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 總資料筆數
        /// </summary>
        int TotalCount { get; set; }
        #endregion
        #endregion
    }
    public interface IPageListItem
    {
        /// <summary>
        /// 項次
        /// </summary>
        string Num { get; set; }
        /// <summary>
        /// 系統代號
        /// </summary>
        string SId { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        string Mdt { get; set; }
    }
}
