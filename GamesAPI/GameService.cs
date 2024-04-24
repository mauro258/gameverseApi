using Oracle.ManagedDataAccess.Client;

namespace GamesAPI
{
	/// <summary>
	/// Clase que proporciona métodos para la obtención de juegos desde la base de datos.
	/// </summary>
	public class GameService
	{
		private readonly String _connectionString;

		/// <summary>
		/// Constructor de la clase GameService.
		/// </summary>
		/// <param name="connectionString">Cadena de conexión a la base de datos Oracle.</param>
		public GameService(String connectionString)
		{
			this._connectionString = connectionString;
		}

		/// <summary>
		/// Obtiene todos los juegos de la base de datos.
		/// </summary>
		/// <returns>Lista de objetos que representan los juegos obtenidos.</returns>
		/// <exception cref="Exception">Se lanza cuando ocurre un error durante el proceso de obtención de los juegos.</exception>
		public List<Object> GetGames()
		{
			List<Object> games = new List<Object>();

			try
			{
				// Establece una conexión con la base de datos Oracle.
				using (OracleConnection connection = new OracleConnection(this._connectionString))
				{
					connection.Open();

					// Crea y ejecuta un comando SQL para seleccionar todos los juegos de la tabla.
					OracleCommand command = new OracleCommand("SELECT id, name, description, imgUrl FROM SIF.SIF_DATOS_MMH", connection);
					using (OracleDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							// Crea un objeto anónimo para representar cada juego, con manejo de valores NULL.
							var game = new
							{
								id = reader.IsDBNull(reader.GetOrdinal("id")) ? (Int32?)null : reader.GetInt32(reader.GetOrdinal("id")),
								name = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name")),
								description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
								imgUrl = reader.IsDBNull(reader.GetOrdinal("imgUrl")) ? null : reader.GetString(reader.GetOrdinal("imgUrl"))
							};

							// Agrega el juego a la lista.
							games.Add(game);
						}
					}
				}
			}
			catch (Exception ex)
			{
				// Captura cualquier excepción que ocurra durante el proceso y la propaga hacia arriba.
				throw;
			}

			return games;
		}
	}
}
