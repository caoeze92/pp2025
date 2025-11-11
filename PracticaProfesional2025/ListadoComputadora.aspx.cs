using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class Computadoras : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
        private const string EDIT_COMPUTER_KEY = "EditingComputerId";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlResultados.Visible = false;
                pnlCompResultados.Visible = false;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = string.Empty;
            string modo = ddlBuscarPor.SelectedValue ?? "Computadora";
            string texto = txtBuscar.Text?.Trim() ?? string.Empty;

            pnlResultados.Visible = false;
            pnlCompResultados.Visible = false;

            if (modo == "Computadora")
            {
                int id;
                if (!int.TryParse(texto, out id))
                {
                    lblMensaje.Text = "Ingresa un ID de computadora válido.";
                    return;
                }

                var repo = new ComputadoraRepository();
                Computadora c = repo.ObtenerPorId(id);
                if (c == null)
                {
                    lblMensaje.Text = "No se encontró la computadora con ese ID.";
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("id_computadora", typeof(int));
                dt.Columns.Add("id_laboratorio", typeof(int));
                dt.Columns.Add("codigo_inventario", typeof(string));
                dt.Columns.Add("numero_serie", typeof(string));
                dt.Columns.Add("descripcion", typeof(string));
                dt.Columns.Add("estado_actual", typeof(string));
                dt.Columns.Add("fecha_alta", typeof(DateTime));
                dt.Columns.Add("fecha_baja", typeof(DateTime));

                var row = dt.NewRow();
                row["id_computadora"] = c.IdComputadora;
                row["id_laboratorio"] = c.IdLaboratorio;
                row["codigo_inventario"] = c.CodigoInventario ?? string.Empty;
                row["numero_serie"] = c.NumeroSerie ?? string.Empty;
                row["descripcion"] = c.Descripcion ?? string.Empty;
                row["estado_actual"] = c.EstadoActual ?? string.Empty;
                row["fecha_alta"] = c.FechaAlta;
                row["fecha_baja"] = c.FechaBaja.HasValue ? (object)c.FechaBaja.Value : DBNull.Value;
                dt.Rows.Add(row);

                rptComputadoras.DataSource = dt;
                rptComputadoras.DataBind();
                pnlResultados.Visible = true;
                return;
            }

            // Modo Componente
            if (modo == "Componente")
            {
                if (string.IsNullOrWhiteSpace(texto))
                {
                    lblMensaje.Text = "Ingresa un término para buscar el componente (ID, SN, Tipo o Marca).";
                    return;
                }

                var repoComp = new ComponenteRepository();
                DataTable dtComp = repoComp.Buscar(texto);
                if (dtComp == null || dtComp.Rows.Count == 0)
                {
                    lblMensaje.Text = "No se encontraron componentes que coincidan.";
                    return;
                }

                gvResultadosComponentes.DataSource = dtComp;
                gvResultadosComponentes.DataBind();
                pnlCompResultados.Visible = true;
            }
        }

        protected void rptComputadoras_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            int idComputadora = Convert.ToInt32(drv["id_computadora"]);

            var gvComponentes = e.Item.FindControl("gvComponentes") as GridView;
            if (gvComponentes != null)
            {
                var repo = new ComponenteRepository();
                var dt = repo.ObtenerPorComputadora(idComputadora);
                gvComponentes.DataSource = dt;
                gvComponentes.DataBind();
                gvComponentes.Attributes["data-computadora-id"] = idComputadora.ToString();
            }

            int editingId = ViewState[EDIT_COMPUTER_KEY] != null ? (int)ViewState[EDIT_COMPUTER_KEY] : 0;
            bool isEditing = editingId == idComputadora;

            var phEdit = e.Item.FindControl("phEditMode") as PlaceHolder;
            var phView = e.Item.FindControl("phViewMode") as PlaceHolder;
            if (phEdit != null) phEdit.Visible = isEditing;
            if (phView != null) phView.Visible = !isEditing;

            if (isEditing)
            {
                var ddlEstadoCompu = e.Item.FindControl("ddlEstadoCompu") as DropDownList;
                if (ddlEstadoCompu != null)
                {
                    ddlEstadoCompu.DataSource = GetEstadosPorTipo(1);
                    ddlEstadoCompu.DataTextField = "descripcion";
                    ddlEstadoCompu.DataValueField = "id_estado";
                    ddlEstadoCompu.DataBind();

                    string estadoActual = Convert.ToString(drv["estado_actual"]);
                    int estadoId;
                    if (int.TryParse(estadoActual, out estadoId))
                        ddlEstadoCompu.SelectedValue = estadoId.ToString();
                }

                var lnkSave = e.Item.FindControl("lnkSave") as LinkButton;
                var lnkCancel = e.Item.FindControl("lnkCancel") as LinkButton;
                var lnkEdit = e.Item.FindControl("lnkEdit") as LinkButton;

                if (lnkSave != null) lnkSave.CssClass = lnkSave.CssClass.Replace("d-none", "").Trim();
                if (lnkCancel != null) lnkCancel.CssClass = lnkCancel.CssClass.Replace("d-none", "").Trim();
                if (lnkEdit != null) lnkEdit.CssClass = (lnkEdit.CssClass + " d-none").Trim();
            }
            else
            {
                var lnkSave = e.Item.FindControl("lnkSave") as LinkButton;
                var lnkCancel = e.Item.FindControl("lnkCancel") as LinkButton;
                var lnkEdit = e.Item.FindControl("lnkEdit") as LinkButton;

                if (lnkSave != null && !lnkSave.CssClass.Contains("d-none")) lnkSave.CssClass += " d-none";
                if (lnkCancel != null && !lnkCancel.CssClass.Contains("d-none")) lnkCancel.CssClass += " d-none";
                if (lnkEdit != null) lnkEdit.CssClass = lnkEdit.CssClass.Replace(" d-none", "").Trim();
            }
        }

        protected void Computer_Command(object sender, CommandEventArgs e)
        {
            string cmd = e.CommandName;
            int id = Convert.ToInt32(e.CommandArgument);

            if (cmd == "EditComputer")
            {
                ViewState[EDIT_COMPUTER_KEY] = id;
                btnBuscar_Click(null, null);
                return;
            }

            if (cmd == "CancelEditComputer")
            {
                ViewState[EDIT_COMPUTER_KEY] = null;
                btnBuscar_Click(null, null);
                return;
            }

            if (cmd == "SaveComputer")
            {
                RepeaterItem target = null;
                foreach (RepeaterItem it in rptComputadoras.Items)
                {
                    var hf = it.FindControl("hfIdComputadora") as HiddenField;
                    if (hf != null && hf.Value == id.ToString())
                    {
                        target = it;
                        break;
                    }
                }
                if (target == null) { btnBuscar_Click(null, null); return; }

                var tbCodigo = target.FindControl("tbCodigoInv") as TextBox;
                var tbSN = target.FindControl("tbNumeroSerie") as TextBox;
                var tbDesc = target.FindControl("tbDescripcion") as TextBox;
                var ddlEstadoCompu = target.FindControl("ddlEstadoCompu") as DropDownList;

                // Obtener datos antes de actualizar
                string numeroSerieAnterior;
                string estadoAnterior;
                ObtenerDatosComputadora(id, out numeroSerieAnterior, out estadoAnterior);

                int estadoSeleccionado = 0;
                if (ddlEstadoCompu != null) int.TryParse(ddlEstadoCompu.SelectedValue, out estadoSeleccionado);

                var computadora = new Computadora
                {
                    IdComputadora = id,
                    IdLaboratorio = 0,
                    CodigoInventario = tbCodigo != null ? tbCodigo.Text : string.Empty,
                    NumeroSerie = tbSN != null ? tbSN.Text : string.Empty,
                    Descripcion = tbDesc != null ? tbDesc.Text : string.Empty,
                    EstadoActual = estadoSeleccionado > 0 ? estadoSeleccionado.ToString() : estadoAnterior,
                    FechaAlta = DateTime.Now,
                    FechaBaja = null
                };

                var repo = new ComputadoraRepository();
                repo.Actualizar(computadora);

                // Registrar cambio de estado si hubo uno
                if (estadoAnterior != computadora.EstadoActual)
                {
                    HistorialManager.RegistrarEvento(
                        3,computadora.IdComputadora, // Tipo modificación
                        "Computadora",
                        (string)Session["NombreInicio"],
                        "Estado de computadora con S/N: " + numeroSerieAnterior +
                        " cambiado de " + estadoAnterior + " a " + computadora.EstadoActual + "."
                    );
                }

                ViewState[EDIT_COMPUTER_KEY] = null;
                btnBuscar_Click(null, null);
                return;

            }

            if (cmd == "DeleteComputer")
            {
                var repo = new ComputadoraRepository();
        repo.EliminarComputadora(id);
                pnlResultados.Visible = false;
                lblMensaje.Text = "Computadora eliminada.";
                return;
            }
}

// GRID de componentes en cada PC (ya existente)
protected void gvComponentes_RowDataBound(object sender, GridViewRowEventArgs e)
{
    if (e.Row.RowType != DataControlRowType.DataRow) return;

    var ddl = e.Row.FindControl("ddlEstadoComp") as DropDownList;
    if (ddl != null)
    {
        ddl.DataSource = GetEstadosPorTipo(2);
        ddl.DataTextField = "descripcion";
        ddl.DataValueField = "id_estado";
        ddl.DataBind();

        object dataItem = e.Row.DataItem;
        if (dataItem is DataRowView drv)
        {
            var estadoVal = drv["Estado_id"];
            if (estadoVal != DBNull.Value)
                ddl.SelectedValue = estadoVal.ToString();
        }
    }
}

protected void gvComponentes_RowEditing(object sender, GridViewEditEventArgs e)
{
    var gv = (GridView)sender;
    gv.EditIndex = e.NewEditIndex;
    RebindComponentGrid(gv);
    OpenCollapseForGrid(gv);
}

private void OpenCollapseForGrid(GridView gv)
{
    if (gv == null) return;

    string idCompu = gv.Attributes["data-computadora-id"];

    if (string.IsNullOrEmpty(idCompu))
    {
        var item = gv.NamingContainer as RepeaterItem;
        if (item != null)
        {
            var hf = item.FindControl("hfIdComputadora") as HiddenField;
            if (hf != null) idCompu = hf.Value;
        }
    }

    if (string.IsNullOrEmpty(idCompu)) return;

    string script = @"
                (function(){
                    var id = 'collapse" + idCompu + @"';
                    var el = document.getElementById(id);
                    if (!el) return;
                    try {
                        var inst = bootstrap.Collapse.getInstance(el);
                        if (!inst) inst = new bootstrap.Collapse(el, {toggle:false});
                        inst.show();
                    } catch(e) {
                        if (!el.classList.contains('show')) el.classList.add('show');
                    }
                })();";

    if (ScriptManager.GetCurrent(Page) != null)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "openCollapse_" + idCompu, script, true);
    }
    else
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "openCollapse_" + idCompu, script, true);
    }
}

protected void gvComponentes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
{
    var gv = (GridView)sender;
    gv.EditIndex = -1;
    RebindComponentGrid(gv);
    OpenCollapseForGrid(gv);
}

protected void gvComponentes_RowUpdating(object sender, GridViewUpdateEventArgs e)
{
    var gv = (GridView)sender;
    int idComponente = Convert.ToInt32(gv.DataKeys[e.RowIndex].Value);

    GridViewRow row = gv.Rows[e.RowIndex];
    string tipo = GetTextBoxFromCell(row, 1);
    string marca = GetTextBoxFromCell(row, 2);
    string modelo = GetTextBoxFromCell(row, 3);
    string numeroSerie = GetTextBoxFromCell(row, 4);

    var ddl = row.FindControl("ddlEstadoComp") as DropDownList;
    int estadoSeleccionado = 0;
    if (ddl != null) int.TryParse(ddl.SelectedValue, out estadoSeleccionado);

    var componente = new Componente
    {
        Id_Componente = idComponente,
        Tipo = tipo,
        Marca = marca,
        Modelo = modelo,
        Caracteristicas = null,
        Numero_Serie = numeroSerie,
        Estado_Id = estadoSeleccionado,
        Fecha_Compra = DateTime.Now
    };

    var repo = new ComponenteRepository();
    repo.Actualizar(componente);
    //Registro de evento
    HistorialManager.RegistrarEvento(3, componente.Id_Componente, "Componente", (string)Session["NombreInicio"], "Componente con S/N: " + numeroSerie + " fue modificado.");
    gv.EditIndex = -1;
    RebindComponentGrid(gv);
    OpenCollapseForGrid(gv);
}

private string ObtenerNumeroSerieComponente(int idComponente)
{
    string numeroSerie = "(sin SN)";
    using (var con = new SqlConnection(connStr))
    using (var cmd = new SqlCommand("SELECT numero_serie FROM Componentes WHERE id_componente = @id", con))
    {
        cmd.Parameters.AddWithValue("@id", idComponente);
        con.Open();
        var result = cmd.ExecuteScalar();
        if (result != null && result != DBNull.Value)
            numeroSerie = result.ToString();
    }
    return numeroSerie;
}

private void ObtenerDatosComputadora(int idComputadora, out string numeroSerie, out string estado)
{
    numeroSerie = "(sin SN)";
    estado = "(sin estado)";

    using (var con = new SqlConnection(connStr))
    using (var cmd = new SqlCommand("SELECT numero_serie, estado_actual FROM Computadoras WHERE id_computadora = @id", con))
    {
        cmd.Parameters.AddWithValue("@id", idComputadora);
        con.Open();

        using (var dr = cmd.ExecuteReader())
        {
            if (dr.Read())
            {
                if (dr["numero_serie"] != DBNull.Value) numeroSerie = dr["numero_serie"].ToString();
                if (dr["estado_actual"] != DBNull.Value) estado = dr["estado_actual"].ToString();
            }
        }
    }
}

protected void gvComponentes_RowDeleting(object sender, GridViewDeleteEventArgs e)
{
    //var gv = (GridView)sender;
    //int idComponente = Convert.ToInt32(gv.DataKeys[e.RowIndex].Value);

    //var repo = new ComponenteRepository();
    //repo.EliminarComponente(idComponente);

    //RebindComponentGrid(gv);
    //OpenCollapseForGrid(gv);
    var gv = (GridView)sender;
    int idComponente = Convert.ToInt32(gv.DataKeys[e.RowIndex].Value);

    // Obtener numero de serie directamente (no depende del repository)
    string numeroSerie = ObtenerNumeroSerieComponente(idComponente);

    // Registrar baja (2)
    HistorialManager.RegistrarEvento(2, idComponente, "Componente", (string)Session["NombreInicio"], "Componente con S/N: " + numeroSerie + " fue eliminado.");

    // Eliminar via repository (manteniendo tu capa de datos)
    var repo = new ComponenteRepository();
    repo.EliminarComponente(idComponente);

    // Refrescar UI
    RebindComponentGrid(gv);
    OpenCollapseForGrid(gv);

}

private void RebindComponentGrid(GridView gv)
{
    var item = gv.NamingContainer as RepeaterItem;
    if (item == null) { btnBuscar_Click(null, null); return; }

    var hf = item.FindControl("hfIdComputadora") as HiddenField;
    int idCompu = 0;
    if (hf != null && int.TryParse(hf.Value, out idCompu))
    {
        var repo = new ComponenteRepository();
        gv.DataSource = repo.ObtenerPorComputadora(idCompu);
        gv.DataBind();
        gv.Attributes["data-computadora-id"] = idCompu.ToString();
        return;
    }

    btnBuscar_Click(null, null);
}

private string GetTextBoxFromCell(GridViewRow row, int cellIndex)
{
    if (cellIndex < 0 || cellIndex >= row.Cells.Count) return string.Empty;
    var cell = row.Cells[cellIndex];
    if (cell.Controls != null && cell.Controls.Count > 0)
    {
        foreach (Control c in cell.Controls)
        {
            var tb = c as TextBox;
            if (tb != null) return tb.Text;
        }
    }
    return string.Empty;
}

protected void btnAgregar_Click(object sender, EventArgs e)
{
    Response.Redirect("~/AltaComputadora.aspx");
}

// -------------------------------------------------------
// Grid de resultados por componente (edición/eliminación)
protected void gvResultadosComponentes_RowDataBound(object sender, GridViewRowEventArgs e)
{
    if (e.Row.RowType != DataControlRowType.DataRow) return;

    var ddl = e.Row.FindControl("ddlEstadoCompGrid") as DropDownList;
    if (ddl != null)
    {
        ddl.DataSource = GetEstadosPorTipo(2);
        ddl.DataTextField = "descripcion";
        ddl.DataValueField = "id_estado";
        ddl.DataBind();

        object dataItem = e.Row.DataItem;
        if (dataItem is DataRowView drv)
        {
            var estadoVal = drv["Estado_id"];
            if (estadoVal != DBNull.Value)
                ddl.SelectedValue = estadoVal.ToString();
        }
    }
}

protected void gvResultadosComponentes_RowEditing(object sender, GridViewEditEventArgs e)
{
    //gvResultadosComponentes.EditIndex = e.NewEditIndex;
    //BindResultadosComponentesFromCurrentSearch();

}

protected void gvResultadosComponentes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
{
    gvResultadosComponentes.EditIndex = -1;
    BindResultadosComponentesFromCurrentSearch();
}

protected void gvResultadosComponentes_RowUpdating(object sender, GridViewUpdateEventArgs e)
{
    int idComponente = Convert.ToInt32(gvResultadosComponentes.DataKeys[e.RowIndex].Value);
    GridViewRow row = gvResultadosComponentes.Rows[e.RowIndex];

    string tipo = GetTextBoxFromCell(row, 1);
    string marca = GetTextBoxFromCell(row, 2);
    string modelo = GetTextBoxFromCell(row, 3);
    string numeroSerie = GetTextBoxFromCell(row, 4);

    var ddl = row.FindControl("ddlEstadoCompGrid") as DropDownList;
    int estadoSeleccionado = 0;
    if (ddl != null) int.TryParse(ddl.SelectedValue, out estadoSeleccionado);

    var componente = new Componente
    {
        Id_Componente = idComponente,
        Tipo = tipo,
        Marca = marca,
        Modelo = modelo,
        Caracteristicas = null,
        Numero_Serie = numeroSerie,
        Estado_Id = estadoSeleccionado,
        Fecha_Compra = DateTime.Now
    };

    var repo = new ComponenteRepository();
    repo.Actualizar(componente);

    HistorialManager.RegistrarEvento(3, componente.Id_Componente, "Componente", (string)Session["NombreInicio"], "Componente con S/N: " + numeroSerie + " fue modificado.");

    gvResultadosComponentes.EditIndex = -1;
    BindResultadosComponentesFromCurrentSearch();
}

protected void gvResultadosComponentes_RowDeleting(object sender, GridViewDeleteEventArgs e)
{
    //int idComponente = Convert.ToInt32(gvResultadosComponentes.DataKeys[e.RowIndex].Value);
    //var repo = new ComponenteRepository();
    //repo.EliminarComponente(idComponente);

    //BindResultadosComponentesFromCurrentSearch();

    int idComponente = Convert.ToInt32(gvResultadosComponentes.DataKeys[e.RowIndex].Value);

    // Obtener número de serie antes de eliminar
    string numeroSerie = ObtenerNumeroSerieComponente(idComponente);

    // Registrar baja (2)
    HistorialManager.RegistrarEvento(2, idComponente, "Componente", (string)Session["NombreInicio"], "Componente con S/N: " + numeroSerie + " fue eliminado.");

    // Eliminar componente
    var repo = new ComponenteRepository();
    repo.EliminarComponente(idComponente);

    // Refrescar la grilla de resultados
    BindResultadosComponentesFromCurrentSearch();
}

private void BindResultadosComponentesFromCurrentSearch()
{
    string filtro = txtBuscar.Text?.Trim() ?? string.Empty;
    if (string.IsNullOrEmpty(filtro)) return;

    var repo = new ComponenteRepository();
    gvResultadosComponentes.DataSource = repo.Buscar(filtro);
    gvResultadosComponentes.DataBind();
    pnlCompResultados.Visible = true;
}


// Helper: obtiene lista de estados por tipo (1 = PC, 2 = Componente)
private DataTable GetEstadosPorTipo(int idTipo)
{
    var dt = new DataTable();
    using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
    using (var cmd = new SqlCommand("SELECT id_estado, descripcion FROM Estados WHERE id_tipo_estado = @tipo ORDER BY descripcion", con))
    {
        cmd.Parameters.AddWithValue("@tipo", idTipo);
        using (var da = new SqlDataAdapter(cmd))
        {
            da.Fill(dt);
        }
    }
    return dt;
}
    }
}