﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GamesControl.Web
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Contexto : DbContext
    {
        public Contexto()
            : base("name=Contexto")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tbArbitro> tbArbitro { get; set; }
        public virtual DbSet<tbAtributoEstatistica> tbAtributoEstatistica { get; set; }
        public virtual DbSet<tbCampeonato> tbCampeonato { get; set; }
        public virtual DbSet<tbCidade> tbCidade { get; set; }
        public virtual DbSet<tbEstatisticaJogoJogadorTime> tbEstatisticaJogoJogadorTime { get; set; }
        public virtual DbSet<tbJogador> tbJogador { get; set; }
        public virtual DbSet<tbJogo> tbJogo { get; set; }
        public virtual DbSet<tbJogoJogadorTime> tbJogoJogadorTime { get; set; }
        public virtual DbSet<tbJogoStatus> tbJogoStatus { get; set; }
        public virtual DbSet<tbPerfil> tbPerfil { get; set; }
        public virtual DbSet<tbTime> tbTime { get; set; }
        public virtual DbSet<tbUsuario> tbUsuario { get; set; }
        public virtual DbSet<tbUsuarioStatus> tbUsuarioStatus { get; set; }
    }
}
