using System.ComponentModel.DataAnnotations;

namespace MedicoMVC.Models.Request
{
    public class MedicoDTORequest
    {
        public int Id { get; set; }
        [Required]
        public string NombreCompleto { get; set; } = default!;
        [Required]
        public string Matricula { get; set; } = default!;
        [Required]
        public string CedulaProfecional { get; set; } = default!;
        [Required]
        public int EspecialidadId { get; set; }
    }
}
