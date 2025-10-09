using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class Computadoras : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarComputadoras();
            }
        }

        private void CargarComputadoras()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT id_computadora, id_laboratorio, codigo_inventario, numero_serie, descripcion, estado_actual, fecha_alta, fecha_baja FROM Computadoras";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvComputadoras.DataSource = dt;
                gvComputadoras.DataBind();
            }
        }

        protected void gvComputadoras_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvComputadoras.EditIndex = e.NewEditIndex;
            CargarComputadoras();
        }

        protected void gvComputadoras_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvComputadoras.EditIndex = -1;
            CargarComputadoras();
        }

        protected void gvComputadoras_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvComputadoras.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvComputadoras.Rows[e.RowIndex];

            int idLaboratorio = int.Parse(((TextBox)row.Cells[1].Controls[0]).Text);
            string codigoInventario = ((TextBox)row.Cells[2].Controls[0]).Text;
            string numeroSerie = ((TextBox)row.Cells[3].Controls[0]).Text;
            string descripcion = ((TextBox)row.Cells[4].Controls[0]).Text;
            string estadoActual = ((TextBox)row.Cells[5].Controls[0]).Text;
            DateTime fechaAlta = DateTime.Parse(((TextBox)row.Cells[6].Controls[0]).Text);
            string fechaBajaStr = ((TextBox)row.Cells[7].Controls[0]).Text;
            DateTime? fechaBaja = string.IsNullOrEmpty(fechaBajaStr) ? (DateTime?)null : DateTime.Parse(fechaBajaStr);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"UPDATE Computadoras
                                 SET id_laboratorio=@idLaboratorio, 
                                     codigo_inventario=@codigoInventario, 
                                     numero_serie=@numeroSerie, 
                                     descripcion=@descripcion, 
                                     estado_actual=@estadoActual, 
                                     fecha_alta=@fechaAlta, 
                                     fecha_baja=@fechaBaja 
                                 WHERE id_computadora=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idLaboratorio", idLaboratorio);
                cmd.Parameters.AddWithValue("@codigoInventario", codigoInventario);
                cmd.Parameters.AddWithValue("@numeroSerie", numeroSerie);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@estadoActual", estadoActual);
                cmd.Parameters.AddWithValue("@fechaAlta", fechaAlta);
                cmd.Parameters.AddWithValue("@fechaBaja", (object)fechaBaja ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            gvComputadoras.EditIndex = -1;
            CargarComputadoras();
        }

        protected void gvComputadoras_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvComputadoras.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Computadoras WHERE id_computadora=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            CargarComputadoras();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"INSERT INTO Computadoras (id_laboratorio, codigo_inventario, numero_serie, descripcion, estado_actual, fecha_alta)
                                 VALUES (0, '', '', '', 'Disponible', GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }

            CargarComputadoras();
        }
    }
}
