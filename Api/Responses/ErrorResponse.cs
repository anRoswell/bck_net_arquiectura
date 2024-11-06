using Core.ModelResponse;
using System.Collections.Generic;

namespace Api.Responses
{
    public static class ErrorResponse
    {
        public static ApiResponse<List<ResponseAction>> GetError(bool estado, string mensaje, int codigoRespuesta)
        {
            List<ResponseAction> response = new List<ResponseAction>();
            var repAction = new ResponseAction();
            repAction.estado = estado;
            repAction.mensaje = mensaje;
            repAction.codigo = 0;
            response.Add(repAction);
            var responseBad = new ApiResponse<List<ResponseAction>>(response, codigoRespuesta);
            return responseBad;
        }

        public static ApiResponse<List<ResponseAction>> GetErrorDescripcion(bool estado, string mensaje, string descrip, int codigoRespuesta)
        {
            List<ResponseAction> response = new List<ResponseAction>();
            var repAction = new ResponseAction();
            repAction.estado = estado;
            repAction.mensaje = mensaje;
            repAction.error = descrip;
            repAction.codigo = 0;
            response.Add(repAction);
            var responseBad = new ApiResponse<List<ResponseAction>>(response, codigoRespuesta, mensaje);
            return responseBad;
        }

        public static ApiResponse<List<ResponseActionOp360>> Op360ErrorTemplate(int estado, string mensaje, int codigoRespuesta)
        {
            List<ResponseActionOp360> response = new List<ResponseActionOp360>();
            var repAction = new ResponseActionOp360();
            repAction.codigo = estado;
            repAction.mensaje = mensaje;
            response.Add(repAction);
            var responseBad = new ApiResponse<List<ResponseActionOp360>>(response, codigoRespuesta, mensaje);
            return responseBad;
        }
    }
}
