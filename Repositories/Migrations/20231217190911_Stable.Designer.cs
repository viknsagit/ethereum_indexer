﻿// <auto-generated />
using Blockchain_Indexer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    [DbContext(typeof(TransactionsRepository))]
    [Migration("20231217190911_Stable")]
    partial class Stable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Blockchain_Indexer.Block", b =>
                {
                    b.Property<int>("BlockNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BlockNumber"));

                    b.Property<decimal>("GasLimit")
                        .HasColumnType("numeric");

                    b.Property<decimal>("GasUsed")
                        .HasColumnType("numeric");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<int>("TransactionsNumber")
                        .HasColumnType("integer");

                    b.HasKey("BlockNumber");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("Blockchain_Indexer.Contract", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("CreatorAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Address");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Blockchain_Indexer.Transaction", b =>
                {
                    b.Property<string>("Hash")
                        .HasColumnType("text");

                    b.Property<int>("Block")
                        .HasColumnType("integer");

                    b.Property<string>("FromAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Gas")
                        .HasColumnType("numeric");

                    b.Property<decimal>("GasPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsContractDeployment")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsContractInteraction")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsReverted")
                        .HasColumnType("boolean");

                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Number"));

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Hash");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Blockchain_Indexer.TransactionError", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("TransactionHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Errors");
                });

            modelBuilder.Entity("Blockchain_Indexer.Wallet", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<decimal>("GasUsed")
                        .HasColumnType("numeric");

                    b.Property<int>("LastBalanceUpdate")
                        .HasColumnType("integer");

                    b.Property<int>("TransactionsCount")
                        .HasColumnType("integer");

                    b.HasKey("Address");

                    b.ToTable("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}
