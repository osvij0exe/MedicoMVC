using MedicoMVC.DataAcces;
using MedicoMVC.Models.Response;
using MedicoMVC.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace MedicoMVC.Services.Implementacion
{
    public class EspecialidadesServices : IEspecialidadesServices
    {
        private readonly SettingStrings _Conections;
        private readonly ILogger<EspecialidadesServices> _Logger;

        public EspecialidadesServices(IOptions<SettingStrings> conections,
            ILogger<EspecialidadesServices> logger)
        {
            _Conections = conections.Value;
            _Logger = logger;
        }


        public async Task<BaseResponseGeneric<List<EspecialidadDTOResponse>>> ListarEspecialidadAsync()
        {
            var response = new BaseResponseGeneric<List<EspecialidadDTOResponse>>();
            var listarEspecialidades = new List<EspecialidadDTOResponse>();

            using (var conection = new SqlConnection(_Conections.MedicoDB))
            {
                await conection.OpenAsync();

                try
                {
                    SqlCommand cmd = new SqlCommand("uspListarEspecialidades", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))
                    {
                        int idPosition = reader.GetOrdinal("Id");
                        int nombreEspecialidadPosition = reader.GetOrdinal("NombreEspecialidad");


                        while (await reader.ReadAsync()) 
                        {
                            listarEspecialidades.Add(new EspecialidadDTOResponse()
                            {
                                Id = reader.IsDBNull(idPosition) ? 0 : reader.GetInt32(idPosition),
                                NombreEspecialidad = reader.IsDBNull(nombreEspecialidadPosition) ? "": reader.GetString(nombreEspecialidadPosition),
                            }) ;
                        }

                        if(listarEspecialidades != null)
                        {
                            response.Data = listarEspecialidades;
                            response.Succsess = true;
                            await reader.CloseAsync();
                        }
                        else
                        {
                            throw new InvalidOperationException("no se encontro ningun registro");
                        }

                    }

                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al listar Especialidades";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;
            }
        }
    }
}
