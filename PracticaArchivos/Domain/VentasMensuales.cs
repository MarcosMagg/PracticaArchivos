using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PracticaArchivos.Domain
{

        public class VentasMensuales
        {
            public DateTime FechaDelInforme { get; set; }
            public char CodigoDelVendedor { get; set; }
            public int Venta { get; set; }
            public bool VentaEmpresaGrande { get; set; }

        }

        public class Rechazos
        {
            public int IdRechazo { get; set; }
            public string Motivo { get; set; }
        }

        public class Parametria
        {
            public DateTime Fecha { get; set; }
        }
    }

