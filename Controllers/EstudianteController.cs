using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using AudisoftPrueba.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AudisoftPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {

        private readonly string __conn;
        public EstudianteController(IConfiguration config)
        {
            __conn = config.GetConnectionString("DefaultConnection");
        }

        // GET: api/<EstudianteController>
        [HttpGet]
        public IActionResult Get()
        {
            var estudiantes = new List<Estudiante>();

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "SELECT id, nombre FROM Estudiante";
            using var cmd = new SqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                estudiantes.Add(new Estudiante
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                });
            }

            return Ok(estudiantes);
        }

        // GET api/<EstudianteController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "SELECT id, nombre FROM Estudiante WHERE id = @id";
            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return NotFound($"No existe el estudiante con id {id}");

            var estudiante = new Estudiante
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1)
            };

            return Ok(estudiante);
        }

        // POST api/<EstudianteController>
        [HttpPost]
        public IActionResult Post([FromBody] Estudiante estudiante)
        {
            if (estudiante == null || string.IsNullOrWhiteSpace(estudiante.Nombre))
                return BadRequest("El nombre es obligatorio");

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = @"
                INSERT INTO Estudiante (nombre)
                OUTPUT INSERTED.id
                VALUES (@nombre)";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nombre", estudiante.Nombre);

            int newId = (int)cmd.ExecuteScalar();

            return Ok(new { message = "Estudiante creado", id = newId });
        }

        // PUT api/<EstudianteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Estudiante estudiante)
        {
            if (estudiante == null || string.IsNullOrWhiteSpace(estudiante.Nombre))
                return BadRequest("El nombre es obligatorio");

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "UPDATE Estudiante SET nombre = @nombre WHERE id = @id";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nombre", estudiante.Nombre);
            cmd.Parameters.AddWithValue("@id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound($"No existe el estudiante con id {id}");

            return Ok("Estudiante actualizado");
        }

        // DELETE api/<EstudianteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "DELETE FROM Estudiante WHERE id = @id";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return NotFound("No existe el estudiante");

                return Ok("Estudiante eliminado");
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                return Conflict(new
                {
                    message = "No se puede eliminar el estudiante porque tiene notas asociadas.",
                    code = "FK_CONSTRAINT",
                    details = ex.Message
                });
            }
        }
    }
}

        
