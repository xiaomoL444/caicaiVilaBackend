using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend.Api
{
	internal class emoticon_set:ApiInterface
	{
		public string url => "/emoticon_set";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			HttpClient httpClient = new HttpClient();
			var res = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, "https://bbs-api.miyoushe.com/misc/api/emoticon_set"));
			return res.Content.ReadAsStringAsync().Result;

		}
	}
}
