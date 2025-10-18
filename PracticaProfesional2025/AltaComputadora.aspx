<%@ Page Title="Alta de Computadora o Componente" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="AltaComputadora.aspx.cs" Inherits="PracticaProfesional2025.AltaComputadora" %>

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
    .btn-crear {
        height: 36px;
        font-size: 0.9rem;
    }
</style>

<div id="content" class="p-4 p-md-5 pt-5">
    <h2 class="mb-4">ALTA DE COMPUTADORA / COMPONENTE</h2>

    <!-- Selector de modo -->
    <div class="row mb-4">
        <div class="col-md-4">
            <div class="filtro-box">
                <label for="ddlTipoCarga">Tipo de Alta</label>
                <asp:DropDownList 
                    ID="ddlTipoCarga" 
                    runat="server"
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlTipoCarga_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione..." Value="" />
                    <asp:ListItem Text="Computadora Completa" Value="computadora" />
                    <asp:ListItem Text="Componente Individual" Value="componente" />
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <!-- Panel Computadora -->
    <asp:Panel ID="pnlComputadora" runat="server" Visible="false">
        <h4>Datos de la Computadora</h4>
        <div class="row g-2">
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="ddlLaboratorio">Laboratorio</label>
                    <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="form-select" AppendDataBoundItems="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator 
                        ID="rfvLaboratorio" 
                        runat="server"
                        ControlToValidate="ddlLaboratorio"
                        InitialValue="0"
                        ErrorMessage="Debe seleccionar un laboratorio"
                        CssClass="text-danger" 
                        Display="Dynamic" />

                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtCodigoInventario">Código de Inventario</label>
                    <asp:TextBox ID="txtCodigoInventario" runat="server" />
                    <asp:RequiredFieldValidator 
                        ID="rfvCodigoInventario" 
                        runat="server" 
                        ControlToValidate="txtCodigoInventario"
                        ErrorMessage="Debe ingresar un código de inventario"
                        CssClass="text-danger" Display="Dynamic" />

                    <asp:RegularExpressionValidator 
                        ID="revCodigoInventario"
                        runat="server"
                        ControlToValidate="txtCodigoInventario"
                        ValidationExpression="^[A-Za-z0-9\-]+$"
                        ErrorMessage="Solo se permiten letras, números y guiones"
                        CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtNumeroSerie">Número de Serie</label>
                    <asp:TextBox ID="txtNumeroSerie" runat="server" />
                    <asp:RequiredFieldValidator 
                        ID="rfvNumeroSerie" 
                        runat="server"
                        ControlToValidate="txtNumeroSerie"
                        ErrorMessage="Debe ingresar un número de serie"
                        CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtDescripcion">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" />
                    <asp:RequiredFieldValidator 
                        ID="rfvDescripcion" 
                        runat="server"
                        ControlToValidate="txtDescripcion"
                        ErrorMessage="Debe ingresar una descripción"
                        CssClass="text-danger" Display="Dynamic" />

                    <asp:RegularExpressionValidator 
                        ID="revDescripcion"
                        runat="server"
                        ControlToValidate="txtDescripcion"
                        ValidationExpression="^.{5,100}$"
                        ErrorMessage="La descripción debe tener entre 5 y 100 caracteres"
                        CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>
        </div>

        <div class="row g-2 mt-2">
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtFechaAlta">Fecha Alta</label>
                    <asp:TextBox ID="txtFechaAlta" runat="server" TextMode="Date" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtCantidad">Cantidad</label>
                    <asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" Text="1" />
                </div>
            </div>
        </div>

        <h5 class="mt-4">Agregar Componentes de la Computadora</h5>
        <asp:Panel ID="pnlAgregarComponente" runat="server">
            <div class="row g-2">
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtTipo">Tipo</label>
                        <asp:TextBox ID="txtTipo" runat="server" />
                        <!--Val- Tipo -->
                        <asp:RequiredFieldValidator 
                            ID="rfvTipo" 
                            runat="server"
                            ControlToValidate="txtTipo"
                            ErrorMessage="Debe ingresar el tipo de componente"
                            CssClass="text-danger" Display="Dynamic" />

                        <asp:RegularExpressionValidator
                            ID="revTipo"
                            runat="server"
                            ControlToValidate="txtTipo"
                            ValidationExpression="^[A-Za-z0-9ÁÉÍÓÚáéíóúñÑ\s\-_]+$"
                            ErrorMessage="Solo se permiten letras, números, espacios y guiones"
                            CssClass="text-danger"
                            Display="Dynamic" />

                    </div>
                </div>
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtMarca">Marca</label>
                        <asp:TextBox ID="txtMarca" runat="server" />
                        <!--Val- Marca -->
                        <asp:RequiredFieldValidator ID="rfvMarca" runat="server"
                            ControlToValidate="txtMarca"
                            ErrorMessage="Debe ingresar la marca"
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revMarca" runat="server"
                            ControlToValidate="txtMarca"
                            ValidationExpression="^[A-Za-z0-9ÁÉÍÓÚáéíóúñÑ\s\-_]+$"
                            ErrorMessage="Solo se permiten letras, números, espacios y guiones"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtModelo">Modelo</label>
                        <asp:TextBox ID="txtModelo" runat="server" />
                        <!-- Val-Modelo -->
                        <asp:RequiredFieldValidator ID="rfvModelo" runat="server"
                            ControlToValidate="txtModelo"
                            ErrorMessage="Debe ingresar el modelo"
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revModelo" runat="server"
                            ControlToValidate="txtModelo"
                            ValidationExpression="^[A-Za-z0-9ÁÉÍÓÚáéíóúñÑ\s\-_]+$"
                            ErrorMessage="Solo se permiten letras, números, espacios y guiones"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtNumeroSerieComp">N° Serie</label>
                        <asp:TextBox ID="txtNumeroSerieComp" runat="server" />
                        <!-- VAL-N° Serie -->
                        <asp:RequiredFieldValidator ID="rfvNumeroSerieComp" runat="server"
                            ControlToValidate="txtNumeroSerieComp"
                            ErrorMessage="Debe ingresar el número de serie"
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revNumeroSerieComp" runat="server"
                            ControlToValidate="txtNumeroSerieComp"
                            ValidationExpression="^[A-Za-z0-9ÁÉÍÓÚáéíóúñÑ\s\-_]+$"
                            ErrorMessage="Solo se permiten letras, números, espacios y guiones"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>
            </div>
            <asp:Button ID="btnAgregarComponente" runat="server" Text="Agregar Componente" CssClass="btn btn-outline-primary mt-2" OnClick="btnAgregarComponente_Click" />
        </asp:Panel>

        <h5 class="mt-4">Componentes Agregados</h5>
        <div class="table-responsive mt-2">
            <asp:GridView ID="gvComponentes" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField HeaderText="Tipo" DataField="Tipo" />
                    <asp:BoundField HeaderText="Marca" DataField="Marca" />
                    <asp:BoundField HeaderText="Modelo" DataField="Modelo" />
                    <asp:BoundField HeaderText="N° Serie" DataField="Numero_Serie" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <!-- Panel Componente individual -->
    <asp:Panel ID="pnlComponente" runat="server" Visible="false">
        <h4>Datos del Componente Individual</h4>
        <!-- Aquí podrías agregar inputs para crear un componente directamente -->
    </asp:Panel>

    <div class="mt-3">
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Registro" CssClass="btn btn-success btn-crear" OnClick="btnGuardar_Click" />
        <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary btn-volver" OnClick="btnVolver_Click" />
    </div>

    <asp:Label ID="lblMensaje" runat="server" CssClass="mt-3 d-block fw-bold" />
</div>

</asp:Content>
