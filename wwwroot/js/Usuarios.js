// 📁 usuarios.js
// Este módulo carga, crea, edita y elimina usuarios desde el backend usando JWT




// 🧩 Punto de entrada del módulo, se llama desde dashboard.html
function initUsuariosModule() {

    // ✅ Mostrar visualmente la sección de usuarios
    document.getElementById("seccion-usuarios")?.classList.remove("d-none");

    cargarUsuarios();
    configurarEventosUsuarios();
}



// 🧠 Configura los listeners para botones y formularios
function configurarEventosUsuarios() {
    document.getElementById("btnNuevoUsuario").addEventListener("click", () => {
        abrirModalUsuario();   // Abre el modal vacío para crear usuario
    });

    document.getElementById("formUsuario").addEventListener("submit", async (e) => {
        e.preventDefault();
        await guardarUsuario(); // Enviar datos al backend (POST o PUT)
    });
}


// 🔄 Fetch: obtener todos los usuarios

async function cargarUsuarios() {
    try {
        const res = await fetch("/api/usuarios", {
            headers: authHeader()
        });
        const usuarios = await res.json();
        renderizarUsuarios(usuarios);
    } catch (error) {
        console.error("Error al cargar usuarios:", error);
    }
}


// 🧱 Renderiza usuarios en la tabla HTML
function renderizarUsuarios(usuarios) {
    const tbody = document.getElementById("usuariosBody");
    tbody.innerHTML = "";

    usuarios.forEach(usuario => {
        const tr = document.createElement("tr");

        tr.innerHTML = `
            <td>${usuario.id}</td>
            <td>${usuario.nombre}</td>
            <td>${usuario.email}</td>
            <td>${formatearRoles(usuario.roles)}</td>
            <td>
                <button class="btn btn-sm btn-outline-light me-1" onclick='abrirModalUsuario(${JSON.stringify(usuario)})'>✏️</button>
                <button class="btn btn-sm btn-outline-danger" onclick='eliminarUsuario(${usuario.id})'>🗑️</button>
            </td>
        `;

        tbody.appendChild(tr);
    });
}



// 🎨 Convierte array de roles en badges bonitos
function formatearRoles(roles) {
    if (!roles || roles.length === 0) return "-";
    return roles.map(r => `<span class='badge text-bg-primary me-1'>${r}</span>`).join("");
}


// ✏️ Abre el modal con datos (si hay) o limpio para nuevo
function abrirModalUsuario(usuario = null) {
    document.getElementById("formUsuario").reset();
    document.getElementById("usuarioId").value = usuario?.id || "";
    document.getElementById("nombreUsuario").value = usuario?.nombre || "";
    document.getElementById("emailUsuario").value = usuario?.email || "";

    // Si es edición, la contraseña no es requerida
    document.getElementById("passwordUsuario").required = !usuario;



    const modal = new bootstrap.Modal(document.getElementById("modalUsuario"));
    modal.show();
}


// 💾 Guarda o actualiza un usuario (POST o PUT)
async function guardarUsuario() {
    const id = document.getElementById("usuarioId").value;
    const nombre = document.getElementById("nombreUsuario").value;
    const email = document.getElementById("emailUsuario").value;
    const password = document.getElementById("passwordUsuario").value;

    // ✅ Primero crea el objeto
    const payload = { nombre, email };

    // ✅ Luego agrega la contraseña solo si se escribió
    if (password && password.trim() !== "") {
        payload.password = password.trim();
    }

    const url = id ? `/api/usuarios/${id}` : "/api/usuarios";
    const method = id ? "PUT" : "POST";

    try {
        const res = await fetch(url, {
            method,
            headers: {
                ...authHeader(),
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        if (!res.ok) throw new Error("Error al guardar usuario");

        Swal.fire("Éxito", id ? "Usuario actualizado" : "Usuario creado", "success");
        bootstrap.Modal.getInstance(document.getElementById("modalUsuario")).hide();
        cargarUsuarios();
    } catch (error) {
        console.error(error);
        Swal.fire("Error", error.message, "error");
    }
}



// ❌ Elimina un usuario después de confirmar con SweetAlert2
async function eliminarUsuario(id) {
    const confirm = await Swal.fire({
        title: "¿Eliminar usuario?",
        text: "Esta acción no se puede deshacer",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    });

    if (!confirm.isConfirmed) return;

    try {
        const res = await fetch(`/api/usuarios/${id}`, {
            method: "DELETE",
            headers: authHeader()
        });

        if (!res.ok) throw new Error("Error al eliminar usuario");

        Swal.fire("Eliminado", "El usuario fue eliminado", "success");
        cargarUsuarios();
    } catch (error) {
        console.error(error);
        Swal.fire("Error", error.message, "error");
    }
}






// 🔐 Adjunta el JWT al header Authorization
function authHeader() {
    const token = localStorage.getItem("jwt_token");
    return {
        "Authorization": `Bearer ${token}`
    };
}

// 🔄 Exportar función de arranque para dashboard.html
window.initUsuariosModule = initUsuariosModule;
