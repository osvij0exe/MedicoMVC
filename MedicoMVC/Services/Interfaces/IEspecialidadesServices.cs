using MedicoMVC.Models.Response;

namespace MedicoMVC.Services.Interfaces
{
    public interface IEspecialidadesServices
    {
        Task<BaseResponseGeneric<List<EspecialidadDTOResponse>>> ListarEspecialidadAsync();

    }
}
