using Microsoft.AspNetCore.Mvc;
using UsuariosAPI_ASP.NETCore.Models;
using MySql.Data.MySqlClient;

namespace UsuariosAPI.Controllers
{
    [ApiController]
    [Route("/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly string _connectionString;

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var usuarios = new List<object>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM usuario", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usuarios.Add(new
                    {
                        Id = reader["id"],
                        Nome = reader["nome"],
                        Email = reader["email"]
                    });
                }
            }

            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult AddUsuario([FromBody] UsuarioModel usuario)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO usuario (nome, email, password) VALUES (@nome, @email, @password)", connection);
                command.Parameters.AddWithValue("@nome", usuario.Nome);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@password", usuario.Password);

                var result = command.ExecuteNonQuery();
                return result > 0 ? Created("", usuario) : BadRequest("Erro ao adicionar usuário.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUsuario(int id, [FromBody] UsuarioModel usuario)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE usuario SET nome = @nome, email = @email, password = @password WHERE id = @id", connection);
                command.Parameters.AddWithValue("@nome", usuario.Nome);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@password", usuario.Password);
                command.Parameters.AddWithValue("@id", id);
                
                var result = command.ExecuteNonQuery();
                return result > 0 ? NoContent() : NotFound("Usuário não encontrado.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM usuario WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                var result = command.ExecuteNonQuery();
                return result > 0 ? NoContent() : NotFound("Usuário não encontrado.");
            }
        }
    }
}
