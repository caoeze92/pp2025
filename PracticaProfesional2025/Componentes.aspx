<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Componentes.aspx.cs" Inherits="PracticaProfesional2025.Componentes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-4" style="padding:30px; margin-left: 300px; margin-top: 30px; margin-left: 30px;">Administración de Componentes</h2>
    <br />
    <div>
    <asp:GridView ID="gvComponentes" runat="server" AutoGenerateColumns="False" DataKeyNames="id_componente"
        CssClass="table table-bordered table-striped"
        OnRowEditing="gvComponentes_RowEditing"
        OnRowUpdating="gvComponentes_RowUpdating"
        OnRowCancelingEdit="gvComponentes_RowCancelingEdit"
        OnRowDeleting="gvComponentes_RowDeleting" style="padding:30px; margin-left: 300px; margin-top: 30px; margin-left: 30px;">

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
    <asp:Button ID="btnVolverInicio" runat="server" Text="Volver" PostBackUrl="~/Inicio.aspx"/>
    </div>
</asp:Content>
