//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperBloki.Domain
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SuperBlokiEntities : DbContext
    {
        public SuperBlokiEntities()
            : base("name=SuperBlokiEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Constructors> Constructors { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Workers> Workers { get; set; }
        public virtual DbSet<History_orders> History_orders { get; set; }
    }
}
