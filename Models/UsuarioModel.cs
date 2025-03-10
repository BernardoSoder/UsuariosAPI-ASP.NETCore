namespace UsuariosAPI_ASP.NETCore.Models
{
    public class UsuarioModel
    {
        public int id;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
