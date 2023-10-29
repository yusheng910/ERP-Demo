using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prjErpDemo.Models;

public partial class db_ErpDemoContext : DbContext
{
    public db_ErpDemoContext()
    {
    }

    public db_ErpDemoContext(DbContextOptions<db_ErpDemoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderDate).HasPrecision(0);
            entity.Property(e => e.ShippedDate).HasPrecision(0);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_OrderStatuses");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Products");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.Property(e => e.StatusDescription).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.Property(e => e.ContactName).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.AccountName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
