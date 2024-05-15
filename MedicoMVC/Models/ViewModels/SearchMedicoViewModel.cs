using MedicoMVC.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MedicoMVC.Models.ViewModels
{
    public class SearchMedicoViewModel
    {
        public int MedicoId { get; set; }
        [Display(Name = "Nombre Completo")]
        public string? NombreCompleto { get; set; } = default!;
        public string? Matricula { get; set; }
        [Display(Name = "Cedula Profecional")]
        public string? CedulaProfecional { get; set; }
        [Display(Name ="Selecione Fecha inicio")]
        public DateTime? FechaIngresoInicio { get; set; }
        [Display(Name = "Selecione Fecha fin")]
        public DateTime? FechaIngresoFin { get; set; }
        [BindProperty]
        public ICollection<EspecialidadDTOResponse> Especialidades { get; set; } = default!;
        [Display(Name = "Especialidad")]
        public int? EspecialidadSeleccionada { get; set; }
        public ICollection<MedicoDTOResponse> Medicos { get; set; } = default!;
    }
}
