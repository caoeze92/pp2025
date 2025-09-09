<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Principal.Master" CodeBehind="NoAutorizado.aspx.cs" Inherits="PracticaProfesional2025.NoAutorizado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content" class="p-4 p-md-5 pt-5">
        <h2 class="mb-4">Usted NO está Autorizado <asp:Label ID="lblSession" runat="server" Text="LabelSesion"></asp:Label></h2>
        <h4 class="mb-4">Tipo de usuario: <asp:Label ID="lblRol" runat="server" Text="LabelRol"></asp:Label></h4>
        <p>&nbsp;Regrese a la pagina de inicio</p>
        <asp:Button ID="btnVolver" runat="server" Text="Volver" PostBackUrl="~/Inicio.aspx" />
    </div>
</asp:Content>
