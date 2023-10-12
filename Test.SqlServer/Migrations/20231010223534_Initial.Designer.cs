﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Test.SqlServer;

#nullable disable

namespace Test.SqlServer.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20231010223534_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Test.Shared.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Test.Shared.Country", b =>
                {
                    b.OwnsOne("Neptunee.EntityFrameworkCore.MultiLanguage.Types.MultiLanguageProperty", "Name", b1 =>
                        {
                            b1.Property<Guid>("CountryId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("CountryId");

                            b1.ToTable("Countries");

                            b1.ToJson("Name");

                            b1.WithOwner()
                                .HasForeignKey("CountryId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
