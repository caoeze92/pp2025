using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PracticaProfesional2025
{
    public partial class ModificarUsuario : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((String)Session["logRol"] != "Admin")
            {
                Response.Redirect("NoAutorizado.aspx");

            } else {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(" SELECT id_usuario, nombre, apellido, rol, email, telefono, activo FROM Usuarios", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvUsuarios.DataSource = dt;
                gvUsuarios.DataBind();
            }
        }

        protected void gvUsuarios_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvUsuarios.EditIndex = e.NewEditIndex;
            CargarUsuarios();
        }

        protected void gvUsuarios_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
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
                {
                    ddlRol.SelectedValue = rolActual;
                }
            }
        }

        protected void gvUsuarios_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            int idUsuario = (int)gvUsuarios.DataKeys[e.RowIndex].Value;
            GridViewRow row = gvUsuarios.Rows[e.RowIndex];
            DropDownList ddlRol = (DropDownList)row.FindControl("ddlRol");


            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET rol=@rol WHERE id_usuario=@ID", conn);
                cmd.Parameters.AddWithValue("@Rol", ddlRol.SelectedValue);
                cmd.Parameters.AddWithValue("@ID", idUsuario);
                cmd.ExecuteNonQuery();
            }
            gvUsuarios.EditIndex = -1;
            CargarUsuarios();
        }

        protected void gvUsuarios_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
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
        }
    }
}