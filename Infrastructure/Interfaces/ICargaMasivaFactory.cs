namespace Infrastructure.Interfaces
{
    using Core.Enumerations;

    public interface ICargaMasivaFactory
	{
        /// <summary>
        /// Crea la carga masiva.
        /// </summary>
        /// <param name="modulo">Modulo.</param>
        /// <returns>ICargeMasivaConfiguracion</returns>
        public ICargaMasivaConfiguracion Crear(CargaInicial modulo);
    }
}

