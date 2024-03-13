using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend
{
	internal interface ApiInterface
	{
		string url { get; }
		string GetContent(Dictionary<string, string> @params);

		/// <summary>
		/// http方法
		/// </summary>
		string httpMethod { get; }

	}
}
