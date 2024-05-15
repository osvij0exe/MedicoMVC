using MedicoMVC.Models.Request;
using MedicoMVC.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace MedicoMVC.Models.ViewModels
{
    public class MedicoViewModel
    {
        public int MedicoId { get; set; }
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = default!;
        public string Matricula { get; set; } = default!;
        [Display(Name = "Cedula Profecional")] 
        public string CedulaProfecional { get; set; } = default!;
        public ICollection<EspecialidadDTOResponse> Especialidades { get; set; } = default!;
        [Display(Name ="Especialidad")]
        public int EspecialidadSeleccionada { get; set; }
        public string NombreEspecialidad { get; set; } = default!;
    }
}
