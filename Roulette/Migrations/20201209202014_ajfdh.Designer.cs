﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Roulette.Context;

namespace Roulette.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201209202014_ajfdh")]
    partial class ajfdh
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Roulette.Models.SteamUsersModel", b =>
                {
                    b.Property<string>("SteamID")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

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

                    b.ToTable("SteamUsers");

                    b.HasData(
                        new
                        {
                            SteamID = "76561100000000000",
                            Balance = 0.0,
                            TotalDeposited = 0f,
                            TotalWon = 0f,
                            avatar = "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/avatars/cb/.jpg",
                            avatarfull = "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/avatars/cb/_full.jpg",
                            avatarmedium = "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/avatars/cb/_medium.jpg",
                            communityvisibilitystate = 3,
                            personaname = "name",
                            personastate = 0,
                            personastateflags = 0,
                            profilestate = 1,
                            profileurl = "https://steamcommunity.com/id/"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
