<%@ Page Title="EVENTOS - Sistema de control de Inventario Institucional - ISFDyT 46" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="Eventos.aspx.cs" Inherits="PracticaProfesional2025.Eventos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content" class="p-4 p-md-5 pt-5">
        <h2 class="mb-4">HISTORIAL DE REGISTROS DE EVENTOS</h2>
          <style>
    /* Estilo de panel de filtros */
    .filtro-box {
        border: 1px solid #ced4da;
        border-radius: 5px;
        padding: 8px;
        background-color: #f8f9fa;
        margin-bottom: 10px;
    }
    .filtro-box label {
        font-size: 0.85rem;
        margin-bottom: 3px;
    }
    .filtro-box input, .filtro-box select {
        width: 100%;
        height: 28px;
        font-size: 0.85rem;
        padding: 2px 6px;
        border: 1px solid #adb5bd;
        border-radius: 4px;
        box-sizing: border-box;
    }
    .btn-buscar {
        height: 34px;
        font-size: 0.85rem;
    }

    /* Tabla resultados */
    #gvResultados {
        border: 2px solid #0d6efd;
        border-collapse: collapse;
        width: 100%;
    }
    #gvResultados th, #gvResultados td {
        border: 1px solid #0d6efd;
        padding: 8px 12px;
        text-align: center;
    }
    #gvResultados tr:nth-child(even) td {
        background-color: #e9f2ff;
    }
    #gvResultados tr:hover td {
        background-color: #d1e7ff;
    }
    </style>
<!-- Panel de filtros -->
<asp:Panel ID="pnlFiltros" runat="server" CssClass="mb-3">
    <h4>BUSCAR / FILTRAR INFORMACIÓN:</h4>
    <div class="row g-2">
        <!-- Primera fila (6 campos) -->
 <%--       <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtIdComputadora">ID Computadora</label>
                <asp:TextBox ID="txtIdComputadora" runat="server" />
            </div>
        </div>
        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtIdComponente">ID Componente</label>
                <asp:TextBox ID="txtIdComponente" runat="server" />
            </div>
        </div>
        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtCodigoInventario">Inventario</label>
                <asp:TextBox ID="txtCodigoInventario" runat="server" />
            </div>
        </div>
        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtNumeroSerie">Nro. Serie</label>
                <asp:TextBox ID="txtNumeroSerie" runat="server" />
            </div>
        </div>
        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtDescripcion">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" />
            </div>
        </div>
        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtIdLaboratorio">Laboratorio</label>
                <asp:TextBox ID="txtIdLaboratorio" runat="server" />
            </div>
        </div>--%>

         <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtIdHistorial">ID</label>
                <asp:TextBox ID="txtIdHistorial" runat="server" />
            </div>
        </div>
<%--        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtIdComponente">Evento</label>
                <asp:TextBox ID="txtIdComponente" runat="server" />
            </div>
        </div>--%>

            <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtEventos">Evento</label>
                <asp:DropDownList ID="comboEventos" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
        </div>

        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtIdEntidad">Entidad</label>
                <asp:TextBox ID="txtIdEntidad" runat="server" />
            </div>
        </div>

        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtIdUsuario">Usuario</label>
                <asp:TextBox ID="txtIdUsuario" runat="server" />
            </div>
        </div>

    </div>

    <div class="row g-2 mt-2">
        <!-- Segunda fila (4 filtros + botón) -->
      <%--  <div class="col-md-2">
            <div class="filtro-box">
                <label for="ddlEstadoPC">Estado de la PC</label>
                <asp:DropDownList ID="ddlEstadoPC" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
        </div>
        <div class="col-md-2">
            <div class="filtro-box">
                <label for="ddlEstadoComponente">Estado del Componente</label>
                <asp:DropDownList ID="ddlEstadoComponente" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>
        </div>
        <div class="col-md-2">
            <div class="filtro-box">
                <label for="txtTipoComponente">Tipo de Componente</label>
                <asp:TextBox ID="txtTipoComponente" runat="server" />
            </div>
        </div>--%>
        <div class="col-md-2 d-flex align-items-end">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary w-100 btn-buscar" BackColor="#0d1b2a" OnClick="btnBuscar_Click" />
        </div>
    </div>
</asp:Panel>

        <!-- Tabla de resultados -->
        <h4>Resultados</h4>
        <div class="table-responsive">
            <asp:GridView ID="gvResultados" runat="server" AutoGenerateColumns="false" CssClass="text-center"  OnPageIndexChanging="gvResultados_PageIndexChanging" OnRowDataBound="gvResultados_RowDataBound_Eventos" AllowPaging="True" PageSize="5" EmptyDataText="SIN INFORMACION DISPONIBLE...">
                
                <HeaderStyle BackColor="#0d1b2a" ForeColor="White" HorizontalAlign="Center" Font-Bold="True" />

                <Columns>
                    <asp:BoundField DataField="id_historial" HeaderText="ID Evento" ItemStyle-Width="70px" />
                    <asp:BoundField DataField="tipo_evento" HeaderText="Codigo de Evento" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="nombre_evento" HeaderText="Evento" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="entidad" HeaderText="Entidad" ItemStyle-Width="70px" />
                    <asp:BoundField DataField="codEntidad" HeaderText="Cod. Entidad" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="usuario" HeaderText="Usuario" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="fecha_solicitud" HeaderText="Fecha de Solicitud" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" ItemStyle-Width="105px" />
                    <asp:BoundField DataField="detalle" HeaderText="Detalle del Evento" ItemStyle-Width="100px" />
                    </Columns>
               </asp:GridView>
            <asp:TextBox ID="lblTotalRows" runat="server" Text="Total de Filas" Width="250px"></asp:TextBox>
            </div>
    </div>
   </asp:Content>
