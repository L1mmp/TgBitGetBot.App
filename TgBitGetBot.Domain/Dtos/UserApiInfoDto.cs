namespace TgBitGetBot.Domain.Dtos;

public class UserApiInfoDto
{
	public string? Token { get; set; }
	public string? Passphrase { get; set; }
	public Guid UserId { get; set; }
	public DateTime CreatedOn { get; set; }
}