using MedicoMVC.Models.Request;
using MedicoMVC.Models.ViewModels;
using MedicoMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;


namespace MedicoMVC.Controllers
{
    public class MedicoController : Controller
    {
        private readonly IMedicoServices _MedicoServices;
        private readonly IEspecialidadesServices _EspecialidadServices;

        public MedicoController(IMedicoServices medicoServices,
            IEspecialidadesServices especialidadServices)
        {
            _MedicoServices = medicoServices;
            _EspecialidadServices = especialidadServices;
        }

        public async Task<IActionResult> Index([FromQuery]string? nombreCompleto,[FromQuery] DateTime? fechaIngresoInicio, [FromQuery] DateTime? fechaIngresoFin, [FromQuery] string? matricula,
            [FromQuery]string? cedulaProfecional, [FromQuery] int? especialidadSeleccionada)
        {
            var listaEspecialidades = await _EspecialidadServices.ListarEspecialidadAsync(); 
            var response = await _MedicoServices.ListarMedicosAsync(nombreCompleto, fechaIngresoInicio, fechaIngresoFin, matricula, cedulaProfecional, especialidadSeleccionada);
            ViewBag.nombreCompleto = nombreCompleto;
            ViewBag.fechaIngresoInicio = fechaIngresoInicio;
            ViewBag.fechaIngresoFin = fechaIngresoInicio;
            ViewBag.matricula = matricula;
            ViewBag.cedulaProfecional = cedulaProfecional;
            ViewBag.especialidadSeleccionada = especialidadSeleccionada;

            var model = new SearchMedicoViewModel()
            {
                NombreCompleto = nombreCompleto,
                FechaIngresoInicio = fechaIngresoInicio,
                FechaIngresoFin = fechaIngresoFin,
                Matricula = matricula,
                CedulaProfecional = cedulaProfecional,
                Especialidades = listaEspecialidades.Data,
                EspecialidadSeleccionada = especialidadSeleccionada,
                Medicos = response.Data
            };

            return View(model);
        }


        public async Task<IActionResult> Crear()
        {
            var model = new MedicoViewModel();

            var listaEspecialidades = await _EspecialidadServices.ListarEspecialidadAsync();
            
            model.Especialidades = listaEspecialidades.Data;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(MedicoViewModel model)
        {


            var response = await _MedicoServices.InsertarAsync(new MedicoDTORequest()
            {
                NombreCompleto = model.NombreCompleto,
                Matricula = model.Matricula,
                CedulaProfecional = model.CedulaProfecional,
                EspecialidadId = model.EspecialidadSeleccionada
                
            });

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var entity = await _MedicoServices.FinMedicoByIdAsync(id);
            var listaEspecialidades = await _EspecialidadServices.ListarEspecialidadAsync();

            if(entity is  null)
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new MedicoViewModel()
            {
                MedicoId = entity.Data.MedicoId,
                NombreCompleto = entity.Data.NombreCompleto,
                Matricula = entity.Data.Matricula,
                CedulaProfecional = entity.Data.CedulaProfecional,
                Especialidades = listaEspecialidades.Data,
                NombreEspecialidad = entity.Data.Especialidad.NombreEspecialidad
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(MedicoViewModel model)
        {

            await _MedicoServices.UpdatearAsync(model.MedicoId,
                new MedicoDTORequest()
                {
                    NombreCompleto = model.NombreCompleto,
                    Matricula = model.Matricula,
                    CedulaProfecional = model.CedulaProfecional,
                    EspecialidadId = model.EspecialidadSeleccionada,
                });

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var existeMedico = await _MedicoServices.FinMedicoByIdAsync(id);

            if(existeMedico is  null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new MedicoViewModel()
            {
                MedicoId = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(MedicoViewModel model)
        {
            var medico = await _MedicoServices.EliminarAsync(model.MedicoId);

            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> MedicosEliminados()
        {
            var listaMedicosEliminados = await _MedicoServices.ListarEliminadosAsync();

            return View(listaMedicosEliminados.Data);
        }

        public async Task<IActionResult> Reactivar(int id)
        {
            var existeMedico = await _MedicoServices.FinMedicoByIdAsync(id);

             if(existeMedico is null)
             {
                return RedirectToAction(nameof(Index));
             }

            var model = new MedicoViewModel()
            {
                MedicoId = id
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Reactivar(MedicoViewModel model)
        {
            var reactivarMedico = await _MedicoServices.ReactivarAsync(model.MedicoId);

            return RedirectToAction(nameof(MedicosEliminados));
        }

        


    }
}
