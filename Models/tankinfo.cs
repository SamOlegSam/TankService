//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TankService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tankinfo
    {
        public int id { get; set; }
        public System.DateTime dt { get; set; }
        public System.DateTime dtwrite { get; set; }
        public int filialid { get; set; }
        public int tankid { get; set; }
        public Nullable<int> level { get; set; }
        public Nullable<double> t { get; set; }
    }
}
