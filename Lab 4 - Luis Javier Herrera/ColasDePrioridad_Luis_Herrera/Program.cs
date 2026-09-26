namespace ColasDePrioridad_Luis_Herrera
{
    class Program
    {
        // Este es el heap donde viven todos los tickets mientras el programa esté abierto
        static BaulTickets montonPrincipal = new BaulTickets();

        static void Main(string[] args)
        {

            bool seguirEjecutando = true;

            while (seguirEjecutando)
            {
                LimpiarPantalla();

                Console.WriteLine("====================================================");
                Console.WriteLine("      SISTEMA DE GESTIÓN DE TICKETS DE SOPORTE");
                Console.WriteLine("                 COLA DE PRIORIDAD");
                Console.WriteLine("====================================================");
                Console.WriteLine();
                Console.WriteLine("[1] Registrar Ticket");
                Console.WriteLine("[2] Mostrar Siguiente Ticket");
                Console.WriteLine("[3] Atender Ticket");
                Console.WriteLine("[4] Mostrar Cola de Prioridad");
                Console.WriteLine("[5] Buscar Ticket");
                Console.WriteLine("[6] Mostrar Cantidad de Tickets");
                Console.WriteLine("[7] Salir");
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------");
                Console.Write("Seleccione una opción: ");

                string opcionElegida = LeerLinea();

                switch (opcionElegida)
                {
                    // Registrar Ticket
                    case "1":
                        LimpiarPantalla();
                        Console.WriteLine("========================================");
                        Console.WriteLine("           REGISTRO DE TICKET");
                        Console.WriteLine("========================================");
                        Console.WriteLine();

                        string codigoIngresado = "";
                        bool codigoListo = false;

                        while (!codigoListo)
                        {
                            Console.Write("Código del Ticket (TCK + 4 dígitos, 0 para cancelar): ");
                            codigoIngresado = LeerLinea().ToUpper();

                            if (codigoIngresado == "0")
                            {
                                Console.WriteLine();
                                Console.WriteLine("Registro cancelado.");
                                EsperarEnter();
                                return;
                            }

                            if (!CodigoTieneFormatoCorrecto(codigoIngresado))
                            {
                                Console.WriteLine("  Código inválido. Debe ser TCK seguido de 4 dígitos (ejemplo: TCK0005).");
                            }
                            else if (montonPrincipal.ExisteCodigo(codigoIngresado))
                            {
                                Console.WriteLine("  Ese código ya está registrado. Ingrese uno diferente.");
                            }
                            else
                            {
                                codigoListo = true;
                            }
                        }

                        // Cliente Ticket
                        string clienteIngresado = "";
                        while (clienteIngresado == "")
                        {
                            Console.Write("Cliente: ");
                            clienteIngresado = LeerLinea();
                            if (clienteIngresado == "")
                            {
                                Console.WriteLine("  El nombre del cliente no puede estar vacío.");
                            }
                        }

                        // Descripcion ticket
                        string descripcionIngresada = "";
                        while (descripcionIngresada == "")
                        {
                            Console.Write("Descripción: ");
                            descripcionIngresada = LeerLinea();
                            if (descripcionIngresada == "")
                            {
                                Console.WriteLine("  La descripción no puede estar vacía.");
                            }
                        }

                        // Prioridad Ticket
                        Console.WriteLine("Niveles: 1 = Crítica, 2 = Alta, 3 = Media, 4 = Baja, 5 = Muy Baja");
                        int prioridadIngresada = 0;
                        bool prioridadLista = false;

                        while (!prioridadLista)
                        {
                            Console.Write("Prioridad (1-5): ");
                            string textoPrioridad = LeerLinea();

                            bool esNumero = int.TryParse(textoPrioridad, out prioridadIngresada);

                            if (esNumero && prioridadIngresada >= 1 && prioridadIngresada <= 5)
                            {
                                prioridadLista = true;
                            }
                            else
                            {
                                Console.WriteLine("  Prioridad inválida. Debe ser un número entre 1 y 5.");
                            }
                        }

                        // Crear ticket
                        SolicitudSoporte ticketNuevo = new SolicitudSoporte(codigoIngresado, clienteIngresado, descripcionIngresada, prioridadIngresada);
                        montonPrincipal.Agregar(ticketNuevo);

                        // ver datos ticket
                        Console.WriteLine();
                        Console.WriteLine("----------------------------------------");
                        Console.WriteLine();
                        MostrarDatosDelTicket(ticketNuevo);
                        Console.WriteLine();
                        Console.WriteLine("----------------------------------------");
                        Console.WriteLine();
                        Console.WriteLine("Ticket registrado exitosamente.");
                        Console.WriteLine();
                        Console.WriteLine("----------------------------------------");

                        EsperarEnter();

                        break;

                    // Ver siguiente ticket
                    case "2":
                        LimpiarPantalla();
                        Console.WriteLine("========================================");
                        Console.WriteLine("       SIGUIENTE TICKET A ATENDER");
                        Console.WriteLine("========================================");
                        Console.WriteLine();

                        SolicitudSoporte siguiente = montonPrincipal.VerRaiz();

                        if (siguiente == null)
                        {
                            Console.WriteLine("No hay tickets en la cola de prioridad.");
                        }
                        else
                        {
                            Console.WriteLine("RESULTADO DE LA CONSULTA");
                            Console.WriteLine("----------------------------------------");
                            Console.WriteLine();
                            MostrarDatosDelTicket(siguiente);
                            Console.WriteLine();
                            Console.WriteLine("----------------------------------------");
                        }

                        EsperarEnter();

                        break;

                    // Atender ticket
                    case "3":
                        LimpiarPantalla();
                        Console.WriteLine("========================================");
                        Console.WriteLine("             ATENDER TICKET");
                        Console.WriteLine("========================================");
                        Console.WriteLine();

                        // Elimina el ticket mas urgente
                        SolicitudSoporte atendido = montonPrincipal.SacarRaiz();

                        if (atendido == null)
                        {
                            Console.WriteLine("No hay tickets para atender. La cola está vacía.");
                        }
                        else
                        {
                            Console.WriteLine("RESULTADO DE LA OPERACIÓN");
                            Console.WriteLine("----------------------------------------");
                            Console.WriteLine();
                            MostrarDatosDelTicket(atendido);
                            Console.WriteLine();
                            Console.WriteLine("----------------------------------------");
                            Console.WriteLine();
                            Console.WriteLine("Ticket atendido correctamente.");
                            Console.WriteLine();
                            Console.WriteLine("----------------------------------------");
                        }

                        EsperarEnter();

                        break;

                    // Mostrar cola de prioridad
                    case "4":
                        LimpiarPantalla();
                        Console.WriteLine("========================================");
                        Console.WriteLine("        COLA DE PRIORIDAD ACTUAL");
                        Console.WriteLine("========================================");
                        Console.WriteLine();

                        if (montonPrincipal.Cantidad == 0)
                        {
                            Console.WriteLine("No hay tickets registrados en la cola de prioridad.");
                        }
                        else
                        {
                            // Encabezado de la tabla
                            Console.WriteLine("Posición".PadRight(10) + "Ticket".PadRight(10) + "Cliente".PadRight(22) + "Prioridad");
                            Console.WriteLine("--------------------------------------------------");
                            Console.WriteLine();

                            // Recorrer arreglo
                            for (int i = 0; i < montonPrincipal.Cantidad; i++)
                            {
                                SolicitudSoporte t = montonPrincipal.ObtenerEnPosicion(i);

                                Console.WriteLine(
                                    i.ToString().PadRight(10) +
                                    t.Codigo.PadRight(10) +
                                    RecortarTexto(t.NombreCliente, 20).PadRight(22) +
                                    t.NivelUrgencia);
                            }

                            Console.WriteLine();
                            Console.WriteLine("--------------------------------------------------");
                            Console.WriteLine("Total de Tickets: " + montonPrincipal.Cantidad);
                        }

                        EsperarEnter();

                        break;

                    // Buscar Ticket
                    case "5":
                        LimpiarPantalla();
                        Console.WriteLine("========================================");
                        Console.WriteLine("           BÚSQUEDA DE TICKET");
                        Console.WriteLine("========================================");
                        Console.WriteLine();

                        Console.Write("Ingrese el código del ticket: ");
                        string codigoABuscar = LeerLinea().ToUpper();
                        Console.WriteLine();

                        SolicitudSoporte encontrado = montonPrincipal.BuscarPorCodigo(codigoABuscar);

                        if (encontrado == null)
                        {
                            Console.WriteLine("No se encontró ningún ticket con el código \"" + codigoABuscar + "\".");
                        }
                        else
                        {
                            Console.WriteLine("RESULTADO DE LA BÚSQUEDA");
                            Console.WriteLine("----------------------------------------");
                            Console.WriteLine();
                            MostrarDatosDelTicket(encontrado);
                            Console.WriteLine();
                            Console.WriteLine("----------------------------------------");
                        }

                        EsperarEnter();

                        break;

                    // Cantidad tickets
                    case "6":
                        LimpiarPantalla();
                        Console.WriteLine("========================================");
                        Console.WriteLine("          CANTIDAD DE TICKETS");
                        Console.WriteLine("========================================");
                        Console.WriteLine();

                        Console.WriteLine("TOTAL DE TICKETS REGISTRADOS");
                        Console.WriteLine("----------------------------------------");
                        Console.WriteLine();
                        Console.WriteLine("Cantidad de Tickets : " + montonPrincipal.Cantidad);
                        Console.WriteLine();
                        Console.WriteLine("----------------------------------------");

                        EsperarEnter();

                        break;

                    // Salir
                    case "7":
                        seguirEjecutando = false;
                        Console.WriteLine();
                        Console.WriteLine("Gracias por contactar con soporte");
                        break;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Opción no válida. Debe elegir un número del 1 al 7.");
                        EsperarEnter();
                        break;
                }
            }
        }

        static void MostrarDatosDelTicket(SolicitudSoporte t)
        {
            // Espacio de separacion
            int anchoEtiqueta = 18;

            Console.WriteLine("Código del Ticket".PadRight(anchoEtiqueta) + ": " + t.Codigo);
            Console.WriteLine("Cliente".PadRight(anchoEtiqueta) + ": " + t.NombreCliente);
            Console.WriteLine("Descripción".PadRight(anchoEtiqueta) + ": " + t.DescripcionProblema);
            Console.WriteLine("Prioridad".PadRight(anchoEtiqueta) + ": " + t.NivelConNombre());
        }

        // Formato de coigo de ticket
        static bool CodigoTieneFormatoCorrecto(string codigo)
        {
            // Debe medir exactamente 7 caracteres y empezar con TCK
            if (codigo.Length != 7 || !codigo.StartsWith("TCK"))
            {
                return false;
            }

            for (int i = 3; i < 7; i++)
            {
                if (codigo[i] < '0' || codigo[i] > '9')
                {
                    return false;
                }
            }
            return true;
        }

        // Leer lina en consola
        static string LeerLinea()
        {
            string texto = Console.ReadLine();
            if (texto == null)
            {
                Environment.Exit(0);
            }
            return texto.Trim();
        }

        static void EsperarEnter()
        {
            Console.WriteLine();
            Console.Write("Presione ENTER para volver al menú...");
            Console.ReadLine();
        }

        static void LimpiarPantalla()
        {
            try
            {
                Console.Clear();
            }
            catch (Exception)
            {}
        }

        static string RecortarTexto(string texto, int maximo)
        {
            if (texto.Length <= maximo)
            {
                return texto;
            }
            return texto.Substring(0, maximo - 3) + "...";
        }
    }

        // Solicitud soporte
    public class SolicitudSoporte
    {
        // Código del ticket
        public string Codigo { get; set; }

        // Nombre cliente
        public string NombreCliente { get; set; }

        // Descripcion
        public string DescripcionProblema { get; set; }

        // Nivel de prioridad
        public int NivelUrgencia { get; set; }

        // Orden llegada
        public int OrdenLlegada { get; set; }

        // Creacion ticket
        public SolicitudSoporte(string codigo, string nombreCliente, string descripcionProblema, int nivelUrgencia)
        {
            Codigo = codigo;
            NombreCliente = nombreCliente;
            DescripcionProblema = descripcionProblema;
            NivelUrgencia = nivelUrgencia;
            OrdenLlegada = 0;
        }

        // Nivel segun prioridad
        public string NombreDelNivel()
        {
            switch (NivelUrgencia)
            {
                case 1: return "Crítica";
                case 2: return "Alta";
                case 3: return "Media";
                case 4: return "Baja";
                case 5: return "Muy Baja";
                default: return "Desconocida";
            }
        }

        public string NivelConNombre()
        {
            return NivelUrgencia + " (" + NombreDelNivel() + ")";
        }
    }


    // Guardar tickets por prioridad
    public class BaulTickets
    {
        // Arreglo donde se guardan los tickets
        private SolicitudSoporte[] casillas;

        // Cantidad de tickets que hay guardados
        private int totalGuardados;

        // Contador para saber en qué orden llega cada ticket
        private int contadorLlegadas;

        // cuantos tickets guardados hay
        public int Cantidad
        {
            get { return totalGuardados; }
        }

        // Espacio para 10 tickets
        public BaulTickets()
        {
            casillas = new SolicitudSoporte[10];
            totalGuardados = 0;
            contadorLlegadas = 0;
        }

        // Ijnsertar ticket
        public void Agregar(SolicitudSoporte nuevoTicket)
        {
            if (totalGuardados == casillas.Length)
            {
                DuplicarEspacio();
            }

            // Se anota el orden de llegada del ticket
            contadorLlegadas++;
            nuevoTicket.OrdenLlegada = contadorLlegadas;

            // 1) El ticket se coloca en la siguiente posición libre del árbol y se mueve segun prioridad
            casillas[totalGuardados] = nuevoTicket;
            totalGuardados++;
            SubirElemento(totalGuardados - 1);
        }

        // ver tickets
        public SolicitudSoporte VerRaiz()
        {
            if (totalGuardados == 0)
            {
                return null;
            }
            return casillas[0];
        }

        // Cambia el nodo raiz de ser necesario
        public SolicitudSoporte SacarRaiz()
        {
            if (totalGuardados == 0)
            {
                return null;
            }

            SolicitudSoporte ticketAtendido = casillas[0];

            // El último nodo del heap pasa a ocupar el lugar de la raíz
            casillas[0] = casillas[totalGuardados - 1];
            casillas[totalGuardados - 1] = null;
            totalGuardados--;

            if (totalGuardados > 0)
            {
                BajarElemento(0);
            }

            return ticketAtendido;
        }

        // Subir ticket si es mas urgente
        private void SubirElemento(int posicion)
        {
            while (posicion > 0)
            {
                int posPadre = (posicion - 1) / 2;

                if (EsMasUrgente(casillas[posicion], casillas[posPadre]))
                {
                    IntercambiarCasillas(posicion, posPadre);
                    posicion = posPadre;
                }
                else
                {
                    break;
                }
            }
        }

        private void BajarElemento(int posicion)
        {
            while (true)
            {
                int posIzq = 2 * posicion + 1;
                int posDer = 2 * posicion + 2;
                int posMasUrgente = posicion; 

                if (posIzq < totalGuardados && EsMasUrgente(casillas[posIzq], casillas[posMasUrgente]))
                {
                    posMasUrgente = posIzq;
                }

                if (posDer < totalGuardados && EsMasUrgente(casillas[posDer], casillas[posMasUrgente]))
                {
                    posMasUrgente = posDer;
                }

                if (posMasUrgente == posicion)
                {
                    break;
                }

                IntercambiarCasillas(posicion, posMasUrgente);
                posicion = posMasUrgente;
            }
        }

        // Comparacion entre niveles de urgencia
        private bool EsMasUrgente(SolicitudSoporte primero, SolicitudSoporte segundo)
        {
            if (primero.NivelUrgencia != segundo.NivelUrgencia)
            {
                return primero.NivelUrgencia < segundo.NivelUrgencia;
            }
            return primero.OrdenLlegada < segundo.OrdenLlegada;
        }

        // Intercambia dos tickets de lugar
        private void IntercambiarCasillas(int a, int b)
        {
            SolicitudSoporte auxiliar = casillas[a];
            casillas[a] = casillas[b];
            casillas[b] = auxiliar;
        }

        // Crea un arreglo del doble de tamaño y copia los tickets al nuevo
        private void DuplicarEspacio()
        {
            SolicitudSoporte[] arregloGrande = new SolicitudSoporte[casillas.Length * 2];
            for (int i = 0; i < totalGuardados; i++)
            {
                arregloGrande[i] = casillas[i];
            }
            casillas = arregloGrande;
        }

        // Devuelve el ticket que está en una posición del arreglo
        public SolicitudSoporte ObtenerEnPosicion(int posicion)
        {
            if (posicion < 0 || posicion >= totalGuardados)
            {
                return null;
            }
            return casillas[posicion];
        }

        // Busca un ticket por su código. Devuelve null si no existe.
        public SolicitudSoporte BuscarPorCodigo(string codigoBuscado)
        {
            for (int i = 0; i < totalGuardados; i++)
            {
                if (casillas[i].Codigo == codigoBuscado)
                {
                    return casillas[i];
                }
            }
            return null;
        }

        // Comprueba que no existan dupicados
        public bool ExisteCodigo(string codigoBuscado)
        {
            return BuscarPorCodigo(codigoBuscado) != null;
        }
    }
}