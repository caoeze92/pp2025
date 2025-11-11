<%@ Page Title="Alta de Computadora o Componente" Language="C#" MasterPageFile="~/Principal.Master"
    AutoEventWireup="true" CodeBehind="AltaComputadora.aspx.cs" Inherits="PracticaProfesional2025.AltaComputadora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style>
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
    .btn-crear {
        height: 36px;
        font-size: 0.9rem;
    }

    /* estilo para campo bloqueado (readOnly) */
    .readonly {
        background-color: #e9ecef;
        color: #6c757d;
    }
</style>

<div id="content" class="p-4 p-md-5 pt-5">
    <h2 class="mb-4">ALTA DE COMPUTADORA / COMPONENTE</h2>

    <!-- Selector de modo -->
    <div class="row mb-4">
        <div class="col-md-4">
            <div class="filtro-box">
                <label for="ddlTipoCarga">Tipo de Alta</label>
                <asp:DropDownList 
                    ID="ddlTipoCarga" 
                    runat="server"
                    CssClass="form-select"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlTipoCarga_SelectedIndexChanged">
                    <asp:ListItem Text="Seleccione..." Value="" />
                    <asp:ListItem Text="Computadora Completa" Value="computadora" />
                    <asp:ListItem Text="Componente Individual" Value="componente" />
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <!-- Panel Computadora -->
    <asp:Panel ID="pnlComputadora" runat="server" Visible="false">
        <h4>Datos de la Computadora</h4>
        <div class="row g-2">
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="ddlLaboratorio">Laboratorio</label>
                    <asp:DropDownList ID="ddlLaboratorio" runat="server" CssClass="form-select" AppendDataBoundItems="true"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtCodigoInventario">Código de Inventario</label>
                    <asp:TextBox ID="txtCodigoInventario" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtNumeroSerie">Número de Serie</label>
                    <asp:TextBox ID="txtNumeroSerie" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtDescripcion">Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" />
                </div>
            </div>
        </div>

        <div class="row g-2 mt-2">
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtFechaAlta">Fecha Alta</label>
                    <asp:TextBox ID="txtFechaAlta" runat="server" TextMode="Date" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtCantidad">Cantidad</label>
                    <asp:TextBox ID="txtCantidad" runat="server" TextMode="Number" Text="1" />
                </div>
            </div>
        </div>

        <h5 class="mt-4">Agregar Componentes de la Computadora</h5>
        <asp:Panel ID="pnlAgregarComponente" runat="server">
            <div class="row g-2">
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtTipo">Tipo</label>
                        <asp:TextBox ID="txtTipo" runat="server" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtMarca">Marca</label>
                        <asp:TextBox ID="txtMarca" runat="server" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtModelo">Modelo</label>
                        <asp:TextBox ID="txtModelo" runat="server" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtCaracComp">Caracteristicas</label>
                        <asp:TextBox ID="txtCarac" runat="server" />
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="filtro-box">
                        <label for="txtNumeroSerieComp">N° Serie</label>
                        <asp:TextBox ID="txtNumeroSerieComp" runat="server" />
                    </div>
                </div>
            </div>
            <asp:Button ID="btnAgregarComponente" runat="server" Text="Agregar Componente" CssClass="btn btn-outline-primary mt-2" OnClick="btnAgregarComponente_Click" />
        </asp:Panel>

        <h5 class="mt-4">Componentes Agregados</h5>
        <div class="table-responsive mt-2">
            <asp:GridView ID="gvComponentes" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowCommand="gvComponentes_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="Tipo" DataField="Tipo" />
                    <asp:BoundField HeaderText="Marca" DataField="Marca" />
                    <asp:BoundField HeaderText="Modelo" DataField="Modelo" />
                    <asp:BoundField HeaderText="Caracteristicas" DataField="Caracteristicas" />
                    <asp:BoundField HeaderText="N° Serie" DataField="Numero_Serie" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Remove" Text="🗑" OnClientClick="return confirm('¿Eliminar este componente?');" />
                        </ItemTemplate>
                        <HeaderStyle Width="60px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <!-- Panel Componente individual -->
    <asp:Panel ID="pnlComponente" runat="server" Visible="false">
        <h4>Datos del Componente Individual</h4>
        <div class="row g-2">
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtTipoCompIndividual">Tipo</label>
                    <asp:TextBox ID="txtTipoCompIndividual" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtMarcaCompIndividual">Marca</label>
                    <asp:TextBox ID="txtMarcaCompIndividual" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtModeloCompIndividual">Modelo</label>
                    <asp:TextBox ID="txtModeloCompIndividual" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtCaracCompIndividual">Caracteristicas</label>
                    <asp:TextBox ID="txtCaracCompIndividual" runat="server" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtNumeroSerieIndividual">Número de Serie</label>
                    <asp:TextBox ID="txtNumeroSerieIndividual" runat="server" />
                </div>
            </div>
        </div>

        <div class="row g-2 mt-2">
            <div class="col-md-3">
                <div class="filtro-box">
                    <label for="txtCantidadIndividual">Cantidad</label>
                    <asp:TextBox ID="txtCantidadIndividual" runat="server" TextMode="Number" Text="1" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="filtro-box">
                    <label for="ddlComputadoraAsociar">Asociar a Computadora</label>
                    <asp:DropDownList ID="ddlComputadoraAsociar" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
            </div>
        </div>
    </asp:Panel>


    <div class="mt-3">
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar Registro" CssClass="btn btn-success btn-crear" OnClick="btnGuardar_Click" />
        <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-secondary btn-volver" OnClick="btnVolver_Click" />
    </div>

    <asp:Label ID="lblMensaje" runat="server" CssClass="mt-3 d-block fw-bold" />
</div>

<!-- Script cliente para bloquear campo S/N cuando la cantidad > 1 y generar prefijo AUTO -->
<script type="text/javascript">
    (function () {
        function toInt(v) {
            var n = parseInt(v, 10);
            return isNaN(n) ? 0 : n;
        }

        function generateAutoPrefix() {
            var t = new Date();
            var ts = t.getFullYear().toString()
                + ('0' + (t.getMonth()+1)).slice(-2)
                + ('0' + t.getDate()).slice(-2)
                + ('0' + t.getHours()).slice(-2)
                + ('0' + t.getMinutes()).slice(-2)
                + ('0' + t.getSeconds()).slice(-2);
            var rnd = Math.random().toString(36).slice(2,8).toUpperCase();
            return 'AUTO-' + ts + '-' + rnd;
        }

        function ensureAuto(el) {
            if (!el) return;
            if (!el.value || el.value.indexOf('AUTO-') !== 0) {
                el.value = generateAutoPrefix();
                el.classList.add('readonly');
            }
        }

        function updateSerialReadonly() {
            var qtyEl = document.getElementById('<%= txtCantidad.ClientID %>');
            var snEl = document.getElementById('<%= txtNumeroSerie.ClientID %>');
            var snCompEl = document.getElementById('<%= txtNumeroSerieComp.ClientID %>');
            if (!qtyEl) return;
            var qty = toInt(qtyEl.value);
            if (qty > 1) {
                if (snEl) { ensureAuto(snEl); snEl.readOnly = true; snEl.classList.add('readonly'); }
                if (snCompEl) { ensureAuto(snCompEl); snCompEl.readOnly = true; snCompEl.classList.add('readonly'); }
            } else {
                if (snEl) { snEl.readOnly = false; snEl.classList.remove('readonly'); }
                if (snCompEl) { snCompEl.readOnly = false; snCompEl.classList.remove('readonly'); }
            }
        }

        function updateSerialIndividualReadonly() {
            var qtyEl = document.getElementById('<%= txtCantidadIndividual.ClientID %>');
            var snEl = document.getElementById('<%= txtNumeroSerieIndividual.ClientID %>');
            if (!qtyEl || !snEl) return;
            var qty = toInt(qtyEl.value);
            if (qty > 1) {
                if (snEl) { if (!snEl.value || snEl.value.indexOf('AUTO-') !== 0) snEl.value = generateAutoPrefix(); snEl.readOnly = true; snEl.classList.add('readonly'); }
            } else {
                snEl.readOnly = false;
                snEl.classList.remove('readonly');
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            updateSerialReadonly();
            updateSerialIndividualReadonly();

            var qtyComp = document.getElementById('<%= txtCantidad.ClientID %>');
            if (qtyComp) {
                qtyComp.addEventListener('input', updateSerialReadonly);
                qtyComp.addEventListener('change', updateSerialReadonly);
            }
            var qtyInd = document.getElementById('<%= txtCantidadIndividual.ClientID %>');
            if (qtyInd) {
                qtyInd.addEventListener('input', updateSerialIndividualReadonly);
                qtyInd.addEventListener('change', updateSerialIndividualReadonly);
            }
        });
    })();
</script>

</asp:Content>