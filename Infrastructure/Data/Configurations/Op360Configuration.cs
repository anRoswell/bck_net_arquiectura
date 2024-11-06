using Core.Entities;
using Core.ModelResponse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure.Data.Configurations
{
    /*
    * fecha: 19/12/2023
    * clave: 5e6ewrt546weasdf _10
    * carlos vargas
    */
    #region Seguridad    
    public class Usuarios_PerfilesConfiguration : IEntityTypeConfiguration<Usuarios_Perfiles>
    {
        public void Configure(EntityTypeBuilder<Usuarios_Perfiles> builder)
        {
            builder.HasNoKey();
        }
    }

    public class ResponseStringConfiguration : IEntityTypeConfiguration<gnl_peticiones_url_origen>
    {
        public void Configure(EntityTypeBuilder<gnl_peticiones_url_origen> builder)
        {
            builder.HasNoKey();
        }
    }
    #endregion
}
