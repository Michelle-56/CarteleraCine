using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace CarteleraCine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FuncionRepositorio repositorio = new FuncionRepositorio();
            int opcion = 0;

            do
            {
                Console.WriteLine();
                Console.WriteLine("--- CARTELERA DE CINE ---");
                Console.WriteLine("1. Registrar función");
                Console.WriteLine("2. Listar funciones");
                Console.WriteLine("3. Buscar función por Id");
                Console.WriteLine("4. Actualizar precio");
                Console.WriteLine("5. Eliminar función");
                Console.WriteLine("6. Salir");
                Console.Write("Seleccione una opción: ");

                string? entradaOpcion = Console.ReadLine();
                if (!int.TryParse(entradaOpcion, out opcion))
                {
                    opcion = 0;
                }

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            RegistrarFuncion(repositorio);
                            break;
                        case 2:
                            ListarFunciones(repositorio);
                            break;
                        case 3:
                            BuscarFuncionPorId(repositorio);
                            break;
                        case 4:
                            ActualizarPrecio(repositorio);
                            break;
                        case 5:
                            EliminarFuncion(repositorio);
                            break;
                        case 6:
                            break;
                        default:
                            Console.WriteLine("Opción inválida.");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error de base de datos: " + ex.Message);
                }

            } while (opcion != 6);
        }

        private static void RegistrarFuncion(FuncionRepositorio repositorio)
        {
            Console.Write("Película: ");
            string? pelicula = Console.ReadLine();

            Console.Write("Sala: ");
            string? sala = Console.ReadLine();

            Console.Write("Precio: ");
            string? entradaPrecio = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(pelicula) ||
                string.IsNullOrWhiteSpace(sala) ||
                !decimal.TryParse(entradaPrecio, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal precio) ||
                precio <= 0)
            {
                Console.WriteLine("Datos inválidos.");
                return;
            }

            Funcion nueva = new Funcion
            {
                Pelicula = pelicula.Trim(),
                Sala = sala.Trim(),
                Precio = precio
            };

            repositorio.Insertar(nueva);
            Console.WriteLine("Función registrada.");
        }

        private static void ListarFunciones(FuncionRepositorio repositorio)
        {
            List<Funcion> funciones = repositorio.ListarTodos();

            if (funciones.Count == 0)
            {
                Console.WriteLine("No hay funciones registradas.");
                return;
            }

            decimal total = 0;
            foreach (Funcion f in funciones)
            {
                Console.WriteLine(f.ToString());
                total += f.Precio;
            }

            Console.WriteLine($"TOTAL DE PRECIOS: Q {total:F2}");
        }

        private static void BuscarFuncionPorId(FuncionRepositorio repositorio)
        {
            Console.Write("Id: ");
            string? entradaId = Console.ReadLine();

            if (!int.TryParse(entradaId, out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            Funcion? encontrada = repositorio.BuscarPorId(id);
            if (encontrada == null)
            {
                Console.WriteLine("No existe ese Id.");
            }
            else
            {
                Console.WriteLine(encontrada.ToString());
            }
        }

        private static void ActualizarPrecio(FuncionRepositorio repositorio)
        {
            Console.Write("Id: ");
            string? entradaId = Console.ReadLine();

            if (!int.TryParse(entradaId, out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            Funcion? existente = repositorio.BuscarPorId(id);
            if (existente == null)
            {
                Console.WriteLine("No existe ese Id.");
                return;
            }

            Console.Write("Nuevo precio: ");
            string? entradaPrecio = Console.ReadLine();

            if (!decimal.TryParse(entradaPrecio, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal nuevoPrecio) ||
                nuevoPrecio <= 0)
            {
                Console.WriteLine("Precio inválido.");
                return;
            }

            existente.Precio = nuevoPrecio;
            bool actualizado = repositorio.Actualizar(existente);

            if (actualizado)
            {
                Console.WriteLine("Función actualizada.");
            }
            else
            {
                Console.WriteLine("No existe ese Id.");
            }
        }

        private static void EliminarFuncion(FuncionRepositorio repositorio)
        {
            Console.Write("Id: ");
            string? entradaId = Console.ReadLine();

            if (!int.TryParse(entradaId, out int id))
            {
                Console.WriteLine("Id inválido.");
                return;
            }

            bool eliminado = repositorio.Eliminar(id);
            if (eliminado)
            {
                Console.WriteLine("Función eliminada.");
            }
            else
            {
                Console.WriteLine("No existe ese Id.");
            }
        }
    }
}