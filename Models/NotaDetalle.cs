namespace AudisoftPrueba.Models
{
    public class NotaDetalle
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }

        public int IdProfesor { get; set; }
        public string Profesor { get; set; }

        public int IdEstudiante { get; set; }
        public string Estudiante { get; set; }
    }
}
