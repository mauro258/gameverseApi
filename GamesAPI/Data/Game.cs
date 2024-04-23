using System.ComponentModel.DataAnnotations;

namespace GamesAPI.Data
{
	/// <summary>
	/// Representa un juego en la aplicación.
	/// </summary>
	public class Game
	{
		/// <summary>
		/// Obtiene o establece el identificador único del juego.
		/// </summary>
		public Int32 id { get; set; }

		/// <summary>
		/// Obtiene o establece el nombre del juego.
		/// </summary>
		[MaxLength(225, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido")]
		public String name { get; set; }

		/// <summary>
		/// Obtiene o establece la descripción del juego.
		/// </summary>
		[MaxLength(1000, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres")]
		[Required(ErrorMessage = "El campo {0} es requerido")]
		public String description { get; set; }

		/// <summary>
		/// Obtiene o establece la URL de la imagen del juego.
		/// </summary>
		[Required(ErrorMessage = "El campo {0} es requerido")]
		public String imgUrl { get; set; }
	}
}
