using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class Componentes : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarComponentes();
            }
        }

        private void CargarComponentes()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT id_componente, tipo, marca, modelo, caracteristicas, numero_serie, estado_id, fecha_compra FROM Componentes";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvComponentes.DataSource = dt;
                gvComponentes.DataBind();
            }
        }

        protected void gvComponentes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvComponentes.EditIndex = e.NewEditIndex;
            CargarComponentes();
        }

        protected void gvComponentes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvComponentes.EditIndex = -1;
            CargarComponentes();
        }

        protected void gvComponentes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvComponentes.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvComponentes.Rows[e.RowIndex];
            string tipo = ((TextBox)row.Cells[1].Controls[0]).Text;
            string marca = ((TextBox)row.Cells[2].Controls[0]).Text;
            string modelo = ((TextBox)row.Cells[3].Controls[0]).Text;
            string caracteristicas = ((TextBox)row.Cells[4].Controls[0]).Text;
            string numeroSerie = ((TextBox)row.Cells[5].Controls[0]).Text;
            int estadoId = int.Parse(((TextBox)row.Cells[6].Controls[0]).Text);
            DateTime fechaCompra = DateTime.Parse(((TextBox)row.Cells[7].Controls[0]).Text);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"UPDATE Componentes 
                                 SET tipo=@tipo, marca=@marca, modelo=@modelo, caracteristicas=@caracteristicas, 
                                     numero_serie=@numeroSerie, estado_id=@estadoId, fecha_compra=@fechaCompra 
                                 WHERE id_componente=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@marca", marca);
                cmd.Parameters.AddWithValue("@modelo", modelo);
                cmd.Parameters.AddWithValue("@caracteristicas", caracteristicas);
                cmd.Parameters.AddWithValue("@numeroSerie", numeroSerie);
                cmd.Parameters.AddWithValue("@estadoId", estadoId);
                cmd.Parameters.AddWithValue("@fechaCompra", fechaCompra);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            gvComponentes.EditIndex = -1;
            CargarComponentes();
        }

        protected void gvComponentes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvComponentes.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Componentes WHERE id_componente=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            CargarComponentes();
        }
    }
}
