using GamesAPI.Data;
using Oracle.ManagedDataAccess.Client;

namespace GamesAPI
{
	/// <summary>
	/// Clase que proporciona métodos para la creación de juegos en la base de datos.
	/// </summary>
	public class CreateGameService
	{
		private readonly String _connectionString;

		/// <summary>
		/// Constructor de la clase CreateGameService.
		/// </summary>
		/// <param name="connectionString">Cadena de conexión a la base de datos Oracle.</param>
		public CreateGameService(String connectionString)
		{
			this._connectionString = connectionString;
		}

		/// <summary>
		/// Crea un nuevo juego en la base de datos.
		/// </summary>
		/// <param name="game">Objeto Game que representa el juego a crear.</param>
		/// <exception cref="Exception">Se lanza cuando ocurre un error durante el proceso de creación del juego.</exception>
		public void CreateGame(Game game)
		{
			try
			{
				// Establece una conexión con la base de datos Oracle.
				using (OracleConnection connection = new OracleConnection(this._connectionString))
				{
					connection.Open();

					// Crea y ejecuta un comando SQL para insertar un nuevo juego en la tabla.
					OracleCommand command = new OracleCommand("INSERT INTO SIF.SIF_DATOS_MMH(name, description, imgUrl) VALUES (:Name, :Description, :ImgUrl)", connection);

					// Asigna los valores de los parámetros del juego al comando SQL.
					command.Parameters.Add(":Name", OracleDbType.Varchar2).Value = game.Name;
					command.Parameters.Add(":Description", OracleDbType.Varchar2).Value = game.Description;
					command.Parameters.Add(":ImgUrl", OracleDbType.Varchar2).Value = game.ImgUrl;

					// Ejecuta el comando SQL para insertar el juego en la base de datos.
					command.ExecuteNonQuery();
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
