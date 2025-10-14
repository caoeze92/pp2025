<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministracionDeUsuarios.aspx.cs" MasterPageFile="~/Principal.Master" Inherits="PracticaProfesional2025.ModificarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <title>Administración De Usuarios - Sistema de control de Inventario Institucional - ISFDyT 46</title>
    <div>
        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false" 
            DataKeyNames="id_usuario" 
            OnRowEditing="gvUsuarios_RowEditing" 
            OnRowCancelingEdit="gvUsuarios_RowCancelingEdit" 
            OnRowUpdating="gvUsuarios_RowUpdating" 
            OnRowDeleting="gvUsuarios_RowDeleting"
            OnRowDataBound="gvUsuarios_RowDataBound" style="padding:30px; margin-left: 300px; margin-top: 30px; margin-left: 30px;">
            
            <Columns>
                <asp:BoundField DataField="id_usuario" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:BoundField DataField="telefono" HeaderText="Telefono" />
                <asp:BoundField DataField="activo" HeaderText="Activo" />

                <asp:TemplateField HeaderText="Rol">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlRol" runat="server">
                            <asp:ListItem Text="admin" Value="admin" />
                            <asp:ListItem Text="usuario" Value="usuario" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# Eval("rol") %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" /> 
            </Columns>
        </asp:GridView>

        <asp:Button ID="btnVolverInicio" runat="server" Text="Volver" PostBackUrl="~/Inicio.aspx" style="margin-left: 300px; margin-top: 30px; margin-left: 30px;" />
    </div>
</asp:Content>
