namespace Core.Entities
{
    using System;

    public partial class sgr_auth_tokens :BaseEntityOracle
	{
        public string auth_token { get; set; }

        public int id_credencial { get; set; }

        public DateTime fecha_registro { get; set; }

        public DateTime fecha_vence { get; set; }

        public string ind_activo { get; set; }
    }
}