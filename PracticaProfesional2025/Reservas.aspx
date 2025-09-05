<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Reservas.aspx.cs" Inherits="PracticaProfesional2025.Reservas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="content" class="p-4 p-md-5 pt-5">
        <h2 class="mb-4">RESERVAS</h2>

        <asp:Panel ID="pnlReserva" runat="server" CssClass="card p-4 shadow">

            <!-- Selección de laboratorio + Inventario -->
            <h3>1. Seleccionar Laboratorio</h3>
            <div class="row mb-3">
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlLaboratorios" runat="server" CssClass="form-select"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlLaboratorios_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-8">
                    <asp:Panel ID="pnlInventario" runat="server" Visible="false">
                        <h5>Inventario del laboratorio</h5>
                        <asp:GridView ID="gvInventario" runat="server" CssClass="table table-bordered table-striped"
                            AutoGenerateColumns="true" AllowPaging="true" PageSize="10" 
                            OnPageIndexChanging="gvInventario_PageIndexChanging"
                            PagerSettings-Mode="NextPreviousFirstLast"
                            PagerSettings-NextPageText="&raquo;" PagerSettings-PreviousPageText="&laquo;"
                            PagerStyle-CssClass="pagination justify-content-center mt-3">
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>

            <!-- Fecha + Hora -->
            <asp:Panel ID="pnlFechaHora" runat="server" Visible="false" CssClass="mt-4">
                <h3>2. Seleccionar Fecha y Horario</h3>
                <div class="row">
                    <!-- Calendario -->
                    <div class="col-md-6">
                        <asp:Calendar ID="calFecha" runat="server" CssClass="mb-3 border rounded w-100"
                            OnDayRender="calFecha_DayRender" OnSelectionChanged="calFecha_SelectionChanged" />
                    </div>

                    <!-- Horarios -->
                    <div class="col-md-6">
                        <div class="row mb-3">
                            <div class="col">
                                <label for="ddlHoraInicio" class="form-label">Desde</label>
                                <asp:DropDownList ID="ddlHoraInicio" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                            <div class="col">
                                <label for="ddlHoraFin" class="form-label">Hasta</label>
                                <asp:DropDownList ID="ddlHoraFin" runat="server" CssClass="form-select"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Motivo y Confirmación -->
            <asp:Panel ID="pnlConfirmacion" runat="server" Visible="false">
                <h3>3. Motivo de la Reserva</h3>
                <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-control mb-3"
                             TextMode="MultiLine" Rows="3" 
                             Style="background-color: #f0f8ff; border: 1px solid #007bff; padding: 5px;"
                             placeholder="Escriba el motivo de la reserva aquí..." />

                <asp:Button ID="btnReservar" runat="server" Text="Reservar"
                    CssClass="btn btn-primary" OnClick="btnReservar_Click" />

                <!-- Mensaje de validación/confirmación -->
                <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mt-3 fw-bold"></asp:Label>
            </asp:Panel>
        </asp:Panel>
    </div>

</asp:Content>
