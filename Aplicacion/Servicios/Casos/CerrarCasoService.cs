using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Repositorio;


public class CerrarCasoService 

{
    private readonly ICasoRepository _casoRepository;

    public CerrarCasoService(ICasoRepository casoRepository)
    {
        _casoRepository = casoRepository;

    }
    public async Task<string> EjecutarAsync(int casoId)
    {
        var caso = await _casoRepository.ObtenerPorIdAsync(casoId);

        if (caso is null)
            throw new InvalidOperationException("El caso no existe.");

        if (caso.EstaCerrado())
            return "El caso ya está cerrado.";

        if (!caso.PuedeSerCerrado())
            return "Solo los casos en proceso pueden cerrarse.";

        caso.Estado = "Cerrado";
        await _casoRepository.ActualizarAsync(caso);

        return "Caso cerrado correctamente.";
    }




}
