﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestAPI.Model;

#nullable disable

namespace TestAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240207150727_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TestAPI.Model.Students", b =>
                {
                    b.Property<int>("studentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("studentID"));

                    b.Property<int>("departmentID")
                        .HasColumnType("int");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("gender")
                        .HasColumnType("int");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("studentID");

                    b.ToTable("students");

                    b.HasData(
                        new
                        {
                            studentID = 1,
                            departmentID = 1,
                            email = "lloyddiaz0205@gmail.com",
                            firstName = "Lloyd Jiperson",
                            gender = 0,
                            lastName = "Diaz"
                        },
                        new
                        {
                            studentID = 2,
                            departmentID = 2,
                            email = "thyronrequez2@gmail.com",
                            firstName = "Kazami",
                            gender = 0,
                            lastName = "Nazukai"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}