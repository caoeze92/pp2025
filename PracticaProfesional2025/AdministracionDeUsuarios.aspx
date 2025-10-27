<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministracionDeUsuarios.aspx.cs" MasterPageFile="~/Principal.Master" Inherits="PracticaProfesional2025.ModificarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <title>Administración De Usuarios - Sistema de control de Inventario Institucional - ISFDyT 46</title>

    <style>
        .custom-grid {
            border-collapse: collapse;
            width: 90%;
            margin: 30px;
            background-color: white;
            box-shadow: 0px 2px 6px rgba(0,0,0,0.1);
            border: 2px solid #ccc; /* Borde exterior */
        }

        .custom-grid th {
            background-color: #f8f9fa;
            font-weight: bold;
            text-align: left;
            padding: 10px;
            border: 2px solid #dee2e6; /* Borde entre columnas del encabezado */
        }

        .custom-grid td {
            padding: 10px;
            border: 2px solid #dee2e6; /* Borde entre columnas del cuerpo */
        }

        .custom-grid tr:nth-child(even) {
            background-color: #f2f2f2; /* Alternar color de filas */
        }

        .custom-grid tr:hover {
            background-color: #e8f0fe; /* Color al pasar el mouse */
        }

        .btn-volver {
            background-color: #0d6efd;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            margin: 30px;
        }

        .btn-volver:hover {
            background-color: #0b5ed7;
        }
    </style>

    <div style="padding: 30px;">
        <h2 style="margin-left: 30px; font-weight: bold;">Gestión de Usuarios</h2>
        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false"
            CssClass="custom-grid"
            DataKeyNames="id_usuario"
            OnRowEditing="gvUsuarios_RowEditing"
            OnRowCancelingEdit="gvUsuarios_RowCancelingEdit"
            OnRowUpdating="gvUsuarios_RowUpdating"
            OnRowDeleting="gvUsuarios_RowDeleting"
            OnRowDataBound="gvUsuarios_RowDataBound">

            <Columns>
                <asp:BoundField DataField="id_usuario" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
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

        <asp:Button ID="btnVolverInicio" runat="server" Text="Volver al inicio" CssClass="btn-volver" PostBackUrl="~/Inicio.aspx" />
    </div>
</asp:Content>
