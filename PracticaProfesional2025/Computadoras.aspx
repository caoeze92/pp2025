<%@ Page Title="ABM Computadoras" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Computadoras.aspx.cs" Inherits="PracticaProfesional2025.Computadoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .table th, .table td {
            border: 2px solid #dee2e6 !important;
        }
        .table-striped tbody tr:nth-of-type(odd) {
            background-color: #f9f9f9;
        }
        .table-hover tbody tr:hover {
            background-color: #e8f0fe !important;
        }
    </style>

    <div class="container mt-5">
        <h2 class="mb-4">Gestión de Computadoras</h2>

        <asp:GridView 
            ID="gvComputadoras" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="table table-bordered table-striped table-hover"
            DataKeyNames="id_computadora"
            OnRowEditing="gvComputadoras_RowEditing"
            OnRowCancelingEdit="gvComputadoras_RowCancelingEdit"
            OnRowUpdating="gvComputadoras_RowUpdating"
            OnRowDeleting="gvComputadoras_RowDeleting">

            <Columns>
                <asp:BoundField DataField="id_computadora" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="id_laboratorio" HeaderText="Laboratorio" />
                <asp:BoundField DataField="codigo_inventario" HeaderText="Código Inventario" />
                <asp:BoundField DataField="numero_serie" HeaderText="Número de Serie" />
                <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="estado_actual" HeaderText="Estado Actual" />
                <asp:BoundField DataField="fecha_alta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="fecha_baja" HeaderText="Fecha Baja" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>

        <asp:Button 
            ID="btnAgregar" 
            runat="server" 
            Text="Agregar nueva computadora" 
            CssClass="btn btn-primary mt-3" 
            OnClick="btnAgregar_Click" />
    </div>
</asp:Content>
