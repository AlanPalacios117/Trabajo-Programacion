using System;
using System.Collections.Generic;

namespace SistemaContableBar
{
    // Clase que representa una cuenta contable
    public class Cuenta
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; } // Activo, Pasivo, Capital, Ingreso, Gasto
        public decimal SaldoDebe { get; set; }
        public decimal SaldoHaber { get; set; }

        public Cuenta(string codigo, string nombre, string tipo)
        {
            Codigo = codigo;
            Nombre = nombre;
            Tipo = tipo;
            SaldoDebe = 0;
            SaldoHaber = 0;
        }

        public decimal ObtenerSaldoNeto()
        {
            if (Tipo == "Activo" || Tipo == "Gasto")
                return SaldoDebe - SaldoHaber;
            else
                return SaldoHaber - SaldoDebe;
        }
    }

    // Clase que representa una línea de movimiento dentro de un asiento
    public class DetalleAsiento
    {
        public Cuenta Cuenta { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }

        public DetalleAsiento(Cuenta cuenta, decimal debe, decimal haber)
        {
            Cuenta = cuenta;
            Debe = debe;
            Haber = haber;
        }
    }

    // Clase que representa un Asiento Contable en el Libro Diario
    public class AsientoContable
    {
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public List<DetalleAsiento> Detalles { get; set; }

        public AsientoContable(int numero, string descripcion)
        {
            Numero = numero;
            Fecha = DateTime.Now;
            Descripcion = descripcion;
            Detalles = new List<DetalleAsiento>();
        }
    }

    class Program
    {
        static List<Cuenta> catalogoCuentas = new List<Cuenta>();
        static List<AsientoContable> libroDiario = new List<AsientoContable>();

        static void Main(string[] args)
        {
            InicializarCatalogoPredeterminado();
            int opcion = 0;

            do
            {
                Console.Clear();
                Console.WriteLine("===========================================");
                Console.WriteLine("       SISTEMA CONTABLE EMPRESARIAL        ");
                Console.WriteLine("===========================================");
                Console.WriteLine("1. Administrar cuentas");
                Console.WriteLine("2. Registrar operación contable");
                Console.WriteLine("3. Consultar cuentas T");
                Console.WriteLine("4. Generar libro diario");
                Console.WriteLine("5. Generar libro mayor");
                Console.WriteLine("6. Generar balanza de comprobación");
                Console.WriteLine("7. Generar estado de resultados");
                Console.WriteLine("8. Generar balance general");
                Console.WriteLine("9. Consultar movimientos");
                Console.WriteLine("10. Respaldar información");
                Console.WriteLine("11. Salir");
                Console.WriteLine("===========================================");
                Console.Write("Seleccione una opción: ");

                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            AdministrarCuentas();
                            break;
                        case 2:
                            RegistrarOperacion();
                            break;
                        case 3:
                            ConsultarCuentasT();
                            break;
                        case 4:
                            GenerarLibroDiario();
                            break;
                        case 5:
                            GenerarLibroMayor();
                            break;
                        case 6:
                            GenerarBalanzaComprobacion();
                            break;
                        case 7:
                            GenerarEstadoResultados();
                            break;
                        case 8:
                            GenerarBalanceGeneral();
                            break;
                        case 9:
                            ConsultarMovimientos();
                            break;
                        case 10:
                            RespaldarInformacion();
                            break;
                        case 11:
                            Console.WriteLine("\nSaliendo del sistema...");
                            break;
                        default:
                            Console.WriteLine("\nOpción inválida. Intente de nuevo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nPor favor, ingrese un número válido.");
                }

                if (opcion != 11)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 11);
        }

        static void InicializarCatalogoPredeterminado()
        {
            // Cuentas basadas en el negocio del Bar
            catalogoCuentas.Add(new Cuenta("1101", "Caja General (Efectivo)", "Activo"));
            catalogoCuentas.Add(new Cuenta("1102", "Bancos (POS / Transferencias)", "Activo"));
            catalogoCuentas.Add(new Cuenta("1103", "Inventario de Licores y Cervezas", "Activo"));
            catalogoCuentas.Add(new Cuenta("2101", "Cuentas por Pagar Proveedores", "Pasivo"));
            catalogoCuentas.Add(new Cuenta("3101", "Capital Social", "Capital"));
            catalogoCuentas.Add(new Cuenta("4101", "Ingresos por Ventas (Bar y Cocina)", "Ingreso"));
            catalogoCuentas.Add(new Cuenta("5101", "Gastos de Salarios por Turno", "Gasto"));
            catalogoCuentas.Add(new Cuenta("5102", "Gastos de Comisiones POS", "Gasto"));
            catalogoCuentas.Add(new Cuenta("5103", "Gastos de Alquiler y Servicios", "Gasto"));
            catalogoCuentas.Add(new Cuenta("5104", "Gastos por Mermas y Cortesías", "Gasto"));
        }

        static void AdministrarCuentas()
        {
            Console.Clear();
            Console.WriteLine("--- CATÁLOGO DE CUENTAS CONTABLES ---");
            foreach (var c in catalogoCuentas)
            {
                Console.WriteLine($"[{c.Codigo}] {c.Nombre} ({c.Tipo})");
            }
        }

        static void RegistrarOperacion()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR NUEVA OPERACIÓN CONTABLE ---");
            Console.Write("Descripción del asiento (ej. Venta diaria del bar): ");
            string desc = Console.ReadLine();

            AsientoContable nuevoAsiento = new AsientoContable(libroDiario.Count + 1, desc);

            bool agregando = true;
            decimal totalDebe = 0;
            decimal totalHaber = 0;

            while (agregando)
            {
                Console.WriteLine("\nCuentas disponibles:");
                foreach (var c in catalogoCuentas)
                {
                    Console.WriteLine($"[{c.Codigo}] {c.Nombre}");
                }

                Console.Write("Ingrese el código de la cuenta a afectar: ");
                string cod = Console.ReadLine();
                Cuenta cuentaSeleccionada = catalogoCuentas.Find(x => x.Codigo == cod);

                if (cuentaSeleccionada != null)
                {
                    Console.Write("Monto a Débito: ");
                    decimal debe = decimal.Parse(Console.ReadLine());
                    Console.Write("Monto a Haber: ");
                    decimal haber = decimal.Parse(Console.ReadLine());

                    nuevoAsiento.Detalles.Add(new DetalleAsiento(cuentaSeleccionada, debe, haber));
                    cuentaSeleccionada.SaldoDebe += debe;
                    cuentaSeleccionada.SaldoHaber += haber;

                    totalDebe += debe;
                    totalHaber += haber;
                }
                else
                {
                    Console.WriteLine("Cuenta no encontrada.");
                }

                Console.Write("¿Desea agregar otra línea a este asiento? (s/n): ");
                if (Console.ReadLine().ToLower() != "s")
                {
                    agregando = false;
                }
            }

            if (totalDebe == totalHaber)
            {
                libroDiario.Add(nuevoAsiento);
                Console.WriteLine("\n¡Asiento registrado con éxito cumpliendo la partida doble!");
            }
            else
            {
                Console.WriteLine($"\nError: El asiento no cuadra. Total Debe ({totalDebe}) != Total Haber ({totalHaber}). Operación descartada.");
                // Revertir saldos temporales si no cuadra
                foreach(var d in nuevoAsiento.Detalles)
                {
                    d.Cuenta.SaldoDebe -= d.Debe;
                    d.Cuenta.SaldoHaber -= d.Haber;
                }
            }
        }

        static void ConsultarCuentasT()
        {
            Console.Clear();
            Console.WriteLine("--- CONSULTA DE CUENTAS T (MAYORIZACIÓN VISUAL) ---");
            foreach (var c in catalogoCuentas)
            {
                Console.WriteLine($"\nCuenta: {c.Codigo} - {c.Nombre} ({c.Tipo})");
                Console.WriteLine($"  DEBE (+)      |      HABER (-)");
                Console.WriteLine($"  {c.SaldoDebe,12:C2}  |  {c.SaldoHaber,12:C2}");
                Console.WriteLine($"  ---------------------------------");
                Console.WriteLine($"  Saldo Neto: {c.ObtenerSaldoNeto():C2}");
            }
        }

        static void GenerarLibroDiario()
        {
            Console.Clear();
            Console.WriteLine("--- LIBRO DIARIO ---");
            if (libroDiario.Count == 0)
            {
                Console.WriteLine("No hay operaciones registradas aún.");
                return;
            }

            foreach (var asiento in libroDiario)
            {
                Console.WriteLine($"\nAsiento #{asiento.Numero} - Fecha: {asiento.Fecha.ToShortDateString()}");
                Console.WriteLine($"Concepto: {asiento.Descripcion}");
                Console.WriteLine($"{"Cuenta",-30} | {"Debe",-10} | {"Haber",-10}");
                Console.WriteLine(new string('-', 56));
                foreach (var d in asiento.Detalles)
                {
                    Console.WriteLine($"{d.Cuenta.Nombre,-30} | {d.Debe,10:C2} | {d.Haber,10:C2}");
                }
            }
        }

        static void GenerarLibroMayor()
        {
            ConsultarCuentasT(); // Las cuentas T reflejan directamente el Libro Mayor
        }

        static void GenerarBalanzaComprobacion()
        {
            Console.Clear();
            Console.WriteLine("--- BALANZA DE COMPROBACIÓN ---");
            Console.WriteLine($"{"Código",-8} | {"Cuenta",-30} | {"Deudor",-12} | {"Acreedor",-12}");
            Console.WriteLine(new string('-', 68));

            decimal sumDeudor = 0;
            decimal sumAcreedor = 0;

            foreach (var c in catalogoCuentas)
            {
                decimal deudor = (c.Tipo == "Activo" || c.Tipo == "Gasto") ? c.ObtenerSaldoNeto() : 0;
                decimal acreedor = (c.Tipo == "Pasivo" || c.Tipo == "Capital" || c.Tipo == "Ingreso") ? c.ObtenerSaldoNeto() : 0;

                // Si manejas saldos netos generales:
                if (c.SaldoDebe > c.SaldoHaber)
                    deudor = c.SaldoDebe - c.SaldoHaber;
                else
                    acreedor = c.SaldoHaber - c.SaldoDebe;

                sumDeudor += deudor;
                sumAcreedor += acreedor;

                Console.WriteLine($"{c.Codigo,-8} | {c.Nombre,-30} | {deudor,12:C2} | {acreedor,12:C2}");
            }

            Console.WriteLine(new string('-', 68));
            Console.WriteLine($"{"TOTALES",-39} | {sumDeudor,12:C2} | {sumAcreedor,12:C2}");
        }

        static void GenerarEstadoResultados()
        {
            Console.Clear();
            Console.WriteLine("--- ESTADO DE RESULTADOS (PÉRDIDAS Y GANANCIAS) ---");
            decimal totalIngresos = 0;
            decimal totalGastos = 0;

            Console.WriteLine("\nINGRESOS:");
            foreach (var c in catalogoCuentas.FindAll(x => x.Tipo == "Ingreso"))
            {
                decimal saldo = c.ObtenerSaldoNeto();
                Console.WriteLine($"  {c.Nombre}: {saldo:C2}");
                totalIngresos += saldo;
            }

            Console.WriteLine("\nGASTOS:");
            foreach (var c in catalogoCuentas.FindAll(x => x.Tipo == "Gasto"))
            {
                decimal saldo = c.ObtenerSaldoNeto();
                Console.WriteLine($"  {c.Nombre}: {saldo:C2}");
                totalGastos += saldo;
            }

            decimal utilidadNeta = totalIngresos - totalGastos;
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"TOTAL INGRESOS: {totalIngresos:C2}");
            Console.WriteLine($"TOTAL GASTOS:   {totalGastos:C2}");
            Console.WriteLine($"UTILIDAD NETA:  {utilidadNeta:C2}");
        }

        static void GenerarBalanceGeneral()
        {
            Console.Clear();
            Console.WriteLine("--- BALANCE GENERAL (SITUACIÓN FINANCIERA) ---");
            decimal totalActivos = 0;
            decimal totalPasivos = 0;
            decimal totalCapital = 0;

            Console.WriteLine("\nACTIVOS:");
            foreach (var c in catalogoCuentas.FindAll(x => x.Tipo == "Activo"))
            {
                decimal saldo = c.ObtenerSaldoNeto();
                Console.WriteLine($"  {c.Nombre}: {saldo:C2}");
                totalActivos += saldo;
            }

            Console.WriteLine("\nPASIVOS:");
            foreach (var c in catalogoCuentas.FindAll(x => x.Tipo == "Pasivo"))
            {
                decimal saldo = c.ObtenerSaldoNeto();
                Console.WriteLine($"  {c.Nombre}: {saldo:C2}");
                totalPasivos += saldo;
            }

            Console.WriteLine("\nPATRIMONIO:");
            foreach (var c in catalogoCuentas.FindAll(x => x.Tipo == "Capital"))
            {
                decimal saldo = c.ObtenerSaldoNeto();
                Console.WriteLine($"  {c.Nombre}: {saldo:C2}");
                totalCapital += saldo;
            }

            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"TOTAL ACTIVOS: {totalActivos:C2}");
            Console.WriteLine($"TOTAL PASIVO + PATRIMONIO: {(totalPasivos + totalCapital):C2}");
        }

        static void ConsultarMovimientos()
        {
            GenerarLibroDiario();
        }

        static void RespaldarInformacion()
        {
            Console.Clear();
            Console.WriteLine("--- RESPALDO DE INFORMACIÓN ---");
            Console.WriteLine("Cuentas y transacciones almacenadas correctamente en memoria.");
            Console.WriteLine("¡Respaldo simulado con éxito!");
        }
    }
}