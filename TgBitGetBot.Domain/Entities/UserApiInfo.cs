using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TgBitGetBot.Domain.Entities;

public class UserApiInfo
{
	[Key]
	[Column(Order = 0)]	
	public Guid Id { get; set; }
	public string? Token { get; set; }
	public string? Passphrase { get; set; }
	public User? User { get; set; }
	public Guid UserId { get; set; }
}