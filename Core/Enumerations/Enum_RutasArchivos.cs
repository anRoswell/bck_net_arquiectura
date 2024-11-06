namespace Core.Enumerations
{
    public enum Enum_RutasArchivos
    {
        OrdenesAire = 1,
        /// <summary>
        /// Modulo exclusivo para Scr Mobile para sus fotografias y firmas en Entornos de qa y productivos. 
        /// </summary>
        ScrOrdenesMovile = 2,
        /// <summary>
        /// Modulo exclusivo para Scr Mobile para sus fotografias y firmas en Entornos Locales. 
        /// </summary>
        ScrOrdenesMovileLocal = 12,
        /// <summary>
        /// Modulo exclusivo para Gestion de daño Mobile para sus fotografias y firmas en Entornos Locales. 
        /// </summary>
        GDOrdenesMovileLocal = 13,

        /// <summary>
        /// Carga Masiva Ordenes
        /// RUTA_RED provee la ruta interna para guardar el archivo del proceso de carga masiva de ordenes en Ambientes de qa y productivos
        /// RUTA_WEB provee la ruta web para descargar el archivo del proceso de carga masiva de ordenes en Ambientes de qa y productivos
        /// </summary>
        ExcelScrMasivOrdBites = 4,
        /// <summary>
        /// Carga Masiva Ordenes
        /// RUTA_RED provee la ruta interna para guardar el archivo del proceso de carga masiva de ordenes en Ambiente Local
        /// RUTA_WEB provee la ruta web para descargar el archivo del proceso de carga masiva de ordenes en Ambiente Local
        /// </summary>
        ExcelScrMasivOrdBitesLocal = 44,

        /// <summary>
        /// prc_consultar_archivos_instancia
        /// RUTA_WEB provee la ruta web para descargar los archivos de excel del cargue masivo de ordenes en Ambientes de qa y productivos
        /// </summary>
        ExcelScrMasivOrdBase64 = 5,
        /// <summary>
        /// prc_consultar_archivos_instancia
        /// RUTA_WEB provee la ruta web para descargar los archivos de excel del cargue masivo de ordenes en Ambiente Local
        /// </summary>
        ExcelScrMasivOrdBase64Local = 55,

        /// <summary>
        /// Plantilla Carga Masiva Ordenes
        /// RUTA_WEB provee la ruta web para descargar la plantilla de carga masiva de ordenes en Ambientes de qa y productivos
        /// </summary>
        TmplteScrOrdenesMasivo = 6,
        /// <summary>
        /// Plantilla Carga Masiva Ordenes
        /// RUTA_WEB provee la ruta web para descargar la plantilla de carga masiva de ordenes en Ambiente Local
        /// </summary>
        TmplteScrOrdenesMasivoLocal = 3,

        /// <summary>
        /// JasperReports: 
        /// RUTA_WEB provee la ruta web para descargar logo Airea.
        /// </summary>
        JasperReportsLogo = 7,
        /// <summary>
        /// JasperReports: 
        /// RUTA_WEB provee la ruta web para descargar imagenes y firmas para el reporte acta cierre de ordenes en entornos de qa y productivos.
        /// </summary>
        JasperReportsImgsScrActa = 8,
        /// <summary>
        /// JasperReports:
        /// RUTA_WEB provee la ruta web para descargar imagenes y firmas para el reporte acta cierre de ordenes en entorno Local.
        /// </summary>
        JasperReportsImgsScrActaLocal = 88,
        /// <summary>
        /// JasperReports:
        /// RUTA_WEB url servidor de Jasper el cual genera reporte scr Acta Ordenes
        /// </summary>
        ServerJasperReportsUrlActaScr = 9,
        /// <summary>
        /// JasperReports:
        /// RUTA_WEB url base del servidor de Jasper para generar url dinamica de reportes
        /// </summary>
        ServerJasperReportsUrlBase = 99,

        #region GOS
        /// <summary>
        /// Modulo gos para imagenes en produccion.
        /// </summary>
        GosImageProd = 21,
        /// <summary>
        /// Modulo gos para excel y pdf en produccion.
        /// </summary>
        GosFileProd = 22,
        /// <summary>
        /// Modulo gos para excel y pdf en local.
        /// </summary>
        GosFileLocal = 23,
        /// <summary>
        /// Modulo gos para imagenes en local.
        /// </summary>
        GosImageLocal = 24,
        #endregion

        /// <summary>
        /// Excel: 
        /// RUTA_WEB provee la ruta web para descargar logo Air-e para archivos excel en entornos productivos.
        /// </summary>
        ExcelLogo = 10,
        /// <summary>
        /// Excel: 
        /// RUTA_WEB provee la ruta web para descargar logo Air-e para archivos excel en entornos locales.
        /// </summary>
        ExcelLogoLocal = 11,


        /// <summary>
        /// Plantilla Carga Masiva Reasignacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo Reasignacion Contratista en otros ambientes.
        /// </summary>
        TmplteScrReasignacionMasivo = 15,
        /// <summary>
        /// Plantilla Carga Masiva Reasignacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo Reasignacion Contratista en local.
        /// </summary>
        TmplteScrReasignacionMasivoLocal = 16,

        /// <summary>
        /// Plantilla Carga Masiva Legalizacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo Legalizacion en otros ambientes.
        /// </summary>
        TmplteScrLegalizacionMasivo = 17,
        /// <summary>
        /// Plantilla Carga Masiva Legalizacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo legalizacion en local.
        /// </summary>
        TmplteScrLegalizacionMasivoLocal = 18,


        /// <summary>
        /// Plantilla Carga Masiva Legalizacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo Asignacion en otros ambientes.
        /// </summary>
        TmplteScrAsignacionMasivo = 100,
        /// <summary>
        /// Plantilla Carga Masiva Legalizacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo Asignacion en local.
        /// </summary>
        TmplteScrAsignacionMasivoLocal = 101,

        /// <summary>
        /// Plantilla Carga Masiva Legalizacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo DesAsignacion en otros ambientes.
        /// </summary>
        TmplteScrDesAsignacionMasivo = 102,
        /// <summary>
        /// Plantilla Carga Masiva Legalizacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo DesAsignacion en local.
        /// </summary>
        TmplteScrDesAsignacionMasivoLocal = 103,

        /// <summary>
        /// Plantilla Carga Masiva Reasignacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo Reasignacion Contratista 2 en otros ambientes.
        /// </summary>
        TmplteScrReasignacionMasivo2 = 106,
        /// <summary>
        /// Plantilla Carga Masiva Reasignacion
        /// RUTA_WEB Exclusivo para descarga plantilla cargue masivo Reasignacion Contratista 2 en local.
        /// </summary>
        TmplteScrReasignacionMasivo2Local = 107,
    }

    public enum Enum_gnl_actividades
    {
        /// <summary>
        /// Modulo SCR
        /// </summary>
        G_SCR = 101,

        /// <summary>
        /// Modulo GOS
        /// </summary>
        G_GOS = 121
    }
}
