using MedicoMVC.Models.Request;
using MedicoMVC.Models.Response;
using MedicoMVC.Models.ViewModels;
using MedicoMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicoMVC.Controllers
{
    public class MedicoPruebaController: Controller
    {
        private readonly IMedicoServices _medicoServices;
        private readonly IEspecialidadesServices _especiañidadServices;

        public MedicoPruebaController(IMedicoServices medicoServices,
            IEspecialidadesServices especiañidadServices)
        {
            _medicoServices = medicoServices;
            _especiañidadServices = especiañidadServices;
        }
        public async Task<IActionResult> Index([FromQuery] string? nombreCompleto, [FromQuery] DateTime? fechaIngresoInicio, [FromQuery] DateTime? fechaIngresoFin, 
            [FromQuery] string? matricula,[FromQuery] string? cedulaProfecional, [FromQuery] int? especialidadSeleccionada)
        {
            var listaEspecialidades = await _especiañidadServices.ListarEspecialidadAsync();
            var response = await _medicoServices.ListarMedicosAsync(nombreCompleto, fechaIngresoInicio, fechaIngresoFin, matricula, cedulaProfecional, especialidadSeleccionada);
            ViewBag.nombreCompleto = nombreCompleto;
            ViewBag.fechaIngresoInicio = fechaIngresoInicio;
            ViewBag.fechaIngresoFin = fechaIngresoFin;
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

            var listaDeEspecialidades = await _especiañidadServices.ListarEspecialidadAsync();

            model.Especialidades = listaDeEspecialidades.Data;

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Crear(MedicoViewModel model)
        {

             


            var response = await _medicoServices.InsertarAsync(new Models.Request.MedicoDTORequest()
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

            var listaEspecialidades = await _especiañidadServices.ListarEspecialidadAsync();
            var medico = await _medicoServices.FinMedicoByIdAsync(id);

            var model = new MedicoViewModel()
            {
                MedicoId = medico.Data.MedicoId,
                NombreCompleto = medico.Data.NombreCompleto,
                Matricula = medico.Data.Matricula,
                CedulaProfecional = medico.Data.CedulaProfecional,
                Especialidades = listaEspecialidades.Data,
                NombreEspecialidad = medico.Data.Especialidad.NombreEspecialidad
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(MedicoViewModel model)
        {
            var medico = await _medicoServices.UpdatearAsync(model.MedicoId,
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
            var existeMedico = await _medicoServices.FinMedicoByIdAsync(id);

            if(existeMedico is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new MedicoViewModel()
            {
                MedicoId = id,
                NombreCompleto = existeMedico.Data.NombreCompleto
            };

            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(MedicoViewModel model)
        {
            var medico = await _medicoServices.EliminarAsync(model.MedicoId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MedicosEliminados()
        {
            var listaMedicosEliminados = await _medicoServices.ListarEliminadosAsync();

            return View(listaMedicosEliminados.Data);
        }

        public async Task<IActionResult> Reactivar(int id)
        {
            var existeMedico = await _medicoServices.FinMedicoByIdAsync(id);

            if(existeMedico is null)
            {
                return RedirectToAction(nameof(MedicosEliminados));
            }
            var model = new MedicoViewModel()
            {
                MedicoId = id,
                NombreCompleto = existeMedico.Data.NombreCompleto
            };
            return View(model);


        }

        [HttpPost]
        public async Task<IActionResult> Reactivar(MedicoViewModel model)
        {
            var medico = await _medicoServices.ReactivarAsync(model.MedicoId);

            return RedirectToAction(nameof(MedicosEliminados));


        }

    }
}
