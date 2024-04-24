using Oracle.ManagedDataAccess.Client;

namespace GamesAPI
{
	/// <summary>
	/// Clase que proporciona métodos para la eliminación de juegos en la base de datos.
	/// </summary>
	public class DeleteGameService
	{
		private readonly String _connectionString;

		/// <summary>
		/// Constructor de la clase DeleteGameService.
		/// </summary>
		/// <param name="connectionString">Cadena de conexión a la base de datos Oracle.</param>
		public DeleteGameService(String connectionString)
		{
			this._connectionString = connectionString;
		}

		/// <summary>
		/// Elimina un juego de la base de datos por su ID.
		/// </summary>
		/// <param name="id">ID del juego que se desea eliminar.</param>
		/// <returns>True si se eliminó el juego correctamente, de lo contrario False.</returns>
		/// <exception cref="Exception">Se lanza cuando ocurre un error durante el proceso de eliminación del juego.</exception>
		public Boolean DeleteGameById(Int32 id)
		{
			try
			{
				// Establece una conexión con la base de datos Oracle.
				using (OracleConnection connection = new OracleConnection(this._connectionString))
				{
					connection.Open();

					// Crea y ejecuta un comando SQL para eliminar el juego de la tabla.
					OracleCommand command = new OracleCommand("DELETE FROM SIF.SIF_DATOS_MMH WHERE id = :Id", connection);
					command.Parameters.Add(":Id", OracleDbType.Int32).Value = id;
					Int32 rowsAffected = command.ExecuteNonQuery();

					// Devuelve True si se eliminaron filas en la base de datos, de lo contrario, devuelve False.
					return rowsAffected > 0;
				}
			}
			catch (Exception ex)
			{
				// Captura cualquier excepción que ocurra durante el proceso y la propaga hacia arriba.
				throw;
			}
		}
	}
}
