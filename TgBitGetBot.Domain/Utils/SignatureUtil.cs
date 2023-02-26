using System.Security.Cryptography;
using System.Text;

namespace TgBitGetBot.Domain.Utils;

public class SignatureUtil
{
	public string Generate(string timestamp, string method, string requestPath,
		string queryString, string body, string secretKey)
	{
		method = method.ToUpper();
		body = string.IsNullOrEmpty(body) ? string.Empty : body;
		queryString = string.IsNullOrWhiteSpace(queryString) ? string.Empty : "?" + queryString;

		var preHash = timestamp + method + requestPath + queryString + body;
		Console.WriteLine(preHash);

		var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

		using var hmacSha256 = new HMACSHA256(secretKeyBytes);

		var hash = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(preHash));
		return Convert.ToBase64String(hash);
	}
}