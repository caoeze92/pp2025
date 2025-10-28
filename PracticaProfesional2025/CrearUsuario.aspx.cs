using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;//Para encriptar pass
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                string msgcCreada = "alert('Cuenta creada con éxito'); window.location='Login.aspx';";
                string script = "INSERT INTO USUARIOS (nombre, apellido, rol, email, password_hash, telefono, activo) " +
                                "VALUES (@nombre, @apellido,@rol, @email, @password_hash, @telefono, @activo)";
                try
                {
                    SqlCommand command = new SqlCommand(script, conexion);
                    conexion.Open();
                    command.Parameters.AddWithValue("@nombre", nTxtNombre.Text);
                    command.Parameters.AddWithValue("@apellido", nTxtApellido.Text);
                    command.Parameters.AddWithValue("@rol", "usuario");
                    command.Parameters.AddWithValue("@email", nTxtEmail.Text);
                    command.Parameters.AddWithValue("@password_hash", HashPassword(nTxtPassword.Text));
                    command.Parameters.AddWithValue("@telefono", nTxtTelefono.Text);
                  //  command.Parameters.AddWithValue("@activo", 0); // cuenta inactiva hasta confirmar email
                    command.Parameters.AddWithValue("@activo", 1); // cuenta Activa hasta corregir lo del mail jaja

                    int resultado = command.ExecuteNonQuery();
                    if (resultado > 0)
                    {
                        // Generar token Base64 URL-safe
                        string email = nTxtEmail.Text;
                        string tokenRaw = email + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        string token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenRaw))
                                            .Replace("+", "-")
                                            .Replace("/", "_")
                                            .TrimEnd('=');

                        EnviarEmailConfirmacion(email, token); // Envia mail de confirmación

                        conexion.Close();
                        Session["NombreInicio"] = nTxtNombre.Text;
                        Session["logRol"] = "Usuario";

                        ClientScript.RegisterStartupScript(this.GetType(), "redirectAlert", msgcCreada, true);
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    Session["ErrorMessage"] = msgErr;
                    Session["ErrorException"] = ex.ToString();
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

        private void EnviarEmailConfirmacion(string destinatario, string token)
        {
            // Generar link de confirmación (reemplazá localhost con tu dominio si es necesario)
            string linkConfirmacion = $"https://localhost:9094/ConfirmarCuenta.aspx?token={HttpUtility.UrlEncode(token)}";

            // Crear el mensaje
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("inventarioInstitucional46@gmail.com", "Sistema de Gestión de Laboratorio");
            mail.To.Add(destinatario);
            mail.Subject = "Confirma tu registro";
            mail.Body = $"Hola,\n\nPor favor confirmá tu cuenta haciendo clic en este enlace:\n\n{linkConfirmacion}\n\nGracias.";
            mail.IsBodyHtml = false;

            // Configurar SMTP para Gmail
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); // puerto 587 con TLS
            smtp.EnableSsl = true; // obligatorio para Gmail

            // Credenciales: usar la cuenta de Gmail + contraseña de aplicación
            smtp.Credentials = new System.Net.NetworkCredential(
                "inventarioInstitucional46@gmail.com",
                "nmox ohrd ltnr xwvf" // 
            );

            try
            {
                // Enviar correo
                smtp.Send(mail);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                // Manejar errores de autenticación o conexión
                throw new Exception("Error enviando correo: " + ex.Message);
            }
        }




    }
}