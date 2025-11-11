using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class ModificarUsuario : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT id_usuario, nombre, apellido, email, telefono, rol, activo FROM Usuarios";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsuarios.DataSource = dt;
                gvUsuarios.DataBind();
            }
        }

        protected void gvUsuarios_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsuarios.EditIndex = e.NewEditIndex;
            CargarUsuarios();
        }

        protected void gvUsuarios_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsuarios.EditIndex = -1;
            CargarUsuarios();
        }

        protected void gvUsuarios_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idUsuario = Convert.ToInt32(gvUsuarios.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvUsuarios.Rows[e.RowIndex];

            // Obtener los valores desde las celdas editables
            string nombre = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string apellido = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string email = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string telefono = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
            string activo = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();

            DropDownList ddlRol = (DropDownList)row.FindControl("ddlRol");
            string rol = ddlRol.SelectedValue;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"UPDATE Usuarios 
                                 SET nombre = @nombre, apellido = @apellido, email = @mail, 
                                     telefono = @telefono, rol = @rol, activo = @activo 
                                 WHERE id_usuario = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@apellido", apellido);
                cmd.Parameters.AddWithValue("@mail", email);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@rol", rol);
                cmd.Parameters.AddWithValue("@activo", activo);
                cmd.Parameters.AddWithValue("@id", idUsuario);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvUsuarios.EditIndex = -1;
            CargarUsuarios();

            // Mensaje de confirmación
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Usuario actualizado con éxito.');", true);
            HistorialManager.RegistrarEvento(3, idUsuario, "Cuenta", (string)Session["NombreInicio"], "Correcion de datos sobre usuario: " + nombre + " " + apellido);
        }

        protected void gvUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int idUsuario = Convert.ToInt32(gvUsuarios.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string querySelect = "SELECT nombre, apellido FROM Usuarios WHERE id_usuario = @id";
                SqlCommand cmdSelect = new SqlCommand(querySelect, conn);
                cmdSelect.Parameters.AddWithValue("@id", idUsuario);

                string nombre = string.Empty;
                string apellido = string.Empty;

                using (SqlDataReader dr = cmdSelect.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        nombre = dr["nombre"].ToString();
                        apellido = dr["apellido"].ToString();
                    }
                }

                string query = "DELETE FROM Usuarios WHERE id_usuario = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idUsuario);

               
                cmd.ExecuteNonQuery();
                conn.Close();
                HistorialManager.RegistrarEvento(2, idUsuario, "Cuenta", (string)Session["NombreInicio"], "Usuario: " + nombre + " " + apellido + " eliminado con éxito");
            }

            CargarUsuarios();

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Usuario eliminado con éxito.');", true);
        
        }

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvUsuarios.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlRol = (DropDownList)e.Row.FindControl("ddlRol");
                string rolActual = DataBinder.Eval(e.Row.DataItem, "rol").ToString();

                if (ddlRol != null && ddlRol.Items.FindByValue(rolActual) != null)
                {
                    ddlRol.SelectedValue = rolActual;
                }
            }
        }
    }
}