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
    
    public partial class qpass
    {
        public System.Guid id { get; set; }
        public int unitid { get; set; }
        public System.DateTime dt { get; set; }
        public System.DateTime dtwrite { get; set; }
        public string dtsample { get; set; }
        public string number { get; set; }
        public Nullable<int> shift { get; set; }
        public Nullable<double> t { get; set; }
        public Nullable<double> pres { get; set; }
        public Nullable<double> dens { get; set; }
        public Nullable<double> dens20 { get; set; }
        public Nullable<double> dens15 { get; set; }
        public Nullable<double> water { get; set; }
        public Nullable<double> saltmg { get; set; }
        public Nullable<double> saltprc { get; set; }
        public Nullable<double> mechan { get; set; }
        public Nullable<double> sulphur { get; set; }
        public Nullable<double> vaporpres { get; set; }
        public Nullable<double> ballast { get; set; }
        public Nullable<double> fr200 { get; set; }
        public Nullable<double> fr300 { get; set; }
        public Nullable<double> fr350 { get; set; }
        public Nullable<double> paraffin { get; set; }
        public Nullable<double> hydrogen { get; set; }
        public Nullable<double> mercaptan { get; set; }
        public Nullable<double> organic { get; set; }
        public string mercaptanstr { get; set; }
        public string organicstr { get; set; }
        public string designpet { get; set; }
        public string laborant { get; set; }
        public string @operator { get; set; }
    }
}