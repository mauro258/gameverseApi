using GamesAPI.Data;
using Oracle.ManagedDataAccess.Client;

namespace GamesAPI
{
	/// <summary>
	/// Clase que proporciona métodos para la actualización de juegos en la base de datos.
	/// </summary>
	public class UpdateGameService
	{
		private readonly String _connectionString;

		/// <summary>
		/// Constructor de la clase UpdateGameService.
		/// </summary>
		/// <param name="connectionString">Cadena de conexión a la base de datos Oracle.</param>
		public UpdateGameService(String connectionString)
		{
			this._connectionString = connectionString;
		}

		/// <summary>
		/// Actualiza los datos de un juego existente en la base de datos.
		/// </summary>
		/// <param name="id">ID del juego que se desea actualizar.</param>
		/// <param name="updateGame">Objeto Game con los nuevos datos del juego.</param>
		/// <returns>True si la actualización fue exitosa, de lo contrario False.</returns>
		/// <exception cref="Exception">Se lanza cuando ocurre un error durante el proceso de actualización del juego.</exception>
		public Boolean UpdateGameById(Int32 id, Game updateGame)
		{
			try
			{
				// Establece una conexión con la base de datos Oracle.
				using (OracleConnection connection = new OracleConnection(this._connectionString))
				{
					connection.Open();

					// Crea y ejecuta un comando SQL para actualizar los datos del juego en la tabla.
					OracleCommand command = new OracleCommand("UPDATE SIF.SIF_DATOS_MMH SET name = :Name, description = :Description, imgUrl = :ImgUrl WHERE id = :Id", connection);

					// Asigna los nuevos valores de los parámetros del juego al comando SQL.
					command.Parameters.Add(":Name", OracleDbType.Varchar2).Value = updateGame.Name;
					command.Parameters.Add(":Description", OracleDbType.Varchar2).Value = updateGame.Description;
					command.Parameters.Add(":ImgUrl", OracleDbType.Varchar2).Value = updateGame.ImgUrl;
					command.Parameters.Add(":Id", OracleDbType.Int32).Value = id;

					// Ejecuta el comando SQL para actualizar el juego en la base de datos.
					Int32 rowsAffected = command.ExecuteNonQuery();

					// Devuelve True si se actualizaron filas en la base de datos, de lo contrario, devuelve False.
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
