namespace Infrastructure.Factories
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Interfaces;
    using System.Linq;
    using Core.Enumerations;

    public class CargaMasivaFactory : ICargaMasivaFactory
    {
        #region Parameters
        private readonly IEnumerable<ICargaMasivaConfiguracion> _cargeMasivaConfiguracion;
        #endregion

        public CargaMasivaFactory(IEnumerable<ICargaMasivaConfiguracion> cargeMasivaConfiguracion)
        {
            _cargeMasivaConfiguracion = cargeMasivaConfiguracion;
        }

        public ICargaMasivaConfiguracion Crear(CargaInicial modulo)
        {
            var configuracion = _cargeMasivaConfiguracion.FirstOrDefault(c => c.CargaInicial == modulo);

            return configuracion ?? throw new Exception($"No existe un carge masivo para {modulo}.");
        }
    }
}

