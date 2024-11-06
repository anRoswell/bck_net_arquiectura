using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Enumerations
{
    public static class HubConections
    {
        public const string Usuarios = "/usuariosocket";
        public const string Notifications = "/notificationssocket";
        public const string Proveedores = "/proveedorsocket";
        public const string Test = "/testsocket";
    }
    public static class HubConectionsMethods
    {
        public const string CreateProveedor = "CreateProveedorEmit";
        public const string UpdateProveedor = "UpdateProveedorEmit";
        public const string Notification = "Notification";
    }

    public static class HubConectionsActions 
    {
        public const string CreateProveedor = "CreateProveedor";
        public const string UpdateProveedor = "UpdateProveedor";
        public const string CreateRequerimiento = "CreateRequerimiento";
        public const string UpdateRequerimiento = "UpdateRequerimiento";
        public const string AdjudicacionRequerimiento = "AdjudicacionRequerimiento";
        public const string UpdateAdjudicacionRequerimiento = "UpdateAdjudicacionRequerimiento";
    }
}
