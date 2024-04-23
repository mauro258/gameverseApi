using GamesAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace GamesAPI.Controllers
{
	/// <summary>
	/// Controlador API para administrar juegos.
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class GamesController : ControllerBase
	{
		private readonly String _connectionString;

		/// <summary>
		/// Constructor de la clase GamesController.
		/// </summary>
		/// <param name="configuration">Configuración de la aplicación.</param>
		public GamesController(IConfiguration configuration)
		{
			this._connectionString = configuration.GetConnectionString("OracleConnection");
		}


		[HttpGet]
		public IActionResult GetGame()
		{
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
						// Inicializa una lista para almacenar los juegos.
						List<Object> games = new List<Object>();

						// Recorre el resultado de la consulta.
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

						// Devuelve una respuesta HTTP 200 OK con la lista de juegos.
						return Ok(new { ok = true, data = games, message = "Lista de juegos" });
					}
				}
			}
			catch (Exception ex)
			{
				// Captura cualquier excepción que ocurra durante el proceso y devuelve un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		/// <summary>
		/// Obtiene un juego específico por su ID.
		/// </summary>
		/// <param name="id">El ID del juego que se desea obtener.</param>
		/// <returns>
		/// Devuelve un objeto JSON que contiene la información del juego si se encuentra, 
		/// o un mensaje de error si el juego no se encuentra.
		/// </returns>
		[HttpGet("{id}")]
		public IActionResult GetGameById(Int32 id)
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
						// Si se encuentra un juego con el ID especificado, crea un objeto para representarlo.
						if (reader.Read())
						{
							var game = new
							{
								id = reader.GetInt32(reader.GetOrdinal("id")),
								name = reader.GetString(reader.GetOrdinal("name")),
								description = reader.GetString(reader.GetOrdinal("description")),
								imgUrl = reader.GetString(reader.GetOrdinal("imgUrl")),
							};

							// Devuelve una respuesta HTTP 200 OK con la información del juego.
							return Ok(new { ok = true, data = game, message = "Juego encontrado" });
						}

						// Si no se encuentra ningún juego con el ID especificado, devuelve un error HTTP 404 Not Found.
						return NotFound(new { ok = false, data = "", message = "Juego no encontrado" });
					}
				}
			}
			catch (Exception ex)
			{
				// Captura cualquier excepción que ocurra durante el proceso y devuelve un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


		/// <summary>
		/// Crea un nuevo juego en la base de datos.
		/// </summary>
		/// <param name="game">Los datos del juego que se va a crear.</param>
		/// <returns>
		/// Devuelve un objeto JSON que indica si el juego se creó correctamente, junto con los datos del juego creado 
		/// si la operación fue exitosa, o un mensaje de error si ocurrió algún problema durante la creación del juego.
		/// </returns>
		[HttpPost]
		public IActionResult CreateGame([FromBody] Game game)
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
					command.Parameters.Add(":Name", OracleDbType.Varchar2).Value = game.name;
					command.Parameters.Add(":Description", OracleDbType.Varchar2).Value = game.description;
					command.Parameters.Add(":ImgUrl", OracleDbType.Varchar2).Value = game.imgUrl;

					// Ejecuta el comando SQL para insertar el juego en la base de datos.
					command.ExecuteNonQuery();

					// Devuelve una respuesta HTTP 201 Created indicando que el juego se ha creado correctamente.
					return Created("", new { ok = true, data = game, message = "Juego creado" });
				}
			}
			catch (Exception ex)
			{
				// Captura cualquier excepción que ocurra durante el proceso y devuelve un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


		/// <summary>
		/// Actualiza los datos de un juego existente en la base de datos mediante su identificador único.
		/// </summary>
		/// <param name="id">El identificador único del juego que se va a actualizar.</param>
		/// <param name="updateGame">Los nuevos datos del juego que se van a actualizar.</param>
		/// <returns>
		/// Devuelve un objeto JSON que indica si el juego se actualizó correctamente, junto con un mensaje de confirmación 
		/// si la operación fue exitosa, o un mensaje de error si ocurrió algún problema durante la actualización del juego.
		/// </returns>
		[HttpPut]
		public IActionResult UpdateGameById(Int32 id, [FromBody] Game updateGame)
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
					command.Parameters.Add(":Name", OracleDbType.Varchar2).Value = updateGame.name;
					command.Parameters.Add(":Description", OracleDbType.Varchar2).Value = updateGame.description;
					command.Parameters.Add(":ImgUrl", OracleDbType.Varchar2).Value = updateGame.imgUrl;
					command.Parameters.Add(":Id", OracleDbType.Int32).Value = id;

					// Ejecuta el comando SQL para actualizar el juego en la base de datos.
					Int32 rowsAffected = command.ExecuteNonQuery();

					// Verifica si se actualizaron filas en la base de datos y devuelve la respuesta correspondiente.
					return rowsAffected > 0
						 ? Ok(new { ok = true, data = "", message = "Juego actualizado" })
						 : NotFound(new { ok = false, data = "", message = "Juego no encontrado" });
				}
			}
			catch (Exception ex)
			{
				// Captura cualquier excepción que ocurra durante el proceso y devuelve un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		/// <summary>
		/// Elimina un juego de la base de datos mediante su identificador único.
		/// </summary>
		/// <param name="id">El identificador único del juego que se va a eliminar.</param>
		/// <returns>
		/// Devuelve un objeto JSON que indica si el juego se eliminó correctamente, junto con un mensaje de confirmación 
		/// si la operación fue exitosa, o un mensaje de error si ocurrió algún problema durante la eliminación del juego.
		/// </returns>
		[HttpDelete("{id}")]
		public IActionResult DeleteGameById(Int32 id)
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

					// Verifica si se eliminaron filas en la base de datos y devuelve la respuesta correspondiente.
					return rowsAffected > 0
						 ? Ok(new { ok = true, data = "", message = "Juego eliminado" })
						 : NotFound(new { ok = false, data = "", message = "Juego no encontrado" });
				}
			}
			catch (Exception ex)
			{
				// Captura cualquier excepción que ocurra durante el proceso y devuelve un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}
	}
}
