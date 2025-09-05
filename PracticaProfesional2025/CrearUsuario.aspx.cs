using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Diagnostics;


using System.Security.Cryptography;//Para encriptar pass

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
                string script = "INSERT INTO USUARIOS (nombre, apellido, rol, email, password_hash, telefono, activo) VALUES (@nombre, @apellido,@rol, @email, @password_hash, @telefono, @activo)";
                try
                {
                    SqlCommand command = new SqlCommand(script, conexion);
                    conexion.Open();
                    command.Parameters.AddWithValue("@nombre", nTxtNombre.Text);
                    command.Parameters.AddWithValue("@apellido", nTxtApellido.Text);
                    command.Parameters.AddWithValue("@rol", "usuario");
                    command.Parameters.AddWithValue("@email", nTxtEmail.Text);
                    command.Parameters.AddWithValue("@password_hash", HashPassword(nTxtPassword.Text));//guardo la hash del pasword
                    command.Parameters.AddWithValue("@telefono", nTxtTelefono.Text);
                    command.Parameters.AddWithValue("@activo", 1);
                        int resultado = command.ExecuteNonQuery();
                        if (resultado > 0)
                        {
                        conexion.Close();
                        Session["NombreInicio"] = nTxtNombre.Text;
                        Session["logRol"] = "Usuario";
                        Response.Redirect("Inicio.aspx", false);
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + msgcCreada + "');", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        conexion.Close();

                        Session["ErrorMessage"] = msgErr;
                        Session["ErrorException"] = ex.ToString(); // stacktrace y detalles

                        Response.Redirect("Error.aspx", false);
                    }
            }
        }

        private string HashPassword(string password)
        {
            // Generar un salt aleatorio
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Generar el hash con PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000); // 10k iteraciones
            byte[] hash = pbkdf2.GetBytes(20);

            // Combinar salt + hash en un solo string
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }


    }
}