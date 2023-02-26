using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TgBitGetBot.Domain.Entities;

public class User
{
	[Key]
	[Column(Order = 0)]
	public long Id { get; set; }
	public string Username { get; set; }

}