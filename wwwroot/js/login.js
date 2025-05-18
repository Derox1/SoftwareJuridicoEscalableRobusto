function mostrarError(texto) {
    const mensajeError = document.getElementById("mensajeError");
    mensajeError.innerText = texto;
    mensajeError.style.display = "block";
    mensajeError.classList.remove("shake"); // Reiniciar animación
    void mensajeError.offsetWidth;          // Trigger reflow
    mensajeError.classList.add("shake");    // Aplicar animación
}

document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const mensajeError = document.getElementById("mensajeError");

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!email || !password) {
        mostrarError("Completa todos los campos.");

        return;
    }

    if (!emailRegex.test(email)) {
        mostrarError("Ingresa un correo válido.");

        return;
    }

    const loginData = {
        email: email,
        password: password
    };

    try {
        const response = await fetch("https://localhost:7266/api/Auth/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(loginData)
        });

        if (!response.ok) {
            throw new Error("Login failed: " + response.status);
        }

        const data = await response.json();

        // Guarda el token
        localStorage.setItem("jwt_token", data.token);

        // Guarda datos del usuario (puedes expandir esto según lo que devuelva tu backend)
        const usuario = {
            email: data.email,
            nombre: data.nombre || "Usuario",
            rol: data.rol
        };
        localStorage.setItem("usuario_actual", JSON.stringify(usuario));

        // Redirigir inmediatamente

        window.location.href = "dashboard.html";



    } catch (error) {
        console.error("Login error:", error);
        mostrarError("Correo o contraseña inválidos.");


    }
});
// Ocultar mensaje de error al escribir nuevamente
document.getElementById("email").addEventListener("input", () => {
    document.getElementById("mensajeError").style.display = "none";
});
document.getElementById("password").addEventListener("input", () => {
    document.getElementById("mensajeError").style.display = "none";
});

