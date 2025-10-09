<%@ Page Title="ABM Computadoras" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="Computadoras.aspx.cs" Inherits="PracticaProfesional2025.Computadoras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h2 class="mb-4">Gestión de Computadoras</h2>
        
        <asp:GridView 
            ID="gvComputadoras" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="table table-bordered"
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
                <asp:BoundField DataField="fecha_alta" HeaderText="Fecha Alta" />
                <asp:BoundField DataField="fecha_baja" HeaderText="Fecha Baja" />
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
