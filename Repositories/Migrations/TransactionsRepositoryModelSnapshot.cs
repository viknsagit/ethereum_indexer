﻿// <auto-generated />
using Blockchain_Indexer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Blockchain_Indexer.Migrations
{
    [DbContext(typeof(TransactionsRepository))]
    partial class TransactionsRepositoryModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
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

            modelBuilder.Entity("Blockchain_Indexer.Blockchain.Addresses.Wallet", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<decimal>("GasUsed")
                        .HasColumnType("numeric");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LastBalanceUpdate")
                        .HasColumnType("integer");

                    b.Property<int>("TransactionsCount")
                        .HasColumnType("integer");

                    b.HasKey("Address");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Blockchain_Indexer.Blockchain.Contracts.Contract", b =>
                {
                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("CreatorAddress")
                        .HasColumnType("text");

                    b.Property<string>("Hash")
                        .HasColumnType("text");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("TransactionsCount")
                        .HasColumnType("integer");

                    b.HasKey("Address");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Blockchain_Indexer.Blockchain.Contracts.Token", b =>
                {
                    b.Property<string>("Contract")
                        .HasColumnType("text");

                    b.Property<int>("Holders")
                        .HasColumnType("integer");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Supply")
                        .HasColumnType("numeric");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TokenType")
                        .HasColumnType("integer");

                    b.HasKey("Contract");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Blockchain_Indexer.Blockchain.Contracts.TokenHolder", b =>
                {
                    b.Property<string>("Owner")
                        .HasColumnType("text");

                    b.Property<string>("TokenAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TokenValue")
                        .HasColumnType("numeric");

                    b.HasKey("Owner");

                    b.HasIndex("TokenAddress");

                    b.ToTable("TokenHolders");
                });

            modelBuilder.Entity("Blockchain_Indexer.Blockchain.Transactions.PendingTransaction", b =>
                {
                    b.Property<string>("Hash")
                        .HasColumnType("text");

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

                    b.Property<long>("Timestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("ToAddress")
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Hash");

                    b.ToTable("PendingTransactions");
                });

            modelBuilder.Entity("Blockchain_Indexer.Blockchain.Transactions.Transaction", b =>
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
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Hash");

                    b.HasIndex("Block", "Number", "Timestamp");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Blockchain_Indexer.Blockchain.TransactionsHistoryDto", b =>
                {
                    b.Property<string>("DateNameId")
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<int>("TransactionsCount")
                        .HasColumnType("integer");

                    b.HasKey("DateNameId");

                    b.HasIndex("DateNameId")
                        .IsUnique();

                    b.ToTable("TransactionsHistory");
                });

            modelBuilder.Entity("Blockchain_Indexer.ProcessingError", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Message")
                        .HasColumnType("text");

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
#pragma warning restore 612, 618
        }
    }
}
