using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PracticaArchivos.Domain;
using PracticaArchivos.Repository;


namespace PracticaArchivos.Repository
{
    internal class DataBaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<VentasMensuales> VentasMensuales { get; set; }
        public DbSet<Rechazos> Rechazos { get; set; }
        public DbSet<Parametria> Parametria { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)//Declaro como en el ejemplo del profe
        {
            modelBuilder.Entity<VentasMensuales>(entity =>//el boldebuilrder es la configuracion del modelo
            {
                entity.ToTable("ventas_mensuales");//asi se puede modificar el nombre


                entity.HasKey(vm => new { vm.IdVendedor, vm.FechaInforme });//defino las llaves

                entity.Property(vm => vm.CodigoDelVendedor).HasMaxLength(3).IsRequired();
                entity.Property(vm => vm.FechaDelInforme).IsRequired();
                entity.Property(vm => vm.Venta).HasColumnType("decimal(8,2)").IsRequired();
                entity.Property(vm => vm.VentaEmpresaGrande).HasMaxLength(1).IsRequired();
            });

            // Config Entity to the table "Rechazos"
            modelBuilder.Entity<Rechazos>(entity =>
            {
                entity.ToTable("rechazos");
                entity.HasKey(re => re.IdRechazo);
                entity.Property(re => re.Motivo).HasMaxLength(100).IsRequired();
            });

            // Config Entity to the table "Parametria"
            modelBuilder.Entity<Parametria>(entity =>
            {
                entity.ToTable("parametria");
                entity.HasKey(pa => pa.Fecha);
                entity.Property(pa => pa.Fecha).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

