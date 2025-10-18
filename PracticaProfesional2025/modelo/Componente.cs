using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PracticaProfesional2025; 

namespace PracticaProfesional2025
{
    [Serializable]
    public class Componente
    {
        // Constructor
        public Componente()
        {
            Computadoras = new List<ComputadoraComponente>();
        }

        public int Id_Componente { get; set; }
        public string Tipo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Caracteristicas { get; set; }
        public string Numero_Serie { get; set; }
        public int Estado_Id { get; set; }
        public DateTime Fecha_Compra { get; set; }


        // Lista de computadoras asociadas (muchos a muchos)
        public List<ComputadoraComponente> Computadoras { get; set; }

       
    }
}