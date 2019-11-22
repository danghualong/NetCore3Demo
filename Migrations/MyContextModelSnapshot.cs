﻿// <auto-generated />
using EFTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EFTest.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1");

            modelBuilder.Entity("EFTest.Models.Entities.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasColumnName("activity_name")
                        .HasColumnType("varchar")
                        .HasMaxLength(40);

                    b.Property<string>("Summary")
                        .HasColumnName("remark")
                        .HasColumnType("varchar")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("activity");
                });

            modelBuilder.Entity("EFTest.Models.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActivityId")
                        .HasColumnName("activity_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnName("team_order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Summary")
                        .HasColumnName("remark")
                        .HasColumnType("varchar")
                        .HasMaxLength(100);

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnName("team_name")
                        .HasColumnType("varchar")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("team");
                });
#pragma warning restore 612, 618
        }
    }
}
