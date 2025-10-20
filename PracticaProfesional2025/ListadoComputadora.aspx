<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListadoComputadora.aspx.cs" Inherits="PracticaProfesional2025.Computadoras" MasterPageFile="~/Principal.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-3">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h3 class="m-0">Computadoras</h3>
            <div>
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar Nuevo" OnClick="btnAgregar_Click" CssClass="btn btn-primary me-2" />
            </div>
        </div>

        <div class="card mb-3 p-3">
            <div class="row g-2 align-items-center">
                <div class="col-auto">
                    <label for="ddlBuscarPor" class="form-label mb-0">Buscar por:</label>
                </div>
                <div class="col-auto">
                    <asp:DropDownList ID="ddlBuscarPor" runat="server" CssClass="form-select" AutoPostBack="false">
                        <asp:ListItem Value="Computadora" Selected="True">Computadora (por ID)</asp:ListItem>
                        <asp:ListItem Value="Componente">Componente (ID / SN / Tipo / Marca)</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-auto">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" Placeholder="Ingresa ID, SN o texto..." />
                </div>

                <div class="col-auto">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-outline-primary" OnClick="btnBuscar_Click" />
                </div>

                <div class="col-auto">
                    <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger fw-bold" />
                </div>
            </div>
        </div>

        <!-- Resultados por PC (Repeater) -->
        <asp:Panel ID="pnlResultados" runat="server" Visible="false">
            <asp:Repeater ID="rptComputadoras" runat="server" OnItemDataBound="rptComputadoras_ItemDataBound">
                <ItemTemplate>
                    <div class="card mb-2">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <div>
                                <asp:HiddenField ID="hfIdComputadora" runat="server" Value='<%# Eval("id_computadora") %>' />

                                <asp:PlaceHolder ID="phViewMode" runat="server">
                                    <strong>ID:</strong> <%# Eval("id_computadora") %> &nbsp;
                                    <strong>Código:</strong> <%# Eval("codigo_inventario") %> &nbsp;
                                    <strong>SN:</strong> <%# Eval("numero_serie") %>
                                </asp:PlaceHolder>

                                <asp:PlaceHolder ID="phEditMode" runat="server" Visible="false">
                                    <div class="row g-2 align-items-center">
                                        <div class="col-auto">
                                            <label class="form-label mb-0">Código</label>
                                            <asp:TextBox ID="tbCodigoInv" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("codigo_inventario") %>' />
                                        </div>
                                        <div class="col-auto">
                                            <label class="form-label mb-0">SN</label>
                                            <asp:TextBox ID="tbNumeroSerie" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("numero_serie") %>' />
                                        </div>
                                        <div class="col-auto">
                                            <label class="form-label mb-0">Descripción</label>
                                            <asp:TextBox ID="tbDescripcion" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("descripcion") %>' />
                                        </div>
                                        <div class="col-auto">
                                            <label class="form-label mb-0">Estado</label>
                                            <asp:DropDownList ID="ddlEstadoCompu" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
                            </div>

                            <div class="btn-group">
                                <asp:LinkButton ID="lnkToggle" runat="server" CssClass="btn btn-sm btn-outline-secondary"
                                    data-bs-toggle="collapse" href='<%# "#collapse" + Eval("id_computadora") %>' role="button"
                                    aria-expanded="false" aria-controls='<%# "collapse" + Eval("id_computadora") %>'>Ver componentes</asp:LinkButton>

                                <asp:LinkButton ID="lnkEdit" runat="server" CssClass="btn btn-sm btn-outline-primary"
                                    CommandName="EditComputer" CommandArgument='<%# Eval("id_computadora") %>' OnCommand="Computer_Command">Editar</asp:LinkButton>

                                <asp:LinkButton ID="lnkSave" runat="server" CssClass="btn btn-sm btn-success d-none"
                                    CommandName="SaveComputer" CommandArgument='<%# Eval("id_computadora") %>' OnCommand="Computer_Command">Guardar</asp:LinkButton>

                                <asp:LinkButton ID="lnkCancel" runat="server" CssClass="btn btn-sm btn-secondary d-none"
                                    CommandName="CancelEditComputer" CommandArgument='<%# Eval("id_computadora") %>' OnCommand="Computer_Command">Cancelar</asp:LinkButton>

                                <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-sm btn-danger"
                                    CommandName="DeleteComputer" CommandArgument='<%# Eval("id_computadora") %>' OnCommand="Computer_Command"
                                    OnClientClick="return confirm('¿Eliminar esta computadora y sus asociaciones?');">Eliminar</asp:LinkButton>
                            </div>
                        </div>

                        <div class="collapse" id='<%# "collapse" + Eval("id_computadora") %>'>
                            <div class="card-body">
                                <h6>Componentes</h6>
                                <asp:GridView ID="gvComponentes" runat="server" AutoGenerateColumns="False" CssClass="table table-sm"
                                    DataKeyNames="id_componente"
                                    OnRowEditing="gvComponentes_RowEditing"
                                    OnRowCancelingEdit="gvComponentes_RowCancelingEdit"
                                    OnRowUpdating="gvComponentes_RowUpdating"
                                    OnRowDeleting="gvComponentes_RowDeleting"
                                    OnRowDataBound="gvComponentes_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="id_componente" HeaderText="ID" ReadOnly="True" ItemStyle-Width="50px" />
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                        <asp:BoundField DataField="Marca" HeaderText="Marca" />
                                        <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                        <asp:BoundField DataField="Numero_Serie" HeaderText="Nº Serie" />
                                        <asp:TemplateField HeaderText="Estado">
                                            <ItemTemplate>
                                                <%# Eval("EstadoDescripcion") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEstadoComp" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>

        <!-- Resultados por componente -->
        <asp:Panel ID="pnlCompResultados" runat="server" Visible="false">
            <div class="card mb-3">
                <div class="card-header">
                    <strong>Resultados - Componentes</strong>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvResultadosComponentes" runat="server" AutoGenerateColumns="False" CssClass="table table-sm"
                        DataKeyNames="id_componente"
                        OnRowEditing="gvResultadosComponentes_RowEditing"
                        OnRowCancelingEdit="gvResultadosComponentes_RowCancelingEdit"
                        OnRowUpdating="gvResultadosComponentes_RowUpdating"
                        OnRowDeleting="gvResultadosComponentes_RowDeleting"
                        OnRowDataBound="gvResultadosComponentes_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="id_componente" HeaderText="ID" ReadOnly="True" ItemStyle-Width="50px" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                            <asp:BoundField DataField="Marca" HeaderText="Marca" />
                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                            <asp:BoundField DataField="Numero_Serie" HeaderText="Nº Serie" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEstadoCompGrid" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Bootstrap bundle (si tu Master ya lo carga, puedes eliminar esta línea) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>