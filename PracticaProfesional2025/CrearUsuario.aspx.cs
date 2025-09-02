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
                string msgErr = "Error al crear la cuenta, alguno de los datos ingresados ya existen.";
                string msgcCreada = "Cuenta creada exitosamente.";
                string script = "INSERT INTO USUARIOS (nombre, apellido, email, password_hash, telefono) VALUES (@nombre, @apellido, @email, @password_hash, @telefono)";
                try
                {
                    SqlCommand command = new SqlCommand(script, conexion);
                    conexion.Open();
                    command.Parameters.AddWithValue("@nombre", nTxtNombre.Text);
                    command.Parameters.AddWithValue("@apellido", nTxtApellido.Text);
                    command.Parameters.AddWithValue("@email", nTxtEmail.Text);
                    command.Parameters.AddWithValue("@password_hash", nTxtPassword.Text);
                    command.Parameters.AddWithValue("@telefono", nTxtTelefono.Text);
                        int resultado = command.ExecuteNonQuery();
                        if (resultado > 0)
                        {
                        conexion.Close();
                        Session["NombreInicio"] = nTxtNombre.Text;
                        Response.Redirect("Inicio.aspx", false);
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msgcCreada + "');", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        conexion.Close();
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msgErr + "');", true);
                        Console.WriteLine(ex);
                    }
            }
        }
    }
}