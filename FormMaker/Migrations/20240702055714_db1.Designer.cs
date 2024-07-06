﻿// <auto-generated />
using FormMaker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FormMaker.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240702055714_db1")]
    partial class db1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FormMaker.Models.DataType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DataTypes");
                });

            modelBuilder.Entity("FormMaker.Models.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("dataTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("dataTypeId");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("FormMaker.Models.Form", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Forms");
                });

            modelBuilder.Entity("FormMaker.Models.FormField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("fieldId")
                        .HasColumnType("int");

                    b.Property<int>("formId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("fieldId");

                    b.HasIndex("formId");

                    b.ToTable("FormFields");
                });

            modelBuilder.Entity("FormMaker.Models.Field", b =>
                {
                    b.HasOne("FormMaker.Models.DataType", "dataType")
                        .WithMany()
                        .HasForeignKey("dataTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("dataType");
                });

            modelBuilder.Entity("FormMaker.Models.FormField", b =>
                {
                    b.HasOne("FormMaker.Models.Field", "field")
                        .WithMany()
                        .HasForeignKey("fieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FormMaker.Models.Form", "form")
                        .WithMany("formFields")
                        .HasForeignKey("formId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("field");

                    b.Navigation("form");
                });

            modelBuilder.Entity("FormMaker.Models.Form", b =>
                {
                    b.Navigation("formFields");
                });
#pragma warning restore 612, 618
        }
    }
}
