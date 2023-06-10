
namespace TgBitGetBot.Domain.Confgis
{
	/// <summary>
	/// Даннные EasyCache
	/// </summary>
	public class EasyCacheConfigs
	{
		/// <summary>
		/// Корневой адрес
		/// </summary>
		public InMemoryConfig inmemory { get; set; }
	}

	/// <summary>
	/// Параметры EasyCache для сохранения в памяти
	/// </summary>
	public class InMemoryConfig
	{
		/// <summary>
		/// Время жизни по умолчанию
		/// </summary>
		public int DefaultLifetimeMin { get; set; }
	}
}
