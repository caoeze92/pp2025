<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="ConsultaStock.aspx.cs" Inherits="PracticaProfesional2025.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
    .filtro-box {
        border: 1px solid #ced4da;
        border-radius: 5px;
        padding: 8px;
        background-color: #f8f9fa;
        margin-bottom: 10px;
    }
    .filtro-box label {
        font-size: 0.85rem;
        margin-bottom: 3px;
    }
    .filtro-box input, .filtro-box select {
        width: 100%;
        height: 28px;
        font-size: 0.85rem;
        padding: 2px 6px;
        border: 1px solid #adb5bd;
        border-radius: 4px;
        box-sizing: border-box;
    }
    .btn-buscar {
        height: 34px;
        font-size: 0.85rem;
    }
    #gvResultados {
        border: 2px solid #0d6efd;
        border-collapse: collapse;
        width: 100%;
    }
    #gvResultados th, #gvResultados td {
        border: 1px solid #0d6efd;
        padding: 8px 12px;
        text-align: center;
    }
    #gvResultados tr:nth-child(even) td {
        background-color: #e9f2ff;
    }
    #gvResultados tr:hover td {
        background-color: #d1e7ff;
    }
    </style>

    <div id="content" class="p-4 p-md-5 pt-5">
        <h2 class="mb-4">CONSULTA DE STOCK</h2>

        <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="fw-bold" EnableViewState="false" />
        <br />

        <asp:ValidationSummary ID="valSummary" runat="server"
            CssClass="alert alert-danger"
            ValidationGroup="buscar"
            HeaderText="Revisá estos campos:" />

        <asp:Panel ID="pnlFiltros" runat="server" CssClass="mb-3">
            <div class="row g-2">
                <!-- ID Computadora -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="txtIdComputadora">ID Computadora</label>
                        <asp:TextBox ID="txtIdComputadora" runat="server" />
                        <asp:RequiredFieldValidator 
                            ID="rfvIdComputadora" 
                            runat="server"
                            ControlToValidate="txtIdComputadora"
                            ErrorMessage="Este campo es obligatorio"
                            ValidationGroup="buscar"
                            Display="Dynamic"
                            ForeColor="Red" />
                        <asp:CompareValidator 
                            ID="cvIdComputadora" 
                            runat="server"
                            ControlToValidate="txtIdComputadora"
                            Operator="DataTypeCheck" 
                            Type="Integer"
                            ErrorMessage="ID Computadora debe ser numérico"
                            ValidationGroup="buscar"
                            Display="Dynamic"
                            ForeColor="Red" />
                    </div>
                </div>

                <!-- ID Componente -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="txtIdComponente">ID Componente</label>
                        <asp:TextBox ID="txtIdComponente" runat="server" />
                        <asp:CompareValidator 
                            ID="cvIdComponente" 
                            runat="server"
                            ControlToValidate="txtIdComponente"
                            Operator="DataTypeCheck" 
                            Type="Integer"
                            ErrorMessage="ID Componente debe ser numérico"
                            ValidationGroup="buscar"
                            Display="Dynamic"
                            ForeColor="Red" />
                    </div>
                </div>

                <!-- Inventario -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="txtCodigoInventario">Inventario</label>
                        <asp:TextBox ID="txtCodigoInventario" runat="server" />
                    </div>
                </div>

                <!-- Nro. Serie -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="txtNumeroSerie">Nro. Serie</label>
                        <asp:TextBox ID="txtNumeroSerie" runat="server" />
                    </div>
                </div>

                <!-- Descripción -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="txtDescripcion">Descripción</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" />
                    </div>
                </div>

                <!-- Laboratorio -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="txtIdLaboratorio">Laboratorio</label>
                        <asp:TextBox ID="txtIdLaboratorio" runat="server" />
                        <asp:CompareValidator 
                            ID="cvIdLaboratorio" 
                            runat="server"
                            ControlToValidate="txtIdLaboratorio"
                            Operator="DataTypeCheck"
                            Type="Integer"
                            Display="Dynamic"
                            ErrorMessage="Laboratorio debe ser numérico"
                            ValidationGroup="buscar" />
                    </div>
                </div>
            </div>

            <div class="row g-2 mt-2">
                <!-- Estado PC -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="ddlEstadoPC">Estado de la PC</label>
                        <asp:DropDownList ID="ddlEstadoPC" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                </div>

                <!-- Estado Componente -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="ddlEstadoComponente">Estado del Componente</label>
                        <asp:DropDownList ID="ddlEstadoComponente" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                </div>

                <!-- Tipo de Componente -->
                <div class="col-md-2">
                    <div class="filtro-box">
                        <label for="txtTipoComponente">Tipo de Componente</label>
                        <asp:TextBox ID="txtTipoComponente" runat="server" />
                    </div>
                </div>

                <!-- Botón Buscar -->
                <div class="col-md-2 d-flex align-items-end">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100 btn-buscar"
                        OnClick="btnBuscar_Click"
                        CausesValidation="true"
                        ValidationGroup="buscar" />
                </div>
            </div>
        </asp:Panel>

        <!-- Tabla de resultados -->
        <h4>Resultados</h4>
        <div class="table-responsive">
            <asp:GridView ID="gvResultados" runat="server" AutoGenerateColumns="false" CssClass="text-center" OnRowDataBound="gvResultados_RowDataBound">
                <HeaderStyle BackColor="#0d6efd" ForeColor="White" HorizontalAlign="Center" Font-Bold="True" />
                <Columns>
                    <asp:BoundField DataField="id_computadora" HeaderText="ID Computadora" ItemStyle-Width="70px" />
                    <asp:BoundField DataField="descripcion" HeaderText="PC Descripción" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="id_componente" HeaderText="ID Componente" ItemStyle-Width="90px" />
                    <asp:BoundField DataField="tipo_componente" HeaderText="Tipo" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="marca" HeaderText="Marca" ItemStyle-Width="90px" />
                    <asp:BoundField DataField="modelo" HeaderText="Modelo" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="estado_pc" HeaderText="Estado PC" ItemStyle-Width="90px" />
                    <asp:BoundField DataField="estado_componente" HeaderText="Estado Componente" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="fecha_asignacion" HeaderText="Asignación" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="fecha_desasignacion" HeaderText="Desasignación" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="110px" />
                    <asp:BoundField DataField="codigo_inventario" HeaderText="Inventario" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="numero_serie" HeaderText="Nro. Serie" ItemStyle-Width="110px" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
