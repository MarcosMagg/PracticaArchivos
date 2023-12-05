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
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext>option): base(option){ }    





        public DbSet<VentasMensuales> VentasMensuales { get; set; }
        public DbSet<Rechazos> Rechazos { get; set; }
        public DbSet<Parametria> Parametria { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)//Declaro como en el ejemplo del profe
        {
            modelBuilder.Entity<VentasMensuales>(entity =>//el boldebuilrder es la configuracion del modelo
            {
                entity.ToTable("ventas_mensuales");//asi se puede modificar el nombre



                entity.Property(vm => vm.Id).HasColumnName("id");
                entity.Property(vm => vm.CodVendedor).HasColumnName("cod_vendedor");
                entity.Property(vm => vm.MontoVenta).HasColumnName("monto_venta");
                entity.Property(vm => vm.FechaProceso).HasColumnName("fecha_proceso");
                entity.Property(vm => vm.McaEmpresaGrande).HasColumnName("mca_empresa_grande");
            });

            // Config Entity to the table "Rechazos"
            modelBuilder.Entity<Rechazos>(entity =>
            {
                entity.ToTable("rechazos");
                entity.Property(re => re.MotivoRechazos).HasColumnName("motivo_rechazo");
                entity.Property(re => re.Id).HasColumnName("id"); 
            });

            // Config Entity to the table "Parametria"
            modelBuilder.Entity<Parametria>(entity =>
            {
                entity.ToTable("parametria");
                entity.Property(pa => pa.Id).HasColumnName("id");
                entity.Property(pa => pa.NombreRegla).HasColumnName("nombre_regla");
                entity.Property(pa => pa.ValorRegla).HasColumnName("valor_regla");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

