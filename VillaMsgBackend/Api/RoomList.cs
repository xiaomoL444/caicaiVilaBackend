using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaMsgBackend.Api.Info;

namespace VillaMsgBackend.Api
{
	internal class RoomList : ApiInterface
	{
		public string url => "/roomlist";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);
			var roomList = new List<RoomListRsp>();

			roomList.AddRange(Core.RoomInstance.Where(q => q.Key.villa_id == roomFeature.villa_id).Select(q => new RoomListRsp() { room_id = q.Key.room_id, room_name = q.Value.RoomName }));

			return JsonConvert.SerializeObject(new { list = roomList, name = Core.RoomInstance[roomFeature].RoomName });
		}
	}
}
