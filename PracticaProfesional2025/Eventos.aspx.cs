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
            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
                string query = @"SELECT * FROM Historial;";
                SqlCommand cmd = new SqlCommand(query, conexion);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                gvResultados.DataSource = dt;
                gvResultados.DataBind();
                int totalFilas = dt.Rows.Count;
                lblTotalRows.Text = "Total: " + gvResultados.Rows.Count + " de " + totalFilas + "registros.";
                ViewState["totalFilas"] = totalFilas;
            }
        }

        // a

        protected void gvResultados_RowDataBound_Eventos(object sender, GridViewRowEventArgs e)
        {
            DateTime fecha;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string cellText = e.Row.Cells[i].Text;

                    // Si la celda está vacía o es NULL
                    if (string.IsNullOrWhiteSpace(cellText) || cellText == "&nbsp;")
                    {
                        e.Row.Cells[i].Text = "-";
                    }
                    else
                    {
                        // Intentar convertir a fecha y formatear
                        if (DateTime.TryParse(cellText, out fecha))
                        {
                            e.Row.Cells[i].Text = fecha.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                    }
                }
            }
        }

        //a


        //b
        protected void gvResultados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvResultados.PageIndex = e.NewPageIndex;
            gvResultados.DataBind();

            // ACTUALIZACION DE TEXTO DE REGISTROS MOSTRADOS

            int totalFilas = (int)ViewState["totalFilas"];
            int paginaActual = gvResultados.PageIndex + 1;
            int pageSize = gvResultados.PageSize;
            int registroActual = paginaActual * pageSize;

            if (registroActual > totalFilas)
                registroActual = totalFilas;

            lblTotalRows.Text = "Total: "+ registroActual +" de " + totalFilas +" registros.";
        }
        //b


    }

}