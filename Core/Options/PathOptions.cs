using System.Linq;

namespace Core.Options
{
    public class PathOptions
    {
        public int Codigo_Documento_Otros { get; set; }
        public string Path_FileServer_root { get; set; }
        public string Path_FileServer { get; set; }
        public string Path_FileServer_TempFiles { get; set; }
        public string Path_WebFileServer { get; set; }
        public string Folder_ArchivoPrincipal { get; set; }
        public string Folder_Archivos { get; set; }
        public string Folder_Archivos_Req { get; set; }
        
        public string Folder_Archivos_Op360 { get; set; }
        public string Folder_Archivos_Op360Incidencias { get; set; }

        public string Folder_Archivos_Contrato { get; set; }
        public string Folder_Archivos_Seguimientos_Contratos { get; set; }
        public string Folder_Archivos_Notificaciones_NoProrroga_Contratos { get; set; }
        public string Folder_Archivos_Terminacion { get; set; }
        public string Folder_Archivos_ParticipacionReq { get; set; }
        public string Folder_Archivos_Noticias { get; set; }
        public string Folder_Archivos_TipoMinuta { get; set; }
        public int IdPathFileServer { get; set; }
        public string Logo { get; set; }
    }

    public class PoliticasOptions
    {
        public string ShouldBeAnAdminAreaCentral { get; set; }
        public string ShouldBeAnAdminContratistas { get; set; }
        public string ShouldBeAnGosAreaCentral { get; set; }
        public string ShouldBeAnAdminAreaCentralReadOnly { get; set; }
        public string ShouldBeAnAdminAreaCentralContratistas { get; set; }
        public string ShouldBeAnAdminAreaCentralContratistasTerritorial { get; set; }
        public string ShouldBeAnAdminContratistasReadOnly { get; set; }
        public string ShouldBeOsfPolicy { get; set; }
        public string ShouldBeAnAdminAreaCentralTerritorial { get; set; }
        public string ShouldBeAnAdminAreaCentralTerritorialReadOnly2 { get; set; }
        public string ShouldBeAnAdminContratistasTerritorial { get; set; }

        public int[] ShouldBeAnAdminAreaCentralArray { 
            get
            {
                return ShouldBeAnAdminAreaCentral.Split(',').Select(int.Parse).ToArray(); 
            }
        }
        public int[] ShouldBeAnAdminContratistasArray
        {
            get
            {
                return ShouldBeAnAdminContratistas.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnGosAreaCentralArray
        {
            get
            {
                return ShouldBeAnGosAreaCentral.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnAdminAreaCentralReadOnlyArray
        {
            get
            {
                return ShouldBeAnAdminAreaCentralReadOnly.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnAdminAreaCentralContratistasArray
        {
            get
            {
                return ShouldBeAnAdminAreaCentralContratistas.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnAdminAreaCentralContratistasTerritorialArray
        {
            get
            {
                return ShouldBeAnAdminAreaCentralContratistasTerritorial.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnAdminContratistasReadOnlyArray
        {
            get
            {
                return ShouldBeAnAdminContratistasReadOnly.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeOsfPolicyArray
        {   get
            {
                return ShouldBeOsfPolicy.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnAdminAreaCentralTerritorialArray
        {
            get
            {
                return ShouldBeAnAdminAreaCentralTerritorial.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnAdminAreaCentralTerritorialReadOnly2Array
        {
            get
            {
                return ShouldBeAnAdminAreaCentralTerritorialReadOnly2.Split(',').Select(int.Parse).ToArray();
            }
        }
        public int[] ShouldBeAnAdminContratistasTerritorialArray
        {
            get
            {
                return ShouldBeAnAdminContratistasTerritorial.Split(',').Select(int.Parse).ToArray();
            }
        }
    }
}