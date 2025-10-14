using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PracticaProfesional2025; 

namespace PracticaProfesional2025
{
    [Serializable]
    public class Computadora
    {
         // Constructor
        public Computadora()
        {
            Componentes = new List<ComputadoraComponente>();
        }

        public int IdComputadora { get; set; }
        public int IdLaboratorio { get; set; }
        public string CodigoInventario { get; set; }
        public string NumeroSerie { get; set; }
        public string Descripcion { get; set; }
        public string EstadoActual { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }

        // Lista de componentes asociados
        public List<ComputadoraComponente> Componentes { get; set; } 
    }
}

 