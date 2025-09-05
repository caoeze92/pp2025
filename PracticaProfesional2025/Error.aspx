<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="PracticaProfesional2025.Error" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Error en la aplicación</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:20px; font-family:Arial;">
            <h2>❌ Ocurrió un error</h2>
            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            <hr />
            <h4>Informacion Tecnica:</h4>
            <asp:Label ID="lblDetalles" runat="server" ForeColor="Gray"></asp:Label>
        </div>
    </form>
</body>
</html>
