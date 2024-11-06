using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresas", "par");

            builder.HasKey(e => e.Id)
                .HasName("PK__Empresa__8A1455DED5EA148A");

            builder.Property(e => e.Id)
                .HasColumnName("Emp_IdEmpresa")
                .HasComment("Código único asignado a la empresa. ");

            builder.Property(e => e.CodArchivo)
                .IsRequired()
                .HasMaxLength(450)
                .HasComment("Se carga una imagen con el logotipo de la empresa, esta debe de ser de maximo 128x128. Conecta con la tabla maestra de archivos.");

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

            builder.Property(e => e.EmpAbreviatura)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Emp_Abreviatura")
                .HasComment("Especifique una abreviatura para ser utilizada en reportes.");

            builder.Property(e => e.EmpDireccion)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Emp_Direccion")
                .HasComment("Dirección fisica dela empresa");

            builder.Property(e => e.EmpEstado)
                .IsRequired()
                .HasColumnName("Emp_Estado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.EmpNit)
                .IsRequired()
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Nit")
                .HasDefaultValueSql("((0))")
                .HasComment("Nit de la compañía");

            builder.Property(e => e.EmpNombreEmpresa)
                .IsRequired()
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("Emp_NombreEmpresa")
                .HasComment("Nombre completo de la empresa");

            builder.Property(e => e.EmpTelefono)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Emp_Telefono")
                .HasComment("Telefono");

            builder.Property(e => e.EmpTrbCodTrabajadorRepresentanteLegal)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Emp_Trb_CodTrabajadorRepresentanteLegal")
                .HasComment("Conecta con la tabla de trabajadores");
            
            builder.Property(e => e.EmpPrefijoContrato)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Emp_PrefijoContrato");

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
        }
    }
}
