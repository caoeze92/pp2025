using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class ReservaExitosa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Mostrar detalles de la reserva desde Session
                lblLaboratorio.Text = Session["ReservaLaboratorio"] != null ? Session["ReservaLaboratorio"].ToString() : "N/A";
                lblFecha.Text = Session["ReservaFecha"] != null ? Session["ReservaFecha"].ToString() : "N/A";
                lblHora.Text = (Session["ReservaHoraInicio"] != null ? Session["ReservaHoraInicio"].ToString() : "N/A")
                               + " - "
                               + (Session["ReservaHoraFin"] != null ? Session["ReservaHoraFin"].ToString() : "N/A");
                lblMotivo.Text = Session["ReservaMotivo"] != null ? Session["ReservaMotivo"].ToString() : "N/A";


                // Limpiar la sesión de la reserva para que no se reutilice
                Session.Remove("ReservaLaboratorio");
                Session.Remove("ReservaFecha");
                Session.Remove("ReservaHoraInicio");
                Session.Remove("ReservaHoraFin");
                Session.Remove("ReservaMotivo");
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reservas.aspx");
        }
    }
}