﻿// <auto-generated />
using System;
using EDennis.Samples.ColorsRepo.EfCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EDennis.Samples.ColorsRepo.EfCore.Migrations
{
    [DbContext(typeof(ColorDb2Context))]
    partial class ColorDb2ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EDennis.Samples.ColorsRepo.EfCore.Models.Color", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int>("Blue");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("Green");

                    b.Property<int>("Red");

                    b.HasKey("Name")
                        .HasName("pkColor");

                    b.ToTable("Color");
                });
#pragma warning restore 612, 618
        }
    }
}
