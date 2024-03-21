using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillaMsgBackend.Api
{
	internal class getEmoticonInfo : ApiInterface
	{
		public string url => "/getEmoticonInfo";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			HttpClient httpClient = new HttpClient();
			var res = httpClient.Send(new HttpRequestMessage(HttpMethod.Get, "https://bbs-api.miyoushe.com/vila/api/getEmoticonInfo"));
			return res.Content.ReadAsStringAsync().Result;
		}
	}
}
