using Aplicacion.Repositorio;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Casos; // porque ahí está CrearCasoService
using Xunit;
using Aplicacion.Servicios;
using Microsoft.Extensions.Logging;
using Aplicacion.DTO;
using Dominio.Entidades;
using FluentAssertions;



namespace CasosJuridicosRobusto.Tests.Servicios
{
    public class CrearCasoServiceTest
    {
        private readonly Mock<ICasoRepository> _mockCasoRepository = new();
        private readonly Mock<IClienteRepository> _mockClienteRepository = new();
        private readonly Mock<ILogger<CrearCasoService>> _mockLogger = new();


        private readonly FormateadorNombreService _formateador = new(); // puede ser real
        private readonly CrearCasoService _service;

        public CrearCasoServiceTest()
        {
            _service = new CrearCasoService(_mockCasoRepository.Object,
                                            _mockClienteRepository.Object,
                                            _formateador,
                                            _mockLogger.Object);
        }

        [Fact]
        public async Task EjecutarAsync_DeberiaCrearCasoSiNoExisteActivo()
        {
            // arrange
            var request = new CrearCasoRequest
            {
                Titulo = "Demanda de pago",
                Descripcion = "Asistencia legal urgente",
                NombreCliente = "Juan Pérez"
            };

            _mockCasoRepository.Setup(r => r.ObtenerTodosAsync())
                .ReturnsAsync(new List<Caso>());

            _mockClienteRepository.Setup(r => r.ObtenerPorNombreAsync("JUAN PEREZ"))
                .ReturnsAsync(new Cliente { Id = 1, Nombre = "JUAN PEREZ" });

            //ACT
            var resultado = await _service.EjecutarAsync(request);

            // ASSERT
            resultado.Should().Be("Caso creado exitosamente.");
            _mockCasoRepository.Verify(r => r.CrearAsync(It.IsAny<Caso>()), Times.Once);

        }
        [Fact]
        public async Task EjecutarAsync_DeberiaRetornarErrorSiExisteCasoActivo()
        {
            // Arrange
            var request = new CrearCasoRequest
            {
                Titulo = "Demanda duplicada",
                Descripcion = "Caso repetido",
                NombreCliente = "Ana Torres"
            };

            var casoExistente = new Caso
            {
                NombreCliente = "Ana Torres",
                Estado = "Pendiente"
            };

            _mockCasoRepository.Setup(r => r.ObtenerTodosAsync())
                .ReturnsAsync(new List<Caso> { casoExistente });

            // Act
            var resultado = await _service.EjecutarAsync(request);

            // Assert
            resultado.Should().Be("Ya existe un caso activo para ese cliente.");
            _mockCasoRepository.Verify(r => r.CrearAsync(It.IsAny<Caso>()), Times.Never);
        }
        [Fact]
        public async Task EjecutarAsync_DeberiaCrearClienteSiNoExiste()
        {
            // Arrange
            var request = new CrearCasoRequest
            {
                Titulo = "Nuevo caso",
                Descripcion = "Cliente nuevo",
                NombreCliente = "Pedro Gómez"
            };

            _mockCasoRepository.Setup(r => r.ObtenerTodosAsync())
                .ReturnsAsync(new List<Caso>());

            _mockClienteRepository.Setup(r => r.ObtenerPorNombreAsync("PEDRO GÓMEZ"))
                .ReturnsAsync((Cliente?)null); // cliente no existe

            // Act
            var resultado = await _service.EjecutarAsync(request);

            // Assert
            resultado.Should().Be("Caso creado exitosamente.");
            _mockClienteRepository.Verify(r => r.CrearAsync(It.Is<Cliente>(c => c.Nombre == "PEDRO GÓMEZ")), Times.Once);
            _mockCasoRepository.Verify(r => r.CrearAsync(It.IsAny<Caso>()), Times.Once);
        }


    }
}
