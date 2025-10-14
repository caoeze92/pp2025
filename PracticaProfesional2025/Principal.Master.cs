using System;
using System.Web;
using System.Web.UI;

namespace PracticaProfesional2025
{
    public partial class Principal : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Mostramos el menú solo si hay sesión iniciada
            if (Session["logRol"] != null &&
                Session["logRol"].ToString().ToLower() == "admin")
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
            Session.Abandon();
            Response.Redirect("login.aspx");
        }
    }
}
