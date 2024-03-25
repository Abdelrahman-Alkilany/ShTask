using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ShTask.Models;

public partial class ShaTaskContext : DbContext
{
    public ShaTaskContext()
    {
    }

    public ShaTaskContext(DbContextOptions<ShaTaskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Cashier> Cashiers { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<InvoiceHeader> InvoiceHeaders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ShaTask;Trusted_Connection=True ;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BranchName)
                .HasMaxLength(200)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.CityId).HasColumnName("CityID");

            entity.HasOne(d => d.City).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branches_Cities");
        });

        modelBuilder.Entity<Cashier>(entity =>
        {
            entity.ToTable("Cashier");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CashierName)
                .HasMaxLength(200)
                .HasDefaultValueSql("('')");

            entity.HasOne(d => d.Branch).WithMany(p => p.Cashiers)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cashier_Branches");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CityName)
                .HasMaxLength(200)
                .HasDefaultValueSql("('')");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InvoiceHeaderId).HasColumnName("InvoiceHeaderID");
            entity.Property(e => e.ItemName)
                .HasMaxLength(200)
                .HasDefaultValueSql("('')");

            entity.HasOne(d => d.InvoiceHeader).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetails_InvoiceHeader");
        });

        modelBuilder.Entity<InvoiceHeader>(entity =>
        {
            entity.ToTable("InvoiceHeader");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CashierId).HasColumnName("CashierID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .HasDefaultValueSql("('')");
            entity.Property(e => e.Invoicedate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Branch).WithMany(p => p.InvoiceHeaders)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceHeader_Branches");

            entity.HasOne(d => d.Cashier).WithMany(p => p.InvoiceHeaders)
                .HasForeignKey(d => d.CashierId)
                .HasConstraintName("FK_InvoiceHeader_Cashier");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
