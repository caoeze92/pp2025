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


   public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
<<<<<<< HEAD
                string script = String.Format("SELECT id_usuario FROM USUARIOS WHERE email = '{0}' AND password_hash =  '{1}'", txtUsuario.Text, txtPass.Text);
=======
                string script = String.Format("SELECT ID_USUARIO, NOMBRE FROM USUARIOS WHERE EMAIL = '{0}' AND PASSWORD_HASH =  '{1}'", logTxtEmail.Text, logTxtPassword.Text);
>>>>>>> 656b1676eca644119aa61531c6fef18eef260390

                    conexion.Open();

                    SqlCommand command = new SqlCommand(script, conexion);

                    SqlDataReader reader = command.ExecuteReader();

                    String id = String.Empty;
                    String Nombre = String.Empty;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = reader.GetInt32(0).ToString();
                            Nombre = reader.GetString(1);
                        }
                    }

                    conexion.Close();


                    if (id != String.Empty)
                    {
                        Session["NombreInicio"] = Nombre;

                        //Redireccionarlo ala pagina correcta

                        Response.Redirect("Inicio.aspx", false);
                    }

                }
        }


    }
}