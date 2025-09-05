using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PracticaProfesional2025
{
    public partial class Reservas : System.Web.UI.Page
    {
        private DataTable reservasLaboratorio
        {
            get { return ViewState["Reservas"] as DataTable; }
            set { ViewState["Reservas"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLaboratorios();
                CargarHoras();
                lblMensaje.Text = ""; // limpiar mensaje al cargar
            }
        }

        private void CargarLaboratorios()
        {
            string query = "SELECT id_laboratorio, nombre FROM Laboratorios";
            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                ddlLaboratorios.DataSource = cmd.ExecuteReader();
                ddlLaboratorios.DataTextField = "nombre";
                ddlLaboratorios.DataValueField = "id_laboratorio";
                ddlLaboratorios.DataBind();
            }
            ddlLaboratorios.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        private void CargarHoras()
        {
            ddlHoraInicio.Items.Clear();
            ddlHoraFin.Items.Clear();
            for (int i = 15; i <= 23; i++) // de 17 a 23 hs
            {
                ddlHoraInicio.Items.Add(i + ":00");
                ddlHoraFin.Items.Add(i + ":00");
            }
        }

        protected void ddlLaboratorios_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            if (!string.IsNullOrEmpty(ddlLaboratorios.SelectedValue))
            {
                CargarInventario();
                CargarReservasDelLaboratorio();
                pnlInventario.Visible = true;
                pnlFechaHora.Visible = true;
                pnlConfirmacion.Visible = true;
            }
            else
            {
                pnlInventario.Visible = false;
                pnlFechaHora.Visible = false;
                pnlConfirmacion.Visible = false;
            }
        }

        private void CargarInventario()
        {
            string queryInventario = @"SELECT 
                                    c.codigo_inventario, 
                                    c.numero_serie,
                                    c.descripcion, 
                                    e.descripcion AS estado
                                FROM Computadoras c
                                INNER JOIN Estados e 
                                    ON c.estado_actual = e.id_estado
                                WHERE c.id_laboratorio = @idLab;";

            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(queryInventario, con))
            {
                cmd.Parameters.AddWithValue("@idLab", ddlLaboratorios.SelectedValue);
                con.Open();

                // Usar DataAdapter + DataTable en vez de DataReader
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvInventario.DataSource = dt;
                gvInventario.DataBind();
            }
        }


        protected void calFecha_DayRender(object sender, DayRenderEventArgs e)
        {
            // Bloquear días pasados
            if (e.Day.Date < DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.BackColor = System.Drawing.Color.LightGray;
                e.Cell.ToolTip = "No se pueden reservar fechas pasadas";
                return;
            }

            // Bloquear días con todas las horas ocupadas
            if (reservasLaboratorio != null && reservasLaboratorio.Rows.Count > 0)
            {
                int totalHoras = 7; // Horas de 17 a 23
                int horasOcupadas = 0;

                foreach (DataRow row in reservasLaboratorio.Rows)
                {
                    DateTime inicio = Convert.ToDateTime(row["fecha_inicio"]);
                    DateTime fin = Convert.ToDateTime(row["fecha_fin"]);

                    if (inicio.Date == e.Day.Date)
                    {
                        horasOcupadas += fin.Hour - inicio.Hour;
                    }
                }

                if (horasOcupadas >= totalHoras)
                {
                    e.Day.IsSelectable = false;
                    e.Cell.BackColor = System.Drawing.Color.LightCoral;
                    e.Cell.ToolTip = "Todas las horas están ocupadas en este día";
                    return;
                }
            }

            // Marcar día seleccionado en verde
            if (e.Day.Date == calFecha.SelectedDate)
            {
                e.Cell.BackColor = System.Drawing.Color.LightGreen;
                e.Cell.ToolTip = "Día seleccionado";
            }
        }

        protected void calFecha_SelectionChanged(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            CargarHoras();
            DateTime fechaSeleccionada = calFecha.SelectedDate;

            // Bloquear horas ocupadas
            if (reservasLaboratorio != null)
            {
                foreach (DataRow row in reservasLaboratorio.Rows)
                {
                    DateTime inicio = Convert.ToDateTime(row["fecha_inicio"]);
                    DateTime fin = Convert.ToDateTime(row["fecha_fin"]);

                    if (inicio.Date == fechaSeleccionada.Date)
                    {
                        for (int i = inicio.Hour; i < fin.Hour; i++)
                        {
                            ListItem itemIni = ddlHoraInicio.Items.FindByText(i + ":00");
                            ListItem itemFin = ddlHoraFin.Items.FindByText((i + 1) + ":00");
                            if (itemIni != null) itemIni.Enabled = false;
                            if (itemFin != null) itemFin.Enabled = false;
                        }
                    }
                }
            }
        }

        protected void btnReservar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            if (string.IsNullOrEmpty(ddlLaboratorios.SelectedValue) ||
                calFecha.SelectedDate == DateTime.MinValue ||
                string.IsNullOrEmpty(ddlHoraInicio.SelectedValue) ||
                string.IsNullOrEmpty(ddlHoraFin.SelectedValue))
            {
                lblMensaje.CssClass = "text-danger fw-bold";
                lblMensaje.Text = "Por favor complete todos los campos.";
                return;
            }

            DateTime fechaSeleccionada = calFecha.SelectedDate;
            DateTime fechaInicio = DateTime.Parse(fechaSeleccionada.ToShortDateString() + " " + ddlHoraInicio.SelectedValue);
            DateTime fechaFin = DateTime.Parse(fechaSeleccionada.ToShortDateString() + " " + ddlHoraFin.SelectedValue);

            // Validar fecha pasada
            if (fechaInicio < DateTime.Now)
            {
                lblMensaje.CssClass = "text-danger fw-bold";
                lblMensaje.Text = "No se pueden reservar horarios en el pasado.";
                return;
            }

            // Validar que hora fin > hora inicio
            if (fechaFin <= fechaInicio)
            {
                lblMensaje.CssClass = "text-danger fw-bold";
                lblMensaje.Text = "La hora de fin debe ser mayor que la de inicio.";
                return;
            }

            // Validar solapamiento con otras reservas
            if (reservasLaboratorio != null)
            {
                foreach (DataRow row in reservasLaboratorio.Rows)
                {
                    DateTime inicioExistente = Convert.ToDateTime(row["fecha_inicio"]);
                    DateTime finExistente = Convert.ToDateTime(row["fecha_fin"]);

                    if (fechaInicio < finExistente && fechaFin > inicioExistente)
                    {
                        lblMensaje.CssClass = "text-danger fw-bold";
                        lblMensaje.Text = "El horario seleccionado ya está ocupado.";
                        return;
                    }
                }
            }

            // Guardar reserva
            string query = @"INSERT INTO Reservas (id_usuario, id_laboratorio, fecha_inicio, fecha_fin, motivo)
                             VALUES (@idUsuario, @idLab, @fechaIni, @fechaFin, @motivo)";

            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idUsuario", Session["idUsuario"]); // id del usuario logueado
                cmd.Parameters.AddWithValue("@idLab", ddlLaboratorios.SelectedValue);
                cmd.Parameters.AddWithValue("@fechaIni", fechaInicio);
                cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                cmd.Parameters.AddWithValue("@motivo", txtMotivo.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Guardamos la info de la reserva en Session
            Session["ReservaLaboratorio"] = ddlLaboratorios.SelectedItem.Text;
            Session["ReservaFecha"] = fechaSeleccionada.ToShortDateString();
            Session["ReservaHoraInicio"] = ddlHoraInicio.SelectedValue;
            Session["ReservaHoraFin"] = ddlHoraFin.SelectedValue;
            Session["ReservaMotivo"] = txtMotivo.Text;

            // Redirigir a la página de éxito
            Response.Redirect("ReservaExitosa.aspx");
        }

        private void CargarReservasDelLaboratorio()
        {
            if (!string.IsNullOrEmpty(ddlLaboratorios.SelectedValue))
            {
                string queryReservas = @"SELECT fecha_inicio, fecha_fin 
                                         FROM Reservas
                                         WHERE id_laboratorio = @idLab";

                using (SqlConnection con = ConnectionFactory.GetConnection())
                using (SqlCommand cmd = new SqlCommand(queryReservas, con))
                {
                    cmd.Parameters.AddWithValue("@idLab", ddlLaboratorios.SelectedValue);
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    reservasLaboratorio = dt;
                }
            }
        }


            // Evento para manejar la paginación del GridView
            protected void gvInventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                gvInventario.PageIndex = e.NewPageIndex;
                CargarInventario(); // recarga los datos para la página seleccionada
            }
        
    }
}
