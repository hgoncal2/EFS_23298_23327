﻿// <auto-generated />
using System;
using EFS_23298_23327.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFS_23298_23327.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

                    b.Property<string>("ClientesId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CriadoPorOid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CriadoPorUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<int>("NumPessoas")
                        .HasColumnType("int");

                    b.Property<double>("Preco")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReservaDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TemaID")
                        .HasColumnType("int");

                    b.Property<string>("UtilizadorID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReservaId");

                    b.HasIndex("ClientesId");

                    b.HasIndex("TemaID");

                    b.HasIndex("UtilizadorID");

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
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Dificuldade")
                        .HasColumnType("int");

                    b.Property<int?>("MaxPessoas")
                        .HasColumnType("int");

                    b.Property<int?>("MinPessoas")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SalaID")
                        .HasColumnType("int");

                    b.Property<int>("TempoEstimado")
                        .HasColumnType("int");

                    b.HasKey("TemaId");

                    b.HasIndex("SalaID");

                    b.ToTable("Temas");
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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("PrimeiroNome")
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

                    b.HasDiscriminator().HasValue("Anfitrioes");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Clientes", b =>
                {
                    b.HasBaseType("EFS_23298_23327.Models.Utilizadores");

                    b.HasDiscriminator().HasValue("Clientes");
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
                    b.HasOne("EFS_23298_23327.Models.Clientes", null)
                        .WithMany("ListaReservas")
                        .HasForeignKey("ClientesId");

                    b.HasOne("EFS_23298_23327.Models.Temas", "Tema")
                        .WithMany()
                        .HasForeignKey("TemaID");

                    b.HasOne("EFS_23298_23327.Models.Utilizadores", "Utilizador")
                        .WithMany()
                        .HasForeignKey("UtilizadorID");

                    b.Navigation("Tema");

                    b.Navigation("Utilizador");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Temas", b =>
                {
                    b.HasOne("EFS_23298_23327.Models.Salas", "Sala")
                        .WithMany()
                        .HasForeignKey("SalaID");

                    b.Navigation("Sala");
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

            modelBuilder.Entity("EFS_23298_23327.Models.Temas", b =>
                {
                    b.Navigation("ListaFotos");
                });

            modelBuilder.Entity("EFS_23298_23327.Models.Clientes", b =>
                {
                    b.Navigation("ListaReservas");
                });
#pragma warning restore 612, 618
        }
    }
}
