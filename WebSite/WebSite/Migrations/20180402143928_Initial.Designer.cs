﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebSite.Models;

namespace WebSite.Migrations
{
    [DbContext(typeof(WebSiteDbContext))]
    [Migration("20180402143928_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebSite.Models.Album", b =>
                {
                    b.Property<Guid>("AlbumFk")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cover");

                    b.Property<string>("LinkToPerson");

                    b.Property<string>("NameAlbum");

                    b.Property<string>("NamePerson");

                    b.HasKey("AlbumFk");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("WebSite.Models.Person", b =>
                {
                    b.Property<Guid>("PersonFk")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Biography");

                    b.Property<string>("Name");

                    b.Property<string>("Photo");

                    b.HasKey("PersonFk");

                    b.ToTable("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
