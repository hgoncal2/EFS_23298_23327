﻿// <auto-generated />
using System;
using EFS_23298_23327.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240713021559_mssql_migration_675")]
    partial class mssql_migration_675
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AnfitrioesReservas", b =>
                {
                    b.Property<string>("ListaAnfitrioesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ListaReservasReservaId")
                        .HasColumnType("int");

                    b.HasKey("ListaAnfitrioesId", "ListaReservasReservaId");

                    b.HasIndex("ListaReservasReservaId");

                    b.ToTable("AnfitrioesReservas");
                });

            modelBuilder.Entity("AnfitrioesSalas", b =>
                {
                    b.Property<string>("ListaAnfitrioesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ListaSalasSalaId")
                        .HasColumnType("int");

                    b.HasKey("ListaAnfitrioesId", "ListaSalasSalaId");

                    b.HasIndex("ListaSalasSalaId");

                    b.ToTable("AnfitrioesSalas");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Fotos", b =>
                {
                    b.Property<int>("FotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FotoId"));

                    b.Property<string>("CriadoPorOid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriadoPorUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataTirada")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Fotos")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TemaId")
                        .HasColumnType("int");

                    b.HasKey("FotoId");

                    b.HasIndex("TemaId");

                    b.ToTable("Fotos");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Reservas", b =>
                {
                    b.Property<int>("ReservaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservaId"));

                    b.Property<bool>("Cancelada")
                        .HasColumnType("bit");

                    b.Property<string>("ClienteID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CriadoPorOid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriadoPorUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCancel")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("NumPessoas")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservaDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReservaEndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SalaId")
                        .HasColumnType("int");

                    b.Property<int?>("TemaDif")
                        .HasColumnType("int");

                    b.Property<string>("TemaNome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPreco")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ReservaId");

                    b.HasIndex("ClienteID");

                    b.HasIndex("SalaId");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Salas", b =>
                {
                    b.Property<int>("SalaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalaId"));

                    b.Property<int>("Area")
                        .HasColumnType("int");

                    b.Property<string>("CriadoPorOid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriadoPorUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.HasKey("SalaId");

                    b.ToTable("Salas");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Temas", b =>
                {
                    b.Property<int>("TemaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TemaId"));

                    b.Property<string>("CriadoPorOid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriadoPorUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Dificuldade")
                        .HasColumnType("int");

                    b.Property<string>("Icone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaxPessoas")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("MinPessoas")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("SalaID")
                        .HasColumnType("int");

                    b.Property<int>("TempoEstimado")
                        .HasColumnType("int");

                    b.HasKey("TemaId");

                    b.HasIndex("SalaID");

                    b.ToTable("Temas");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.UserPrefAnfCores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Key")
                        .HasColumnType("int");

                    b.Property<int>("UserPrefId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserPrefId");

                    b.ToTable("UserPrefAnfCores");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.UserPrefsAnf", b =>
                {
                    b.Property<int>("UserPrefId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserPrefId"));

                    b.Property<string>("AnfId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("mostrarCanceladas")
                        .HasColumnType("bit");

                    b.HasKey("UserPrefId");

                    b.ToTable("UserPrefsAnf");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Utilizadores", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriadoPorOid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriadoPorUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PrimeiroNome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UltimoNome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Utilizadores");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("EFS_23298_23327.ViewModel.LoginViewModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LoginViewModel");
                });

            modelBuilder.Entity("EFS_23298_23327.ViewModel.UtilizadoresViewModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CriadoPor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimeiroNome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UltimoNome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UtilizadoresViewModel");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "anf",
                            Name = "Anfitriao",
                            NormalizedName = "ANFITRIAO"
                        },
                        new
                        {
                            Id = "adm",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "cl",
                            Name = "Cliente",
                            NormalizedName = "CLIENTE"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Anfitrioes", b =>
                {
                    b.HasBaseType("EFS_23298_23327.Models.Utilizadores");

                    b.Property<int?>("userPrefsAnfId")
                        .HasColumnType("int");

                    b.HasIndex("userPrefsAnfId")
                        .IsUnique()
                        .HasFilter("[userPrefsAnfId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("Anfitrioes");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Clientes", b =>
                {
                    b.HasBaseType("EFS_23298_23327.Models.Utilizadores");

                    b.HasDiscriminator().HasValue("Clientes");
                });

            modelBuilder.Entity("AnfitrioesReservas", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Anfitrioes", null)
                        .WithMany()
                        .HasForeignKey("ListaAnfitrioesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFS_23298_23327.Models.Reservas", null)
                        .WithMany()
                        .HasForeignKey("ListaReservasReservaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AnfitrioesSalas", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Anfitrioes", null)
                        .WithMany()
                        .HasForeignKey("ListaAnfitrioesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFS_23298_23327.Models.Salas", null)
                        .WithMany()
                        .HasForeignKey("ListaSalasSalaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Fotos", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Temas", "Tema")
                        .WithMany("ListaFotos")
                        .HasForeignKey("TemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tema");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Reservas", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Clientes", "Cliente")
                        .WithMany("ListaReservas")
                        .HasForeignKey("ClienteID");

                    b.HasOne("EFS_23298_23327.Models.Salas", "Sala")
                        .WithMany("ListaReservas")
                        .HasForeignKey("SalaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Sala");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Temas", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Salas", "Sala")
                        .WithMany()
                        .HasForeignKey("SalaID");

                    b.Navigation("Sala");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.UserPrefAnfCores", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.UserPrefsAnf", "UserPrefsAnf")
                        .WithMany("Cores")
                        .HasForeignKey("UserPrefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserPrefsAnf");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Utilizadores", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Utilizadores", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFS_23298_23327.Models.Utilizadores", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Utilizadores", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Anfitrioes", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.UserPrefsAnf", "userPrefsAn")
                        .WithOne("Anfitriao")
                        .HasForeignKey("EFS_23298_23327.Models.Anfitrioes", "userPrefsAnfId");

                    b.Navigation("userPrefsAn");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Salas", b =>
                {
                    b.Navigation("ListaReservas");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Temas", b =>
                {
                    b.Navigation("ListaFotos");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.UserPrefsAnf", b =>
                {
                    b.Navigation("Anfitriao");

                    b.Navigation("Cores");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Clientes", b =>
                {
                    b.Navigation("ListaReservas");
                });
#pragma warning restore 612, 618
        }
    }
}
