namespace Core.Entities
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// Id del registro
        /// </summary>
        public int Id { get; set; }
    }

    public abstract class BaseEntityOracle
    {
        /// <summary>
        /// Id del registro
        /// </summary>
        public int Id { get; set; }
    }

    public abstract class BaseEntityOracle2
    {
        /// <summary>
        /// Id del registro
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Codigo utilizado como id unico en la base de datos Oracle
        /// </summary>
        public string Codigox { get; set; }
    }
}
