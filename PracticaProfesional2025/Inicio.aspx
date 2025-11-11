<%@ Page Title="INICIO - Sistema de control de Inventario Institucional - ISFDyT 46" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true"
    CodeBehind="Inicio.aspx.cs" Inherits="PracticaProfesional2025.Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content" class="p-4 p-md-5 pt-5">
        <h2 class="mb-2">BIENVENIDO <asp:Label ID="lblSession" runat="server" Text="LabelSesion"></asp:Label></h2>
        <h5 class="mb-4">Tipo de usuario: <asp:Label ID="lblRol" runat="server" Text="LabelRol"></asp:Label></h5>

        <!-- Acordeón principal: cada título es clicable y muestra/oculta su contenido -->
        <main class="accordion-root">

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="panel-1">1 - Inicio</button>
            <div id="panel-1" class="accordion-panel" aria-hidden="true">
                <p>Página principal. Desde aquí obtendrá una guía rápida sobre qué hace cada pantalla y dónde dirigirse para realizar acciones.</p>
            </div>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="panel-2">2 - Consulta de Stock</button>
            <div id="panel-2" class="accordion-panel" aria-hidden="true">
                <h6>🔹 Pantalla: Consulta de Stock</h6>
                <p><strong>Objetivo:</strong><br />
                Esta sección permite buscar y consultar el estado de las computadoras y sus componentes dentro del laboratorio.</p>

                <p><strong>Cómo usar esta pantalla:</strong></p>
                <ol>
                    <li><strong>Campos de búsqueda:</strong></li>
                    <ul>
                        <li><strong>ID Computadora / ID Componente:</strong> permiten buscar un equipo o componente específico por su identificador.</li>
                        <li><strong>Inventario / Nro. Serie:</strong> sirven para localizar elementos mediante su número interno o de serie.</li>
                        <li><strong>Descripción:</strong> se utiliza para buscar por nombre o detalle del equipo.</li>
                        <li><strong>Laboratorio:</strong> permite filtrar por el laboratorio donde se encuentra el equipo.</li>
                    </ul>

                    <li><strong>Filtros de estado:</strong></li>
                    <ul>
                        <li><strong>Estado de la PC:</strong> muestra las opciones disponibles.</li>
                        <li><strong>Estado del Componente:</strong> indica si el componente está operativo, en mantenimiento, o dado de baja.</li>
                        <li><strong>Tipo de Componente:</strong> filtra los resultados por tipo (por ejemplo: monitor, teclado, disco, etc.).</li>
                    </ul>

                    <li><strong>Botón “Buscar”:</strong><br />
                    Una vez completados los filtros deseados, presione este botón para mostrar los resultados en la sección Resultados, ubicada debajo.</li>

                    <li><strong>Resultados:</strong><br />
                    Aquí se mostrarán los equipos o componentes que coinciden con los criterios de búsqueda seleccionados.</li>
                </ol>
            </div>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="panel-3">3 - Resultados de la búsqueda</button>
            <div id="panel-3" class="accordion-panel" aria-hidden="true">
                <h6>🔹 Pantalla: Consulta de Stock (Resultados de la búsqueda)</h6>
                <p><strong>Objetivo:</strong><br />
                Visualizar la información detallada de las computadoras y sus componentes que coinciden con los filtros seleccionados.</p>

                <p><strong>Descripción:</strong><br />
                Una vez que el usuario ingresa uno o más criterios de búsqueda (por ejemplo, el ID Computadora = 1) y presiona el botón Buscar, el sistema muestra en la sección Resultados una tabla con los datos correspondientes.</p>

                <p><strong>Estructura de los resultados:</strong></p>
                <ul>
                    <li><strong>ID Computadora:</strong> identifica la PC dentro del inventario.</li>
                    <li><strong>PC Descripción:</strong> muestra el nombre o modelo del equipo (por ejemplo: PC Dell Optiplex con Windows 10).</li>
                    <li><strong>ID Componente:</strong> indica el número interno asignado a cada componente asociado a la PC.</li>
                    <li><strong>Tipo, Marca y Modelo:</strong> describen las características del componente (por ejemplo: CPU, Memoria, Disco, Monitor, etc.).</li>
                    <li><strong>Estado PC / Estado Componente:</strong> informan si el equipo o el componente están en funcionamiento, disponibles, o en reparación.</li>
                    <li><strong>Asignación / Desasignación:</strong> muestran las fechas en que el componente fue asignado o retirado.</li>
                    <li><strong>Inventario / Nro. Serie:</strong> permiten identificar físicamente cada equipo dentro del laboratorio.</li>
                </ul>

                <p><strong>Ejemplo:</strong><br />
                En la imagen, se consultó la computadora N°1, la cual posee varios componentes (CPU, memoria, disco y monitor). Cada componente tiene su propio estado y número de serie, lo que facilita la trazabilidad y el control del inventario.</p>
            </div>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="panel-4">4 - Reservas</button>
            <div id="panel-4" class="accordion-panel" aria-hidden="true">
                <h6>🔹 Pantalla: Reservas</h6>
                <p><strong>Objetivo:</strong><br />
                Permitir al usuario reservar equipos del laboratorio para su uso en una fecha y horario determinados, indicando el motivo de la reserva.</p>

                <p><strong>Descripción paso a paso:</strong></p>
                <ol>
                    <li><strong>Selección del Laboratorio:</strong><br />
                    En la parte superior, el usuario debe elegir el laboratorio desde el menú desplegable. Al seleccionarlo, el sistema muestra automáticamente el inventario de ese laboratorio, con una tabla que detalla:
                        <ul>
                            <li>Código de inventario</li>
                            <li>Número de serie</li>
                            <li>Descripción del equipo (por ejemplo, PC Dell Optiplex con Windows 10)</li>
                            <li>Estado actual</li>
                        </ul>
                    Esto permite visualizar rápidamente qué equipos están disponibles para reservar.</li>

                    <li><strong>Seleccionar Fecha y Horario:</strong><br />
                    Debajo del inventario aparece un calendario donde el usuario puede elegir el día de la reserva. A la derecha, hay dos campos desplegables:
                        <ul>
                            <li>Desde: hora de inicio del uso del equipo.</li>
                            <li>Hasta: hora en que finaliza la reserva.</li>
                        </ul>
                    </li>

                    <li><strong>Motivo de la Reserva:</strong><br />
                    En esta sección, el usuario debe escribir una breve descripción del motivo.</li>

                    <li><strong>Confirmar la reserva:</strong><br />
                    Finalmente, al presionar el botón “Reservar”, el sistema registra la solicitud con los datos ingresados.</li>
                </ol>
            </div>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="panel-5">5 - Historial</button>
            <div id="panel-5" class="accordion-panel" aria-hidden="true">
                <h6>🔹 Pantalla: Historial de Registros de Eventos</h6>
                <p><strong>Objetivo:</strong><br />
                Permitir al usuario consultar, filtrar y analizar los registros históricos de eventos realizados en el sistema, como altas, bajas o modificaciones de computadoras, componentes o entidades.</p>

                <p><strong>Descripción:</strong><br />
                Esta pantalla muestra un registro detallado de todas las operaciones que se han realizado dentro del sistema Lab Stock.
                Los datos se presentan en forma de tabla y pueden filtrarse utilizando distintos criterios de búsqueda.</p>

                <p><strong>Opciones de búsqueda y filtrado:</strong></p>
                <ol>
                    <li><strong>ID:</strong> permite localizar un evento específico por su número de identificación.</li>
                    <li><strong>Evento:</strong> filtra según el tipo de operación registrada.</li>
                    <li><strong>Entidad:</strong> permite buscar eventos asociados a un tipo particular de entidad.</li>
                    <li><strong>Usuario:</strong> muestra los eventos realizados por un usuario determinado del sistema.</li>
                </ol>

                <p><strong>Resultados:</strong><br />
                Al presionar el botón “Buscar”, se despliega una tabla con los registros que cumplen con los filtros seleccionados. Cada fila del resultado incluye: ID Evento, Código/Evento, Entidad, Usuario, Fecha de Solicitud y Detalle del Evento.</p>

                <p><strong>Ejemplo:</strong><br />
                En la imagen, se muestran registros de diferentes usuarios que realizaron diversas operaciones como instalación de software o solicitudes de mantenimiento.</p>
            </div>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="panel-6">6 - Agregar PC/Componentes</button>
            <div id="panel-6" class="accordion-panel" aria-hidden="true">
                <h6>Pantalla: Agregar PC’s / Componentes</h6>
                <p>Esta pantalla corresponde al módulo de gestión de computadoras del sistema. Al ingresar, se muestra un listado completo de todas las computadoras registradas en los distintos laboratorios, junto con la información principal de cada una:</p>
                <ul>
                    <li>ID</li>
                    <li>Laboratorio asignado</li>
                    <li>Código de inventario</li>
                    <li>Número de serie</li>
                    <li>Descripción del equipo</li>
                    <li>Estado actual</li>
                    <li>Fechas de alta y baja</li>
                </ul>

                <p>A la derecha de cada registro se incluyen las opciones Editar y Eliminar. En la parte inferior se encuentra un botón “Nuevo” para crear entradas.</p>

                <h6>🖥️ Alta de Computadora / Componente</h6>
                <p><strong>Descripción general:</strong><br />
                Esta pantalla permite registrar nuevas computadoras completas o componentes individuales en el sistema de gestión de laboratorio.</p>

                <p><strong>Pasos de uso:</strong></p>
                <ol>
                    <li><strong>Seleccionar el tipo de alta:</strong> Computadora Completa o Componente Individual.</li>
                    <li><strong>Cargar los datos:</strong> laboratorio, código de inventario, número de serie, descripción, fecha de alta y cantidad.</li>
                    <li><strong>Agregar componentes:</strong> ingresar tipo, marca, modelo y N° de serie por componente.</li>
                    <li><strong>Finalizar registro:</strong> presionar Guardar Registro o Volver para cancelar.</li>
                </ol>

                <h6>🔍 Preguntas Frecuentes Adicionales</h6>
                <ul>
                    <li><strong>¿Qué diferencia hay entre 'Computadora Completa' y 'Componente Individual'?</strong><br />
                    La opción 'Computadora Completa' permite registrar un equipo completo con todos sus componentes asociados. En cambio, 'Componente Individual' sirve para agregar o reemplazar una parte específica.</li>

                    <li><strong>¿Qué sucede si ingreso un número de serie repetido?</strong><br />
                    El sistema verifica automáticamente si el número de serie ya existe en la base de datos. Si está duplicado, se mostrará un mensaje de error.</li>

                    <li><strong>¿Cómo puedo editar los componentes ya agregados?</strong><br />
                    Desde el listado de computadoras abra el detalle de la PC y edite los componentes allí.</li>

                    <li><strong>¿Dónde se guardan los datos que ingreso?</strong><br />
                    Todos los datos se almacenan en la base de datos SQL Server del proyecto.</li>
                </ul>
            </div>

            <!-- FAQ como acordeón independiente -->
            <h3 class="mt-4">Preguntas frecuentes</h3>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="faq-1">¿Qué diferencia hay entre 'Computadora Completa' y 'Componente Individual'?</button>
            <div id="faq-1" class="accordion-panel" aria-hidden="true">
                <p>La opción 'Computadora Completa' permite registrar un equipo completo con todos sus componentes asociados. En cambio, 'Componente Individual' sirve para agregar o reemplazar una parte específica (como RAM, disco o placa madre).</p>
            </div>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="faq-2">¿Qué sucede si ingreso un número de serie repetido?</button>
            <div id="faq-2" class="accordion-panel" aria-hidden="true">
                <p>El sistema verifica automáticamente si el número de serie ya existe en la base de datos. Si está duplicado, se mostrará un mensaje de error solicitando que ingreses uno diferente.</p>
            </div>

            <button type="button" class="accordion-btn" aria-expanded="false" aria-controls="faq-3">¿Cómo edito los componentes ya agregados?</button>
            <div id="faq-3" class="accordion-panel" aria-hidden="true">
                <p>Una vez cargada la computadora, podrás editar sus componentes desde la sección 'Listado de Computadoras'. Allí se mostrará el detalle de cada una y tendrás la opción de modificar los registros.</p>
            </div>

        </main>
    </div>

    <style>
        /* Acordeón: cada título es clicable; múltiples paneles pueden estar abiertos */
        .accordion-root { max-width: 960px; margin: 0 auto; }
        .accordion-btn {
            display: block;
            width: 100%;
            text-align: left;
            padding: 12px 16px;
            margin: 10px 0 6px 0;
            border: 1px solid #d0d7de;
            border-radius: 6px;
            background: #f8f9fb;
            font-weight: 600;
            cursor: pointer;
        }
        .accordion-btn[aria-expanded="true"] { background: #e9f2ff; }
        .accordion-panel {
            padding: 12px 16px;
            margin-bottom: 8px;
            border-left: 3px solid #d0d7de;
            background: #fff;
            display: none;
        }
        .accordion-panel.show { display: block; }
        .mt-4 { margin-top: 1.5rem; }
        .mb-2 { margin-bottom: .5rem; }
        .mb-4 { margin-bottom: 1.5rem; }
        @media (max-width:767px) { .accordion-root { padding: 0 10px; } }
    </style>

    <script type="text/javascript">
        (function () {
            // Toggle simple: permite abrir varios paneles. Actualiza aria attributes.
            document.addEventListener('DOMContentLoaded', function () {
                var buttons = document.querySelectorAll('.accordion-btn');
                buttons.forEach(function (btn) {
                    var panelId = btn.getAttribute('aria-controls');
                    var panel = panelId ? document.getElementById(panelId) : null;

                    // inicializar atributos
                    btn.setAttribute('aria-expanded', 'false');
                    if (panel) panel.setAttribute('aria-hidden', 'true');

                    btn.addEventListener('click', function (e) {
                        // prevenir comportamiento por defecto si dentro de form
                        e.preventDefault();
                        var isExpanded = btn.getAttribute('aria-expanded') === 'true';
                        if (!isExpanded) {
                            btn.setAttribute('aria-expanded', 'true');
                            if (panel) {
                                panel.classList.add('show');
                                panel.setAttribute('aria-hidden', 'false');
                                // desplazamiento suave para visibilidad
                                panel.scrollIntoView({ behavior: 'smooth', block: 'start' });
                            }
                        } else {
                            btn.setAttribute('aria-expanded', 'false');
                            if (panel) {
                                panel.classList.remove('show');
                                panel.setAttribute('aria-hidden', 'true');
                            }
                        }
                    });
                });
            });
        })();
    </script>
</asp:Content>