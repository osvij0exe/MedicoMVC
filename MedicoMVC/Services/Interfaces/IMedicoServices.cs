using MedicoMVC.Models.Request;
using MedicoMVC.Models.Response;

namespace MedicoMVC.Services.Interfaces
{
    public interface IMedicoServices
    {
        Task<BaseResponse> InsertarAsync(MedicoDTORequest request);
        Task<BaseResponse> UpdatearAsync(int id,MedicoDTORequest request);
        Task<BaseResponse> EliminarAsync(int id);
        Task<BaseResponse> ReactivarAsync(int id);
        Task<BaseResponseGeneric<MedicoDTOResponse>> FinMedicoByIdAsync(int id);
        Task<BaseResponseGeneric<List<MedicoDTOResponse>>> ListarMedicosAsync(string? Nombre,DateTime? fechaIngresoInicio, DateTime? fechaIngresoFin, string? Matricula,string? cedulaProfecional,int? especialidad);
        Task<BaseResponseGeneric<List<MedicoDTOResponse>>> ListarEliminadosAsync();

    }
}
