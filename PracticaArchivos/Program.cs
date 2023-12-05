using PracticaArchivos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using PracticaArchivos.Repository;
using PracticaArchivos.Domain;


namespace PracticaArchivos
{
    public class Program
    {


        private static readonly string CODIGO_FECHA_PROCESO = "fecha_proceso";//variable de lectura que usuamos
        //en este caso para buscar en el resultParametry
        public delegate void imprimirCustom(string path);


        static void Main(string[] args)
        {

            /*
            string path = @"C:\Users\Dell\Desktop\CDA practicas\data.txt";
            string[] data = File.ReadAllLines(path);//devuelve un string,separa por salto de linea en array 

            foreach (string line in data)//solo para ver el archivo
            {
                Console.WriteLine(line);

            }*/
             

            //conectamos al sql
            var optionsBuilder = new DbContextOptionsBuilder<DataBaseContext>().UseSqlServer(@"Data Source=localhost;Initial Catalog=PracticaArchivo;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
            //string de conexion DBtextopctions
            var context = new DataBaseContext(optionsBuilder.Options);

            string resultParametria = context.Parametria.Where( w=> w.NombreRegla.Equals(CODIGO_FECHA_PROCESO)).First().ValorRegla;//agarramos el primer valor que venga, traemos solo el ValorRegla
            DateTime fechaProceso = DateTime.ParseExact(resultParametria, "yyyy-MM-dd", CultureInfo.InvariantCulture);//formateamos como viene la fecha

        
            
            //archivos
            string path = @"C:\Users\Dell\Desktop\CDA practicas\data.txt";
            string[] lines = File.ReadAllLines(path);//devuelve un string,separa por salto de linea en array 





            //procesamos el archivo
            foreach( string line in lines )
            {
                Rechazos? rechazos = BuildRechazos(line,fechaProceso);
                if( rechazos != null)
                {
                    context.Rechazos.Add(rechazos);//lo agrega a la base de datos cuando termine
                }
                else
                {
                    VentasMensuales ventasMensuales = BuildVentasMensuales(line);
                    context.VentasMensuales.Add(ventasMensuales);
                }
            }

            context.SaveChanges();
            bool exit = false;

            while( !exit )
            {
                Console.Clear();
                Console.WriteLine("Por favor elija una opcion");

                Console.WriteLine("1.Mostar rechazos");               
                Console.WriteLine("2.Lista de vendedores que superaron los  100.000 en el mes");
                Console.WriteLine("3.Lista vendedores qu eno hayan superado el umbral");
                Console.WriteLine("4.Lista de vendedores con al menos 1 venta a empresa grande");

                int inputUsuario = int.Parse(Console.ReadLine()!);

                if (inputUsuario == 1)
                {
                    Console.Clear();
                    var listRechazos = context.Rechazos.ToList();//los pasamos a una lista

                    foreach(var rechazo in listRechazos) {
                        Console.WriteLine($"{rechazo.MotivoRechazos}");
                    }
                }
                if (inputUsuario == 2)
                {
                    Console.Clear();

                    var lstVendedores = context.VentasMensuales.GroupBy(g=>g.CodVendedor).
                                                                Where(w=>w.ToList().Sum(s=>s.MontoVenta) >= 100000m).
                                                                Select(s=>new {s.Key,totalVenta = s.ToList().Sum(s => s.MontoVenta)}).ToList();    
                                                    
                   if(lstVendedores.Count == 0) Console.WriteLine("No hay vendedores que hayan superado el umbral");

                    if (lstVendedores.Count > 0)
                    {
                        lstVendedores.ForEach(f => Console.WriteLine($"El vendedor {f.Key} vendio {f.totalVenta}"));
                    }
                }
                if(inputUsuario == 3)
                {
                    Console.Clear();

                    var lstVendedores = context.VentasMensuales.GroupBy(g => g.CodVendedor).
                                                                Where(w => w.ToList().Sum(s => s.MontoVenta) <= 100000m).
                                                                Select(s => new { s.Key, totalVenta = s.ToList().Sum(s => s.MontoVenta) }).ToList();

                    if (lstVendedores.Count == 0) Console.WriteLine("No hay vendedores que no hayan superado el umbral");

                    if (lstVendedores.Count > 0)
                    {
                        lstVendedores.ForEach(f => Console.WriteLine($"El vendedor {f.Key} vendio {f.totalVenta}"));
                    }

                }
                if(inputUsuario == 4)
                {
                    Console.Clear();
                    var lstVendedores = context.VentasMensuales.GroupBy(g => g.CodVendedor).
                                                               Where(w => w.ToList().Any(a=>a.McaEmpresaGrande)).
                                                               Select(s => new { s.Key, totalVenta = s.ToList().Sum(s => s.MontoVenta) }).ToList();

                    if (lstVendedores.Count == 0) Console.WriteLine("No hay vendedores que no hayan vendido a empresas grandes");

                    if (lstVendedores.Count > 0)
                    {
                        lstVendedores.ForEach(f => Console.WriteLine($"{f.Key}"));
                    }
                }

            }
        }








        private static Rechazos? BuildRechazos(string data, DateTime fechaProceso)
        {
            string[] marcasPermitidas = new string[] {"S","N"};
            StringBuilder rechazos = new StringBuilder();//stringBuilder permite construir un string agregando lineas

            string fechaProcesoOriginal = data.Substring(0,8).Trim();//con el subString recortamos lineas
            string codVendedorOriginal = data.Substring(8,3).Trim();
            string ventaOriginal = data.Substring(11,11).Trim();
            string mcaEmpresaOriginal = data.Substring(22,1).Trim();

            bool flagConvertionFechaProceso = DateTime.TryParseExact(fechaProcesoOriginal, "yyyyMMdd", CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out DateTime fechaArchivo);//boleano que indica si pudo parsear la fecha bien o no

            if(flagConvertionFechaProceso && fechaArchivo != fechaProceso) { rechazos.AppendLine("Fecha de proceso incorrecta"); }
            if(string.IsNullOrEmpty(fechaProcesoOriginal)) { rechazos.AppendLine("Codigo de vendedor incorrecto"); }
            if (!marcasPermitidas.Contains(mcaEmpresaOriginal)) { rechazos.AppendLine("Marca empresa grande incorrecta"); }
            //si no esta incluido marcaspermitas en mcaEmpresa origninal lo rechaza


            if(string.IsNullOrEmpty(rechazos.ToString())) { return null; }
            
            var response = new Rechazos() { MotivoRechazos= rechazos.ToString() };//a motivos rechazos se le asigna el vlaor que fuimos contruyendo
            return response;
        } 
        
        private static VentasMensuales BuildVentasMensuales(string data)
        {
            string fechaProcesoOriginal = data.Substring(0, 8).Trim();//con el subString recortamos lineas
            string codVendedorOriginal = data.Substring(8, 3).Trim();
            string ventaOriginal = data.Substring(11, 11).Trim();
            string mcaEmpresaOriginal = data.Substring(22, 1).Trim();

            DateTime fechaProceso = DateTime.ParseExact(fechaProcesoOriginal, "yyyyMMdd", CultureInfo.InvariantCulture);
            
            decimal montoVenta = decimal.Parse(ventaOriginal, CultureInfo.InvariantCulture);

            return new VentasMensuales()
            {
                FechaProceso = fechaProceso,
                CodVendedor = codVendedorOriginal,
                MontoVenta = montoVenta,
                McaEmpresaGrande = mcaEmpresaOriginal == "S"
            };
        }


    }     
    }

