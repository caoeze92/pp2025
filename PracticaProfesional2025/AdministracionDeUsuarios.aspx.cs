using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class ModificarUsuario : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
<<<<<<< Updated upstream
                CargarUsuarios();

=======
            if (!IsPostBack)
>>>>>>> Stashed changes
                CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT id_usuario, nombre, apellido, rol, email, telefono, activo FROM Usuarios", conn);
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

        protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvUsuarios.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlRol = (DropDownList)e.Row.FindControl("ddlRol");
                string rolActual = DataBinder.Eval(e.Row.DataItem, "rol").ToString();

                if (ddlRol.Items.FindByValue(rolActual) != null)
                    ddlRol.SelectedValue = rolActual;
            }
        }

        protected void gvUsuarios_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idUsuario = (int)gvUsuarios.DataKeys[e.RowIndex].Value;
            GridViewRow row = gvUsuarios.Rows[e.RowIndex];

            // Tomar los valores editados
            string nombre = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string apellido = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string email = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string telefono = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();
            string activo = ((TextBox)row.Cells[5].Controls[0]).Text.Trim();

            DropDownList ddlRol = (DropDownList)row.FindControl("ddlRol");

            // Actualizar en BD
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"UPDATE Usuarios 
                                 SET nombre = @Nombre,
                                     apellido = @Apellido,
                                     email = @Email,
                                     telefono = @Telefono,
                                     rol = @Rol,
                                     activo = @Activo
                                 WHERE id_usuario = @ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Telefono", telefono);
                cmd.Parameters.AddWithValue("@Rol", ddlRol.SelectedValue);
                cmd.Parameters.AddWithValue("@ID", idUsuario);
                cmd.Parameters.AddWithValue("@Activo", activo);

                cmd.ExecuteNonQuery();
            }

            gvUsuarios.EditIndex = -1;
            CargarUsuarios();
        }

        protected void gvUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int idUsuario = (int)gvUsuarios.DataKeys[e.RowIndex].Value;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Usuarios WHERE id_usuario=@ID", conn);
                cmd.Parameters.AddWithValue("@ID", idUsuario);
                cmd.ExecuteNonQuery();
            }

            CargarUsuarios();

            lblMensaje.Text = "✅ Usuario eliminado correctamente.";
            lblMensaje.Visible = true;
        }


    }
}
