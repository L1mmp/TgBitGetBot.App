using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TgBitGetBot.Domain.Entities
{
	public class TokenInfo
	{
		[Key]
		[Column(Order = 0)]
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public decimal Depth24H { get; set; }
	}
}
