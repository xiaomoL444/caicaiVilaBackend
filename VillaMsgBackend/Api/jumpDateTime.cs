using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaMsgBackend.Api.Info;

namespace VillaMsgBackend.Api
{
	internal class jumpDateTime : ApiInterface
	{
		public string url => "/jumpDateTime";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);
			var ts = (DateTime.ParseExact(@params["d"], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None) - Core.standTimeEpoch).TotalMilliseconds;

			return JsonConvert.SerializeObject(new SearchRsp(roomFeature,Core.RoomInstance[roomFeature].MsgInstance.First(q => q.MsgTime >= ts)));
		}
	}
}
