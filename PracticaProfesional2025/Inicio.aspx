<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="Inicio.aspx.cs" Inherits="PracticaProfesional2025.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content" class="p-4 p-md-5 pt-5">
        <h2 class="mb-4">
            BIENVENIDO
            <asp:Label ID="lblSession" runat="server" Text="LabelSesion"></asp:Label>
        </h2> 
        <p>&nbsp;Esta es la pagina de inicio</p>
        
    </div>
</asp:Content>
