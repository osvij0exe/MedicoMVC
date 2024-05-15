namespace MedicoMVC.Models.Response
{
    public class MedicoDTOResponse
    {
        public int MedicoId { get; set; } = default!;
        public string NombreCompleto { get; set; } = default!;
        public string FechaIngreso { get; set; } = default!;
        public string Matricula { get; set; } = default!;
        public string CedulaProfecional { get; set; } = default!;
        public string EspecialidadId { get; set; } = default!;
        public MedicoEspecialidadDTOResponse Especialidad { get; set; } = default!;
    }
}
