using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaMsgBackend.Api.Info;

namespace VillaMsgBackend.Api
{
	internal class SearchUserInRoom : ApiInterface
	{
		public string url => "/searchUserInRoom";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);

			return JsonConvert.SerializeObject(Core.RoomInstance[roomFeature].SpokenUser);
		}
	}
}
