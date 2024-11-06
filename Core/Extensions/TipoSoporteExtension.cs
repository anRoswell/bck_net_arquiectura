namespace Core.Extensions
{
    using System;
    using Core.Enumerations;

    public static class TipoSoporteExtension
	{
        public static TipoSoporteEnum GetTipoSoporte(this string codigoTipoSoporte)
        {
            if (Enum.TryParse(codigoTipoSoporte, out TipoSoporteEnum tipoSoporte) && Enum.IsDefined(typeof(TipoSoporteEnum), tipoSoporte))
            {
                return tipoSoporte;
            }

            throw new NotImplementedException($"El tipo de soporte '{codigoTipoSoporte}' no está implementado.");
        }
    }
}