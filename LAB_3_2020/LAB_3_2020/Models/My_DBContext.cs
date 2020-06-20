using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LAB_3_2020.Models
{
    public partial class My_DBContext : DbContext
    {
        public My_DBContext()
        {
        }

        public My_DBContext(DbContextOptions<My_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alquiler> Alquiler { get; set; }
        public virtual DbSet<Inmueble> Inmueble { get; set; }
        public virtual DbSet<Inquilino> Inquilino { get; set; }
        public virtual DbSet<Pago> Pago { get; set; }
        public virtual DbSet<Propietario> Propietario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=HP-PRINTER-9856;Initial Catalog=API_Lab3;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alquiler>(entity =>
            {
                entity.Property(e => e.AlquilerId).HasColumnName("alquiler_id");

                entity.Property(e => e.Fin)
                    .HasColumnName("fin")
                    .HasColumnType("date");

                entity.Property(e => e.Inicio)
                    .HasColumnName("inicio")
                    .HasColumnType("date");

                entity.Property(e => e.InmuebleId).HasColumnName("inmueble_id");

                entity.Property(e => e.InquilinoId).HasColumnName("inquilino_id");

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Inmueble)
                    .WithMany(p => p.Alquiler)
                    .HasForeignKey(d => d.InmuebleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Alquiler_Inmueble");

                entity.HasOne(d => d.Inquilino)
                    .WithMany(p => p.Alquiler)
                    .HasForeignKey(d => d.InquilinoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Alquiler_Inquilino");
            });

            modelBuilder.Entity<Inmueble>(entity =>
            {
                entity.Property(e => e.InmuebleId).HasColumnName("inmueble_id");

                entity.Property(e => e.Ambientes).HasColumnName("ambientes");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnName("direccion")
                    .HasMaxLength(50);

                entity.Property(e => e.Disponible).HasColumnName("disponible");

                entity.Property(e => e.Precio)
                    .HasColumnName("precio")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PropietarioId).HasColumnName("propietario_id");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasMaxLength(50);

                entity.Property(e => e.Uso)
                    .IsRequired()
                    .HasColumnName("uso")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Propietario)
                    .WithMany(p => p.Inmueble)
                    .HasForeignKey(d => d.PropietarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inmueble_Propietario");
            });

            modelBuilder.Entity<Inquilino>(entity =>
            {
                entity.Property(e => e.InquilinoId).HasColumnName("inquilino_id");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(50);

                entity.Property(e => e.Dni)
                    .HasColumnName("dni")
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(50);

                entity.Property(e => e.Telefono)
                    .HasColumnName("telefono")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.Property(e => e.PagoId).HasColumnName("pago_id");

                entity.Property(e => e.AlquilerId).HasColumnName("alquiler_id");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.Importe)
                    .HasColumnName("importe")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.NroPago).HasColumnName("nroPago");

                entity.HasOne(d => d.Alquiler)
                    .WithMany(p => p.Pago)
                    .HasForeignKey(d => d.AlquilerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pago_Alquiler");
            });

            modelBuilder.Entity<Propietario>(entity =>
            {
                entity.Property(e => e.PropietarioId).HasColumnName("propietario_id");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasColumnName("apellido")
                    .HasMaxLength(50);

                entity.Property(e => e.Dni)
                    .IsRequired()
                    .HasColumnName("dni")
                    .HasMaxLength(50);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasColumnName("mail")
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasColumnName("telefono")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
