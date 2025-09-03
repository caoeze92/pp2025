<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="ReservaExitosa.aspx.cs" Inherits="PracticaProfesional2025.ReservaExitosa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content" class="p-4 p-md-5 pt-5">
        <h2 class="text-success mb-4">¡Reserva realizada con éxito!</h2>
        <p>Tu reserva ha sido registrada correctamente. Detalles de la reserva:</p>

        <ul class="list-group mb-4">
            <li class="list-group-item"><strong>Laboratorio:</strong> <asp:Label ID="lblLaboratorio" runat="server" /></li>
            <li class="list-group-item"><strong>Fecha:</strong> <asp:Label ID="lblFecha" runat="server" /></li>
            <li class="list-group-item"><strong>Hora:</strong> <asp:Label ID="lblHora" runat="server" /></li>
            <li class="list-group-item"><strong>Motivo:</strong> <asp:Label ID="lblMotivo" runat="server" /></li>
        </ul>

        <asp:Button ID="btnVolver" runat="server" Text="Volver a Reservas" CssClass="btn btn-primary" OnClick="btnVolver_Click" />
    </div>
</asp:Content>
