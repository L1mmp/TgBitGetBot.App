namespace TgBitGetBot.Domain.Models;
public class Coin
{
	public string? CoinId { get; set; }
	public string? CoinName { get; set; }
	public bool Transfer { get; set; }
	public List<Chain>? Chains { get; set; }
}

public partial class Chain
{
	public string? ChainName { get; set; }
	public bool NeedTag { get; set; }
	public bool Withdrawable { get; set; }
	public bool Rechargeable { get; set; }
	public decimal WithdrawFee { get; set; }
	public decimal ExtraWithDrawFee { get; set; }
	public int DepositConfirm { get; set; }
	public int WithdrawConfirm { get; set; }
	public decimal MinDepositAmount { get; set; }
	public decimal MinWithdrawAmount { get; set; }
	public string? BrowserUrl { get; set; }
}