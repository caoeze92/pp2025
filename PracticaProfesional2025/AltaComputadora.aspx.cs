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
            // Crear un componente temporal tomando los valores de los TextBox
            var componente = new Componente
            {
                Id_Componente = 0, // temporal hasta guardar en DB
                Tipo = txtTipo.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Numero_Serie = txtNumeroSerieComp.Text,
                Caracteristicas = "",       // si tenés input, reemplazalo por txtCaracteristicas.Text
                Estado_Id = 1,              // por defecto o según tu lógica
                Fecha_Compra = DateTime.Now // o tomar de un input si lo tenés
            };

            // Agregar a la lista temporal en ViewState
            var lista = ComponentesTemp;
            lista.Add(componente);
            ComponentesTemp = lista;

            // Refrescar GridView
            gvComponentes.DataSource = ComponentesTemp;
            gvComponentes.DataBind();

            // Limpiar los inputs
            txtTipo.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtNumeroSerieComp.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListadoComputadora.aspx");
        }

        private List<Componente> ComponentesTemp
        {
            get
            {
                if (ViewState["ComponentesTemp"] == null)
                    ViewState["ComponentesTemp"] = new List<Componente>();
                return (List<Componente>)ViewState["ComponentesTemp"];
            }
            set
            {
                ViewState["ComponentesTemp"] = value;
            }
        }

    }
}
