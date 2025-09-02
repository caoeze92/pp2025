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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)//QUE SOLO CARGEUA LA PRIMRA VEZ
            {
                CargarEstados();
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
                string query = @"
                    SELECT 
                        c.id_computadora,
                        comp.id_componente,
                        ccomp.fecha_asignacion,
                        ccomp.fecha_desasignacion,
                        c.id_laboratorio,
                        c.codigo_inventario,
                        c.numero_serie,
                        c.descripcion,
                        comp.tipo AS tipo_componente,
                        comp.marca,
                        comp.modelo,
                        estComp.descripcion AS estado_componente,
                        estPC.descripcion AS estado_pc
                    FROM Componentes comp
                    LEFT JOIN Computadora_Componentes ccomp ON comp.id_componente = ccomp.id_componente
                    LEFT JOIN Computadoras c ON ccomp.id_computadora = c.id_computadora
                    LEFT JOIN Estados estPC 
                        ON estPC.id_estado = c.estado_actual
                        AND estPC.id_tipo_estado = 1     -- PC
                    LEFT JOIN Estados estComp 
                        ON estComp.id_estado = comp.estado_id 
                        AND estComp.id_tipo_estado = 2   -- Componente
                    WHERE (@idComputadora IS NULL OR c.id_computadora = @idComputadora)
                      AND (@idComponente IS NULL OR comp.id_componente = @idComponente)
                      AND (@codigoInventario IS NULL OR c.codigo_inventario LIKE @codigoInventario + '%')
                      AND (@numeroSerie IS NULL OR c.numero_serie LIKE @numeroSerie + '%')
                      AND (@descripcion IS NULL OR c.descripcion LIKE '%' + @descripcion + '%')
                      AND (@idLaboratorio IS NULL OR c.id_laboratorio = @idLaboratorio)
                      AND (@estadoPC IS NULL OR estPC.descripcion = @estadoPC)
                      AND (@estadoComp IS NULL OR estComp.descripcion = @estadoComp)
                      AND (@tipoComponente IS NULL OR comp.tipo LIKE @tipoComponente + '%')";

                SqlCommand cmd = new SqlCommand(query, conexion);

                // Parámetros
                cmd.Parameters.AddWithValue("@idComputadora", string.IsNullOrEmpty(txtIdComputadora.Text) ? (object)DBNull.Value : txtIdComputadora.Text);
                cmd.Parameters.AddWithValue("@idComponente", string.IsNullOrEmpty(txtIdComponente.Text) ? (object)DBNull.Value : txtIdComponente.Text);
                cmd.Parameters.AddWithValue("@codigoInventario", string.IsNullOrEmpty(txtCodigoInventario.Text) ? (object)DBNull.Value : txtCodigoInventario.Text);
                cmd.Parameters.AddWithValue("@numeroSerie", string.IsNullOrEmpty(txtNumeroSerie.Text) ? (object)DBNull.Value : txtNumeroSerie.Text);
                cmd.Parameters.AddWithValue("@descripcion", string.IsNullOrEmpty(txtDescripcion.Text) ? (object)DBNull.Value : txtDescripcion.Text);
                cmd.Parameters.AddWithValue("@idLaboratorio", string.IsNullOrEmpty(txtIdLaboratorio.Text) ? (object)DBNull.Value : txtIdLaboratorio.Text);
                cmd.Parameters.AddWithValue("@estadoPC", string.IsNullOrEmpty(ddlEstadoPC.SelectedValue) ? (object)DBNull.Value : ddlEstadoPC.SelectedValue);
                cmd.Parameters.AddWithValue("@estadoComp", string.IsNullOrEmpty(ddlEstadoComponente.SelectedValue) ? (object)DBNull.Value : ddlEstadoComponente.SelectedValue);
                cmd.Parameters.AddWithValue("@tipoComponente", string.IsNullOrEmpty(txtTipoComponente.Text) ? (object)DBNull.Value : txtTipoComponente.Text);
                

                System.Diagnostics.Debug.WriteLine("Parametros para la query de Consulta Stock");
                foreach (SqlParameter p in cmd.Parameters)
                {
                    string result = String.Format("{0} = {1}", p.ParameterName, p.Value);
                    System.Diagnostics.Debug.WriteLine(result);
                }


                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);




                gvResultados.DataSource = dt;
                gvResultados.DataBind();
            }
        }

        protected void gvResultados_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Aquí puedes manejar la fila seleccionada
            GridViewRow row = gvResultados.SelectedRow;
            // Ejemplo: mostrar el ID de la computadora seleccionada
            string idComp = row.Cells[0].Text;
            // Hacer algo con idComp...
        }

        protected void gvResultados_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    e.Row.Cells[i].Text = fecha.ToString("dd/MM/yyyy");
                }
            }
        }
    }
}


        private void CargarEstados()
        {
            using (SqlConnection con = ConnectionFactory.GetConnection())
            {
                con.Open();

                // Estados de PC
                SqlCommand cmdPC = new SqlCommand("SELECT descripcion FROM Estados WHERE id_tipo_estado = 1", con);
                SqlDataReader drPC = cmdPC.ExecuteReader();
                ddlEstadoPC.DataSource = drPC;
                ddlEstadoPC.DataTextField = "descripcion";
                ddlEstadoPC.DataValueField = "descripcion";
                ddlEstadoPC.DataBind();
                drPC.Close();
                System.Diagnostics.Debug.WriteLine("Cargo combo EstadosPC");
                ddlEstadoPC.Items.Insert(0, new ListItem("-- Seleccione --", ""));

                // Estados de Componentes
                SqlCommand cmdComp = new SqlCommand("SELECT descripcion FROM Estados WHERE id_tipo_estado = 2", con);
                SqlDataReader drComp = cmdComp.ExecuteReader();
                ddlEstadoComponente.DataSource = drComp;
                ddlEstadoComponente.DataTextField = "descripcion";
                ddlEstadoComponente.DataValueField = "descripcion";
                ddlEstadoComponente.DataBind();
                drComp.Close();
                System.Diagnostics.Debug.WriteLine("Cargo combo estadosComp");
                ddlEstadoComponente.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            }
        }
    }
}
