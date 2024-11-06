using System;

namespace Core.Entities;

public partial class TipoContrato : BaseEntity
{
    /// <summary>
    /// Nombre del tipo de contrato
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Cedula del usuario que crea el registro
    /// </summary>
    public string CodUser { get; set; }

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime FechaRegistro { get; set; }

    /// <summary>
    /// Cedula del ultimo usuario que actualizó el registro
    /// </summary>
    public string CodUserUpdate { get; set; }

    /// <summary>
    /// Fecha de la ultima actualización del registro.
    /// </summary>
    public DateTime FechaRegistroUpdate { get; set; }

    /// <summary>
    /// En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.
    /// </summary>
    public string Info { get; set; }

    /// <summary>
    /// En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.
    /// </summary>
    public string InfoUpdate { get; set; }
}