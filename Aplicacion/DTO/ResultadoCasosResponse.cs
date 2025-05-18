using Aplicacion.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.DTO
{
    
        public class ResultadoCasosResponse
        {
            public int Total { get; set; } // Total de casos en la base de datos
            public int TotalFiltrados { get; set; } // Total según filtros activos
            public int PaginaActual { get; set; }
            public int TotalPaginas { get; set; }
            public List<CasoDto> Items { get; set; } = new(); // Casos para esta página

        }
    
}
