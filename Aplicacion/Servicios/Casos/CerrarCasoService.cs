using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTO;
using Aplicacion.Repositorio;
using Dominio.Entidades;
using Microsoft.AspNetCore.Http;


public class CerrarCasoService
{
    private readonly ICasoRepository _casoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CerrarCasoService(ICasoRepository casoRepository , IHttpContextAccessor httpContextAccessor)
    {
        _casoRepository = casoRepository;
        _httpContextAccessor = httpContextAccessor;

    }

    public async Task<CerrarCasoResultado> EjecutarAsync(int casoId, CerrarCasoRequest request)
    {
        // Validación defensiva

        if (request == null)
        {
            return new CerrarCasoResultado
            {
                Exito = false,
                EsErrorNegocio = true,
                Mensaje = "La solicitud no contiene datos válidos para el cierre."
            };
        }

        //  Normalizar motivo
        request.MotivoCierre ??= string.Empty; // 🔒 Protección defensiva


        // Auditoría de usuario
        var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema";


        //  Obtener caso
        var caso = await _casoRepository.ObtenerPorIdAsync(casoId);
  

        if (caso is null)
            return new CerrarCasoResultado { Exito = false, NoEncontrado = true, Mensaje = "El caso no existe." };

        if (caso.EstaCerrado())
            return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "El caso ya está cerrado." };

        if (caso is null)
            return new CerrarCasoResultado
            {
                Exito = false,
                NoEncontrado = true,
                Mensaje = "El caso no existe."
            };

        if (caso.Estado == EstadoCaso.Cerrado || caso.EstaCerrado())
            return new CerrarCasoResultado
            {
                Exito = false,
                EsErrorNegocio = true,
                Mensaje = "Este caso ya está cerrado y no puede volver a cerrarse."
            };


        // 🔄 Lógica según estado
        if (caso.Estado == EstadoCaso.EnProceso)
        {
            if (string.IsNullOrWhiteSpace(caso.Descripcion))
                return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "No se puede cerrar un caso sin descripción." };

            caso.Estado = EstadoCaso.Cerrado;
            caso.FechaCierre = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(request.MotivoCierre))
                caso.MotivoCierre = request.MotivoCierre;
        }
        else if (caso.Estado == EstadoCaso.Pendiente)
        {
            if (string.IsNullOrWhiteSpace(request.MotivoCierre))
                return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "Debe ingresar un motivo para cerrar un caso pendiente." };

            caso.Estado = EstadoCaso.Cerrado;
            caso.FechaCierre = DateTime.UtcNow;
            caso.MotivoCierre = request.MotivoCierre;
        }
        else
        {
            return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "No se puede cerrar este caso en su estado actual." };
        }


        // Auditoría profesional
        caso.UpdatedAt = DateTime.UtcNow;
        caso.ModifiedBy = userName;
        caso.FechaCambioEstado = DateTime.UtcNow;

        // 💾 Persistir cambios

        await _casoRepository.ActualizarAsync(caso);
        return new CerrarCasoResultado { Exito = true, Mensaje = "Caso cerrado correctamente." };
    }
}
