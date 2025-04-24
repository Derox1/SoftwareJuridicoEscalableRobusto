using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.DTO;
using Aplicacion.Repositorio;
using Dominio.Entidades;


public class CerrarCasoService 

    //esto es inyeccion de dependencias, simplemente se inyecta la interfaz en el servicio que queremos utilizar 
{
    private readonly ICasoRepository _casoRepository;

    public CerrarCasoService(ICasoRepository casoRepository)
    {
        _casoRepository = casoRepository;
    }
    public async Task<CerrarCasoResultado> EjecutarAsync(int casoId)
    {
        var caso = await _casoRepository.ObtenerPorIdAsync(casoId);
     
        if (caso is null)
            return new CerrarCasoResultado { Exito = false, NoEncontrado = true, Mensaje = "El caso no existe." };

        if (caso.EstaCerrado())
            return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "El caso ya está cerrado." };

        if (!caso.PuedeSerCerrado())
            return new CerrarCasoResultado { Exito = false, EsErrorNegocio = true, Mensaje = "Solo los casos en proceso pueden cerrarse." };

        caso.Estado = EstadoCaso.Cerrado;
        await _casoRepository.ActualizarAsync(caso);


        return new CerrarCasoResultado { Exito = true, Mensaje = "Caso cerrado correctamente." };

    }




}
