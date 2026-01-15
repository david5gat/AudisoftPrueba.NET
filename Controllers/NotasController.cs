using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using AudisoftPrueba.Models;

namespace AudisoftPrueba.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotasController : ControllerBase
    {
        private readonly string __conn;

        public NotasController(IConfiguration config)
        {
            __conn = config.GetConnectionString("DefaultConnection");
        }

        // GET: api/<NotasController>
        [HttpGet]
        public IActionResult Get()
        {
            var notas = new List<NotaDetalle>();

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = @"
                SELECT 
                    n.id, n.nombre, n.valor,
                    n.idProfesor, p.nombre AS profesor,
                    n.idEstudiante, e.nombre AS estudiante
                FROM Nota n
                INNER JOIN Profesor p ON n.idProfesor = p.id
                INNER JOIN Estudiante e ON n.idEstudiante = e.id
            ";

            using var cmd = new SqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                notas.Add(new NotaDetalle
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Valor = reader.GetDecimal(2),

                    IdProfesor = reader.GetInt32(3),
                    Profesor = reader.GetString(4),

                    IdEstudiante = reader.GetInt32(5),
                    Estudiante = reader.GetString(6),
                });
            }

            return Ok(notas);
        }

        // GET api/<NotasController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = @"
                SELECT 
                    n.id, n.nombre, n.valor,
                    n.idProfesor, p.nombre AS profesor,
                    n.idEstudiante, e.nombre AS estudiante
                FROM Nota n
                INNER JOIN Profesor p ON n.idProfesor = p.id
                INNER JOIN Estudiante e ON n.idEstudiante = e.id
                WHERE n.id = @id
            ";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return NotFound($"No existe la nota con id {id}");

            var nota = new NotaDetalle
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Valor = reader.GetDecimal(2),

                IdProfesor = reader.GetInt32(3),
                Profesor = reader.GetString(4),

                IdEstudiante = reader.GetInt32(5),
                Estudiante = reader.GetString(6),
            };

            return Ok(nota);
        }

        // POST api/<NotasController>
        [HttpPost]
        public IActionResult Post([FromBody] Nota nota)
        {
            if (nota == null)
                return BadRequest("Datos inválidos");

            if (string.IsNullOrWhiteSpace(nota.Nombre))
                return BadRequest("El nombre es obligatorio");

            using var connection = new SqlConnection(__conn);
            connection.Open();

            using (var cmdProf = new SqlCommand("SELECT COUNT(*) FROM Profesor WHERE id = @id", connection))
            {
                cmdProf.Parameters.AddWithValue("@id", nota.IdProfesor);
                int existeProf = (int)cmdProf.ExecuteScalar();
                if (existeProf == 0)
                    return BadRequest($"No existe el profesor con id {nota.IdProfesor}");
            }

            using (var cmdEst = new SqlCommand("SELECT COUNT(*) FROM Estudiante WHERE id = @id", connection))
            {
                cmdEst.Parameters.AddWithValue("@id", nota.IdEstudiante);
                int existeEst = (int)cmdEst.ExecuteScalar();
                if (existeEst == 0)
                    return BadRequest($"No existe el estudiante con id {nota.IdEstudiante}");
            }

            var query = @"
                INSERT INTO Nota (nombre, idProfesor, idEstudiante, valor)
                OUTPUT INSERTED.id
                VALUES (@nombre, @idProfesor, @idEstudiante, @valor)
            ";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nombre", nota.Nombre);
            cmd.Parameters.AddWithValue("@idProfesor", nota.IdProfesor);
            cmd.Parameters.AddWithValue("@idEstudiante", nota.IdEstudiante);
            cmd.Parameters.AddWithValue("@valor", nota.Valor);

            int newId = (int)cmd.ExecuteScalar();

            return Ok(new { message = "Nota creada", id = newId });
        }

        // PUT api/<NotasController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Nota nota)
        {
            if (nota == null || string.IsNullOrWhiteSpace(nota.Nombre))
                return BadRequest("Datos inválidos. El nombre es obligatorio.");

            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = @"
                UPDATE Nota
                SET nombre = @nombre,
                    idProfesor = @idProfesor,
                    idEstudiante = @idEstudiante,
                    valor = @valor
                WHERE id = @id
            ";

            using var cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nombre", nota.Nombre);
            cmd.Parameters.AddWithValue("@idProfesor", nota.IdProfesor);
            cmd.Parameters.AddWithValue("@idEstudiante", nota.IdEstudiante);
            cmd.Parameters.AddWithValue("@valor", nota.Valor);
            cmd.Parameters.AddWithValue("@id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound($"No existe la nota con id {id}");

            return Ok("Nota actualizada ");
        }

        // DELETE api/<NotasController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var connection = new SqlConnection(__conn);
            connection.Open();

            var query = "DELETE FROM Nota WHERE id = @id";

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
