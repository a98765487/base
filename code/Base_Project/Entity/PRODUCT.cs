//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Base_Project.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class PRODUCT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODUCT()
        {
            this.ORDERs = new HashSet<ORDER>();
        }
    
        public string SID { get; set; }
        public string CSID { get; set; }
        public string MSID { get; set; }
        public System.DateTime CDT { get; set; }
        public System.DateTime MDT { get; set; }
        public bool ENABLED { get; set; }
        public string NAME { get; set; }
        public string CAT_SID { get; set; }
        public Nullable<int> PRICE { get; set; }
        public string IMG_SRC { get; set; }
        public string DESC { get; set; }
        public string CONTENT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER> ORDERs { get; set; }
    }
}
