using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using GodnoscCup.Models;

namespace GodnoscCup.Migrations
{
    [DbContext(typeof(CustomContext))]
    partial class CustomContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("GodnoscCup.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("GameDate");

                    b.Property<int>("TeamOneId");

                    b.Property<int>("TeamOnePoints");

                    b.Property<int>("TeamOneScore");

                    b.Property<int>("TeamTwoId");

                    b.Property<int>("TeamTwoPoints");

                    b.Property<int>("TeamTwoScore");

                    b.HasKey("GameId");
                });

            modelBuilder.Entity("GodnoscCup.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PlayerName");

                    b.Property<int>("ScoredGoals");

                    b.Property<int>("SequenceNumber");

                    b.Property<int>("TeamId");

                    b.HasKey("PlayerId");
                });

            modelBuilder.Entity("GodnoscCup.Models.Scorer", b =>
                {
                    b.Property<int>("ScorerId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<int>("PlayerId");

                    b.HasKey("ScorerId");
                });

            modelBuilder.Entity("GodnoscCup.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TeamName")
                        .IsRequired();

                    b.HasKey("TeamId");
                });

            modelBuilder.Entity("GodnoscCup.Models.Player", b =>
                {
                    b.HasOne("GodnoscCup.Models.Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("GodnoscCup.Models.Scorer", b =>
                {
                    b.HasOne("GodnoscCup.Models.Game")
                        .WithMany()
                        .HasForeignKey("GameId");
                });
        }
    }
}
