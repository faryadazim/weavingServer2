﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_weavingEntities : DbContext
    {
        public db_weavingEntities()
            : base("name=db_weavingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<BorderQuality> BorderQuality { get; set; }
        public virtual DbSet<BorderSize> BorderSize { get; set; }
        public virtual DbSet<employeeDesignation> employeeDesignation { get; set; }
        public virtual DbSet<Modules> Modules { get; set; }
        public virtual DbSet<PagePermission> PagePermission { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<employeeList> employeeList { get; set; }
        public virtual DbSet<weavingUnit> weavingUnit { get; set; }
        public virtual DbSet<LoomList> LoomList { get; set; }
        public virtual DbSet<grayProductList> grayProductList { get; set; }
        public virtual DbSet<account_type> account_type { get; set; }
        public virtual DbSet<chart_of_accounts> chart_of_accounts { get; set; }
        public virtual DbSet<finance_entries> finance_entries { get; set; }
        public virtual DbSet<finance_main> finance_main { get; set; }
        public virtual DbSet<production> production { get; set; }
        public virtual DbSet<production_shift> production_shift { get; set; }
        public virtual DbSet<voucher_types> voucher_types { get; set; }
    }
}
