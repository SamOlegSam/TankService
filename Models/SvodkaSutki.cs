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
    
    public partial class SvodkaSutki
    {
        public int ID { get; set; }
        public System.DateTime Dat { get; set; }
        public int FilialID { get; set; }
        public Nullable<decimal> VNeftItog { get; set; }
        public Nullable<decimal> MassaBalItog { get; set; }
        public Nullable<decimal> MassaNettoItog { get; set; }
        public Nullable<decimal> MassaBruttoOstat { get; set; }
        public Nullable<decimal> MassaNettoMinItog { get; set; }
        public Nullable<decimal> MassaItog { get; set; }
        public string Primechanie { get; set; }
        public Nullable<System.DateTime> DatWrite { get; set; }
    }
}
