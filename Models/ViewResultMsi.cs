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
    
    public partial class ViewResultMsi
    {
        public System.Guid id { get; set; }
        public Nullable<System.DateTime> dt { get; set; }
        public Nullable<int> filialid { get; set; }
        public bool iscompare { get; set; }
        public string samplelocationstr { get; set; }
        public int samplenum { get; set; }
        public System.DateTime sampledt { get; set; }
        public Nullable<System.DateTime> endsampledt { get; set; }
        public int samplesourceid { get; set; }
        public int samplelocationid { get; set; }
        public string laborant { get; set; }
        public int laborantid { get; set; }
        public Nullable<System.DateTime> startdt { get; set; }
        public Nullable<System.DateTime> enddt { get; set; }
        public Nullable<double> tempareom { get; set; }
        public Nullable<double> tempreal { get; set; }
        public Nullable<double> densareom { get; set; }
        public Nullable<double> densreal { get; set; }
        public Nullable<double> dens20 { get; set; }
        public Nullable<double> dens15 { get; set; }
        public Nullable<double> water { get; set; }
        public Nullable<double> saltmg { get; set; }
        public Nullable<double> mechan { get; set; }
        public Nullable<double> sulphur { get; set; }
        public Nullable<double> vaporpres { get; set; }
        public Nullable<double> fr200 { get; set; }
        public Nullable<double> fr300 { get; set; }
        public Nullable<double> fr350 { get; set; }
        public Nullable<double> paraffin { get; set; }
        public Nullable<double> hydrogen { get; set; }
        public Nullable<double> mercaptan { get; set; }
        public Nullable<double> organic { get; set; }
        public Nullable<double> visc { get; set; }
        public Nullable<double> pressure { get; set; }
    }
}
