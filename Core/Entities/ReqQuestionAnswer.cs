using System;

#nullable disable

namespace Core.Entities
{
    public partial class ReqQuestionAnswer: BaseEntity
    {
        public int? CodReqQuestionAnswer { get; set; }
        public int RqaCodRequerimiento { get; set; }
        public int RqaCodUsuario { get; set; }
        public bool RqaIsGestor { get; set; }
        public string NombreUsuario { get; set; }
        public string RqaComentario { get; set; }
        public bool RqahasUploadFile { get; set; }
        public bool RqaisPrivate { get; set; }
        public bool? RqaEstado { get; set; }
        public string? RqaFilePath { get; set; }
        public string? RqaFileRelativo { get; set; }
        public int? RqaFileSize { get; set; }
        public string? RqaFileExt { get; set; }
        public bool ShowResponse { get; set; }
        public int Usr_Tusr_CodTipoUsuario { get; set; }
        public string Tusr_Descripcion { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
