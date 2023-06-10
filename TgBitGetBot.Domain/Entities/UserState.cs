using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TgBitGetBot.Domain.Enums;

namespace TgBitGetBot.Domain.Entities
{
	public class UserState
	{
		[Key]
		[Column(Order = 0)]
		public Guid Id { get; set; }
        public long TelegramId { get; set; }
        public TelegramDialogState State { get; set; }
    }
}
