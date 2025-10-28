using System;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace PracticaProfesional2025
{
    public partial class ConfirmarCuenta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        // Revertir Base64 URL-safe a estándar
                        string base64 = token.Replace("-", "+").Replace("_", "/");
                        switch (base64.Length % 4)
                        {
                            case 2: base64 += "=="; break;
                            case 3: base64 += "="; break;
                        }

                        // Decodificar el token (formato: email|fecha)
                        string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                        string email = decoded.Split('|')[0];

                        using (SqlConnection conexion = ConnectionFactory.GetConnection())
                        {
                            conexion.Open();
                            string sql = "UPDATE USUARIOS SET activo = 1 WHERE email = @Email";

                            SqlCommand cmd = new SqlCommand(sql, conexion);
                            cmd.Parameters.AddWithValue("@Email", email);
                            int rows = cmd.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                lblMensaje.ForeColor = System.Drawing.Color.Green;
                                lblMensaje.Text = "✅ ¡Tu cuenta fue confirmada exitosamente! Serás redirigido al login en 5 segundos.";

                                // Redirección automática a Login.aspx después de 5 segundos
                                string script = "setTimeout(function(){ window.location='Login.aspx'; }, 5000);";
                                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                            }
                            else
                            {
                                lblMensaje.ForeColor = System.Drawing.Color.Red;
                                lblMensaje.Text = "⚠️ No se encontró una cuenta asociada a este enlace.";
                            }
                        }
                    }
                    catch (FormatException)
                    {
                        lblMensaje.ForeColor = System.Drawing.Color.Red;
                        lblMensaje.Text = "❌ Enlace inválido o corrupto.";
                    }
                    catch (Exception ex)
                    {
                        lblMensaje.ForeColor = System.Drawing.Color.Red;
                        lblMensaje.Text = "❌ Ocurrió un error al confirmar tu cuenta: " + ex.Message;
                    }
                }
                else
                {
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                    lblMensaje.Text = "❌ No se proporcionó un token de confirmación.";
                }
            }
        }
    }
}
