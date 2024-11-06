using Core.DTOs.CargaInicialMovilDto;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Newtonsoft.Json;
using Core.DTOs.GDMovilDto;

namespace Core.Services
{
    public class GDMovilService : IGDMovilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GDMovilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> CargaInicialAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_scr.prc_carga_inicial_scr_movil");  //sp de ejemplo
        }

        public async Task<ResponseDto> ConsultaOrdenesAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_scr.prc_carga_inicial_scr_movil");  //sp de ejemplo
        }

        public async Task<ResponseDto> OrdenXIdAsync(Op360GD_OrdenIdDto gestionDanioDto)
        {
            var param = JsonConvert.SerializeObject(gestionDanioDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_scr.prc_carga_inicial_scr_movil");  //sp de ejemplo
        }

        public async Task<ResponseDto<IList<Op360GD_OrdenesAsignadasTecnicoMovilDto>>> GetOrdenesAsignadasTecnicoAsync(Op360GD_OrdenesAsignadasTecnicoMovilRequestDto op360GD_OrdenesAsignadasTecnicoMovilRequest)
        {
            var param = JsonConvert.SerializeObject(op360GD_OrdenesAsignadasTecnicoMovilRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto<IList<Op360GD_OrdenesAsignadasTecnicoMovilDto>>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_ordenes_asignadas_tecnico", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> RegistarGestionOrdenAsync(Op360GD_GestionOrdenDto op360GD_GestionOrden)
        {
            /*Op360GD_RegistarGestionOrdenDto registarGestionOrdenDto = new(op360GD_GestionOrden.NUM_OS, op360GD_GestionOrden.NUM_SOLICITUD, op360GD_GestionOrden.TIPO_TRABAJO, op360GD_GestionOrden.TIP_OS, op360GD_GestionOrden.F_GEN, op360GD_GestionOrden.F_ESTM_REST, op360GD_GestionOrden.NIC,
                                                                op360GD_GestionOrden.NUM_CAMP, op360GD_GestionOrden.TIPO_SUSPENSION, op360GD_GestionOrden.COMENT_OS, op360GD_GestionOrden.COMENT_OS2, op360GD_GestionOrden.DATOSUM, op360GD_GestionOrden.ACTIVIDADES,
                                                                op360GD_GestionOrden.PRECINTOS, op360GD_GestionOrden.RECIBOS, op360GD_GestionOrden.APACOEN);*/

            var param = JsonConvert.SerializeObject(op360GD_GestionOrden);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_daniel_gonzalez_test.prc_resgistrar_ordenes_ws_osf", param);
        }

        public async Task<ResponseDto> RechazarOrdenAsync(Op360GD_RechazarOrdenRequestDto op360GD_RechazarOrdenRequest)
        {
            var param = JsonConvert.SerializeObject(op360GD_RechazarOrdenRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_rechazar_orden_scr", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> ComprometerOrdenAsync(Op360GD_ComprometerOrdenRequestDto op360GD_ComprometerOrdenRequest)
        {
            var param = JsonConvert.SerializeObject(op360GD_ComprometerOrdenRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_comprometer_orden_scr", param);  //sp de ejemplo
        }
    }
}
