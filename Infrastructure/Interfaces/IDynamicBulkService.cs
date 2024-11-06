namespace Infrastructure.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDynamicBulkService
	{
        /// <summary>
        /// Metodo encargado de copiar el array de informacion.
        /// </summary>
        /// <param name="destinationTable">Tabla destino.</param>
        /// <param name="columnDefinitions">Definicion de columnas.</param>
        /// <param name="data">Data a guardar.</param>
        /// <returns></returns>
        public Task BulkCopyAsync(string destinationTable, Dictionary<string, Type> columnDefinitions, List<Dictionary<string, object>> data);
    }
}