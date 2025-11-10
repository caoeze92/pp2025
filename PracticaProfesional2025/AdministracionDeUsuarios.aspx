<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministracionDeUsuarios.aspx.cs" MasterPageFile="~/Principal.Master" Inherits="PracticaProfesional2025.ModificarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <title>Administración De Usuarios - Sistema de control de Inventario Institucional - ISFDyT 46</title>

    <style>
        body {
            background-color: #f5f7fb;
        }

        .container-usuarios {
            max-width: 95%;
            margin: 40px auto;
            background: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0px 4px 15px rgba(0, 0, 0, 0.1);
        }

        .titulo-usuarios {
            text-align: center;
            font-size: 28px;
            font-weight: bold;
            color: #003366;
            margin-bottom: 30px;
            border-bottom: 3px solid #0d6efd;
            padding-bottom: 10px;
        }

        .custom-grid {
            width: 100%;
            border-collapse: collapse;
            font-family: 'Segoe UI', sans-serif;
            font-size: 14px;
        }

        .custom-grid th {
            background-color: #0d6efd;
            color: white;
            text-align: center;
            padding: 12px;
            border: 1px solid #ddd;
        }

        .custom-grid td {
            text-align: center;
            padding: 10px;
            border: 1px solid #ddd;
        }

        .custom-grid tr:nth-child(even) {
            background-color: #f8f9fa;
        }

        .custom-grid tr:hover {
            background-color: #e9f2ff;
            transition: background-color 0.2s ease;
        }

        .btn {
            padding: 6px 12px;
            border-radius: 6px;
            font-weight: 500;
            text-decoration: none;
            display: inline-block;
            border: none;
            cursor: pointer;
        }

        .btn-primary {
            background-color: #0d6efd;
            color: white;
        }

        .btn-primary:hover {
            background-color: #0b5ed7;
        }

        .btn-danger {
            background-color: #dc3545;
            color: white;
        }

        .btn-danger:hover {
            background-color: #bb2d3b;
        }

        .btn-success {
            background-color: #198754;
            color: white;
        }

        .btn-success:hover {
            background-color: #157347;
        }

        .btn-secondary {
            background-color: #6c757d;
            color: white;
        }

        .btn-secondary:hover {
            background-color: #5c636a;
        }

        .btn-volver {
            background-color: #0d6efd;
            color: white;
            padding: 10px 25px;
            border-radius: 8px;
            border: none;
            font-weight: bold;
            display: block;
            margin: 25px auto;
            cursor: pointer;
        }

        .btn-volver:hover {
            background-color: #0b5ed7;
        }

        .alert {
            text-align: center;
            margin: 20px auto;
            width: 60%;
            padding: 12px;
            border-radius: 8px;
            font-weight: 500;
            display: none;
        }

        .alert-success {
            background-color: #d1e7dd;
            color: #0f5132;
            border: 1px solid #badbcc;
            display: block;
        }
    </style>

    <div class="container-usuarios">
        <h2 class="titulo-usuarios">Gestión de Usuarios</h2>

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

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEditar" runat="server" CommandName="Edit" Text="Editar"
                            CssClass="btn btn-primary" />
                        &nbsp;
                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Delete" Text="Eliminar"
                            CssClass="btn btn-danger"
                            OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este usuario?');" />
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:LinkButton ID="btnActualizar" runat="server" CommandName="Update" Text="Actualizar"
                            CssClass="btn btn-success" />
                        &nbsp;
                        <asp:LinkButton ID="btnCancelar" runat="server" CommandName="Cancel" Text="Cancelar"
                            CssClass="btn btn-secondary" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <!-- Mensaje visual -->
        <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-success" Visible="false"></asp:Label>

        <asp:Button ID="btnVolverInicio" runat="server" Text="Volver al inicio" CssClass="btn-volver" PostBackUrl="~/Inicio.aspx" />
    </div>
</asp:Content>