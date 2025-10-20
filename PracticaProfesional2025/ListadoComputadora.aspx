<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListadoComputadora.aspx.cs" Inherits="PracticaProfesional2025.Computadoras" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Listado Computadoras</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" CssClass="btn btn-primary mb-3" />
            <asp:GridView ID="gvComputadoras" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered"
                OnRowEditing="gvComputadoras_RowEditing"
                OnRowCancelingEdit="gvComputadoras_RowCancelingEdit"
                OnRowUpdating="gvComputadoras_RowUpdating"
                OnRowDeleting="gvComputadoras_RowDeleting"
                OnRowDataBound="gvComputadoras_RowDataBound"
                DataKeyNames="id_computadora">
                <Columns>
                    <asp:BoundField DataField="id_computadora" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="id_laboratorio" HeaderText="Laboratorio" />
                    <asp:BoundField DataField="codigo_inventario" HeaderText="Código inventario" />
                    <asp:BoundField DataField="numero_serie" HeaderText="Nº Serie" />
                    <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="estado_actual" HeaderText="Estado" />
                    <asp:BoundField DataField="fecha_alta" HeaderText="Fecha Alta" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="fecha_baja" HeaderText="Fecha Baja" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:TemplateField HeaderText="Componentes">
                        <ItemTemplate>
                            <asp:GridView ID="gvComponentes" runat="server" AutoGenerateColumns="False" CssClass="table table-sm"
                                DataKeyNames="id_componente" OnRowCommand="gvComponentes_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="id_componente" HeaderText="ID" ReadOnly="True" />
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                    <asp:BoundField DataField="Marca" HeaderText="Marca" />
                                    <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                                    <asp:BoundField DataField="Numero_Serie" HeaderText="Nº Serie" />
                                    <asp:TemplateField HeaderText="Acciones">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Text="Editar" CssClass="btn btn-sm btn-secondary me-1"
                                                PostBackUrl='<%# Eval("id_componente", "AltaComputadora.aspx?componenteId={0}") %>' />
                                            <asp:LinkButton runat="server" Text="Eliminar" CssClass="btn btn-sm btn-danger"
                                                CommandName="DeleteComponent" CommandArgument='<%# Eval("id_componente") %>' OnClientClick="return confirm('¿Eliminar este componente?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>