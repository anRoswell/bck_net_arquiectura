namespace Core.CustomEntities
{
    public class DataModelSignalResponse<D>
    {
        public string Id { get; set; }
        public D Data { get; set; }
    }

    public class DataProveedores
    {
        public double Progress { get; set; }
    }

    public class DataNotifications
    { 
        public string Method{ get; set; }
        public string Action { get; set; }
    }
}
