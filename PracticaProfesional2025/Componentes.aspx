<%@ Page Title="ABM Componentes" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Componentes.aspx.cs" Inherits="PracticaProfesional2025.Componentes" %>

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
        <h2 class="mb-4">Gestión de Componentes</h2>

        <asp:GridView 
            ID="gvComponentes" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="table table-bordered table-striped table-hover"
            DataKeyNames="id_componente"
            OnRowEditing="gvComponentes_RowEditing"
            OnRowUpdating="gvComponentes_RowUpdating"
            OnRowCancelingEdit="gvComponentes_RowCancelingEdit"
            OnRowDeleting="gvComponentes_RowDeleting">

            <Columns>
                <asp:BoundField DataField="id_componente" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                <asp:BoundField DataField="marca" HeaderText="Marca" />
                <asp:BoundField DataField="modelo" HeaderText="Modelo" />
                <asp:BoundField DataField="caracteristicas" HeaderText="Características" />
                <asp:BoundField DataField="numero_serie" HeaderText="N° Serie" />
                <asp:BoundField DataField="estado_id" HeaderText="Estado ID" />
                <asp:BoundField DataField="fecha_compra" HeaderText="Fecha de Compra" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>

        <asp:Button 
            ID="btnVolverInicio" 
            runat="server" 
            Text="Volver al inicio" 
            CssClass="btn btn-primary mt-3" 
            PostBackUrl="~/Inicio.aspx" />
    </div>

</asp:Content>
