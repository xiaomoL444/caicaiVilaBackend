using Newtonsoft.Json;

namespace VillaMsgBackend.Api
{
	internal class GetMaxPage : ApiInterface
	{
		public string url => "/maxPage";

		public string httpMethod => "GET";

		public string GetContent(Dictionary<string, string> @params)
		{
			var roomFeature = Util.GetRoomFeature(@params);

			//未传入房间id
			if (roomFeature.room_id == "false")
			{
				var default_room_id = Core.RoomInstance.Where(q => q.Key.villa_id == roomFeature.villa_id).ToList()[0].Key.room_id;
				return $"{{\"jumpToRoom\":{default_room_id}}}";
			}
			return JsonConvert.SerializeObject(new { page = Core.RoomInstance[roomFeature].MsgInstance.Count / 50 });
		}
	}
}
