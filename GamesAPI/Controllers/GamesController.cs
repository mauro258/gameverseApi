using GamesAPI.Data;
using Microsoft.AspNetCore.Mvc;

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
				// Instancia de GameService para obtener los juegos desde la base de datos.
				GameService gameService = new GameService(this._connectionString);

				// Obtener la lista de juegos.
				List<Object> games = gameService.GetGames();

				// Devolver una respuesta HTTP 200 OK con la lista de juegos.
				return Ok(new { ok = true, data = games, message = "Lista de juegos" });
			}
			catch (Exception ex)
			{
				// Capturar cualquier excepción que ocurra durante el proceso y devolver un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}



		[HttpGet("{id}")]
		public IActionResult GetGameById(Int32 id)
		{
			try
			{
				// Instancia de GameServiceById para obtener un juego por su ID desde la base de datos.
				GameServiceById gameServiceById = new GameServiceById(this._connectionString);

				// Obtener el juego por su ID.
				Object game = gameServiceById.GetGameById(id);

				// Verificar si se encontró un juego y devolver la respuesta correspondiente.
				if (game != null && game.GetType().GetProperties().Length > 0)
				{
					return Ok(new { ok = true, data = game, message = "Juego encontrado" });
				}
				else
				{
					return NotFound(new { ok = false, data = "", message = "Juego no encontrado" });
				}
			}
			catch (Exception ex)
			{
				// Capturar cualquier excepción que ocurra durante el proceso y devolver un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}




		[HttpPost]
		public IActionResult CreateGame([FromBody] Game game)
		{
			try
			{
				// Instancia de CreateGameService para crear un nuevo juego en la base de datos.
				CreateGameService createGameService = new CreateGameService(this._connectionString);

				// Crear un nuevo juego.
				createGameService.CreateGame(game);

				// Devolver una respuesta HTTP 201 Created indicando que el juego se ha creado correctamente.
				return Created("", new { ok = true, data = game, message = "Juego creado" });
			}
			catch (Exception ex)
			{
				// Capturar cualquier excepción que ocurra durante el proceso y devolver un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}





		[HttpPut("{id}")]
		public IActionResult UpdateGameById(Int32 id, [FromBody] Game updateGame)
		{
			try
			{
				// Instancia de UpdateGameService para actualizar un juego por su ID en la base de datos.
				UpdateGameService updateGameService = new UpdateGameService(this._connectionString);

				// Actualizar el juego por su ID.
				Boolean success = updateGameService.UpdateGameById(id, updateGame);

				// Verificar si se actualizó el juego y devolver la respuesta correspondiente.
				if (success)
				{
					return Ok(new { ok = true, data = "", message = "Juego actualizado" });
				}
				else
				{
					return NotFound(new { ok = false, data = "", message = "Juego no encontrado" });
				}
			}
			catch (Exception ex)
			{
				// Capturar cualquier excepción que ocurra durante el proceso y devolver un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}




		[HttpDelete("{id}")]
		public IActionResult DeleteGameById(Int32 id)
		{
			try
			{
				// Instancia de DeleteGameService para eliminar un juego por su ID en la base de datos.
				DeleteGameService deleteGameService = new DeleteGameService(this._connectionString);

				// Eliminar el juego por su ID.
				Boolean success = deleteGameService.DeleteGameById(id);

				// Verificar si se eliminó el juego y devolver la respuesta correspondiente.
				if (success)
				{
					return Ok(new { ok = true, data = "", message = "Juego eliminado" });
				}
				else
				{
					return NotFound(new { ok = false, data = "", message = "Juego no encontrado" });
				}
			}
			catch (Exception ex)
			{
				// Capturar cualquier excepción que ocurra durante el proceso y devolver un error HTTP 500.
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


	}
}
