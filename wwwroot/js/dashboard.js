document.addEventListener("DOMContentLoaded", () => {
    const apiUrl = "https://localhost:7266/api/Casos";
    const token = localStorage.getItem("jwt_token");
    const usuario = JSON.parse(localStorage.getItem("usuario_actual") || "{}");

    const saludo = document.getElementById("saludoUsuario");
    const filtroEstado = document.getElementById("filtroEstado");
    const choicesEstado = new Choices(filtroEstado, {
        searchEnabled: false,
        itemSelectText: '',
        shouldSort: false
    });

    const filtros = {
        estado: null,
        pagina: 1,
        tamanio: 10
    };

    if (!token) {
        alert("Token no encontrado. Redirigiendo al login...");
        window.location.href = "login.html";
        return;
    }

    if (saludo && usuario.nombre) {
        saludo.textContent = `Hola, ${usuario.nombre}`;
    }

    cargarCasosDesdeBackend();

    filtroEstado.addEventListener("change", () => {
        const estadoSeleccionado = filtroEstado.value?.trim();
        filtros.estado = estadoSeleccionado || null;
        filtros.pagina = 1;
        cargarCasosDesdeBackend();
    });

    function construirQueryString(filtros) {
        const params = new URLSearchParams();
        if (filtros.estado) params.append("estado", filtros.estado);
        if (filtros.pagina) params.append("pagina", filtros.pagina);
        if (filtros.tamanio) params.append("tamanio", filtros.tamanio);
        return "?" + params.toString();
    }

    async function cargarCasosDesdeBackend() {
        const query = construirQueryString(filtros);

        const response = await fetch(`${apiUrl}${query}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

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

            const row = `
            <tr class="${claseFila}">
                <td>${caso.id}</td>
                <td>${caso.titulo}</td>
                <td>${estadoBadge}</td>
                <td>${tipoIcono}${caso.tipoCaso}</td>
                <td>${caso.nombreCliente || 'No Client'}</td>
                <td>${new Date(caso.fechaCreacion).toLocaleDateString()}</td>
                <td class="text-nowrap">
                    <button class="btn btn-sm btn-outline-light me-1" title="Ver">
                        <i class="bi bi-eye-fill"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-warning" title="Editar">
                        <i class="bi bi-pencil-fill"></i>
                    </button>
                </td>
            </tr>
        `;
            tbody.innerHTML += row;
        });

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

    function mostrarMensajeInformativo(mostrados, total) {
        const estadoTabla = document.getElementById("contadorResultados");
        if (!estadoTabla) return;
        estadoTabla.textContent = `Mostrando ${mostrados} de ${total} casos`;
    }

    function getEstadoBadge(estado) {
        switch (estado.toLowerCase()) {
            case "pendiente":
                return '<span class="badge bg-warning text-dark badge-estado">Pendiente</span>';
            case "resuelto":
                return '<span class="badge bg-success badge-estado">Resuelto</span>';
            case "rechazado":
                return '<span class="badge bg-danger badge-estado">Rechazado</span>';
            default:
                return '<span class="badge bg-secondary badge-estado">' + estado + '</span>';
        }
    }

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

    document.getElementById("logoutBtn")?.addEventListener("click", () => {
        localStorage.removeItem("jwt_token");
        localStorage.removeItem("usuario_actual");
        window.location.href = "login.html";
    });
});

