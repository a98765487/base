using Base_Project.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Base_Project.Models.Member
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
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }
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
    [Display(Name = "信箱"), Required, EmailAddress(ErrorMessage = "請輸入正確的信箱格式"), MaxLength(100)]
    public string EMAIL { get; set; }
    [Display(Name = "密碼"), RegularExpression("^\\w{6,16}$", ErrorMessage = "只允許6-16位英文字母、數字或者下底線")]
    public string PWD { get; set; }
    [Display(Name = "HashKey")]
    public string HASHKEY { get; set; }
    [Display(Name = "姓名"), Required, MaxLength(100)]
    public string NAME { get; set; }
    [Display(Name = "FB_ID")]
    public string FBID { get; set; }
    [Display(Name = "Google_ID")]
    public string GOOGLEID { get; set; }
    [Display(Name = "信箱驗證"), Required]
    public bool VERIFY { get; set; }
    #endregion
    }
    #endregion
}
