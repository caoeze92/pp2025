using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Diagnostics;

namespace PracticaProfesional2025
{
    public partial class CrearUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
                string script = "INSERT INTO USUARIOS (usuario, password) VALUES (@usuario, @password)";
                    
                    SqlCommand command = new SqlCommand(script, conexion);
                    conexion.Open();
                    command.Parameters.AddWithValue("@usuario", ntxtUsuario.Text);
                    command.Parameters.AddWithValue("@password", ntxtPassword.Text);                    
                    int resultado = command.ExecuteNonQuery();
                    if (resultado < 0)
                    {
                        conexion.Close();
                        Console.WriteLine("Error al crear la cuenta en la db");
                        
                        }
                    else {
                        Console.WriteLine("Cuenta {0} creada exitosamente", ntxtUsuario.Text);
                        Response.Redirect("Inicio.aspx", false);
                    }
                 //String id = String.Empty;
                //if (id != String.Empty)
                //{
                //    Session["Usuario"] = ntxtUsuario.Text;

                //    //Redireccionarlo ala pagina correcta

                //}
            }
        }
    }
}