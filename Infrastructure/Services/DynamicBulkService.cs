namespace Infrastructure.Services
{
    using System;
    using Oracle.ManagedDataAccess.Client;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Microsoft.Extensions.Configuration;
    using Infrastructure.Options;
    using Infrastructure.Interfaces;
    using System.Drawing;

    public class DynamicBulkService : IDynamicBulkService
    {
        #region
        private readonly IConfiguration _configuration;
        private DynamicBulkOptions _dynamicBulkOptions;
        #endregion

        public DynamicBulkService(IConfiguration configuration)
        {
            _configuration = configuration;
            GetConfiguration();
        }

        public async Task BulkCopyAsync(string destinationTable, Dictionary<string, Type> columnDefinitions, List<Dictionary<string, object>> data)
        {
            DataTable dt = columnDefinitions.CreateDataTable();
            foreach (var rowValues in data)
            {
                dt.AddRowWithValues(rowValues);
            }
            using (var cnn = new OracleConnection(_dynamicBulkOptions.ConnectionString))
            {
                //await cnn.OpenAsync();
                cnn.Open();
                //OracleBulkCopy bulkCopy = new(cnn);
                using (OracleBulkCopy bulkCopy = new(cnn))
                {
                    //OracleTransaction tran = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
                    foreach (DataColumn column in dt.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.BatchSize = _dynamicBulkOptions.BatchSize;
                    bulkCopy.BulkCopyOptions = OracleBulkCopyOptions.UseInternalTransaction;
                    bulkCopy.DestinationTableName = destinationTable;
                    bulkCopy.DestinationSchemaName = _dynamicBulkOptions.DestinationSchemaName;
                    bulkCopy.BulkCopyTimeout = _dynamicBulkOptions.BulkCopyTimeout;

                    bulkCopy.WriteToServer(dt);
                    dt.Clear();
                }
                await cnn.CloseAsync();
                //cnn.CloseAsync();
            }
        }

        #region PrivateMethod
        private void GetConfiguration()
        {
            DynamicBulkOptions instance = _dynamicBulkOptions = new DynamicBulkOptions();
            _configuration.Bind(DynamicBulkOptionsConstant.DynamicBulk, instance);
            instance.ConnectionString = _dynamicBulkOptions.ConnectionString;
            instance.BatchSize = _dynamicBulkOptions.BatchSize;
            instance.DestinationSchemaName = _dynamicBulkOptions.DestinationSchemaName;
            instance.BulkCopyTimeout = _dynamicBulkOptions.BulkCopyTimeout;
        }
        #endregion
    }
}