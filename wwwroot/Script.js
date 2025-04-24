// JavaScript source code
// 🔁 BLOQUE 1: Obtener todos los casos (ya lo tenías)
const apiUrl = 'https://localhost:5001/api/casos'; // ajusta el puerto si cambia

async function cargarCasos() {
    try {
        //Hace un fetch al backend y convierte la respuesta en JSON.

        const response = await fetch(apiUrl);
        const casos = await response.json();


        // Limpia la tabla antes de rellenar

        //Selecciona el tbody de la tabla y lo limpia antes de llenar filas nuevas.
        //Selects the table body and clears its content before adding new rows.


        const tabla = document.getElementById("casos-body");
        tabla.innerHTML = "";


        //Recorre cada caso y genera una fila HTML dinámica con sus datos.


        casos.forEach(caso => {
            const fila = `
                <tr>
                    <td>${caso.titulo}</td>
                    <td>${caso.nombreCliente}</td>
                    <td>${caso.estado}</td>
                    <td>${new Date(caso.fechaCreacion).toLocaleDateString()}</td>
                </tr>
            `;
            tabla.innerHTML += fila;
        });
    } catch (error) {
        console.error("Error al cargar los casos:", error);
    }
}
//Ejecuta la función automáticamente al cargar la página.

cargarCasos();


// 🔁 BLOQUE 2: Crear nuevo caso (formulario HTML + fetch POST)
const form = document.getElementById("form-crear-caso");
const mensaje = document.getElementById("mensaje");


//Captura el evento de enviar formulario y evita que recargue la página.
form.addEventListener("submit", async (e) => {
    e.preventDefault();


    //Obtiene los valores ingresados y quita espacios vacíos.

    const titulo = document.getElementById("titulo").value.trim();
    const descripcion = document.getElementById("descripcion").value.trim();
    const nombreCliente = document.getElementById("nombreCliente").value.trim();


    //Valida que los campos obligatorios no estén vacíos. Si están vacíos, muestra error.


    if (!titulo || !nombreCliente) {
        mensaje.innerHTML = `<div class="alert alert-danger">Título y Cliente son obligatorios.</div>`;
        return;
    }


    //Crea un objeto con los datos del nuevo caso que vas a enviar.

    const nuevoCaso = {
        titulo,
        descripcion,
        nombreCliente
    };

    try {

        //Envía los datos al backend usando fetch con método POST.
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(nuevoCaso)
        });


        //Muestra el mensaje de éxito, limpia el formulario y recarga la tabla para mostrar el nuevo caso.

        const resultado = await response.text();
        mensaje.innerHTML = `<div class="alert alert-success">${resultado}</div>`;
        form.reset();
        await cargarCasos(); // recarga la tabla con el nuevo caso
    } catch (error) {
        console.error(error);
        mensaje.innerHTML = `<div class="alert alert-danger">Error al crear el caso.</div>`;
    }
});