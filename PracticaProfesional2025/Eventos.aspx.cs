using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace PracticaProfesional2025
{
    public partial class Eventos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();   // carga general
                CargarEstados(); // carga del combo
            }
        }

        // CargarDatos con parámetro opcional
        private void CargarDatos(string eventoSeleccionado = "")
        {
            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
                string query = @"
                    SELECT 
                        h.id_historial,
                        h.tipo_evento,
                        te.nombre AS nombre_evento,
                        h.entidad,
                        h.codentidad,
                        h.usuario,
                        h.fecha_solicitud,
                        h.detalle
                    FROM historial h
                    INNER JOIN tipos_evento te 
                        ON h.tipo_evento = te.id_tipo_evento";

                // Si hay un filtro, agregamos WHERE
                if (!string.IsNullOrEmpty(eventoSeleccionado))
                {
                    query += " WHERE te.nombre = @nombreEvento";
                }

                SqlCommand cmd = new SqlCommand(query, conexion);

                // Si hay filtro, asignar parámetro
                if (!string.IsNullOrEmpty(eventoSeleccionado))
                {
                    cmd.Parameters.AddWithValue("@nombreEvento", eventoSeleccionado);
                }

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvResultados.DataSource = dt;
                gvResultados.DataBind();

                int totalFilas = dt.Rows.Count;
                lblTotalRows.Text = "Total: " + gvResultados.Rows.Count + " de " + totalFilas + " registros.";
                ViewState["totalFilas"] = totalFilas;
            }
        }

        protected void gvResultados_RowDataBound_Eventos(object sender, GridViewRowEventArgs e)
        {
            DateTime fecha;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string cellText = e.Row.Cells[i].Text;
                    if (string.IsNullOrWhiteSpace(cellText) || cellText == "&nbsp;")
                    {
                        e.Row.Cells[i].Text = "-";
                    }
                    else if (DateTime.TryParse(cellText, out fecha))
                    {
                        e.Row.Cells[i].Text = fecha.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                }
            }
        }

        protected void gvResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResultados.PageIndex = e.NewPageIndex;

            // Mantener el filtro actual si existe
            string eventoSeleccionado = comboEventos.SelectedValue;
            CargarDatos(eventoSeleccionado);

            int totalFilas = (int)ViewState["totalFilas"];
            int paginaActual = gvResultados.PageIndex + 1;
            int pageSize = gvResultados.PageSize;
            int registroActual = paginaActual * pageSize;
            if (registroActual > totalFilas)
                registroActual = totalFilas;

            lblTotalRows.Text = "Total: " + registroActual + " de " + totalFilas + " registros.";
        }

        // BTN Buscar
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string eventoSeleccionado = comboEventos.SelectedValue;
            CargarDatos(eventoSeleccionado);
        }

        private void CargarEstados()
        {
            using (SqlConnection con = ConnectionFactory.GetConnection())
            {
                con.Open();
                SqlCommand cmdEventos = new SqlCommand("SELECT nombre FROM Tipos_Evento", con);
                SqlDataReader opcEventos = cmdEventos.ExecuteReader();
                comboEventos.DataSource = opcEventos;
                comboEventos.DataTextField = "nombre";
                comboEventos.DataValueField = "nombre";
                comboEventos.DataBind();
                opcEventos.Close();
                comboEventos.Items.Insert(0, new ListItem("-- Eventos --", ""));
            }
        }
    }
}
