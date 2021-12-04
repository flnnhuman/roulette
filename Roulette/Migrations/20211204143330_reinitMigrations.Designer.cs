﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Roulette.Context;

namespace Roulette.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211204143330_reinitMigrations")]
    partial class reinitMigrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Roulette.Models.BetModel", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<float>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<string>("ConnectionId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<long?>("GameModelId")
                        .HasColumnType("bigint");

                    b.Property<string>("SteamID")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("GameModelId");

                    b.ToTable("BetModel");
                });

            modelBuilder.Entity("Roulette.Models.GameModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("WonColor")
                        .HasColumnType("int");

                    b.Property<int>("WonNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GamesHistory");
                });

            modelBuilder.Entity("Roulette.Models.MessageModel", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Avatar")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Message")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PersonaName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SteamId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("ChatHistory");
                });

            modelBuilder.Entity("Roulette.Models.ReferralModel", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SteamUsersId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<ulong>("TotalBets")
                        .HasColumnType("bigint unsigned");

                    b.Property<uint>("TotalProfit")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("Usages")
                        .HasColumnType("int unsigned");

                    b.HasKey("Id");

                    b.HasIndex("SteamUsersId")
                        .IsUnique();

                    b.ToTable("ReferralModels");
                });

            modelBuilder.Entity("Roulette.Models.SteamUsersModel", b =>
                {
                    b.Property<string>("SteamID")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<uint?>("ParentReferralId")
                        .HasColumnType("int unsigned");

                    b.Property<float>("TotalDeposited")
                        .HasColumnType("float");

                    b.Property<float>("TotalWon")
                        .HasColumnType("float");

                    b.Property<string>("TradeLink")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("avatar")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("avatarfull")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("avatarhash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("avatarmedium")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("communityvisibilitystate")
                        .HasColumnType("int");

                    b.Property<string>("personaname")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("personastate")
                        .HasColumnType("int");

                    b.Property<int>("personastateflags")
                        .HasColumnType("int");

                    b.Property<int>("profilestate")
                        .HasColumnType("int");

                    b.Property<string>("profileurl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("SteamID");

                    b.HasIndex("ParentReferralId");

                    b.ToTable("SteamUsers");
                });

            modelBuilder.Entity("Roulette.Models.BetModel", b =>
                {
                    b.HasOne("Roulette.Models.GameModel", null)
                        .WithMany("AllBets")
                        .HasForeignKey("GameModelId");
                });

            modelBuilder.Entity("Roulette.Models.ReferralModel", b =>
                {
                    b.HasOne("Roulette.Models.SteamUsersModel", "SteamUsers")
                        .WithOne("Referral")
                        .HasForeignKey("Roulette.Models.ReferralModel", "SteamUsersId");

                    b.Navigation("SteamUsers");
                });

            modelBuilder.Entity("Roulette.Models.SteamUsersModel", b =>
                {
                    b.HasOne("Roulette.Models.ReferralModel", "ParentReferral")
                        .WithMany()
                        .HasForeignKey("ParentReferralId");

                    b.Navigation("ParentReferral");
                });

            modelBuilder.Entity("Roulette.Models.GameModel", b =>
                {
                    b.Navigation("AllBets");
                });

            modelBuilder.Entity("Roulette.Models.SteamUsersModel", b =>
                {
                    b.Navigation("Referral");
                });
#pragma warning restore 612, 618
        }
    }
}
