using Azure.Core;
using MedicoMVC.DataAcces;
using MedicoMVC.Models.Request;
using MedicoMVC.Models.Response;
using MedicoMVC.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedicoMVC.Services.Implementacion
{
    public class MedicoServices : IMedicoServices
    {
        private readonly SettingStrings _Conection;
        private readonly ILogger<MedicoServices> _Logger;

        public MedicoServices(IOptions<SettingStrings> conection,
            ILogger<MedicoServices> logger)
        {
            _Conection = conection.Value;
            _Logger = logger;
        }
        public async Task<BaseResponse> InsertarAsync(MedicoDTORequest request)
        {
            var response = new BaseResponse();

            using (var conection = new SqlConnection(_Conection.MedicoDB))
            {
                await conection.OpenAsync();
                var SQLResponse = 0;

                try
                {
                    SqlCommand cmd = new SqlCommand("uspInsertarMedico", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreCompleto", request.NombreCompleto);
                    cmd.Parameters.AddWithValue("@Matricula", request.Matricula);
                    cmd.Parameters.AddWithValue("@CedulaProfecional", request.CedulaProfecional);
                    cmd.Parameters.AddWithValue("@EspecialidadId", request.EspecialidadId);
                    SQLResponse = await cmd.ExecuteNonQueryAsync();

                    if (SQLResponse != 0)
                    {
;                        response.Succsess = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("No se pudo agregar al medico");
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al insertar al nuevo Medico";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();

                }
                return response;


            }
        }

        public async Task<BaseResponseGeneric<MedicoDTOResponse>> FinMedicoByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<MedicoDTOResponse>();   
            var medico = new MedicoDTOResponse();
            using (var conection = new SqlConnection(_Conection.MedicoDB))
            {
                await conection.OpenAsync();

                try
                {
                    SqlCommand cmd = new SqlCommand("FindMedicoById", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))
                    {
                        int idPosition = reader.GetOrdinal("Id");
                        int nombreCompletoPosition = reader.GetOrdinal("NombreCompleto");
                        int fechaIngresoPosition = reader.GetOrdinal("FechaIngreso");
                        int matriculaPosition = reader.GetOrdinal("Matricula");
                        int cedulaPosition = reader.GetOrdinal("CedulaPorfecional");
                        int especialiadaIdposition = reader.GetOrdinal("EspecialidadId");
                        int NombreEspecialidadPosition = reader.GetOrdinal("NombreEspecialidad");

                        while(await reader.ReadAsync())
                        {
                            medico = new MedicoDTOResponse()
                            {
                                MedicoId = reader.IsDBNull(idPosition) ? 0 : reader.GetInt32(idPosition),
                                NombreCompleto = reader.IsDBNull(nombreCompletoPosition) ? "" : reader.GetString(nombreCompletoPosition),
                                FechaIngreso =  reader.IsDBNull(fechaIngresoPosition) ? "" : reader.GetDateTime(fechaIngresoPosition).ToString(),
                                Matricula = reader.IsDBNull(matriculaPosition) ? "" : reader.GetString(matriculaPosition),
                                CedulaProfecional = reader.IsDBNull(cedulaPosition) ? "" :reader.GetString(cedulaPosition),
                                EspecialidadId = reader.IsDBNull(especialiadaIdposition) ? "" : reader.GetInt32(especialiadaIdposition).ToString(),
                                Especialidad = new MedicoEspecialidadDTOResponse()
                                {
                                    NombreEspecialidad = reader.IsDBNull(NombreEspecialidadPosition) ? "" : reader.GetString(NombreEspecialidadPosition)
                                }
                               
                            };
                        }
                        if(medico != null)
                        {
                            response.Data = medico;
                            response.Succsess = true;
                            await reader.CloseAsync();
                        }
                        else
                        {
                            throw new InvalidOperationException("No se encontro ningun Medico con ese Id");
                        }


                    }

                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al encontrar Medicp por Id";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;

            }

        }
        public async Task<BaseResponseGeneric<List<MedicoDTOResponse>>> ListarMedicosAsync(string? Nombre, DateTime? fechaIngresoInicio, DateTime? fechaIngresoFin, string? Matricula, string? cedulaProfecional, int? especialidad)
        {
            var response = new BaseResponseGeneric<List<MedicoDTOResponse>>();
            var listaDeMedicos = new List<MedicoDTOResponse>();

            using (var conection = new SqlConnection(_Conection.MedicoDB))
            {
                await conection.OpenAsync();
                try
                {
                    SqlCommand cmd = new SqlCommand("ListarMedicos", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreCompleto", Nombre);
                    cmd.Parameters.AddWithValue("@FechaIngresoInicio", fechaIngresoInicio);
                    cmd.Parameters.AddWithValue("@FechaIngresoFin", fechaIngresoFin);
                    cmd.Parameters.AddWithValue("@Matricula", Matricula);
                    cmd.Parameters.AddWithValue("@CedulaProfecional", cedulaProfecional);
                    cmd.Parameters.AddWithValue("@EspecialidadId", especialidad);

                    using (var reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))
                    {
                        int idPosition = reader.GetOrdinal("Id");
                        int nombreCompletoPosition = reader.GetOrdinal("NombreCompleto");
                        int fechaIngresoPosition = reader.GetOrdinal("FechaIngreso");
                        int matriculaPosition = reader.GetOrdinal("Matricula");
                        int cedulaPosition = reader.GetOrdinal("CedulaPorfecional");
                        int especialiadaIdposition = reader.GetOrdinal("EspecialidadId");
                        int NombreEspecialidadPosition = reader.GetOrdinal("NombreEspecialidad");

                        while (await reader.ReadAsync())
                        {
                            listaDeMedicos.Add(new MedicoDTOResponse()
                            {
                                MedicoId = reader.IsDBNull(idPosition) ? 0 : reader.GetInt32(idPosition),
                                NombreCompleto = reader.IsDBNull(nombreCompletoPosition) ? "" : reader.GetString(nombreCompletoPosition),
                                FechaIngreso = reader.IsDBNull(fechaIngresoPosition) ? "" : reader.GetDateTime(fechaIngresoPosition).ToString(),
                                Matricula = reader.IsDBNull(matriculaPosition) ? "" : reader.GetString(matriculaPosition),
                                CedulaProfecional = reader.IsDBNull(cedulaPosition) ? "" : reader.GetString(cedulaPosition),
                                EspecialidadId = reader.IsDBNull(especialiadaIdposition) ? "" : reader.GetInt32(especialiadaIdposition).ToString(),
                                Especialidad = new MedicoEspecialidadDTOResponse()
                                {
                                    NombreEspecialidad = reader.IsDBNull(NombreEspecialidadPosition) ? "" : reader.GetString(NombreEspecialidadPosition)
                                }

                            });
                        }
                        if (listaDeMedicos != null)
                        {
                            response.Data = listaDeMedicos;
                            response.Succsess = true;
                            await reader.CloseAsync();
                        }
                        else
                        {
                            throw new InvalidOperationException("No se encontro ningun Medico ");
                        }
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al listar Medicos";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;
            }

        }
        public async Task<BaseResponse> UpdatearAsync(int id, MedicoDTORequest request)
        {
            var response = new BaseResponse();

            using (var connection = new SqlConnection(_Conection.MedicoDB))
            {
                await connection.OpenAsync();

                try
                {
                    var SQLResponse = 0;
                    var existeMedico = await FinMedicoByIdAsync(id);

                    if(existeMedico is not null)
                    {
                        SqlCommand cmd = new SqlCommand("ActualizarMedico", connection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@NombreCompleto", request.NombreCompleto);
                        cmd.Parameters.AddWithValue("@Matricula", request.Matricula);
                        cmd.Parameters.AddWithValue("@CedulaProfecional", request.CedulaProfecional);
                        cmd.Parameters.AddWithValue("@EspecialidadId", request.EspecialidadId);
                        SQLResponse = await cmd.ExecuteNonQueryAsync();

                        if(SQLResponse != 0)
                        {
                            
                            response.Succsess = existeMedico != null;
                        }
                        else
                        {
                            throw new InvalidOperationException("No se pudo Actualizar el Medico");
                        }

                    }
                    
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al actualizar Medico";
                    _Logger.LogCritical(ex, "{ErrorMesage}{Message}", response.ErrorMessage, ex.Message);
                    await connection.CloseAsync();
                }
                return response;
            }


        }

        public async Task<BaseResponse> EliminarAsync(int id)
        {
            var response = new BaseResponse();

            using (var connection = new SqlConnection(_Conection.MedicoDB))
            {
                await connection.OpenAsync();

                try
                {
                    var existeMedico = await FinMedicoByIdAsync(id);
                    if(existeMedico is not null)
                    {
                        var SQLResponse = 0;

                        SqlCommand cmd = new SqlCommand("EliminarMedico", connection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id",id);
                        SQLResponse = await cmd.ExecuteNonQueryAsync();

                        if(SQLResponse != 0)
                        {
                            response.Succsess = true;
                        }
                        else
                        {
                            throw new InvalidOperationException("No se pudo eliminar el Medico correctamente");
                        }


                    }
                    else
                    {
                        throw new InvalidOperationException("El medico con ese Id no existe");
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Erorr al eliminar el Medico";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await connection.CloseAsync();
                }
                return response;
            }

        }



        public async Task<BaseResponseGeneric<List<MedicoDTOResponse>>> ListarEliminadosAsync()
        {
            var response = new BaseResponseGeneric<List<MedicoDTOResponse>>();

            var ListaDeMedicos = new List<MedicoDTOResponse>();

            using (var conection = new SqlConnection(_Conection.MedicoDB))
            {
                await conection.OpenAsync();
                try
                {
                    SqlCommand cmd = new SqlCommand("ListarMedicosEliminados", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))
                    {
                        int idPosition = reader.GetOrdinal("Id");
                        int nombreCompletoPosition = reader.GetOrdinal("NombreCompleto");
                        int fechaIngresoPosition = reader.GetOrdinal("FechaIngreso");
                        int matriculaPosition = reader.GetOrdinal("Matricula");
                        int cedulaPosition = reader.GetOrdinal("CedulaPorfecional");
                        int especialiadaIdposition = reader.GetOrdinal("EspecialidadId");
                        int NombreEspecialidadPosition = reader.GetOrdinal("NombreEspecialidad");

                        while (await reader.ReadAsync())
                        {
                            ListaDeMedicos.Add(new MedicoDTOResponse()
                            {
                                MedicoId = reader.IsDBNull(idPosition) ? 0 : reader.GetInt32(idPosition),
                                NombreCompleto = reader.IsDBNull(nombreCompletoPosition) ? "" : reader.GetString(nombreCompletoPosition),
                                FechaIngreso = reader.IsDBNull(fechaIngresoPosition) ? "" : reader.GetDateTime(fechaIngresoPosition).ToString(),
                                Matricula = reader.IsDBNull(matriculaPosition) ? "" : reader.GetString(matriculaPosition),
                                CedulaProfecional = reader.IsDBNull(cedulaPosition) ? "" : reader.GetString(cedulaPosition),
                                EspecialidadId = reader.IsDBNull(especialiadaIdposition) ? "" : reader.GetInt32(especialiadaIdposition).ToString(),
                                Especialidad = new MedicoEspecialidadDTOResponse()
                                {
                                    NombreEspecialidad = reader.IsDBNull(NombreEspecialidadPosition) ? "" : reader.GetString(NombreEspecialidadPosition)
                                }

                            });
                        }
                        if (ListaDeMedicos != null)
                        {
                            response.Data = ListaDeMedicos;
                            response.Succsess = true;
                            await reader.CloseAsync();
                        }
                        else
                        {
                            throw new InvalidOperationException("No se encontro ningun Medico ");
                        }
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al listar Medicos";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;
            }
        }

        public async Task<BaseResponse> ReactivarAsync(int id)
        {
            var response = new BaseResponse();

            using (var connection = new SqlConnection(_Conection.MedicoDB))
            {
                await connection.OpenAsync();

                try
                {
                    var existeMedico = await FinMedicoByIdAsync(id);
                    if (existeMedico is not null)
                    {
                        var SQLResponse = 0;

                        SqlCommand cmd = new SqlCommand("ReactivarMedico", connection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        SQLResponse = await cmd.ExecuteNonQueryAsync();

                        if (SQLResponse != 0)
                        {
                            response.Succsess = true;
                        }
                        else
                        {
                            throw new InvalidOperationException("No se pudo eliminar el Medico correctamente");
                        }


                    }
                    else
                    {
                        throw new InvalidOperationException("El medico con ese Id no existe");
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Erorr al eliminar el Medico";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await connection.CloseAsync();
                }
                return response;
            }

        }


    }
}
