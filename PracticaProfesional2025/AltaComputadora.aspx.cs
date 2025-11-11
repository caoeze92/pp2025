using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class AltaComputadora : System.Web.UI.Page
    {
        // Variable global de conexion
        string connStr = ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLaboratorios();
                CargarComputadorasExistentes(); // para asociar componentes individuales
                txtFechaAlta.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }

            // Asegurar bloqueo en servidor (por si viene directo postback)
            SetSerialFieldsReadonly();
        }

        private void SetSerialFieldsReadonly()
        {
            int cant;
            if (int.TryParse(txtCantidad.Text, out cant) && cant > 1)
            {
                // crear y almacenar un prefijo AUTO por lote si no existe
                if (ViewState["AutoPrefix_Computadora"] == null)
                    ViewState["AutoPrefix_Computadora"] = GenerateAutoPrefixServer();
                txtNumeroSerie.Text = ViewState["AutoPrefix_Computadora"].ToString();
                txtNumeroSerie.ReadOnly = true;

                if (ViewState["AutoPrefix_Componente"] == null)
                    ViewState["AutoPrefix_Componente"] = GenerateAutoPrefixServer();
                txtNumeroSerieComp.Text = ViewState["AutoPrefix_Componente"].ToString();
                txtNumeroSerieComp.ReadOnly = true;
            }
            else
            {
                txtNumeroSerie.ReadOnly = false;
                txtNumeroSerieComp.ReadOnly = false;
                // eliminar prefijos si el usuario bajó la cantidad a 1
                ViewState.Remove("AutoPrefix_Computadora");
                ViewState.Remove("AutoPrefix_Componente");
            }

            int cantInd;
            if (int.TryParse(txtCantidadIndividual.Text, out cantInd) && cantInd > 1)
            {
                if (ViewState["AutoPrefix_Individual"] == null)
                    ViewState["AutoPrefix_Individual"] = GenerateAutoPrefixServer();
                txtNumeroSerieIndividual.Text = ViewState["AutoPrefix_Individual"].ToString();
                txtNumeroSerieIndividual.ReadOnly = true;
            }
            else
            {
                txtNumeroSerieIndividual.ReadOnly = false;
                ViewState.Remove("AutoPrefix_Individual");
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
            // Si se están creando varias computadoras, usar el prefijo AUTO para los componentes agregados
            int cant;
            bool ok = int.TryParse(txtCantidad.Text, out cant);
            bool multipleComputadoras = ok && cant > 1;

            string componenteSN = txtNumeroSerieComp.Text;
            if (multipleComputadoras)
            {
                // tomar el prefijo ya guardado o generar uno nuevo
                if (ViewState["AutoPrefix_Componente"] == null)
                    ViewState["AutoPrefix_Componente"] = GenerateAutoPrefixServer();
                componenteSN = ViewState["AutoPrefix_Componente"].ToString();
            }

            var componente = new Componente
            {
                Tipo = txtTipo.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Caracteristicas = txtCarac != null ? txtCarac.Text : string.Empty,
                Estado_Id = 1,
                Numero_Serie = componenteSN,
                Fecha_Compra = DateTime.Now
            };

            Componentes.Add(componente);
            gvComponentes.DataSource = Componentes;
            gvComponentes.DataBind();

            txtTipo.Text = txtMarca.Text = txtModelo.Text = (txtCarac != null ? txtCarac.Text = "" : "");
            // no borramos prefijo en textbox si existe; dejamos vacío para nueva entrada
            if (!multipleComputadoras)
                txtNumeroSerieComp.Text = "";
            else
                txtNumeroSerieComp.Text = ViewState["AutoPrefix_Componente"].ToString();
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
                {
                    GuardarComputadoraConComponentes();
                    // Select del ultimo registro creado de id compu y SN
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        conn.Open();
                        string querySelect = "SELECT TOP 1 id_computadora, numero_serie FROM Computadoras ORDER BY id_computadora DESC";
                        SqlCommand cmdSelect = new SqlCommand(querySelect, conn);

                        string idComp = string.Empty;
                        string serialNum = string.Empty;

                        using (SqlDataReader dr = cmdSelect.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                idComp = dr["id_computadora"].ToString();
                                serialNum = dr["numero_serie"].ToString();
                            }
                        }

                        cmdSelect.ExecuteNonQuery();
                        conn.Close();
                        HistorialManager.RegistrarEvento(1, int.Parse(idComp) , "Computadora", (string)Session["NombreInicio"], "Alta de nueva Computadora" + " ID: " + idComp + " S/N: " + serialNum + " generada con éxito");
                    }
                }
                // Fin select + Metodo aplicado
                else if (tipoCarga == "componente")
                {
                    GuardarComponenteIndividual();
                }
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

            // obtener prefijo de lote para computadoras si existe
            string basePrefixComputadora = ViewState["AutoPrefix_Computadora"] as string;
            if (cantidadComputadoras > 1 && string.IsNullOrWhiteSpace(basePrefixComputadora))
            {
                basePrefixComputadora = !string.IsNullOrWhiteSpace(txtNumeroSerie.Text) && txtNumeroSerie.Text.StartsWith("AUTO-")
                    ? txtNumeroSerie.Text
                    : GenerateAutoPrefixServer();
            }

            for (int i = 1; i <= cantidadComputadoras; i++)
            {
                string numeroSerie;
                if (cantidadComputadoras > 1)
                {
                    string candidateBase = !string.IsNullOrWhiteSpace(basePrefixComputadora) ? basePrefixComputadora : (txtNumeroSerie != null ? txtNumeroSerie.Text ?? string.Empty : string.Empty);
                    if (string.IsNullOrWhiteSpace(candidateBase))
                        candidateBase = "SN" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    numeroSerie = string.Format("{0}_{1}", candidateBase, i);
                }
                else
                {
                    // única unidad: respetar lo ingresado o generar si está vacío
                    numeroSerie = !string.IsNullOrWhiteSpace(txtNumeroSerie.Text) ? txtNumeroSerie.Text : "SN" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                // Asegurar unicidad consultando el repo
                int sufijo = 0;
                string uniqueSN = numeroSerie;
                while (repoCompu.ExisteNumeroSerie(uniqueSN))
                {
                    sufijo++;
                    uniqueSN = string.Format("{0}_{1}", numeroSerie, sufijo);
                }

                var computadora = new Computadora
                {
                    IdLaboratorio = idLaboratorio,
                    CodigoInventario = string.Format("{0}_{1}", txtCodigoInventario.Text, i),
                    NumeroSerie = uniqueSN,
                    Descripcion = txtDescripcion.Text,
                    FechaAlta = fechaAlta,
                    EstadoActual = "1" // en funcionamiento
                };

                // preparar componentes para insertar asociados a esta computadora
                var componentesParaInsertar = new List<Componente>();
                int compIndex = 1;
                foreach (var compOriginal in Componentes)
                {
                    var compCopy = new Componente
                    {
                        Tipo = compOriginal.Tipo,
                        Marca = compOriginal.Marca,
                        Modelo = compOriginal.Modelo,
                        Caracteristicas = compOriginal.Caracteristicas,
                        Estado_Id = compOriginal.Estado_Id,
                        Fecha_Compra = compOriginal.Fecha_Compra
                    };

                    // generar SN por componente por cada computadora si el prefijo es AUTO o si se crean múltiples unidades
                    if (cantidadComputadoras > 1)
                    {
                        string compBase = compOriginal.Numero_Serie;
                        if (string.IsNullOrWhiteSpace(compBase) || !compBase.StartsWith("AUTO-"))
                            compBase = ViewState["AutoPrefix_Componente"] as string ?? GenerateAutoPrefixServer();

                        string candidateCompSN = string.Format("{0}_{1}_{2}", compBase, i, compIndex);
                        // asegurar unicidad
                        var repoCompCheck = new ComponenteRepository();
                        int intent = 0;
                        string uniqueCompSN = candidateCompSN;
                        while (repoCompCheck.ExisteNumeroSerie(uniqueCompSN))
                        {
                            intent++;
                            uniqueCompSN = string.Format("{0}_{1}", candidateCompSN, intent);
                        }
                        compCopy.Numero_Serie = uniqueCompSN;
                    }
                    else
                    {
                        // una sola computadora: respetar el SN ingresado o generar temporal
                        compCopy.Numero_Serie = string.IsNullOrWhiteSpace(compOriginal.Numero_Serie) ? GenerateAutoPrefixServer() : compOriginal.Numero_Serie;
                        // asegurar unicidad
                        var repoCompCheck = new ComponenteRepository();
                        string candidateCompSN = compCopy.Numero_Serie;
                        int intent = 0;
                        while (repoCompCheck.ExisteNumeroSerie(candidateCompSN))
                        {
                            intent++;
                            candidateCompSN = string.Format("{0}_{1}", compCopy.Numero_Serie, intent);
                        }
                        compCopy.Numero_Serie = candidateCompSN;
                    }

                    componentesParaInsertar.Add(compCopy);
                    compIndex++;
                }

                // Insertar la computadora con sus componentes (el repo debe insertar los componentes que reciba)
                int idComputadora = repoCompu.InsertarComputadoraConComponentes(computadora, componentesParaInsertar);
            }

            // limpiar estado
            Componentes.Clear();
            gvComponentes.DataSource = null;
            gvComponentes.DataBind();
            ViewState.Remove("AutoPrefix_Computadora");
            ViewState.Remove("AutoPrefix_Componente");
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
                (cantidad == 1 && string.IsNullOrWhiteSpace(txtNumeroSerieIndividual.Text)))
            {
                lblMensaje.Text = "Debe completar todos los campos del componente.";
                lblMensaje.CssClass = "text-danger fw-bold";
                return;
            }

            var repoComponente = new ComponenteRepository();
            int componentesGuardados = 0;

            // obtener prefijo de lote para individuales si existe
            string basePrefixIndividual = ViewState["AutoPrefix_Individual"] as string;
            if (cantidad > 1 && string.IsNullOrWhiteSpace(basePrefixIndividual))
            {
                basePrefixIndividual = !string.IsNullOrWhiteSpace(txtNumeroSerieIndividual.Text) && txtNumeroSerieIndividual.Text.StartsWith("AUTO-")
                    ? txtNumeroSerieIndividual.Text
                    : GenerateAutoPrefixServer();
            }

            for (int i = 1; i <= cantidad; i++)
            {
                string numeroSerie;
                if (cantidad > 1)
                {
                    string candidateBase = !string.IsNullOrWhiteSpace(basePrefixIndividual) ? basePrefixIndividual : txtNumeroSerieIndividual.Text;
                    if (string.IsNullOrWhiteSpace(candidateBase))
                        candidateBase = GenerateAutoPrefixServer();
                    numeroSerie = string.Format("{0}_{1}", candidateBase, i);
                }
                else
                {
                    numeroSerie = !string.IsNullOrWhiteSpace(txtNumeroSerieIndividual.Text) ? txtNumeroSerieIndividual.Text : GenerateAutoPrefixServer();
                }

                // Asegurar unicidad
                int sufijo = 0;
                string uniqueSN = numeroSerie;
                while (repoComponente.ExisteNumeroSerie(uniqueSN))
                {
                    sufijo++;
                    uniqueSN = string.Format("{0}_{1}", numeroSerie, sufijo);
                }

                var componente = new Componente
                {
                    Tipo = txtTipoCompIndividual.Text,
                    Marca = txtMarcaCompIndividual.Text,
                    Modelo = txtModeloCompIndividual.Text,
                    Caracteristicas = txtCaracCompIndividual.Text,
                    Numero_Serie = uniqueSN,
                    Estado_Id = (idComputadora != 0) ? 6 : 7, // asigna 6 si tiene PC, 7 si no
                    Fecha_Compra = DateTime.Now
                };

                // Guardar en DB
                int idComponente = repoComponente.Insert(componente);

                // Vincular con computadora si aplica
                if (idComputadora > 0)
                    repoComponente.VincularConComputadora(idComputadora, idComponente, txtFechaAlta.Text);

                // Guardo registro en la tabla eventos/historial
                HistorialManager.RegistrarEvento(1, idComponente,"Componente",(string)Session["NombreInicio"],"Nuevo componente agregado con S/N: " + uniqueSN + ", cargado con éxito");

                componentesGuardados++;
            }

            lblMensaje.Text = string.Format("{0} componente(s) guardado(s) correctamente.", componentesGuardados);
            lblMensaje.CssClass = "text-success fw-bold";

            // Limpiar campos
            txtTipoCompIndividual.Text = txtMarcaCompIndividual.Text =
                txtModeloCompIndividual.Text = txtNumeroSerieIndividual.Text = txtCaracCompIndividual.Text = "";
            ddlComputadoraAsociar.SelectedIndex = 0;
            ViewState.Remove("AutoPrefix_Individual");
        }


        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListadoComputadora.aspx");
        }

        // Genera un prefijo AUTO legible y corto para lotes
        private static string GenerateAutoPrefixServer()
        {
            return "AUTO-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}
