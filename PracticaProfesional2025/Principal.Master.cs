using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (Session["Rol"] != null && Session["Rol"].ToString() == "admin")
                {
                    menuAdmin.Visible = true;
                }
                else
                {
                    menuAdmin.Visible = false;
                }
            }
            // Verifica siempre el rol, no solo en postback 
            if (Session["logRol"] != null && Session["logRol"].ToString() == "Admin")
            {
                menuAdmin.Visible = true;
            }
            else
            {
                menuAdmin.Visible = false;
            }
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon(); // termina la sesión
            Response.Redirect("login.aspx"); // redirige al login
        }
    }
}