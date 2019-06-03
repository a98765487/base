using Base_Project.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Base_Project.Models.Product
{
    #region IndexViewModel
    public class IndexViewModel : PageListViewModel
    {
        public List<PageListItem> Items { get; set; } = new List<PageListItem>();
    }
    public class PageListItem : IPageListItem
    {
        /// <summary>
        /// 項次
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 系統代號
        /// </summary>
        public string SId { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>
        public string Mdt { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
    }
    #endregion

    #region EditViewModel
    public class EditViewModel : BaseViewModel {
    #region 表單欄位
    [Display(Name = "系統代號")]
    public string SID { get; set; }
    [Display(Name = "創建人")]
    public string CSID { get; set; }
    [Display(Name = "修改人")]
    public string MSID { get; set; }
    [Display(Name = "創建日期"), Required]
    public System.DateTime CDT { get; set; }
    [Display(Name = "修改日期"), Required]
    public System.DateTime MDT { get; set; }
    [Display(Name = "啟用"), Required]
    public virtual bool ENABLED { get; set; }
    [Display(Name = "姓名"), Required, MaxLength(100)]
    public string NAME { get; set; }
    [Display(Name = "分類")]
    public string CAT_SID { get; set; }
    [Display(Name = "價格") , Range(0,int.MaxValue)]
    public Nullable<int> PRICE { get; set; }
    [Display(Name = "列表頁圖片")]
    public string IMG_SRC { get; set; }
    [Display(Name = "簡述")]
    public string DESC { get; set; }
    [Display(Name = "內文")]
    public string CONTENT { get; set; }
        #endregion
    }
    #endregion
}
