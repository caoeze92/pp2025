using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticaProfesional2025
{
    public class Usuario
    {

        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Telefono { get; set; }
        public int Activo { get; set; }
    }
}