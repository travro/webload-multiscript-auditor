using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WMSA_DAL.Models;

namespace WMSA_DAL.Context
{
    public class WLContext : DbContext
    {
        public virtual DbSet<Correlation> Correlations { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestVerb> RequestVerbs { get; set; }
        public virtual DbSet<Script> Scripts { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<TransactionName> TransactionNames { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        public WLContext()
            : base("name=WLScriptsDB")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>()
                .HasMany(e => e.Correlations)
                .WithOptional(e => e.Request)
                .HasForeignKey(e => e.req_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<RequestVerb>()
                .Property(e => e.verb)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RequestVerb>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.RequestVerb)
                .HasForeignKey(e => e.verb_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Script>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.Script)
                .HasForeignKey(e => e.script_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Scripts)
                .WithOptional(e => e.Test)
                .HasForeignKey(e => e.test_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TransactionName>()
                .HasMany(e => e.Transactions)
                .WithOptional(e => e.TransactionName)
                .HasForeignKey(e => e.trans_nm_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Transaction>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.Transaction)
                .HasForeignKey(e => e.trans_id)
                .WillCascadeOnDelete();
        }
    }
}
