using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic; 
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

using PracticaProfesional2025; 

namespace PracticaProfesional2025
{
    public partial class AltaComputadora : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLaboratorios();
                txtFechaAlta.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            int cantidad = 1;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 1)
            {
                lblMensaje.Text = "Ingrese una cantidad válida.";
                lblMensaje.CssClass = "text-danger fw-bold";
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString))
            {
                string query = @"INSERT INTO Computadoras 
                                (id_laboratorio, codigo_inventario, numero_serie, descripcion, estado_actual, fecha_alta, fecha_baja)
                                VALUES (@id_laboratorio, @codigo_inventario, @numero_serie, @descripcion, @estado_actual, @fecha_alta, @fecha_baja)";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                for (int i = 1; i <= cantidad; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@id_laboratorio", ddlLaboratorio.SelectedValue);
                    cmd.Parameters.AddWithValue("@codigo_inventario", txtCodigoInventario.Text + "_" + i); // diferenciamos el código
                    cmd.Parameters.AddWithValue("@numero_serie", txtNumeroSerie.Text + "_" + i);           // o podrías dejarlo igual
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                    cmd.Parameters.AddWithValue("@estado_actual", "Activo");
                    cmd.Parameters.AddWithValue("@fecha_alta", txtFechaAlta.Text);

                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }

            lblMensaje.Text = "Se registraron {txtCantidad.Text} computadoras correctamente.";
            lblMensaje.CssClass = "text-success fw-bold";
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

        protected void ddlTipoCarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tipo = ddlTipoCarga.SelectedValue;
            pnlComputadora.Visible = (tipo == "computadora");
            pnlComponente.Visible = (tipo == "componente");
        }


        protected void btnAgregarComponente_Click(object sender, EventArgs e)
        {
            // Crear un nuevo componente desde los campos de texto
            var componente = new Componente
            {
                Tipo = txtTipo.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Numero_Serie = txtNumeroSerieComp.Text
            };

            // Agregar a la lista en memoria
            Componentes.Add(componente);

            // Actualizar la grilla
            gvComponentes.DataSource = Componentes;
            gvComponentes.DataBind();

            // Limpiar los campos
            txtTipo.Text = txtMarca.Text = txtModelo.Text = txtNumeroSerieComp.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Determinar si el usuario está creando una computadora o un componente individual
                string tipoCarga = ddlTipoCarga.SelectedValue;

                if (tipoCarga == "computadora")
                {
                    GuardarComputadoraConComponentes();
                }
                else if (tipoCarga == "componente")
                {
                    GuardarComponenteIndividual();
                }
                else
                {
                    // No es un error crítico, podemos mostrar mensaje en la misma página
                    lblMensaje.Text = "Debe seleccionar un tipo de alta.";
                    lblMensaje.CssClass = "text-danger fw-bold";
                    return;
                }

                lblMensaje.Text = "Registro guardado correctamente.";
                lblMensaje.CssClass = "text-success fw-bold";
            }
            catch (Exception ex)
            {
                // Guardamos la excepción completa en sesión para mostrar en Error.aspx
                Session["ErrorMessage"] = "Ocurrió un error al guardar el registro.";
                Session["ErrorException"] = ex.ToString(); // stack trace completo

                Response.Redirect("Error.aspx");
            }

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListadoComputadora.aspx");
        }

        private List<Componente> Componentes
        {
            get
            {
                if (ViewState["Componentes"] == null)
                    ViewState["Componentes"] = new List<Componente>();
                return (List<Componente>)ViewState["Componentes"];
            }
            set
            {
                ViewState["Componentes"] = value;
            }
        }

        private void GuardarComputadoraConComponentes()
        {
            // 1️⃣ Crear la computadora
            var computadora = new Computadora
            {
                IdLaboratorio = int.Parse(ddlLaboratorio.SelectedValue),
                CodigoInventario = txtCodigoInventario.Text,
                NumeroSerie = txtNumeroSerie.Text,
                Descripcion = txtDescripcion.Text,
                FechaAlta = DateTime.Parse(txtFechaAlta.Text),
                EstadoActual = "1" // Activo
            };

            DebugHelper.PrintObjectProperties(computadora);
            var repoCompu = new ComputadoraRepository();
            int idComputadora = repoCompu.Insert(computadora);

            // 2️⃣ Insertar los componentes asociados
            var repoComponente = new ComponenteRepository();

            foreach (var comp in Componentes)
            {
                int idComponente = repoComponente.Insert(comp);
                repoComponente.VincularConComputadora(idComputadora, idComponente);
            }

            // Limpiar después de guardar
            Componentes.Clear();
            gvComponentes.DataSource = null;
            gvComponentes.DataBind();
        }

        private void GuardarComponenteIndividual()
        {
            var componente = new Componente
            {
                Tipo = txtTipo.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Numero_Serie = txtNumeroSerieComp.Text
            };

            DebugHelper.PrintObjectProperties(componente);
            var repo = new ComponenteRepository();
            repo.Insert(componente);
        }


    }
}
