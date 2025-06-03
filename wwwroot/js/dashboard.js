

document.addEventListener("DOMContentLoaded", () => {

    /*➡️ Espera que el DOM esté completamente cargado antes de ejecutar el código JS (buena práctica para manipular el DOM).
    
    */

    const apiUrl = "https://localhost:7266/api/Casos";
    /*➡️ Define la URL base para la API de casos.
    */
    const token = localStorage.getItem("jwt_token");

    /*➡️ Recupera el token JWT desde localStorage (para autorizar las peticiones).
*/
    const usuario = JSON.parse(localStorage.getItem("usuario_actual") || "{}");

    /*➡️ Obtiene el usuario actual guardado (si existe).

*/

    const saludo = document.getElementById("saludoUsuario");
    /*➡️ Elemento donde mostrarás “Hola, Usuario”.
    
    */
    // Aplicamos Choise.Js
    const filtroEstado = document.getElementById("filtroEstado");
    const choicesEstado = new Choices(filtroEstado, {
        searchEnabled: false,
        itemSelectText: '',
        shouldSort: false
    });

    // 🆕 Choices para el modal "Nuevo Caso"
    const formEstado = document.getElementById("form-estado");
    const formTipo = document.getElementById("form-tipo");

    const choicesEstadoForm = new Choices(formEstado, {
        searchEnabled: false,
        itemSelectText: '',
        shouldSort: false,
        placeholder: true,
    });

    const choicesTipoForm = new Choices(formTipo, {
        searchEnabled: false,
        itemSelectText: '',
        shouldSort: false,
        placeholder: true,
    });

    const filtros = {
        estado: null,
        pagina: 1,
        tamanio: 10
    };

    /*➡️ Filtro inicial: sin estado aplicado, página 1, 10 resultados por página.*/


    if (!token) {
        alert("Token no encontrado. Redirigiendo al login...");
        window.location.href = "login.html";
        return;
    }

    /*if (!token) {
    alert("Token no encontrado. Redirigiendo al login...");
    window.location.href = "login.html";
    return;*/

    if (saludo && usuario.nombre) {
        saludo.textContent = `Hola, ${usuario.nombre}`;
    }

    /*➡️ Personaliza el saludo si el usuario tiene nombre guardado.*/

    cargarCasosDesdeBackend();

    /**➡️ Ejecuta carga inicial. */


    filtroEstado.addEventListener("change", () => {
        const estadoSeleccionado = filtroEstado.value?.trim();
        filtros.estado = estadoSeleccionado || null;
        filtros.pagina = 1;
        cargarCasosDesdeBackend();
    });

    /**➡️ Actualiza el filtro de estado y recarga la tabla */


    function construirQueryString(filtros) {
        const params = new URLSearchParams();
        if (filtros.estado) params.append("estado", filtros.estado);
        if (filtros.pagina) params.append("pagina", filtros.pagina);
        if (filtros.tamanio) params.append("tamanio", filtros.tamanio);
        return "?" + params.toString();
    }

    /**➡️ Transforma tu objeto filtros en un query string para el fetch. */

    async function cargarCasosDesdeBackend() {
        const query = construirQueryString(filtros);

        const response = await fetch(`${apiUrl}${query}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        /**➡️ Consulta la API con token.
➡️ Maneja errores y expiración de sesión.
➡️ Valida estructura del response (items y resumen).
➡️ Llama a funciones auxiliares: renderizarTabla, actualizarResumen, mostrarMensajeInformativo. */


        if (!response.ok) {
            if (response.status === 401) {
                alert("Sesión expirada. Redirigiendo...");
                localStorage.removeItem("jwt_token");
                localStorage.removeItem("usuario_actual");
                window.location.href = "login.html";
                return;
            }

            console.error("Error al obtener los casos:", response.status);
            return;
        }

        const data = await response.json();

        // ✅ Validación mínima para evitar errores si backend falla
        if (!data.items || !data.resumen) {
            console.warn("⚠️ La respuesta del backend no tiene el formato esperado:", data);
            document.getElementById("contadorResultados").textContent =
                "⚠️ No se pudieron cargar los datos correctamente.";
            return;
        }

        renderizarTabla(data.items);
        actualizarResumen(data.resumen);
        mostrarMensajeInformativo(data.items.length, data.totalRegistros);
    }
    function renderizarTabla(lista) {
        const tbody = document.getElementById("casosBody");
        tbody.classList.remove("opacity-100");
        tbody.classList.add("opacity-0");
        tbody.innerHTML = "";

        lista.forEach(caso => {
            const estadoBadge = getEstadoBadge(caso.estado);
            const tipoIcono = getTipoIcono(caso.tipoCaso);
            const claseFila = `tr-${caso.estado.toLowerCase()}`;

            //validacion para mostrar mensaje de cerrar solo si no esta cerrado
            const puedeCerrar = caso.estado.toLowerCase() !== "cerrado";


            const row = `
            <tr class="${claseFila}">
                <td>${caso.id}</td>
                <td>${caso.titulo}</td>
                <td>${estadoBadge}</td>
                <td>${tipoIcono}${caso.tipoCaso}</td>
                <td>${caso.nombreCliente || 'No Client'}</td>
                <td>${new Date(caso.fechaCreacion).toLocaleDateString()}</td>
                <td class="text-nowrap">
                   <button class="btn btn-sm btn-outline-light me-1 btn-ver" data-id="${caso.id}" title="Ver">
                   <i class="bi bi-eye-fill"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-warning" title="Editar">
                        <i class="bi bi-pencil-fill"></i>
                    </button>
                      <button class="btn btn-sm btn-outline-danger btn-eliminar" data-id="${caso.id}" title="Eliminar">
                    <i class="bi bi-trash-fill"></i>
                     </button>
                      ${puedeCerrar ? `
                     <button class="btn btn-sm btn-outline-secondary btn-cerrar" data-id="${caso.id}" title="Cerrar">
                       <i class="bi bi-lock-fill"></i>
                     </button>` : ""}
                </td>
            </tr>
        `;
            tbody.innerHTML += row;
        });

        /**➡️ Limpia y vuelve a renderizar la tabla de casos con animación suave (opacity).
➡️ Inserta HTML dinámico fila por fila */



        setTimeout(() => {
            tbody.classList.remove("opacity-0");
            tbody.classList.add("opacity-100");
        }, 50);
    }
    function actualizarResumen(resumen) {
        document.getElementById("totalCasos").textContent = resumen.total;
        document.getElementById("casosPendientes").textContent = resumen.pendientes;
        document.getElementById("casosResueltos").textContent = resumen.resueltos;
    }

    /**➡️ Muestra totales y contadores en la parte superior del dashboard.

 */

    function mostrarMensajeInformativo(mostrados, total) {
        const estadoTabla = document.getElementById("contadorResultados");
        if (!estadoTabla) return;
        estadoTabla.textContent = `Mostrando ${mostrados} de ${total} casos`;
    }

    /** ℹ️ Mensaje inferior: “Mostrando X de Y casos”*/




    function mostrarDetalleCaso(id) {
        fetch(`${apiUrl}/${id}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(res => {
                if (!res.ok) throw new Error("Error al obtener detalle");
                return res.json();
            })
            .then(data => {
                document.getElementById("detalle-titulo").innerText = data.titulo;
                document.getElementById("detalle-descripcion").innerText = data.descripcion;
                document.getElementById("detalle-estado").innerText = data.estado;
                document.getElementById("detalle-tipo").innerText = data.tipoCaso;
                document.getElementById("detalle-cliente").innerText = data.nombreCliente;
                document.getElementById("detalle-fecha").innerText = new Date(data.fechaCreacion).toLocaleDateString();

                const modal = new bootstrap.Modal(document.getElementById('modalDetalle'));
                modal.show();
            })
            .catch(error => {
                console.error("Error al cargar detalle:", error);
                alert("No se pudo cargar el detalle del caso.");
            });
    }

    /**➡️ Trae los datos de un caso por ID.
➡️ Rellena un modal Bootstrap para mostrar info detallada */

    function getEstadoBadge(estado) {
        switch (estado.toLowerCase()) {
            case "pendiente":
                return '<span class="badge estado-pendiente">Pendiente</span>';
            case "enproceso":
                return '<span class="badge estado-enproceso">En Proceso</span>';
            case "cerrado":
                return '<span class="badge estado-cerrado">Cerrado</span>';
            default:
                return '<span class="badge bg-secondary badge-estado">' + estado + '</span>';
        }
    }

    /**➡️ Devuelven HTML para mostrar el estado y tipo con íconos y colores personalizados. */
    function getTipoIcono(tipo) {
        switch (tipo.toLowerCase()) {
            case "laboral":
                return '<i class="bi bi-people text-primary me-2"></i>';
            case "familia":
                return '<i class="bi bi-house-heart text-success me-2"></i>';
            case "civil":
                return '<i class="bi bi-bank text-info me-2"></i>';
            case "penal":
                return '<i class="bi bi-shield-exclamation text-danger me-2"></i>';
            default:
                return '<i class="bi bi-folder2-open text-secondary me-2"></i>';
        }
    }

    // 🔁 Función reutilizable para mostrar errores claros al usuario
    async function mostrarErrorDesdeResponse(respuesta, mensajePorDefecto) {
        try {
            const errorJson = await respuesta.json();
            const mensaje = errorJson.detail || mensajePorDefecto;

            Swal.fire({
                icon: 'warning',
                title: 'Error',
                text: mensaje,
            });
        } catch {
            Swal.fire({
                icon: 'error',
                title: 'Error inesperado',
                text: mensajePorDefecto,
            });
        }
    }

    document.getElementById("logoutBtn")?.addEventListener("click", () => {
        localStorage.removeItem("jwt_token");
        localStorage.removeItem("usuario_actual");
        window.location.href = "login.html";
    });
    /**➡️ Limpia el token y usuario del almacenamiento, redirige al login. **/
    document.addEventListener("click", async (e) => {
        // 👁 Ver detalle

        if (e.target.closest(".btn-ver")) {
            const btn = e.target.closest(".btn-ver");
            const id = btn.dataset.id;
            mostrarDetalleCaso(id);
        }


        // ✏️ Editar
        if (e.target.closest(".btn-outline-warning")) {
            const row = e.target.closest("tr");
            const id = row.children[0].textContent;

            fetch(`${apiUrl}/${id}`, {
                headers: { "Authorization": `Bearer ${token}` }
            })
                .then(res => res.json())
                .then(data => {
                    document.getElementById("form-id").value = data.id;
                    document.getElementById("form-titulo").value = data.titulo;
                    document.getElementById("form-cliente").value = data.nombreCliente;
                    document.getElementById("form-cliente").readOnly = true; //  SOLO LECTURA
                    document.getElementById("grupo-cliente").style.display = "block"; //  Mostrar campo
                    document.getElementById("form-descripcion").value = data.descripcion;
                    choicesEstadoForm.setChoiceByValue(data.estado);
                    choicesTipoForm.setChoiceByValue(data.tipoCaso);


                    document.getElementById("form-cliente").value = data.nombreCliente || "";

                    document.getElementById("modalGestionCasoLabel").textContent = "✏️ Editar Caso";
                    const modal = new bootstrap.Modal(document.getElementById("modalGestionCaso"));
                    modal.show();
                });
        }
        // 🗑️ Eliminar con SweetAlert (esto va *fuera* del bloque de editar)
        if (e.target.closest(".btn-eliminar")) {
            const btn = e.target.closest(".btn-eliminar");
            const id = btn.dataset.id;

            // 🔐 Validación local: no permitir eliminar si ya está cerrado
            const fila = btn.closest("tr");
            const estado = fila.children[2].innerText.trim().toLowerCase(); // Columna de estado

            if (estado === "cerrado") {
                Swal.fire({
                    icon: "warning",
                    title: "No se puede eliminar",
                    text: "Este caso está cerrado y no puede ser eliminado.",
                });
                return; // ⚠️ No seguimos con el fetch
            }


            // ✅ Confirmación visual
            Swal.fire({
                title: '¿Estás seguro?',
                text: "Esta acción eliminará el caso permanentemente.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then(async (result) => {
                if (result.isConfirmed) {
                    try {
                        Swal.fire({
                            title: 'Eliminando...',
                            allowOutsideClick: false,
                            didOpen: () => {
                                Swal.showLoading();
                            }
                        });

                        const res = await fetch(`${apiUrl}/${id}`, {
                            method: "DELETE",
                            headers: { "Authorization": `Bearer ${token}` }
                        });

                        if (!res.ok) {
                            const errorJson = await res.json();
                            const mensajeError = errorJson.detail || "Error inesperado";
                            // ⚠️ Mostrar mensaje personalizado según tipo de error
                            if (res.status === 400 || res.status === 404) {
                                Swal.fire({
                                    icon: 'warning',
                                    title: 'No se pudo eliminar',
                                    text: mensajeError,
                                });
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error inesperado',
                                    text: 'Ocurrió un problema al intentar eliminar el caso.',
                                });
                            }

                            return; // detenemos el flujo
                        }
                        Swal.fire({
                            toast: true,
                            position: 'top-end',
                            icon: 'success',
                            title: 'Caso eliminado exitosamente',
                            showConfirmButton: false,
                            timer: 2000,
                            timerProgressBar: true
                        });

                        await cargarCasosDesdeBackend();
                    } catch (error) {
                        Swal.fire('Error', error.message, 'error');
                    }
                }
            });
        }


        // 🔒 Cerrar caso
        if (e.target.closest(".btn-cerrar")) {
            const btn = e.target.closest(".btn-cerrar");
            const id = btn.dataset.id;

            // 🔄 Preguntar motivo de cierre
            const { value: motivo } = await Swal.fire({
                title: "Cerrar caso",
                input: "textarea",
                inputLabel: "Motivo del cierre (opcional)",
                inputPlaceholder: "Escribe el motivo si corresponde...",
                inputAttributes: {
                    "aria-label": "Motivo de cierre"
                },
                showCancelButton: true,
                confirmButtonText: "Cerrar caso",
                cancelButtonText: "Cancelar"
            });

            if (motivo === undefined) return; // Usuario canceló

            try {
                Swal.fire({
                    title: "Cerrando caso...",
                    allowOutsideClick: false,
                    didOpen: () => Swal.showLoading()
                });

                const res = await fetch(`${apiUrl}/${id}/cerrar`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify({ motivoCierre: motivo })
                });

                if (!res.ok) {
                    const errorJson = await res.json();
                    throw new Error(errorJson.detail || "Error inesperado");
                }

                await cargarCasosDesdeBackend();

                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: 'Caso cerrado con éxito',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true
                });

            } catch (error) {
                console.error("Error al cerrar caso:", error);
                Swal.fire({
                    icon: "error",
                    title: "Error al cerrar",
                    text: error.message || "No se pudo cerrar el caso."
                });
            }
        }

    });









    document.getElementById("btnNuevoCaso")?.addEventListener("click", () => {
        // Limpiar el formulario antes de abrir

        document.getElementById("formGestionCaso").reset();
        document.getElementById("form-id").value = ""; // dejar vacío para saber que es nuevo
        // 🆕 Limpiar campo cliente (solo visual)

        document.getElementById("form-cliente").value = "";
        document.getElementById("form-cliente").readOnly = false; // ✅ ACTIVAR campo cliente
        document.getElementById("grupo-cliente").style.display = "block"; // ✅ Mostrar campo

        // Actualizar el título del modal
        document.getElementById("modalGestionCasoLabel").textContent = "📝 Nuevo Caso";

        // Mostrar el modal
        const modal = new bootstrap.Modal(document.getElementById("modalGestionCaso"));
        modal.show();
    });

    document.getElementById("formGestionCaso")?.addEventListener("submit", async (e) => {
        e.preventDefault();

        const id = document.getElementById("form-id").value.trim();
        const titulo = document.getElementById("form-titulo").value.trim();
        const descripcion = document.getElementById("form-descripcion").value.trim();
        const estado = document.getElementById("form-estado").value;
        const tipoCaso = document.getElementById("form-tipo").value;
        const nombreCliente = document.getElementById("form-cliente").value.trim(); // nuevo

        // Validación básica para evitar envío si el usuario no seleccionó valores
        if (!estado || !tipoCaso) {
            Swal.fire({
                icon: 'warning',
                title: 'Campos requeridos',
                text: 'Por favor selecciona un estado y un tipo de caso.',
            });
            return;
        }

        const caso = {
            Titulo: titulo,
            Descripcion: descripcion,
            Estado: estado,
            TipoCaso: tipoCaso,
            nombreCliente: nombreCliente
        };

        const esNuevo = id === "";

        const url = esNuevo ? "https://localhost:7266/api/Casos" : `https://localhost:7266/api/Casos/${id}`;
        const metodo = esNuevo ? "POST" : "PUT";

        try {
            const response = await fetch(url, {
                method: metodo,
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
                body: JSON.stringify(caso)
            });

            if (!response.ok) {
                const errorJson = await response.json();
                throw new Error(errorJson.detail || `Error al ${esNuevo ? "crear" : "actualizar"} el caso`);
            }
            bootstrap.Modal.getInstance(document.getElementById("modalGestionCaso")).hide();


            // ✅ Recarga la tabla
            await cargarCasosDesdeBackend();

            Swal.fire({
                toast: true,
                position: 'top-end',
                icon: 'success',
                title: `Caso ${esNuevo ? "creado" : "actualizado"} con éxito`,
                showConfirmButton: false,
                timer: 2000,
                timerProgressBar: true
            });


        } catch (error) {
            await mostrarErrorDesdeResponse(response, `No se pudo ${esNuevo ? "crear" : "actualizar"} el caso.`);
        
        }
      

    });
});

