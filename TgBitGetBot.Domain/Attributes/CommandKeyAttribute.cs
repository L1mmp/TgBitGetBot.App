using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TgBitGetBot.Domain.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class CommandKeyAttribute : Attribute
	{
		public string Key { get; }

		public CommandKeyAttribute(string key)
		{
			Key = key;
		}

	}
}
