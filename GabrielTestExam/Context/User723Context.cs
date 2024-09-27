using System;
using System.Collections.Generic;
using GabrielTestExam.Models;
using Microsoft.EntityFrameworkCore;

namespace GabrielTestExam.Context;

public partial class User723Context : DbContext
{
    public User723Context()
    {
    }

    public User723Context(DbContextOptions<User723Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderAndProduct> OrderAndProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=192.168.2.159;Database=user723;Username=user723;Password=53344");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("order_pk");

            entity.ToTable("Order", "practice");

            entity.Property(e => e.OrderId)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("order_id");
            entity.Property(e => e.OrderCode)
                .HasColumnType("character varying")
                .HasColumnName("order_code");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderDeliver).HasColumnName("order_deliver");
            entity.Property(e => e.OrderPoint)
                .HasColumnType("character varying")
                .HasColumnName("order_point");
            entity.Property(e => e.OrderPrice).HasColumnName("order_price");
            entity.Property(e => e.OrderStatus)
                .HasColumnType("character varying")
                .HasColumnName("order_status");
        });

        modelBuilder.Entity<OrderAndProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_and_product_pk");

            entity.ToTable("Order_And_Product", "practice");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PriceWithDiscount).HasColumnName("price_with_discount");
            entity.Property(e => e.PriceWithoutDiscount).HasColumnName("price_without_discount");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderAndProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_and_product_order_fk");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderAndProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_and_product_product_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pk");

            entity.ToTable("Product", "practice");

            entity.Property(e => e.ProductId)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("product_id");
            entity.Property(e => e.ProductDescription)
                .HasColumnType("character varying")
                .HasColumnName("product_description");
            entity.Property(e => e.ProductDiscount).HasColumnName("product_discount");
            entity.Property(e => e.ProductManufacturer)
                .HasColumnType("character varying")
                .HasColumnName("product_manufacturer");
            entity.Property(e => e.ProductName)
                .HasColumnType("character varying")
                .HasColumnName("product_name");
            entity.Property(e => e.ProductPhoto)
                .HasColumnType("character varying")
                .HasColumnName("product_photo");
            entity.Property(e => e.ProductPrice).HasColumnName("product_price");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("receipt_pk");

            entity.ToTable("Receipt", "practice");

            entity.Property(e => e.Number)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(null, null, 0L, null, null, null)
                .HasColumnName("number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
