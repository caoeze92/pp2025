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
<<<<<<< Updated upstream
=======


>>>>>>> Stashed changes
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
<<<<<<< Updated upstream
                string script = String.Format("SELECT ID FROM USUARIOS WHERE USUARIO = '{0}' AND PASSWORD =  '{1}'", txtUsuario.Text, txtPass.Text);
=======

                string script = String.Format("SELECT ID_USUARIO, NOMBRE, ROL FROM USUARIOS WHERE EMAIL = '{0}' AND PASSWORD_HASH =  '{1}'", logTxtEmail.Text, logTxtPassword.Text);

>>>>>>> Stashed changes

                conexion.Open();
                SqlCommand command = new SqlCommand(script, conexion);
                SqlDataReader reader = command.ExecuteReader();
                String id = String.Empty;
                String Nombre = String.Empty;
                String logRol = String.Empty;

<<<<<<< Updated upstream
                    SqlCommand command = new SqlCommand(script, conexion);

                    SqlDataReader reader = command.ExecuteReader();

                    String id = String.Empty;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            id = reader.GetInt32(0).ToString();
                        }
                    }

                    conexion.Close();


                    if (id != String.Empty)
                    {
                        Session["Usuario"] = txtUsuario.Text;

                        //Redireccionarlo ala pagina correcta

                        Response.Redirect("Inicio.aspx", false);
                    }
=======
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0).ToString();
                        Nombre = reader.GetString(1);
                        logRol = reader.GetString(2);
                    }
>>>>>>> Stashed changes
                }

                conexion.Close();


                if (id != String.Empty)
                {
                    Session["NombreInicio"] = Nombre;
                    Session["logRol"] = logRol;

                    //Redireccionarlo ala pagina correcta
                    Response.Redirect("Inicio.aspx", false);
                }

            }
        }


    }
}