﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class asopnEntities : DbContext
    {
        public asopnEntities()
            : base("name=asopnEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<act> act { get; set; }
        public virtual DbSet<act_syn> act_syn { get; set; }
        public virtual DbSet<act_tank> act_tank { get; set; }
        public virtual DbSet<calibration> calibration { get; set; }
        public virtual DbSet<calibrationARCHIV> calibrationARCHIV { get; set; }
        public virtual DbSet<Correct> Correct { get; set; }
        public virtual DbSet<DonnyOstat> DonnyOstat { get; set; }
        public virtual DbSet<filials> filials { get; set; }
        public virtual DbSet<oper> oper { get; set; }
        public virtual DbSet<Podpisanty> Podpisanty { get; set; }
        public virtual DbSet<qpass> qpass { get; set; }
        public virtual DbSet<qpass_tank> qpass_tank { get; set; }
        public virtual DbSet<ResursiNefti> ResursiNefti { get; set; }
        public virtual DbSet<shift> shift { get; set; }
        public virtual DbSet<sikn495masscounter> sikn495masscounter { get; set; }
        public virtual DbSet<sikn495volumecounter> sikn495volumecounter { get; set; }
        public virtual DbSet<sut> sut { get; set; }
        public virtual DbSet<SvodkaSutki> SvodkaSutki { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TagCorrespondence> TagCorrespondence { get; set; }
        public virtual DbSet<taginfo> taginfo { get; set; }
        public virtual DbSet<tags> tags { get; set; }
        public virtual DbSet<tagsuzr> tagsuzr { get; set; }
        public virtual DbSet<tankinfo> tankinfo { get; set; }
        public virtual DbSet<TankInv> TankInv { get; set; }
        public virtual DbSet<tanktags> tanktags { get; set; }
        public virtual DbSet<trl_tank> trl_tank { get; set; }
        public virtual DbSet<trl_tanktype> trl_tanktype { get; set; }
        public virtual DbSet<twohours> twohours { get; set; }
        public virtual DbSet<twohoursconf> twohoursconf { get; set; }
        public virtual DbSet<units> units { get; set; }
        public virtual DbSet<valveinfo> valveinfo { get; set; }
        public virtual DbSet<valvetag> valvetag { get; set; }
        public virtual DbSet<valvettype> valvettype { get; set; }
        public virtual DbSet<trl_tankview> trl_tankview { get; set; }
        public virtual DbSet<unicUID> unicUID { get; set; }
        public virtual DbSet<valveinfo_view> valveinfo_view { get; set; }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    }
}