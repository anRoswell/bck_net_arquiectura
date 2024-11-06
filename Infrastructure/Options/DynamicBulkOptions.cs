namespace Infrastructure.Options
{
	public class DynamicBulkOptionsConstant
    {
        public const string DynamicBulk = "DynamicBulk";
    }

    public class DynamicBulkOptions
	{
        public string ConnectionString { get; set; }
        public int BatchSize { get; set; }
        public string DestinationSchemaName { get; set; }
        public int BulkCopyTimeout { get; set; }
    }
}