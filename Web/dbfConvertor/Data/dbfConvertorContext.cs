using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExcelToDbfConvertor.Models
{
    public partial class dbfConvertorContext : DbContext
    {
        public dbfConvertorContext(DbContextOptions<dbfConvertorContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ElmahError> ElmahError { get; set; }
        public virtual DbSet<Import> Import { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ElmahError>(entity =>
            {
                entity.HasKey(e => e.ErrorId)
                    .HasName("PK_ELMAH_Error");

                entity.ToTable("ELMAH_Error");

                entity.HasIndex(e => new { e.Application, e.TimeUtc, e.Sequence })
                    .HasName("IX_ELMAH_Error_App_Time_Seq");

                entity.Property(e => e.ErrorId).HasDefaultValueSql("newid()");

                entity.Property(e => e.AllXml)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Application)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Host)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Sequence).ValueGeneratedOnAdd();

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.TimeUtc).HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Import>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AutorIdC)
                    .HasColumnName("autor_id_c")
                    .HasMaxLength(255);

                entity.Property(e => e.AutorIdI)
                    .HasColumnName("autor_id_i")
                    .HasMaxLength(255);

                entity.Property(e => e.CetC)
                    .HasColumnName("cet_c")
                    .HasMaxLength(255);

                entity.Property(e => e.CodBanca).HasColumnName("cod_banca");

                entity.Property(e => e.CodC).HasColumnName("cod_c");

                entity.Property(e => e.CodI).HasColumnName("cod_i");

                entity.Property(e => e.CodRep).HasColumnName("cod_rep");

                entity.Property(e => e.CodSuc).HasColumnName("cod_suc");

                entity.Property(e => e.CodValuta)
                    .HasColumnName("cod_valuta")
                    .HasMaxLength(255);

                entity.Property(e => e.ContC).HasColumnName("cont_c");

                entity.Property(e => e.ContExt)
                    .HasColumnName("cont_ext")
                    .HasMaxLength(255);

                entity.Property(e => e.DataIdC)
                    .HasColumnName("data_id_c")
                    .HasMaxLength(255);

                entity.Property(e => e.DataIdI)
                    .HasColumnName("data_id_i")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataOp)
                    .HasColumnName("data_op")
                    .HasMaxLength(255);

                entity.Property(e => e.DataRap)
                    .HasColumnName("data_rap")
                    .HasColumnType("datetime");

                entity.Property(e => e.DenExt)
                    .HasColumnName("den_ext")
                    .HasMaxLength(255);

                entity.Property(e => e.FelCt).HasColumnName("fel_ct");

                entity.Property(e => e.FelOp)
                    .HasColumnName("fel_op")
                    .HasMaxLength(255);

                entity.Property(e => e.JudetC).HasColumnName("judet_c");

                entity.Property(e => e.JudetI).HasColumnName("judet_i");

                entity.Property(e => e.LocaC).HasColumnName("loca_c");

                entity.Property(e => e.LocaI).HasColumnName("loca_i");

                entity.Property(e => e.LocaReg)
                    .HasColumnName("loca_reg")
                    .HasMaxLength(255);

                entity.Property(e => e.NrActI)
                    .HasColumnName("nr_act_i")
                    .HasMaxLength(255);

                entity.Property(e => e.NrC).HasColumnName("nr_c");

                entity.Property(e => e.NrI).HasColumnName("nr_i");

                entity.Property(e => e.NrIdC).HasColumnName("nr_id_c");

                entity.Property(e => e.NrIdI).HasColumnName("nr_id_i");

                entity.Property(e => e.NumeC)
                    .HasColumnName("nume_c")
                    .HasMaxLength(255);

                entity.Property(e => e.NumeExt)
                    .HasColumnName("nume_ext")
                    .HasMaxLength(255);

                entity.Property(e => e.NumeI)
                    .HasColumnName("nume_i")
                    .HasMaxLength(255);

                entity.Property(e => e.NumeRep)
                    .HasColumnName("nume_rep")
                    .HasMaxLength(255);

                entity.Property(e => e.Observatii).HasMaxLength(255);

                entity.Property(e => e.PepC).HasColumnName("pep_c");

                entity.Property(e => e.PepExt)
                    .HasColumnName("pep_ext")
                    .HasMaxLength(255);

                entity.Property(e => e.PepI)
                    .HasColumnName("pep_i")
                    .HasMaxLength(255);

                entity.Property(e => e.PepRep)
                    .HasColumnName("pep_rep")
                    .HasMaxLength(255);

                entity.Property(e => e.PrenC)
                    .HasColumnName("pren_c")
                    .HasMaxLength(255);

                entity.Property(e => e.PrenExt)
                    .HasColumnName("pren_ext")
                    .HasMaxLength(255);

                entity.Property(e => e.PrenI)
                    .HasColumnName("pren_i")
                    .HasMaxLength(255);

                entity.Property(e => e.PrenRep)
                    .HasColumnName("pren_rep")
                    .HasMaxLength(255);

                entity.Property(e => e.RezC)
                    .HasColumnName("rez_c")
                    .HasMaxLength(255);

                entity.Property(e => e.SectorC)
                    .HasColumnName("sector_c")
                    .HasMaxLength(255);

                entity.Property(e => e.SectorI)
                    .HasColumnName("sector_i")
                    .HasMaxLength(255);

                entity.Property(e => e.SeriaIdC)
                    .HasColumnName("seria_id_c")
                    .HasMaxLength(255);

                entity.Property(e => e.SeriaIdI)
                    .HasColumnName("seria_id_i")
                    .HasMaxLength(255);

                entity.Property(e => e.StrC)
                    .HasColumnName("str_c")
                    .HasMaxLength(255);

                entity.Property(e => e.StrI)
                    .HasColumnName("str_i")
                    .HasMaxLength(255);

                entity.Property(e => e.SumaOp).HasColumnName("suma_op");

                entity.Property(e => e.TaraC).HasColumnName("tara_c");

                entity.Property(e => e.TaraExt).HasColumnName("tara_ext");

                entity.Property(e => e.TaraI).HasColumnName("tara_i");

                entity.Property(e => e.TipActI)
                    .HasColumnName("tip_act_i")
                    .HasMaxLength(255);

                entity.Property(e => e.TipC)
                    .HasColumnName("tip_c")
                    .HasMaxLength(255);

                entity.Property(e => e.TipCt).HasColumnName("tip_ct");

                entity.Property(e => e.TipExt)
                    .HasColumnName("tip_ext")
                    .HasMaxLength(255);

                entity.Property(e => e.TipIdC)
                    .HasColumnName("tip_id_c")
                    .HasMaxLength(255);

                entity.Property(e => e.TipIdI).HasColumnName("tip_id_i");
            });




        }
    }
}