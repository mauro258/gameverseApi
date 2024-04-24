using Oracle.ManagedDataAccess.Client;

namespace GamesAPI
{
	/// <summary>
	/// Clase que proporciona métodos para la obtención de un juego por su ID desde la base de datos.
	/// </summary>
	public class GameServiceById
	{
		private readonly String _connectionString;

		/// <summary>
		/// Constructor de la clase GameServiceById.
		/// </summary>
		/// <param name="connectionString">Cadena de conexión a la base de datos Oracle.</param>
		public GameServiceById(String connectionString)
		{
			this._connectionString = connectionString;
		}

		/// <summary>
		/// Obtiene un juego de la base de datos por su ID.
		/// </summary>
		/// <param name="id">ID del juego que se desea obtener.</param>
		/// <returns>Objeto que representa el juego obtenido.</returns>
		/// <exception cref="Exception">Se lanza cuando ocurre un error durante el proceso de obtención del juego.</exception>
		public Object GetGameById(Int32 id)
		{
			try
			{
				// Establece una conexión con la base de datos Oracle.
				using (OracleConnection connection = new OracleConnection(this._connectionString))
				{
					connection.Open();

					// Crea y ejecuta un comando SQL para seleccionar el juego con el ID especificado.
					OracleCommand command = new OracleCommand("SELECT * FROM SIF.SIF_DATOS_MMH WHERE id = :Id", connection);
					command.Parameters.Add(":Id", OracleDbType.Int32).Value = id;

					using (OracleDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							// Crea un objeto anónimo para representar el juego obtenido.
							var game = new
							{
								id = reader.GetInt32(reader.GetOrdinal("id")),
								name = reader.GetString(reader.GetOrdinal("name")),
								description = reader.GetString(reader.GetOrdinal("description")),
								imgUrl = reader.GetString(reader.GetOrdinal("imgUrl")),
							};

							return game;
						}

						// Si no se encuentra ningún juego con el ID especificado, devuelve un objeto vacío.
						return new { };
					}
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
