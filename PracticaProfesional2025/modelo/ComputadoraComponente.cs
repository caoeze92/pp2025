using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PracticaProfesional2025; 

namespace PracticaProfesional2025
{
    [Serializable]
    public class ComputadoraComponente
    {
        public int IdComputadora { get; set; }
        public Computadora Computadora { get; set; }

        public int IdComponente { get; set; }
        public Componente Componente { get; set; }
    }
}