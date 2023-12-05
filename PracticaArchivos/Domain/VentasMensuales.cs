using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PracticaArchivos.Domain
{

        public class VentasMensuales
        {
            public DateTime FechaProceso { get; set; }
            public string CodVendedor { get; set; }
            public decimal MontoVenta { get; set; }
            public int Id { get; set; }
            public bool McaEmpresaGrande { get; set; }

        }


        public class Rechazos
        {
        public int Id { get; set; }
        public string MotivoRechazos {  get; set; }

        }


        public class Parametria
        {
        public int Id { get; set; }
        public string? NombreRegla { get; set; }
        public string? ValorRegla { get; set; }
        }
    }

