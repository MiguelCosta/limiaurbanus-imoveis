using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mpc.LimiaUrbanus.DataBase.Models
{
    public partial class LimiaUrbanusContext : DbContext
    {
        public LimiaUrbanusContext(DbContextOptions<LimiaUrbanusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClasseEnergetica> ClasseEnergetica { get; set; }
        public virtual DbSet<Concelho> Concelho { get; set; }
        public virtual DbSet<Distrito> Distrito { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<FilePath> FilePath { get; set; }
        public virtual DbSet<Freguesia> Freguesia { get; set; }
        public virtual DbSet<Imovel> Imovel { get; set; }
        public virtual DbSet<Objetivo> Objetivo { get; set; }
        public virtual DbSet<Tipo> Tipo { get; set; }
        public virtual DbSet<Tipologia> Tipologia { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("ConnectioString");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concelho>(entity =>
            {
                entity.HasOne(d => d.Distrito)
                    .WithMany(p => p.Concelho)
                    .HasForeignKey(d => d.DistritoId)
                    .HasConstraintName("FK_dbo.Concelho_dbo.Distrito_DistritoId");
            });

            modelBuilder.Entity<FilePath>(entity =>
            {
                entity.Property(e => e.FileName).HasMaxLength(255);

                entity.HasOne(d => d.Imovel)
                    .WithMany(p => p.FilePath)
                    .HasForeignKey(d => d.ImovelId)
                    .HasConstraintName("FK_dbo.FilePath_dbo.Imovel_ImovelId");
            });

            modelBuilder.Entity<Freguesia>(entity =>
            {
                entity.HasOne(d => d.Concelho)
                    .WithMany(p => p.Freguesia)
                    .HasForeignKey(d => d.ConcelhoId)
                    .HasConstraintName("FK_dbo.Freguesia_dbo.Concelho_ConcelhoId");
            });

            modelBuilder.Entity<Imovel>(entity =>
            {
                entity.Property(e => e.IsAtivo).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsOportunidade).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ClasseEnergetica)
                    .WithMany(p => p.Imovel)
                    .HasForeignKey(d => d.ClasseEnergeticaId)
                    .HasConstraintName("FK_dbo.Imovel_dbo.ClasseEnergetica_ClasseEnergeticaId");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Imovel)
                    .HasForeignKey(d => d.EstadoId)
                    .HasConstraintName("FK_dbo.Imovel_dbo.Estado_EstadoId");

                entity.HasOne(d => d.Freguesia)
                    .WithMany(p => p.Imovel)
                    .HasForeignKey(d => d.FreguesiaId)
                    .HasConstraintName("FK_dbo.Imovel_dbo.Freguesia_FreguesiaId");

                entity.HasOne(d => d.Objetivo)
                    .WithMany(p => p.Imovel)
                    .HasForeignKey(d => d.ObjetivoId)
                    .HasConstraintName("FK_dbo.Imovel_dbo.Objetivo_ObjetivoId");

                entity.HasOne(d => d.Tipo)
                    .WithMany(p => p.Imovel)
                    .HasForeignKey(d => d.TipoId)
                    .HasConstraintName("FK_dbo.Imovel_dbo.Tipo_TipoId");

                entity.HasOne(d => d.Tipologia)
                    .WithMany(p => p.Imovel)
                    .HasForeignKey(d => d.TipologiaId)
                    .HasConstraintName("FK_dbo.Imovel_dbo.Tipologia_TipologiaId");
            });
        }
    }
}
