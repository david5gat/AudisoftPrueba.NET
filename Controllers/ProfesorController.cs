using AudisoftPrueba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AudisoftPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorController : ControllerBase
    {
        private readonly string __conn;

        public ProfesorController(IConfiguration config)
        {
            __conn = config.GetConnectionString("DefaultConnection");
        }
        
        // GET: api/<ProfesorController>
        [HttpGet]
        public IActionResult Get()
        {
            var profesores = new List<Profesor>();

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "SELECT id, nombre FROM Profesor";
            using var cmd = new SqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                profesores.Add(new Profesor
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                });
            }

            return Ok(profesores);
        }

        // GET api/<ProfesorController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "SELECT id, nombre FROM Profesor WHERE id = @id";
            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return NotFound($"No existe el profesor con id {id}");

            var profesor = new Profesor
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1)
            };

            return Ok(profesor);
        }

        // POST api/<ProfesorController>
        [HttpPost]
        public IActionResult Post([FromBody] Profesor profesor)
        {
            if (profesor == null || string.IsNullOrWhiteSpace(profesor.Nombre))
                return BadRequest("El nombre es obligatorio");

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = @"
                INSERT INTO Profesor (nombre)
                OUTPUT INSERTED.id
                VALUES (@nombre)";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nombre", profesor.Nombre);

            int newId = (int)cmd.ExecuteScalar();

            return Ok(new { message = "Profesor creado", id = newId });
        }

        // PUT api/<ProfesorController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Profesor profesor)
        {
            if (profesor == null || string.IsNullOrWhiteSpace(profesor.Nombre))
                return BadRequest("El nombre es obligatorio");

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "UPDATE Profesor SET nombre = @nombre WHERE id = @id";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nombre", profesor.Nombre);
            cmd.Parameters.AddWithValue("@id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound($"No existe el profesor con id {id}");

            return Ok("Profesor actualizado");
        }

        // DELETE api/<ProfesorController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "DELETE FROM Profesor WHERE id = @id";

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
