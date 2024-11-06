namespace Core.Enumerations
{
    /// <summary>
    /// Clase para manejo de nombres de variables de entorno
    /// NOTA:: revisar archivo launchSettings.json (ASPNETCORE_ENVIRONMENT)
    /// </summary>
    public static class ApiEnvironments
    {
        public static readonly string QA = "QA";
        public static readonly string Development = "Development";
        public static readonly string DevLocal = "devlocal";
        public static readonly string Production = "Production";
        public static readonly string Sirion = "Sirion";
    }
}
