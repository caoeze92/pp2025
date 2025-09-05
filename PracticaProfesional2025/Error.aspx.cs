using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ErrorMessage"] != null)
            {
                lblMensaje.Text = Session["ErrorMessage"].ToString();
            }

            if (Session["ErrorException"] != null)
            {
                lblDetalles.Text = Session["ErrorException"].ToString();
            }

            // Limpiar sesión para que no persista
            Session["ErrorMessage"] = null;
            Session["ErrorException"] = null;
        }
    }
}