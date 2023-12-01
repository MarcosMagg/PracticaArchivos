using PracticaArchivos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using PracticaArchivos.Repository;
using PracticaArchivos.Domain;
using PracticaArchivos.Repository;


namespace PracticaArchivos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\Dell\Desktop\CDA practicas\data.txt";
            string[] data = File.ReadAllLines(path);//devuelve un string,separa por salto de linea en array 

            foreach (string line in data)//solo para ver el archivo
            {
                Console.WriteLine(line);

            }
            string stringConnection = @"Data Source=localhost;Initial Catalog=PracticaArchivo;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(stringConnection).Options;
            //string de conexion DBtextopctions
            var context = new DatabaseContext(options);

        }
        }     
    }

