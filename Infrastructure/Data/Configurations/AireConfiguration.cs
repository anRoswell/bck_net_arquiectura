using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    /*
    * fecha: 19/12/2023
    * clave: asd6f4sad65f4sad4f6asd65f65as6f
    * carlos vargas
    */
    #region ÓrdenesSCR
    public class OrdenesSCRConfiguration : IEntityTypeConfiguration<OrdenesSCR>
    {
        public void Configure(EntityTypeBuilder<OrdenesSCR> builder)
        {
            builder.HasNoKey();
        }
    }    
    #endregion
}
