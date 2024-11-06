namespace Core.Services
{
    using Core.CustomEntities;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.ModelProcess;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using Core.Tools;
    using ImageMagick;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ContratoService : IContratoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdobeSignService _adobeSignService;
        private readonly IFilesProcess _filesProcess;
        private readonly IConfiguration _configuration;

        public ContratoService(
            IUnitOfWork unitOfWork,
            IAdobeSignService adobeSignService,
            IFilesProcess filesProcess,
            IConfiguration configuration
        )
        {
            _unitOfWork = unitOfWork;
            _adobeSignService = adobeSignService;
            _filesProcess = filesProcess;
            _configuration = configuration;
        }

        public async Task<List<Contrato>> SearchAll()
        {
            return await _unitOfWork.ContratoRepository.SearchAll();
        }

        public async Task<List<Contrato>> SearchById(QueryContrato par)
        {
            return await _unitOfWork.ContratoRepository.SearchById(par.IdContrato);
        }

        public async Task<List<ResponseAction>> Post(ContratoDto contrato)
        {
            return await _unitOfWork.ContratoRepository.Post(contrato);
        }

        public async Task<List<ResponseAction>> Update(ContratoDto contrato)
        {
            return await _unitOfWork.ContratoRepository.Update(contrato);
        }

        public async Task<List<ResponseAction>> UpdateEstado(QueryContrato par)
        {
            return await _unitOfWork.ContratoRepository.UpdateEstado(par);
        }

        public async Task<ContratoDetalle> GetContratoDetallePorID(int id)
        {
            return await _unitOfWork.ContratoRepository.GetContratoDetallePorID(id);
        }

        public async Task<List<ResponseAction>> EliminarDocContrato(int id)
        {
            return await _unitOfWork.ContratoRepository.EliminarDocContrato(id);
        }

        public async Task<ParametrosContrato> GetParametrosContrato()
        {
            var parametros = await _unitOfWork.ContratoRepository.GetParametrosContrato();

            parametros.Empresas = await _unitOfWork.EmpresasRepository.GetEmpresas();

            return parametros;
        }

        public async Task<List<ResponseAction>> AprobacionContrato(QueryAprobacionContrato par)
        {
            return await _unitOfWork.ContratoRepository.AprobacionContrato(par);
        }

        public async Task<List<TimelineDto>> GetTimelineContratoById(int id)
        {
            return await _unitOfWork.ContratoRepository.GetTimelineContratoById(id);
        }

        public async Task<List<ResponseAction>> AsociarDocumentosProveedor(DocumentosProveedorDto documentos)
        {
            return await _unitOfWork.ContratoRepository.AsociarDocumentosProveedor(documentos);
        }

        public async Task<List<ResponseAction>> HabilitarContrato_FirmaElectronica(QueryHabilitarContrato parameters)
        {
            int IdPathFS = int.Parse(_configuration.GetSection("IdPathFileServer")?.Value);

            List<ResponseAction> response = null;

            if (!File.Exists(parameters.Info.PathTmp_Contrato))
            {
                Directory.CreateDirectory(parameters.Info.PathTmp_Contrato);
            }
            parameters.Info.PathTmp_Contrato = Path.Combine(parameters.Info.PathTmp_Contrato, parameters.Info.NombreArchivo);

            string pathContrato = Path.Combine(parameters.Info.Path_Contrato, parameters.Info.CodContrato.ToString());
            if (!File.Exists(pathContrato))
            {
                Directory.CreateDirectory(pathContrato);
            }
            parameters.Info.Path_Contrato = Path.Combine(pathContrato, parameters.Info.NombreArchivo);
            (bool, string) resp = await Funciones.GenerateArchivoPdfFirmaElectronica(parameters.File, parameters.Info.PathTmp_Contrato, parameters.Info.Path_Contrato);

            if (resp.Item1)
            {
                // Actualizamos la URL del contrato
                string pathRelContrato = string.Concat(parameters.Info.PathRel_Contrato, "/", parameters.Info.NombreArchivo);
                string pathWebContrato = string.Concat(parameters.Info.PathWebAbsolute_Contrato, "/", pathRelContrato);

                response = await _unitOfWork.ContratoRepository.UpdateUrlContrato(new QueryUpdateUrlContrato()
                {
                    CodContrato = parameters.Info.CodContrato,
                    Url = pathRelContrato,
                    UrlAbsolute = pathWebContrato,
                    CodUserUpdate = parameters.CodUserUpdate
                });

                if (response[0].estado)
                {
                    _adobeSignService.Tipo_Agreement = AdbSign_TipoAcuerdo.Contrato;
                    // 1-- Cargamos contrato en Adobe Sign
                    TransientDocument transientPdfDocument = await _adobeSignService.TransientDocumentsAsync(parameters.Info.Path_Contrato, id: parameters.Info.CodContrato);

                    if (!(transientPdfDocument is null))
                    {
                        // Registramos evento Adobde Sign en la tabla [cont].[ContAdobeSign] en Estado 1 (UPLOADED)
                        response = await _unitOfWork.AdobeSignRepository.GuardarTrazaAdobeSign_Contrato(parameters.Info.CodContrato, (int)AdobeSign_Estados.UPLOADED, transientPdfDocument.TransientDocumentId, null);

                        if (response[0].estado)
                        {
                            var contratoDetalle = await _unitOfWork.ContratoRepository.GetContratoDetallePorID(parameters.Info.CodContrato);

                            var contrato = contratoDetalle.Contratos.FirstOrDefault();

                            var proveedor = (await _unitOfWork.ProveedorRepository.GetProveedorPorNit(contrato.ContNitContratista, IdPathFS)).FirstOrDefault();


                            //2-- Enviamos el pdf para firma
                            AgreementsResponse sendAgreements = await _adobeSignService.AgreementsAsync(
                                transientDocumentId: transientPdfDocument.TransientDocumentId,
                                nombreAcuerdo: AdobeSign_NombreAcuerdos.Contrato,
                                emailProveedor: proveedor.PrvMail,
                                emailRteLegal: contrato.ContEmailRteLegalCtante,
                                id: parameters.Info.CodContrato
                            );

                            if (!(sendAgreements is null))
                            {
                                // Registramos evento Adobde Sign en la tabla ContAdobeSign en Estado 2 (OUT_FOR_SIGNATURE)
                                response = await _unitOfWork.AdobeSignRepository.GuardarTrazaAdobeSign_Contrato(parameters.Info.CodContrato, (int)AdobeSign_Estados.OUT_FOR_SIGNATURE, sendAgreements.Id, null);
                            }
                            else
                            {
                                throw new BusinessException("No se pudo generar contrato para firma, por favor contactar al administrador del sistema.");
                            }
                        }
                    }
                    else
                    {
                        throw new BusinessException("No se pudo cargar contrato para firma, por favor contactar al administrador del sistema.");
                    }
                }
                else
                {
                    throw new BusinessException("No se pudo guardar URL del contrato, por favor contactar al administrador del sistema.");
                }
            }

            if (response[0].estado)
            {
                response = new List<ResponseAction> {
                    new ResponseAction()
                    {
                        estado = resp.Item1,
                        error = resp.Item1 ? "" : "Error al habilitar contrato para firma.",
                        mensaje = resp.Item2
                    }
                };
            }

            return response;
        }

        public async Task<List<ResponseAction>> UpdateUrlContrato(QueryUpdateUrlContrato par)
        {
            return await _unitOfWork.ContratoRepository.UpdateUrlContrato(par);
        }

        public async Task<List<ResponseAction>> UpdateUrlContrato_CargadoManual(QueryUpdateUrlContrato par)
        {
            return await _unitOfWork.ContratoRepository.UpdateUrlContrato_CargadoManual(par);
        }

        public async Task<string> ValidarArchivoContrato(int idContrato)
        {
            List<Contrato> resp = await _unitOfWork.ContratoRepository.SearchById(idContrato);

            if (resp.Any())
            {
                return resp.FirstOrDefault().ContUrlPdf;
            }

            return null;
        }

        public async Task<List<ResponseAction>> SolicitarDocumentosProveedor(QuerySolicitarDocumentosProveedor parameters)
        {
            return await _unitOfWork.ContratoRepository.SolicitarDocumentosProveedor(parameters);
        }

        public async Task<ParametrosContrato> SearchByProveedor(int idUsuario)
        {
            return await _unitOfWork.ContratoRepository.SearchByProveedor(idUsuario);
        }

        public async Task<List<ResponseAction>> RechazarDocumentosProveedor(QueryRechazarDocumentosProveedor parameters)
        {
            return await _unitOfWork.ContratoRepository.RechazarDocumentosProveedor(parameters);
        }

        public async Task<List<ResponseAction>> GuardarSeguimiento(SeguimientosContratoDto parameters)
        {
            return await _unitOfWork.ContratoRepository.GuardarSeguimiento(parameters);
        }

        public async Task<List<ResponseAction>> GuardarNotificacionNoProrroga(ContratoNotificacionNoProrroga parameters)
        {
            return await _unitOfWork.ContratoRepository.GuardarNotificacionNoProrroga(parameters);
        }

        public async Task<List<ResponseAction>> GuardarNotificacionTerminacionAnticipada(ContratoNotificacionTerminacion parameters)
        {
            return await _unitOfWork.ContratoRepository.GuardarNotificacionTerminacionAnticipada(parameters);
        }

        public async Task<List<ResponseAction>> AprobarProrroga(QueryAprobarProrroga parameters)
        {
            return await _unitOfWork.ContratoRepository.AprobarProrroga(parameters);
        }

        public async Task<List<ResponseAction>> AsociarPolizasRenovadas(QueryContratoPolizasRenovadas parameters)
        {
            return await _unitOfWork.ContratoRepository.AsociarPolizasRenovadas(parameters);
        }

        public async Task<List<ResponseAction>> GuardarActaLiquidacion(ContratoActaLiquidacion parameters)
        {
            return await _unitOfWork.ContratoRepository.GuardarActaLiquidacion(parameters);
        }

        public async Task<List<ResponseAction>> SolicitarModificacionContrato(QuerySolicitudModificacion par)
        {
            return await _unitOfWork.ContratoRepository.SolicitarModificacionContrato(par);
        }

        public async Task<ParametrosHistoricos> GetParametrosHistoricos()
        {
            return await _unitOfWork.ContratoRepository.GetParametrosHistoricos();
        }

        public async Task<List<ResponseAction>> GuardarComentarioContrato(CommandCreateComentarioContrato command)
        {
            try
            {
                ContQuestionAnswer comentario = JsonConvert.DeserializeObject<ContQuestionAnswer>(command.Comentario);

                if (command?.Files == null || !command.Files.Any())
                {
                    comentario.FileExt = string.Empty;
                    comentario.FilePath = string.Empty;
                    comentario.FileSize = 0;
                    comentario.FileRelativo = string.Empty;
                }

                if (command?.Files != null && command.Files.Any())
                {
                    string fileFolder = _configuration.GetSection("Folder_Archivos")?.Value;
                    int idPathFS = int.Parse(_configuration.GetSection("IdPathFileServer")?.Value);

                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = command.Files,
                        IdPathFileServer = idPathFS
                    };

                    //List<FileResponse> filesComentario = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesComentario = new() { };

                    if (filesComentario.Any())
                    {
                        FileResponse documentoGuardado = filesComentario.FirstOrDefault();

                        comentario.FileExt = documentoGuardado.Extension;
                        comentario.FilePath = documentoGuardado.PathWebAbsolute;
                        comentario.FileSize = Convert.ToInt32(documentoGuardado.Size);
                        comentario.FileRelativo = documentoGuardado.PathWebRelative;
                    }
                }

                return await _unitOfWork.ContratoRepository.GuardarComentario(comentario);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }

        public async Task<List<ContratoComentarios>> ConsultarComentariosContrato(int IdContrato) 
        {
            return await _unitOfWork.ContratoRepository.ConsultarComentariosPorContratoId(IdContrato);
        }

        public async Task<List<ResponseAction>> CambiarEstadoContrato(CommandUpdateEstadoContrato command)
        {
            return await _unitOfWork.ContratoRepository.CambiarEstadoContrato(command);
        }

        //public async Task<List<ContratoListado>> ListadoContratos(QueryListadoContratosReporte query)
        //{
        //    return await _unitOfWork.ContratoRepository.ListadoContratosReporte(query);
        //}

        //public async Task<byte[]> ListadoContratosExcel(QueryListadoContratosReporte query) 
        //{
        //    var listado = await _unitOfWork.ContratoRepository.ListadoContratosReporte(query);

        //    var listadoHistorico = await _unitOfWork.ContratoRepository.ListadoContratosHistoricosReporte(query);

        //    var datosReporte = await GenerarReporteListado(listado, listadoHistorico);

        //    return datosReporte;
        //}

        ////public async Task<byte[]> ListadoContratosHistoricoExcel(QueryListadoContratosReporte query)
        ////{
        ////    var listado = await _unitOfWork.ContratoRepository.ListadoContratosHistoricosReporte(query);

        ////    var datosReporte = await GenerarReporteListadoHistorico(listado);

        ////    return datosReporte;
        ////}

        //private static async Task<byte[]> GenerarReporteListado(List<ContratoListado> listado, List<ContratoListado> listadoHistorico) 
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using ExcelPackage package = new();
            
        //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Contratos");
            
        //    //Configuraciones

        //    var row = 1;
            
        //    //Títulos
        //    using ExcelRange rangoTitle = worksheet.Cells[$"A{row}:O{row}"];
        //    rangoTitle.Style.Font.Color.SetColor(Color.White);
        //    rangoTitle.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //    rangoTitle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 32, 96));
            

        //    worksheet.Cells[$"A{row}"].Value = "Nit Contratista";
        //    worksheet.Cells[$"B{row}"].Value = "Nombre Contratista";
        //    worksheet.Cells[$"C{row}"].Value = "Tipo de Contrato";
        //    worksheet.Cells[$"D{row}"].Value = "Numero de Contrato";
        //    worksheet.Cells[$"E{row}"].Value = "Objeto";
        //    worksheet.Cells[$"F{row}"].Value = "Duración en meses";
        //    worksheet.Cells[$"G{row}"].Value = "Fecha inicio";
        //    worksheet.Cells[$"H{row}"].Value = "Fecha fin";
        //    worksheet.Cells[$"I{row}"].Value = "Fecha acta de inicio";
        //    worksheet.Cells[$"J{row}"].Value = "Dias faltantes para finalizar";
        //    worksheet.Cells[$"K{row}"].Value = "Valor del contrato";
        //    worksheet.Cells[$"L{row}"].Value = "Administrador del contrato";
        //    worksheet.Cells[$"M{row}"].Value = "Estado actual";
        //    worksheet.Cells[$"N{row}"].Value = "Pólizas";
        //    worksheet.Cells[$"O{row}"].Value = "Tiene OtroSi";

        //    //Llenamos el detalle

        //    foreach (var contrato in listado)
        //    {
        //        row++;

        //        worksheet.Cells[$"A{row}"].Value = contrato.NitContratista;
        //        worksheet.Cells[$"B{row}"].Value = contrato.NombreContratista;
        //        worksheet.Cells[$"C{row}"].Value = contrato.Tipo;
        //        worksheet.Cells[$"D{row}"].Value = contrato.Numero;
        //        worksheet.Cells[$"E{row}"].Value = contrato.Objeto;
        //        worksheet.Cells[$"F{row}"].Value = contrato.NumeroMeses;
                
        //        worksheet.Cells[$"G{row}"].Value = contrato.FechaInicio;
        //        worksheet.Cells[$"G{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //        worksheet.Cells[$"H{row}"].Value = contrato.FechaFinal;
        //        worksheet.Cells[$"H{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //        worksheet.Cells[$"I{row}"].Value = contrato.FechaInicioActa;
        //        worksheet.Cells[$"I{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //        worksheet.Cells[$"J{row}"].Value = contrato.NumeroDiasFinalizacion;
                
        //        worksheet.Cells[$"K{row}"].Value = contrato.Valor;
        //        worksheet.Cells[$"K{row}"].Style.Numberformat.Format = "[$$-409]#,##0";

        //        worksheet.Cells[$"L{row}"].Value = contrato.Administrador;
        //        worksheet.Cells[$"M{row}"].Value = contrato.Estado;
                
        //        worksheet.Cells[$"N{row}"].Value = contrato.Polizas;
        //        worksheet.Cells[$"N{row}"].Style.WrapText = true;

        //        worksheet.Cells[$"O{row}"].Value = contrato.OtrosSies;
                
        //        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //    }

        //    worksheet.Columns.AutoFit();
        //    worksheet.Column(14).Width = 69.57;
        //    worksheet.View.FreezePanes(2, 1);


        //    var contratistas = from contrato in listadoHistorico
        //                       group contrato by new { contrato.NitContratista, contrato.NombreContratista, contrato.Tipo, contrato.IdContrato };

        //    ExcelWorksheet worksheetHistorico = package.Workbook.Worksheets.Add("otroSi");

        //    row = 1;

        //    //Títulos
        //    using ExcelRange rangoTitleHistorico = worksheetHistorico.Cells[$"A{row}:O{row}"];
        //    rangoTitleHistorico.Style.Font.Color.SetColor(Color.White);
        //    rangoTitleHistorico.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //    rangoTitleHistorico.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 32, 96));


        //    worksheetHistorico.Cells[$"A{row}"].Value = "Nit Contratista";
        //    worksheetHistorico.Cells[$"B{row}"].Value = "Nombre Contratista";
        //    worksheetHistorico.Cells[$"C{row}"].Value = "Tipo de Contrato";
        //    worksheetHistorico.Cells[$"D{row}"].Value = "Numero de OtroSi";
        //    worksheetHistorico.Cells[$"E{row}"].Value = "Numero de Contrato";
        //    worksheetHistorico.Cells[$"F{row}"].Value = "Objeto";
        //    worksheetHistorico.Cells[$"G{row}"].Value = "Duración en meses";
        //    worksheetHistorico.Cells[$"H{row}"].Value = "Fecha inicio";
        //    worksheetHistorico.Cells[$"I{row}"].Value = "Fecha fin";
        //    worksheetHistorico.Cells[$"J{row}"].Value = "Fecha acta de inicio";
        //    worksheetHistorico.Cells[$"K{row}"].Value = "Dias faltantes para finalizar";
        //    worksheetHistorico.Cells[$"L{row}"].Value = "Valor del contrato";
        //    worksheetHistorico.Cells[$"M{row}"].Value = "Administrador del contrato";
        //    worksheetHistorico.Cells[$"N{row}"].Value = "Estado actual";
        //    worksheetHistorico.Cells[$"O{row}"].Value = "Pólizas";

        //    //Llenamos el detalle

        //    foreach (var contratista in contratistas)
        //    {
        //        row++;

        //        var cantidadContratos = contratista.Count();

        //        if (cantidadContratos == 1)
        //        {
        //            worksheetHistorico.Cells[$"A{row}"].Value = contratista.Key.NitContratista;
        //            worksheetHistorico.Cells[$"A{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            worksheetHistorico.Cells[$"B{row}"].Value = contratista.Key.NombreContratista;
        //            worksheetHistorico.Cells[$"B{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            worksheetHistorico.Cells[$"C{row}"].Value = contratista.Key.Tipo;
        //            worksheetHistorico.Cells[$"C{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        }

        //        if (cantidadContratos > 1)
        //        {
        //            //Agrupamos
        //            var rowFinal = row + cantidadContratos - 1;

        //            using ExcelRange rangoNitContratista = worksheetHistorico.Cells[$"A{row}:A{rowFinal}"];
        //            rangoNitContratista.Merge = true;
        //            rangoNitContratista.Value = contratista.Key.NitContratista;
        //            rangoNitContratista.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            using ExcelRange rangoNombreContratista = worksheetHistorico.Cells[$"B{row}:B{rowFinal}"];
        //            rangoNombreContratista.Merge = true;
        //            rangoNombreContratista.Value = contratista.Key.NombreContratista;
        //            rangoNombreContratista.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            using ExcelRange rangoTipoContrato = worksheetHistorico.Cells[$"C{row}:C{rowFinal}"];
        //            rangoTipoContrato.Merge = true;
        //            rangoTipoContrato.Value = contratista.Key.Tipo;
        //            rangoTipoContrato.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        }

        //        int contadorContrato = 0;

        //        foreach (var contrato in contratista)
        //        {
        //            contadorContrato++;

        //            worksheetHistorico.Cells[$"D{row}"].Value = contadorContrato;
        //            worksheetHistorico.Cells[$"E{row}"].Value = contrato.Numero;
        //            worksheetHistorico.Cells[$"F{row}"].Value = contrato.Objeto;
        //            worksheetHistorico.Cells[$"G{row}"].Value = contrato.NumeroMeses;

        //            worksheetHistorico.Cells[$"H{row}"].Value = contrato.FechaInicio;
        //            worksheetHistorico.Cells[$"H{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //            worksheetHistorico.Cells[$"I{row}"].Value = contrato.FechaFinal;
        //            worksheetHistorico.Cells[$"I{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //            worksheetHistorico.Cells[$"J{row}"].Value = contrato.FechaInicioActa;
        //            worksheetHistorico.Cells[$"J{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //            worksheetHistorico.Cells[$"K{row}"].Value = contrato.NumeroDiasFinalizacion;

        //            worksheetHistorico.Cells[$"L{row}"].Value = contrato.Valor;
        //            worksheetHistorico.Cells[$"L{row}"].Style.Numberformat.Format = "[$$-409]#,##0";

        //            worksheetHistorico.Cells[$"M{row}"].Value = contrato.Administrador;
        //            worksheetHistorico.Cells[$"N{row}"].Value = contrato.Estado;

        //            worksheetHistorico.Cells[$"O{row}"].Value = contrato.Polizas;
        //            worksheetHistorico.Cells[$"O{row}"].Style.WrapText = true;

        //            if (contadorContrato != cantidadContratos)
        //            {
        //                row++;
        //            }
        //        }
        //    }

        //    //Configuraciones
        //    //worksheetHistorico.Columns.AutoFit();
        //    worksheetHistorico.Column(15).Width = 69.57;
        //    worksheetHistorico.View.FreezePanes(2, 1);

        //    return await package.GetAsByteArrayAsync();
        //}
        
        
        //private static async Task<byte[]> GenerarReporteListadoHistorico(List<ContratoListado> listado)
        //{
        //    var contratistas = from contrato in listado
        //                       group contrato by new { contrato.NitContratista, contrato.NombreContratista, contrato.Tipo, contrato.IdContrato };

        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using ExcelPackage package = new();

        //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Contratos");

        //    var row = 1;

        //    //Títulos
        //    using ExcelRange rangoTitle = worksheet.Cells[$"A{row}:O{row}"];
        //    rangoTitle.Style.Font.Color.SetColor(Color.White);
        //    rangoTitle.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //    rangoTitle.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 32, 96));


        //    worksheet.Cells[$"A{row}"].Value = "Nit Contratista";
        //    worksheet.Cells[$"B{row}"].Value = "Nombre Contratista";
        //    worksheet.Cells[$"C{row}"].Value = "Tipo de Contrato";
        //    worksheet.Cells[$"D{row}"].Value = "Numero de Contrato";
        //    worksheet.Cells[$"E{row}"].Value = "Objeto";
        //    worksheet.Cells[$"F{row}"].Value = "Duración en meses";
        //    worksheet.Cells[$"G{row}"].Value = "Fecha inicio";
        //    worksheet.Cells[$"H{row}"].Value = "Fecha fin";
        //    worksheet.Cells[$"I{row}"].Value = "Fecha acta de inicio";
        //    worksheet.Cells[$"J{row}"].Value = "Dias faltantes para finalizar";
        //    worksheet.Cells[$"K{row}"].Value = "Valor del contrato";
        //    worksheet.Cells[$"L{row}"].Value = "Administrador del contrato";
        //    worksheet.Cells[$"M{row}"].Value = "Estado actual";
        //    worksheet.Cells[$"N{row}"].Value = "Pólizas";

        //    //Llenamos el detalle

        //    foreach (var contratista in contratistas)
        //    {
        //        row++;
                
        //        var cantidadContratos = contratista.Count();

        //        if (cantidadContratos == 1) 
        //        {
        //            worksheet.Cells[$"A{row}"].Value = contratista.Key.NitContratista;
        //            worksheet.Cells[$"A{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            worksheet.Cells[$"B{row}"].Value = contratista.Key.NombreContratista;
        //            worksheet.Cells[$"B{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            worksheet.Cells[$"C{row}"].Value = contratista.Key.Tipo;
        //            worksheet.Cells[$"C{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        }

        //        if (cantidadContratos > 1) 
        //        {
        //            //Agrupamos
        //            var rowFinal = row + cantidadContratos - 1;
                    
        //            using ExcelRange rangoNitContratista = worksheet.Cells[$"A{row}:A{rowFinal}"];
        //            rangoNitContratista.Merge = true;
        //            rangoNitContratista.Value = contratista.Key.NitContratista;
        //            rangoNitContratista.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            using ExcelRange rangoNombreContratista = worksheet.Cells[$"B{row}:B{rowFinal}"];
        //            rangoNombreContratista.Merge = true;
        //            rangoNombreContratista.Value = contratista.Key.NombreContratista;
        //            rangoNombreContratista.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            using ExcelRange rangoTipoContrato = worksheet.Cells[$"C{row}:C{rowFinal}"];
        //            rangoTipoContrato.Merge = true;
        //            rangoTipoContrato.Value = contratista.Key.Tipo;
        //            rangoTipoContrato.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //        }

        //        int contadorContrato = 0;

        //        foreach (var contrato in contratista) 
        //        {
        //            contadorContrato++;

        //            worksheet.Cells[$"D{row}"].Value = contrato.Numero;
        //            worksheet.Cells[$"E{row}"].Value = contrato.Objeto;
        //            worksheet.Cells[$"F{row}"].Value = contrato.NumeroMeses;

        //            worksheet.Cells[$"G{row}"].Value = contrato.FechaInicio;
        //            worksheet.Cells[$"G{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //            worksheet.Cells[$"H{row}"].Value = contrato.FechaFinal;
        //            worksheet.Cells[$"H{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //            worksheet.Cells[$"I{row}"].Value = contrato.FechaInicioActa;
        //            worksheet.Cells[$"I{row}"].Style.Numberformat.Format = "dd/mm/yyyy";

        //            worksheet.Cells[$"J{row}"].Value = contrato.NumeroDiasFinalizacion;

        //            worksheet.Cells[$"K{row}"].Value = contrato.Valor;
        //            worksheet.Cells[$"K{row}"].Style.Numberformat.Format = "[$$-409]#,##0";

        //            worksheet.Cells[$"L{row}"].Value = contrato.Administrador;
        //            worksheet.Cells[$"M{row}"].Value = contrato.Estado;

        //            worksheet.Cells[$"N{row}"].Value = contrato.Polizas;
        //            worksheet.Cells[$"N{row}"].Style.WrapText = true;

        //            worksheet.Cells[$"O{row}"].Value = contrato.OtrosSies;

        //            //worksheet.Row(row).Height = ;

        //            if (contadorContrato != cantidadContratos) 
        //            {
        //                row++;
        //            }
        //        }
        //    }

        //    //Configuraciones
        //    worksheet.Columns.AutoFit();
        //    worksheet.Column(14).Width = 69.57;
        //    worksheet.View.FreezePanes(2, 1);

        //    return await package.GetAsByteArrayAsync();
        //}

        public async Task<List<ResponseAction>> AprobacionAdministrador(CommandUpdateEstadoContrato command) 
        {
            return await _unitOfWork.ContratoRepository.AprobacionAdministrador(command);
        }
    }
}