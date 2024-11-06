using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ParamsGeneraleConfiguration : IEntityTypeConfiguration<ParamsGenerale>
    {
        public void Configure(EntityTypeBuilder<ParamsGenerale> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ParamsGe__76B0BA1AC01963B4");

            builder.ToTable("ParamsGenerales", "par");

            builder.HasIndex(e => e.PgeKeyParam, "IX_KeyParam")
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("pgeIdParamsGenerales")
                .HasComment("Identificación del registro");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.PgeEncriptedValorParam)
                .IsRequired()
                .HasMaxLength(8000)
                .HasColumnName("pgeEncriptedValorParam")
                .HasDefaultValueSql("((0))")
                .HasComment("VAlor del token incriptado");

            builder.Property(e => e.PgeEstado)
                .IsRequired()
                .HasColumnName("pgeEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.PgeKeyParam)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("pgeKeyParam")
                .HasComment("Descripcion de la parametrización");

            builder.Property(e => e.PgeValorParam)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("pgeValorParam")
                .HasDefaultValueSql("('Sin Valor Param')")
                .HasComment("Valor del parametro");
        }
    }
}
