using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TraductorOriginalUEA
{
    // Clase encargada de la lógica del diccionario
    public class MotorTraduccion
    {
        private Dictionary<string, string> palabras;

        public MotorTraduccion()
        {
            // Inicialización con CaseInsensitive para que ignore mayúsculas/minúsculas
            palabras = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            CargarPalabrasIniciales();
        }

        private void CargarPalabrasIniciales()
        {
            palabras.Add("Time", "tiempo");
            palabras.Add("Person", "persona");
            palabras.Add("Year", "año");
            palabras.Add("Day", "día");
            palabras.Add("Thing", "cosa");
            palabras.Add("Man", "hombre");
            palabras.Add("World", "mundo");
            palabras.Add("Life", "vida");
            palabras.Add("Hand", "mano");
            palabras.Add("Eye", "ojo");
        }

        public void AgregarNueva(string ingles, string espanol)
        {
            if (!string.IsNullOrEmpty(ingles) && !palabras.ContainsKey(ingles))
                palabras.Add(ingles, espanol);
        }

        public string ProcesarFrase(string frase)
        {
            if (string.IsNullOrEmpty(frase)) return string.Empty;

            string resultado = frase;
            foreach (var item in palabras)
            {
                // Traducir de Inglés a Español
                resultado = ReemplazarConPreservacion(resultado, item.Key, item.Value);
                // Traducir de Español a Inglés
                resultado = ReemplazarConPreservacion(resultado, item.Value, item.Key);
            }
            return resultado;
        }

        private string ReemplazarConPreservacion(string texto, string buscar, string reemplazar)
        {
            // El uso de \b asegura que solo se traduzcan palabras completas
            string patron = $@"\b{Regex.Escape(buscar)}\b";
            return Regex.Replace(texto, patron, reemplazar, RegexOptions.IgnoreCase);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MotorTraduccion traductor = new MotorTraduccion();
            string? opcion = "";

            while (opcion != "0")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n==================== MENÚ INTERACTIVO ====================");
                Console.ResetColor();
                Console.WriteLine("1. Traducir una frase");
                Console.WriteLine("2. Agregar palabras al diccionario");
                Console.WriteLine("0. Salir");
                Console.Write("\nSeleccione una opción: ");
                opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine("\nEscriba la frase a procesar:");
                        string entrada = Console.ReadLine() ?? "";
                        string salida = traductor.ProcesarFrase(entrada);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\nResultado: {salida}"); // Corregido: antes decía 'salidas'
                        Console.ResetColor();
                        break;

                    case "2":
                        Console.Write("Palabra en inglés: ");
                        string ing = Console.ReadLine() ?? "";
                        Console.Write("Palabra en español: ");
                        string esp = Console.ReadLine() ?? "";
                        traductor.AgregarNueva(ing, esp);
                        Console.WriteLine("✔ Palabra guardada exitosamente.");
                        break;
                }
            }
        }
    }
}