<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministracionDeUsuarios.aspx.cs" Inherits="PracticaProfesional2025.ModificarUsuario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Administración De Usuarios</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="false" DataKeyNames="id_usuario" OnRowEditing="gvUsuarios_RowEditing" OnRowCancelingEdit="gvUsuarios_RowCancelingEdit" OnRowUpdating="gvUsuarios_RowUpdating" OnRowDeleting="gvUsuarios_RowDeleting">
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
                            <asp:ListItem Text="user" Value="user" />
                            <asp:ListItem Text="invitado" Value="invitado" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <%# Eval("Rol") %>
                    </ItemTemplate>
                </asp:TemplateField>

             <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" /> 

            </Columns>
        
        
        </asp:GridView>
    </div>
    </form>
</body>
</html>
