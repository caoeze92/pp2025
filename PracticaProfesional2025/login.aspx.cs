using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Diagnostics;

using System.Security.Cryptography;

namespace PracticaProfesional2025
{


    public partial class login : System.Web.UI.Page
    {
        Usuario usuario = null;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
                string query = "SELECT * FROM USUARIOS WHERE EMAIL = @Email";

                conexion.Open();
                SqlCommand command = new SqlCommand(query, conexion);
                command.Parameters.AddWithValue("@Email", logTxtEmail.Text);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) // si encontró el usuario
                {
                    usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(reader["id_usuario"]),
                        Nombre = reader["nombre"].ToString(),
                        Apellido = reader["apellido"].ToString(),
                        Rol = reader["rol"].ToString(),
                        Email = reader["email"].ToString(),
                        PasswordHash = reader["password_hash"].ToString(),
                        Telefono = reader["telefono"].ToString(),
                        Activo = Convert.ToInt32(reader["activo"])
                    };

                    if (usuario.Activo == 0)
                    {
                        // Usuario no activo
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert",
                            "alert('Usuario no activo, consulte con el administrador');", true);
                    }
                    else
                    {
                        try
                        {
                            // Verificar la contraseña ingresada contra el hash
                            if (VerifyPassword(logTxtPassword.Text, usuario.PasswordHash))
                            {
                                // Guardar en sesión
                                Session["idUsuario"] = usuario.IdUsuario;
                                Session["NombreInicio"] = usuario.Nombre;

                                Response.Redirect("Inicio.aspx", false);
                            }
                            else
                            {
                                // Contraseña incorrecta
                                ClientScript.RegisterStartupScript(this.GetType(), "myalert",
                                    "alert('Contraseña incorrecta');", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            Session["ErrorMessage"] = "Ocurrió un error al querer logear el usuario";
                            Session["ErrorException"] = ex.ToString(); // stacktrace y detalles
                            Response.Redirect("Error.aspx", false);
                        }
                    }
                }
                else
                {
                    // Usuario no encontrado
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert",
                        "alert('Usuario no encontrado');", true);
                }

                conexion.Close();
            }
        }








        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Convertir el hash almacenado en bytes
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extraer el salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Volver a calcular el hash con la contraseña ingresada
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Comparar byte por byte
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }

    }
}