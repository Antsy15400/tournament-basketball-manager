﻿// <auto-generated />
using System;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Presentation.API.Migrations
{
    [DbContext(typeof(TournamentBasketballManagerDbContext))]
    [Migration("20230613180717_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Managers.Manager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("ManagerId");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("Domain.Managers.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("TeamName");

                    b.Property<Guid?>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("TeamId");

                    b.HasIndex("ManagerId")
                        .IsUnique();

                    b.HasIndex("TournamentId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Domain.Organizers.Match", b =>
                {
                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeamAId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeamBId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TournamentId", "TeamAId", "TeamBId");

                    b.HasIndex("TeamAId")
                        .IsUnique();

                    b.HasIndex("TeamBId")
                        .IsUnique();

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("Domain.Organizers.Organizer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("OrganizerId");

                    b.ToTable("Organizers");
                });

            modelBuilder.Entity("Domain.Organizers.Tournament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("TournamentName");

                    b.Property<Guid>("OrganizerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("TournamentId");

                    b.HasIndex("OrganizerId")
                        .IsUnique();

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("Domain.Players.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Domain.Managers.Manager", b =>
                {
                    b.OwnsOne("Domain.Managers.ManagerPersonalInfo", "PersonalInfo", b1 =>
                        {
                            b1.Property<Guid>("ManagerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("City");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Country");

                            b1.Property<DateTime>("DateOfBirth")
                                .HasColumnType("datetime2")
                                .HasColumnName("DateOfBirth");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Email");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("HouseNumber");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("LastName");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Street");

                            b1.HasKey("ManagerId");

                            b1.ToTable("Managers");

                            b1.WithOwner()
                                .HasForeignKey("ManagerId");
                        });

                    b.Navigation("PersonalInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Managers.Team", b =>
                {
                    b.HasOne("Domain.Managers.Manager", "Manager")
                        .WithOne("Team")
                        .HasForeignKey("Domain.Managers.Team", "ManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Organizers.Tournament", "Tournament")
                        .WithMany("Teams")
                        .HasForeignKey("TournamentId");

                    b.Navigation("Manager");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Domain.Organizers.Match", b =>
                {
                    b.HasOne("Domain.Managers.Team", "TeamA")
                        .WithOne()
                        .HasForeignKey("Domain.Organizers.Match", "TeamAId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Managers.Team", "TeamB")
                        .WithOne()
                        .HasForeignKey("Domain.Organizers.Match", "TeamBId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Organizers.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("TeamA");

                    b.Navigation("TeamB");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Domain.Organizers.Organizer", b =>
                {
                    b.OwnsOne("Domain.Organizers.OrganizerPersonalInfo", "PersonalInfo", b1 =>
                        {
                            b1.Property<Guid>("OrganizerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("City");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Country");

                            b1.Property<DateTime>("DateOfBirth")
                                .HasColumnType("datetime2")
                                .HasColumnName("DateOfBirth");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Email");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("HouseNumber");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("LastName");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Street");

                            b1.HasKey("OrganizerId");

                            b1.ToTable("Organizers");

                            b1.WithOwner()
                                .HasForeignKey("OrganizerId");
                        });

                    b.Navigation("PersonalInfo")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Organizers.Tournament", b =>
                {
                    b.HasOne("Domain.Organizers.Organizer", "Organizer")
                        .WithOne("Tournament")
                        .HasForeignKey("Domain.Organizers.Tournament", "OrganizerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organizer");
                });

            modelBuilder.Entity("Domain.Players.Player", b =>
                {
                    b.HasOne("Domain.Managers.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.OwnsOne("Domain.Players.PlayerPersonalInfo", "PersonalInfo", b1 =>
                        {
                            b1.Property<Guid>("PlayerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("City");

                            b1.Property<string>("Code")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Country");

                            b1.Property<DateTime>("DateOfBirth")
                                .HasColumnType("datetime2")
                                .HasColumnName("DateOfBirth");

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Email");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("FirstName");

                            b1.Property<float>("Height")
                                .HasColumnType("real")
                                .HasColumnName("Height");

                            b1.Property<string>("HouseNumber")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("HouseNumber");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("LastName");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Street");

                            b1.Property<float>("Weight")
                                .HasColumnType("real")
                                .HasColumnName("Weight");

                            b1.HasKey("PlayerId");

                            b1.ToTable("Players");

                            b1.WithOwner()
                                .HasForeignKey("PlayerId");
                        });

                    b.Navigation("PersonalInfo")
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Domain.Managers.Manager", b =>
                {
                    b.Navigation("Team");
                });

            modelBuilder.Entity("Domain.Managers.Team", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("Domain.Organizers.Organizer", b =>
                {
                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Domain.Organizers.Tournament", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Teams");
                });
#pragma warning restore 612, 618
        }
    }
}
