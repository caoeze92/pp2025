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
                string script = String.Format("SELECT ID FROM USUARIOS WHERE USUARIO = '{0}' AND PASSWORD =  '{1}'", txtUsuario.Text, txtPass.Text);

                    conexion.Open();

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
                }
        }


    }
}