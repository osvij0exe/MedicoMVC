using MedicoMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicoMVC.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly IEspecialidadesServices _Services;

        public EspecialidadController(IEspecialidadesServices services)
        {
            _Services = services;
        }
        public async Task<IActionResult> Index()
        {
            var resoponse = await _Services.ListarEspecialidadAsync();
            return View(resoponse.Data);
        }
    }
}
