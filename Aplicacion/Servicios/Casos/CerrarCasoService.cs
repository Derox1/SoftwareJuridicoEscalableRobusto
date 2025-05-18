using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTO;
using Aplicacion.Repositorio;
using Dominio.Entidades;


public class CerrarCasoService
{
    private readonly ICasoRepository _casoRepository;

    public CerrarCasoService(ICasoRepository casoRepository)
    {
        _casoRepository = casoRepository;
    }

    public async Task<CerrarCasoResultado> EjecutarAsync(int casoId, CerrarCasoRequest request)
    {
        var caso = await _casoRepository.ObtenerPorIdAsync(casoId);
        if (caso is null)
            return new CerrarCasoResultado { Exito = false, NoEncontrado = true, Mensaje = "El caso no existe." };

        if (caso.EstaCerrado())
            return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "El caso ya está cerrado." };

        if (caso.Estado == EstadoCaso.EnProceso)
        {
            if (string.IsNullOrWhiteSpace(caso.Descripcion))
                return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "No se puede cerrar un caso sin descripción." };

            //caso.Estado = EstadoCaso.Cerrado;
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

        await _casoRepository.ActualizarAsync(caso);
        return new CerrarCasoResultado { Exito = true, Mensaje = "Caso cerrado correctamente." };
    }
}
