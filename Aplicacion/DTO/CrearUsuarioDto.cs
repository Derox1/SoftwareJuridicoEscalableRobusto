namespace API.Aplicacion.DTO
{
    public class CrearUsuarioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        //public List<int> RolesId { get; set; } = new();
    }
}
