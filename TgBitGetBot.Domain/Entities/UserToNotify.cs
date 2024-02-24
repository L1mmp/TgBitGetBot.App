using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TgBitGetBot.Domain.Enums;

namespace TgBitGetBot.Domain.Entities
{
	public class UserToNotify
	{
		[Key]
		[Column(Order = 0)]
		public Guid Id { get; set; }
		public User User { get; set; } = null!;
		public Guid UserId { get; set; }
		public NotifyType NotifyType { get; set; }
	}
}
