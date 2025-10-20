using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class AltaComputadora : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLaboratorios();
                CargarComputadorasExistentes(); // para asociar componentes individuales
                txtFechaAlta.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void CargarLaboratorios()
        {
            string query = "SELECT id_laboratorio, nombre FROM Laboratorios";
            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                ddlLaboratorio.DataSource = cmd.ExecuteReader();
                ddlLaboratorio.DataTextField = "nombre";
                ddlLaboratorio.DataValueField = "id_laboratorio";
                ddlLaboratorio.DataBind();
            }
            ddlLaboratorio.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        private void CargarComputadorasExistentes()
        {
            string query = "SELECT id_computadora, codigo_inventario FROM Computadoras";
            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                ddlComputadoraAsociar.DataSource = cmd.ExecuteReader();
                ddlComputadoraAsociar.DataTextField = "codigo_inventario";
                ddlComputadoraAsociar.DataValueField = "id_computadora";
                ddlComputadoraAsociar.DataBind();
            }
            ddlComputadoraAsociar.Items.Insert(0, new ListItem("-- Seleccione Computadora --", ""));
        }

        protected void ddlTipoCarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlComputadora.Visible = ddlTipoCarga.SelectedValue == "computadora";
            pnlComponente.Visible = ddlTipoCarga.SelectedValue == "componente";
        }

        private List<Componente> Componentes
        {
            get
            {
                if (ViewState["Componentes"] == null)
                    ViewState["Componentes"] = new List<Componente>();
                return (List<Componente>)ViewState["Componentes"];
            }
            set { ViewState["Componentes"] = value; }
        }

        protected void btnAgregarComponente_Click(object sender, EventArgs e)
        {
            var componente = new Componente
            {
                Tipo = txtTipo.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,    
                Caracteristicas = txtCarac != null ? txtCarac.Text : string.Empty,
                Estado_Id = 1,
                Numero_Serie = txtNumeroSerieComp.Text,
                Fecha_Compra = DateTime.Now
            };

            Componentes.Add(componente);
            gvComponentes.DataSource = Componentes;
            gvComponentes.DataBind();

            txtTipo.Text = txtMarca.Text = txtModelo.Text = (txtCarac != null ? txtCarac.Text = "" : "");
            txtNumeroSerieComp.Text = "";
        }

        // Maneja el botón de eliminar en la grilla usando NamingContainer para obtener el índice real
        protected void gvComponentes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                var btn = e.CommandSource as Button;
                if (btn == null) return;

                var row = btn.NamingContainer as GridViewRow;
                if (row == null) return;

                int index = row.RowIndex;
                if (index >= 0 && index < Componentes.Count)
                {
                    Componentes.RemoveAt(index);
                    gvComponentes.DataSource = Componentes;
                    gvComponentes.DataBind();
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string tipoCarga = ddlTipoCarga.SelectedValue;

                if (tipoCarga == "computadora")
                    GuardarComputadoraConComponentes();
                else if (tipoCarga == "componente")
                    GuardarComponenteIndividual();
                else
                {
                    lblMensaje.Text = "Debe seleccionar un tipo de alta.";
                    lblMensaje.CssClass = "text-danger fw-bold";
                    return;
                }

                lblMensaje.Text = "Registro guardado correctamente.";
                lblMensaje.CssClass = "text-success fw-bold";
            }
            catch (Exception ex)
            {
                Session["ErrorMessage"] = "Ocurrió un error al guardar el registro.";
                Session["ErrorException"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }

        private void GuardarComputadoraConComponentes()
        {
            int cant;
            bool ok = int.TryParse(txtCantidad.Text, out cant);
            int cantidadComputadoras = ok ? cant : 1;

            // Validar laboratorio seleccionado
            int idLaboratorio;
            if (!int.TryParse(ddlLaboratorio.SelectedValue, out idLaboratorio))
            {
                lblMensaje.Text = "Debe seleccionar un laboratorio válido.";
                lblMensaje.CssClass = "text-danger fw-bold";
                return;
            }

            // Validar/parsear fecha
            DateTime fechaAlta;
            if (!DateTime.TryParse(txtFechaAlta.Text, out fechaAlta))
            {
                fechaAlta = DateTime.Now;
            }

            var repoCompu = new ComputadoraRepository();

            for (int i = 1; i <= cantidadComputadoras; i++)
            {
                // Usar el campo correcto para base de número de serie (campo de la computadora)
                string baseNumeroSerie = txtNumeroSerie != null ? txtNumeroSerie.Text ?? string.Empty : string.Empty;
                if (string.IsNullOrWhiteSpace(baseNumeroSerie))
                {
                    // Generar uno temporal para evitar errores de formato
                    baseNumeroSerie = "SN" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                // Generar número de serie único consultando el repo
                int sufijo = i;
                string numeroSerie = string.Format("{0}_{1}", baseNumeroSerie, sufijo);
                while (repoCompu.ExisteNumeroSerie(numeroSerie))
                {
                    sufijo++;
                    numeroSerie = string.Format("{0}_{1}", baseNumeroSerie, sufijo);
                }

                var computadora = new Computadora
                {
                    IdLaboratorio = idLaboratorio,
                    CodigoInventario = string.Format("{0}_{1}", txtCodigoInventario.Text, i),
                    NumeroSerie = numeroSerie, // asignamos el SN único generado
                    Descripcion = txtDescripcion.Text,
                    FechaAlta = fechaAlta,
                    EstadoActual = "1" // en funcionamiento
                };

                // Insertar la computadora con sus componentes (observación más abajo)
                int idComputadora = repoCompu.InsertarComputadoraConComponentes(computadora, Componentes);
            }

            Componentes.Clear();
            gvComponentes.DataSource = null;
            gvComponentes.DataBind();
        }

        private void GuardarComponenteIndividual()
        {
            // Para cantidad 
            int cant;
            bool okCantidad = int.TryParse(txtCantidadIndividual.Text, out cant);
            int cantidad = okCantidad ? cant : 1;

            // Para idComputadora 
            int id;
            bool okId = int.TryParse(ddlComputadoraAsociar.SelectedValue, out id);
            int idComputadora = okId ? id : 0;

            // Validaciones mínimas
            if (string.IsNullOrWhiteSpace(txtTipoCompIndividual.Text) ||
                string.IsNullOrWhiteSpace(txtMarcaCompIndividual.Text) ||
                string.IsNullOrWhiteSpace(txtNumeroSerieIndividual.Text))
            {
                lblMensaje.Text = "Debe completar todos los campos del componente.";
                lblMensaje.CssClass = "text-danger fw-bold";
                return;
            }

            var repoComponente = new ComponenteRepository();
            int componentesGuardados = 0;

            for (int i = 1; i <= cantidad; i++)
            {
                // Generar un Numero_Serie único
                string baseNumeroSerie = txtNumeroSerieIndividual.Text;
                string numeroSerie = string.Format("{0}_{1}", baseNumeroSerie, i);
                int sufijo = i;

                while (repoComponente.ExisteNumeroSerie(numeroSerie))
                {
                    sufijo++;
                    numeroSerie = string.Format("{0}_{1}", baseNumeroSerie, sufijo);
                }

                var componente = new Componente
                {
                    Tipo = txtTipoCompIndividual.Text,
                    Marca = txtMarcaCompIndividual.Text,
                    Modelo = txtModeloCompIndividual.Text,
                    Caracteristicas = txtCaracCompIndividual.Text,
                    Numero_Serie = numeroSerie,
                    Estado_Id = (idComputadora != 0) ? 6 : 7, // asigna 6 si tiene PC, 7 si no
                    Fecha_Compra = DateTime.Now
                };

                // Guardar en DB
                int idComponente = repoComponente.Insert(componente);

                // Vincular con computadora si aplica
                if (idComputadora > 0)
                    repoComponente.VincularConComputadora(idComputadora, idComponente, txtFechaAlta.Text);

                componentesGuardados++;
            }

            lblMensaje.Text = string.Format("{0} componente(s) guardado(s) correctamente.", componentesGuardados);
            lblMensaje.CssClass = "text-success fw-bold";

            // Limpiar campos
            txtTipoCompIndividual.Text = txtMarcaCompIndividual.Text =
                txtModeloCompIndividual.Text = txtNumeroSerieIndividual.Text = txtCaracCompIndividual.Text = "";
            ddlComputadoraAsociar.SelectedIndex = 0;
        }


        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListadoComputadora.aspx");
        }
    }
}
