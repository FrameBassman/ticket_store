﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TicketStore.Api.Data;

namespace TicketStore.Api.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TicketStore.Api.Model.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnName("amount");

                    b.Property<string>("Email")
                        .HasColumnName("email");

                    b.HasKey("Id");

                    b.ToTable("payments");
                });

            modelBuilder.Entity("TicketStore.Api.Model.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<bool>("Expired")
                        .HasColumnName("expired");

                    b.Property<string>("Number")
                        .HasColumnName("number");

                    b.Property<int>("PaymentId")
                        .HasColumnName("payment_id");

                    b.Property<int>("Roubles")
                        .HasColumnName("roubles");

                    b.HasKey("Id");

                    b.ToTable("tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
