
using System;
using FurnitureERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FurnitureERP.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Employees.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("HourlyRate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Materials.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("CurrentStock")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("MinimumStock")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PricePerUnit")
                        .HasColumnType("TEXT");

                    b.Property<string>("Supplier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Orders.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ActualCompletionDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerEmail")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("DeliveryAddress")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpectedCompletionDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CustomerName");

                    b.HasIndex("OrderDate");

                    b.HasIndex("OrderNumber")
                        .IsUnique();

                    b.HasIndex("Status");

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Orders.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductArticle")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("UnitPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems", (string)null);
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Products.LaborBom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("HoursRequired")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SequenceNumber")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId")
                        .HasDatabaseName("IX_LaborBoms_EmployeeId");

                    b.HasIndex("Position")
                        .HasDatabaseName("IX_LaborBoms_Position");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("IX_LaborBoms_ProductId");

                    b.HasIndex("ProductId", "SequenceNumber")
                        .HasDatabaseName("IX_LaborBoms_ProductId_SequenceNumber");

                    b.ToTable("LaborBoms", (string)null);
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Products.MaterialBom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("QuantityRequired")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("WastagePercentage")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(5, 2)
                        .HasColumnType("TEXT")
                        .HasDefaultValue(0m);

                    b.HasKey("Id");

                    b.HasIndex("MaterialId")
                        .HasDatabaseName("IX_MaterialBoms_MaterialId");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("IX_MaterialBoms_ProductId");

                    b.HasIndex("ProductId", "MaterialId")
                        .HasDatabaseName("IX_MaterialBoms_ProductId_MaterialId");

                    b.ToTable("MaterialBoms", (string)null);
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Article")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BasePrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("SalePrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Article")
                        .IsUnique()
                        .HasDatabaseName("IX_Products_Article");

                    b.HasIndex("Category")
                        .HasDatabaseName("IX_Products_Category");

                    b.HasIndex("IsActive")
                        .HasDatabaseName("IX_Products_IsActive");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Orders.OrderItem", b =>
                {
                    b.HasOne("FurnitureERP.Domain.Aggregates.Orders.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Products.LaborBom", b =>
                {
                    b.HasOne("FurnitureERP.Domain.Aggregates.Products.Product", null)
                        .WithMany("LaborBoms")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Products.MaterialBom", b =>
                {
                    b.HasOne("FurnitureERP.Domain.Aggregates.Products.Product", null)
                        .WithMany("MaterialBoms")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Orders.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Products.Product", b =>
                {
                    b.Navigation("LaborBoms");

                    b.Navigation("MaterialBoms");
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Materials.StockTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("MaterialUnit")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("StockAfter")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("StockBefore")
                        .HasPrecision(18, 4)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId")
                        .HasDatabaseName("IX_StockTransactions_MaterialId");

                    b.HasIndex("TransactionDate")
                        .HasDatabaseName("IX_StockTransactions_TransactionDate");

                    b.ToTable("StockTransactions", (string)null);
                });

            modelBuilder.Entity("FurnitureERP.Domain.Aggregates.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
