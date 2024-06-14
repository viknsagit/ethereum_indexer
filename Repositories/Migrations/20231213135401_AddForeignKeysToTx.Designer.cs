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
    [Migration("20231213135401_AddForeignKeysToTx")]
    partial class AddForeignKeysToTx
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

                    b.Property<int>("TransactionsNumber")
                        .HasColumnType("integer");

                    b.HasKey("BlockNumber");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("Blockchain_Indexer.Token", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<int>("Holders")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Supply")
                        .HasColumnType("numeric");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Address");

                    b.ToTable("Tokens");
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

                    b.Property<string>("FromWalletAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ToWalletAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Hash");

                    b.HasIndex("FromWalletAddress");

                    b.HasIndex("ToWalletAddress");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Blockchain_Indexer.TransactionError", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("TransactionHash")
                        .IsRequired()
                        .HasColumnType("text");

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

                    b.Property<int>("TokenTransfersCount")
                        .HasColumnType("integer");

                    b.Property<int>("TransactionsCount")
                        .HasColumnType("integer");

                    b.HasKey("Address");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Blockchain_Indexer.Transaction", b =>
                {
                    b.HasOne("Blockchain_Indexer.Wallet", "FromWallet")
                        .WithMany()
                        .HasForeignKey("FromWalletAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Blockchain_Indexer.Wallet", "ToWallet")
                        .WithMany()
                        .HasForeignKey("ToWalletAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromWallet");

                    b.Navigation("ToWallet");
                });
#pragma warning restore 612, 618
        }
    }
}
